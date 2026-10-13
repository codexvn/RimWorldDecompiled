using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C25 RID: 15397
	public class FastTileFinder : IDisposable
	{
		// Token: 0x170033AA RID: 13226
		// (get) Token: 0x06014E30 RID: 85552 RVA: 0x00641867 File Offset: 0x0063FA67
		public PlanetLayer Layer { get; }

		// Token: 0x170033AB RID: 13227
		// (get) Token: 0x06014E31 RID: 85553 RVA: 0x0064186F File Offset: 0x0063FA6F
		public int MaxResults
		{
			get
			{
				return math.min(this.Layer.TilesCount, 50);
			}
		}

		// Token: 0x06014E32 RID: 85554 RVA: 0x00641884 File Offset: 0x0063FA84
		public FastTileFinder(PlanetLayer layer)
		{
			this.Layer = layer;
			this.tmpNormalResults = new NativeArray<PlanetTile>(this.MaxResults, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.tmpDesperateResults = new NativeArray<PlanetTile>(this.MaxResults, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.cachedTileData = new NativeArray<FastTileFinder.CachedTileData>(layer.TilesCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06014E33 RID: 85555 RVA: 0x006418E9 File Offset: 0x0063FAE9
		public void Dispose()
		{
			ref this.tmpNormalResults.EnsureDisposed<PlanetTile>();
			ref this.tmpDesperateResults.EnsureDisposed<PlanetTile>();
			ref this.cachedTileData.EnsureDisposed<FastTileFinder.CachedTileData>();
		}

		// Token: 0x06014E34 RID: 85556 RVA: 0x0064190C File Offset: 0x0063FB0C
		public void DirtyCache()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06014E35 RID: 85557 RVA: 0x00641915 File Offset: 0x0063FB15
		public void DirtyTile(PlanetTile tile)
		{
			if (this.cacheDirty)
			{
				return;
			}
			this.tileChangedSinceQuery = true;
			this.cachedTileData[tile.tileId] = new FastTileFinder.CachedTileData(tile.Tile);
		}

		// Token: 0x06014E36 RID: 85558 RVA: 0x00641944 File Offset: 0x0063FB44
		public List<PlanetTile> Query(FastTileFinder.TileQueryParams query, List<BiomeDef> biomes = null, List<LandmarkDef> landmarks = null, FastTileFinder.TileQueryParams desperate = default(FastTileFinder.TileQueryParams))
		{
			if (this.cacheDirty)
			{
				this.RegenerateCache();
			}
			else if (!this.tileChangedSinceQuery && this.lastNormalQuery == query && this.lastDesperateQuery == desperate)
			{
				return this.tmpOutputResults;
			}
			NativeArray<ushort> nBiomes;
			NativeArray<ushort> nLandmarks;
			this.PrepareBuffers(biomes, landmarks, out nBiomes, out nLandmarks);
			using (ProfilerBlock.Scope("Compute Tile Query"))
			{
				this.ParameterizeJob(query, desperate, nBiomes, nLandmarks).Schedule(this.cachedTileData.Length, UnityData.GetIdealBatchCount(this.cachedTileData.Length), default(JobHandle)).Complete();
			}
			nBiomes.Dispose();
			nLandmarks.Dispose();
			this.lastNormalQuery = query;
			this.lastDesperateQuery = desperate;
			this.tileChangedSinceQuery = false;
			if (this.tmpNormalResults.Length > 0)
			{
				NativeArrayUtility.CopyArrayToList<PlanetTile>(this.tmpOutputResults, this.tmpNormalResults);
				FastTileFinder.RemoveInvalidTiles(this.tmpOutputResults);
			}
			if (this.tmpOutputResults.Count == 0 && this.tmpDesperateResults.Length > 0)
			{
				NativeArrayUtility.CopyArrayToList<PlanetTile>(this.tmpOutputResults, this.tmpDesperateResults);
				FastTileFinder.RemoveInvalidTiles(this.tmpOutputResults);
			}
			return this.tmpOutputResults;
		}

		// Token: 0x06014E37 RID: 85559 RVA: 0x00641A8C File Offset: 0x0063FC8C
		public bool Closest(FastTileFinder.TileQueryParams query, out PlanetTile tile, List<BiomeDef> biomes = null, List<LandmarkDef> landmarks = null, FastTileFinder.TileQueryParams desperate = default(FastTileFinder.TileQueryParams))
		{
			if (this.cacheDirty)
			{
				this.RegenerateCache();
			}
			else if (!this.tileChangedSinceQuery && this.lastClosestNormalQuery == query && this.lastClosestDesperateQuery == desperate)
			{
				tile = this.tmpOutputClosest;
				return tile.Valid;
			}
			NativeArray<ushort> nBiomes;
			NativeArray<ushort> nLandmarks;
			this.PrepareBuffers(biomes, landmarks, out nBiomes, out nLandmarks);
			using (ProfilerBlock.Scope("Compute Tile Query"))
			{
				FastTileFinder.ComputeQueryJob jobData = this.ParameterizeJob(query, desperate, nBiomes, nLandmarks);
				jobData.closest = true;
				jobData.Schedule(this.cachedTileData.Length, UnityData.GetIdealBatchCount(this.cachedTileData.Length), default(JobHandle)).Complete();
			}
			nBiomes.Dispose();
			nLandmarks.Dispose();
			this.lastClosestNormalQuery = query;
			this.lastClosestDesperateQuery = desperate;
			this.tileChangedSinceQuery = false;
			if (this.TryGetClosest(out tile))
			{
				this.tmpOutputClosest = tile;
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x06014E38 RID: 85560 RVA: 0x00641BA8 File Offset: 0x0063FDA8
		private bool TryGetClosest(out PlanetTile closest)
		{
			int num = -1;
			float num2 = float.MaxValue;
			for (int i = 0; i < ThreadResultPool.closestSqrDists.Length; i++)
			{
				if (ThreadResultPool.closestSqrDists[i] < num2)
				{
					num = ThreadResultPool.closestResults[i];
					num2 = ThreadResultPool.closestSqrDists[i];
				}
			}
			if (num == -1)
			{
				closest = PlanetTile.Invalid;
				return false;
			}
			closest = new PlanetTile(num, this.Layer);
			return true;
		}

		// Token: 0x06014E39 RID: 85561 RVA: 0x00641C1C File Offset: 0x0063FE1C
		private FastTileFinder.ComputeQueryJob ParameterizeJob(FastTileFinder.TileQueryParams query, FastTileFinder.TileQueryParams desperate, NativeArray<ushort> nBiomes, NativeArray<ushort> nLandmarks)
		{
			PlanetTile origin = query.origin;
			int region = (origin.Valid && origin.Layer == this.Layer) ? this.cachedTileData[origin.tileId].regionId : -1;
			return new FastTileFinder.ComputeQueryJob
			{
				tiles = this.cachedTileData,
				normalResult = this.tmpNormalResults,
				desperateResult = this.tmpDesperateResults,
				closestResults = ThreadResultPool.closestResults,
				closestSqrDists = ThreadResultPool.closestSqrDists,
				biomes = nBiomes,
				landmarks = nLandmarks,
				normal = query,
				desperate = desperate,
				layerId = this.Layer.LayerID,
				layerRadius = this.Layer.Radius,
				averageTileSize = this.Layer.AverageTileSize,
				maximum = this.MaxResults,
				countNormal = 0,
				countDesperate = 0,
				vOrigin = origin.Layer.GetTileCenter(origin),
				region = region
			};
		}

		// Token: 0x06014E3A RID: 85562 RVA: 0x00641D40 File Offset: 0x0063FF40
		private void PrepareBuffers(List<BiomeDef> biomes, List<LandmarkDef> landmarks, out NativeArray<ushort> nBiomes, out NativeArray<ushort> nLandmarks)
		{
			this.ResetNativeCache();
			ThreadResultPool.EnsureReady();
			nBiomes = new NativeArray<ushort>(biomes.NullOrEmpty<BiomeDef>() ? 0 : biomes.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			nLandmarks = new NativeArray<ushort>(landmarks.NullOrEmpty<LandmarkDef>() ? 0 : landmarks.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			if (!biomes.NullOrEmpty<BiomeDef>())
			{
				for (int i = 0; i < biomes.Count; i++)
				{
					nBiomes[i] = biomes[i].shortHash;
				}
			}
			if (!landmarks.NullOrEmpty<LandmarkDef>())
			{
				for (int j = 0; j < landmarks.Count; j++)
				{
					nLandmarks[j] = landmarks[j].shortHash;
				}
			}
		}

		// Token: 0x06014E3B RID: 85563 RVA: 0x00641DF0 File Offset: 0x0063FFF0
		private static void RemoveInvalidTiles(List<PlanetTile> tiles)
		{
			for (int i = tiles.Count - 1; i >= 0; i--)
			{
				if (!tiles[i].Valid)
				{
					tiles.RemoveAt(i);
				}
			}
		}

		// Token: 0x06014E3C RID: 85564 RVA: 0x00641E28 File Offset: 0x00640028
		private void ResetNativeCache()
		{
			for (int i = 0; i < this.MaxResults; i++)
			{
				this.tmpNormalResults[i] = PlanetTile.Invalid;
				this.tmpDesperateResults[i] = PlanetTile.Invalid;
			}
		}

		// Token: 0x06014E3D RID: 85565 RVA: 0x00641E68 File Offset: 0x00640068
		public void RegenerateCache()
		{
			if (Find.WorldGrid == null)
			{
				return;
			}
			this.cacheDirty = false;
			this.tileChangedSinceQuery = true;
			Parallel.For(0, this.Layer.TilesCount, delegate(int i)
			{
				this.cachedTileData[i] = new FastTileFinder.CachedTileData(this.Layer.Tiles[i]);
			});
		}

		// Token: 0x06014E3F RID: 85567 RVA: 0x00641EC2 File Offset: 0x006400C2
		public static void Initialize$ComputeQueryJob_SphericalDistance_00014E3B$BurstDirectCall()
		{
			FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.Initialize();
		}

		// Token: 0x0400E8C3 RID: 59587
		private NativeArray<PlanetTile> tmpNormalResults;

		// Token: 0x0400E8C4 RID: 59588
		private NativeArray<PlanetTile> tmpDesperateResults;

		// Token: 0x0400E8C5 RID: 59589
		private NativeArray<FastTileFinder.CachedTileData> cachedTileData;

		// Token: 0x0400E8C6 RID: 59590
		private ParallelLoopResult rebuildingTask;

		// Token: 0x0400E8C7 RID: 59591
		private bool cacheDirty = true;

		// Token: 0x0400E8C8 RID: 59592
		private bool tileChangedSinceQuery;

		// Token: 0x0400E8CA RID: 59594
		private FastTileFinder.TileQueryParams lastNormalQuery;

		// Token: 0x0400E8CB RID: 59595
		private FastTileFinder.TileQueryParams lastDesperateQuery;

		// Token: 0x0400E8CC RID: 59596
		private readonly List<PlanetTile> tmpOutputResults = new List<PlanetTile>();

		// Token: 0x0400E8CD RID: 59597
		private FastTileFinder.TileQueryParams lastClosestNormalQuery;

		// Token: 0x0400E8CE RID: 59598
		private FastTileFinder.TileQueryParams lastClosestDesperateQuery;

		// Token: 0x0400E8CF RID: 59599
		private PlanetTile tmpOutputClosest;

		// Token: 0x02003C26 RID: 15398
		public enum LandmarkMode
		{
			// Token: 0x0400E8D1 RID: 59601
			Any,
			// Token: 0x0400E8D2 RID: 59602
			Required,
			// Token: 0x0400E8D3 RID: 59603
			Forbidden
		}

		// Token: 0x02003C27 RID: 15399
		public readonly struct TileQueryParams : IEquatable<FastTileFinder.TileQueryParams>
		{
			// Token: 0x170033AC RID: 13228
			// (get) Token: 0x06014E40 RID: 85568 RVA: 0x00641EC9 File Offset: 0x006400C9
			public bool Valid { get; }

			// Token: 0x06014E41 RID: 85569 RVA: 0x00641ED4 File Offset: 0x006400D4
			public TileQueryParams(PlanetTile origin, float minDistTiles = 0f, float maxDistTiles = 3.4028235E+38f, FastTileFinder.LandmarkMode landmarkMode = FastTileFinder.LandmarkMode.Any, bool reachable = true, Hilliness minHilliness = Hilliness.Undefined, Hilliness maxHilliness = Hilliness.Undefined, bool checkBiome = true, bool validSettlement = true, bool comboLandmarks = true)
			{
				this.origin = origin;
				this.minDistTiles = minDistTiles;
				this.maxDistTiles = maxDistTiles;
				this.landmarkMode = landmarkMode;
				this.reachable = reachable;
				this.minHilliness = minHilliness;
				this.maxHilliness = maxHilliness;
				this.checkBiome = checkBiome;
				this.validSettlement = validSettlement;
				this.comboLandmarks = comboLandmarks;
				this.Valid = 1;
			}

			// Token: 0x06014E42 RID: 85570 RVA: 0x00641F35 File Offset: 0x00640135
			public static bool operator ==(FastTileFinder.TileQueryParams lhs, FastTileFinder.TileQueryParams rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06014E43 RID: 85571 RVA: 0x00641F3F File Offset: 0x0064013F
			public static bool operator !=(FastTileFinder.TileQueryParams lhs, FastTileFinder.TileQueryParams rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x06014E44 RID: 85572 RVA: 0x00641F4C File Offset: 0x0064014C
			public bool Equals(FastTileFinder.TileQueryParams other)
			{
				return this.origin.Equals(other.origin) && this.landmarkMode == other.landmarkMode && this.reachable == other.reachable && this.minHilliness == other.minHilliness && this.maxHilliness == other.maxHilliness && this.checkBiome == other.checkBiome && this.validSettlement == other.validSettlement && this.comboLandmarks == other.comboLandmarks && math.abs(this.minDistTiles - other.minDistTiles) < 0.01f && math.abs(this.maxDistTiles - other.maxDistTiles) < 0.01f;
			}

			// Token: 0x06014E45 RID: 85573 RVA: 0x00642008 File Offset: 0x00640208
			public override bool Equals(object obj)
			{
				if (obj is FastTileFinder.TileQueryParams)
				{
					FastTileFinder.TileQueryParams other = (FastTileFinder.TileQueryParams)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x06014E46 RID: 85574 RVA: 0x00642030 File Offset: 0x00640230
			public override int GetHashCode()
			{
				int value = HashCode.Combine<PlanetTile, FastTileFinder.LandmarkMode, bool, Hilliness, Hilliness, bool>(this.origin, this.landmarkMode, this.reachable, this.minHilliness, this.maxHilliness, this.checkBiome);
				int value2 = HashCode.Combine<bool, float, float, bool>(this.validSettlement, this.minDistTiles, this.maxDistTiles, this.comboLandmarks);
				return HashCode.Combine<int, int>(value, value2);
			}

			// Token: 0x0400E8D4 RID: 59604
			public readonly PlanetTile origin;

			// Token: 0x0400E8D5 RID: 59605
			public readonly float minDistTiles;

			// Token: 0x0400E8D6 RID: 59606
			public readonly float maxDistTiles;

			// Token: 0x0400E8D7 RID: 59607
			public readonly FastTileFinder.LandmarkMode landmarkMode;

			// Token: 0x0400E8D8 RID: 59608
			public readonly bool reachable;

			// Token: 0x0400E8D9 RID: 59609
			public readonly bool checkBiome;

			// Token: 0x0400E8DA RID: 59610
			public readonly bool comboLandmarks;

			// Token: 0x0400E8DB RID: 59611
			public readonly bool validSettlement;

			// Token: 0x0400E8DC RID: 59612
			public readonly Hilliness minHilliness;

			// Token: 0x0400E8DD RID: 59613
			public readonly Hilliness maxHilliness;
		}

		// Token: 0x02003C28 RID: 15400
		private readonly struct CachedTileData : IEquatable<FastTileFinder.CachedTileData>
		{
			// Token: 0x170033AD RID: 13229
			// (get) Token: 0x06014E47 RID: 85575 RVA: 0x0064208A File Offset: 0x0064028A
			public bool Valid { get; }

			// Token: 0x06014E48 RID: 85576 RVA: 0x00642094 File Offset: 0x00640294
			public CachedTileData(Tile tile)
			{
				this.center = tile.Layer.GetTileCenter(tile.tile);
				this.regionId = Find.WorldReachability.GetLocalFieldId(tile.tile);
				this.passable = Find.WorldPathGrid.PassableFast(tile.tile);
				this.landmark = (tile.Landmark != null);
				this.biomeHash = tile.PrimaryBiome.shortHash;
				Landmark landmark = tile.Landmark;
				this.landmarkHash = ((landmark != null) ? landmark.def.shortHash : 0);
				this.hilliness = tile.hilliness;
				this.validForSettlement = TileFinder.IsValidTileForNewSettlement(tile.tile, null, false);
				Landmark landmark2 = tile.Landmark;
				this.comboLandmark = (landmark2 != null && landmark2.isComboLandmark);
				this.Valid = 1;
			}

			// Token: 0x06014E49 RID: 85577 RVA: 0x00642164 File Offset: 0x00640364
			public bool Equals(FastTileFinder.CachedTileData other)
			{
				return this.biomeHash == other.biomeHash && this.landmarkHash == other.landmarkHash && this.regionId == other.regionId && this.landmark == other.landmark && this.validForSettlement == other.validForSettlement && this.center.Equals(other.center) && this.hilliness == other.hilliness && this.passable == other.passable;
			}

			// Token: 0x06014E4A RID: 85578 RVA: 0x006421EC File Offset: 0x006403EC
			public override bool Equals(object obj)
			{
				if (obj is FastTileFinder.CachedTileData)
				{
					FastTileFinder.CachedTileData other = (FastTileFinder.CachedTileData)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x06014E4B RID: 85579 RVA: 0x00642211 File Offset: 0x00640411
			public override int GetHashCode()
			{
				return HashCode.Combine<ushort, ushort, int, bool, bool, float3, int, bool>(this.biomeHash, this.landmarkHash, this.regionId, this.landmark, this.validForSettlement, this.center, (int)this.hilliness, this.passable);
			}

			// Token: 0x0400E8DF RID: 59615
			public readonly ushort biomeHash;

			// Token: 0x0400E8E0 RID: 59616
			public readonly ushort landmarkHash;

			// Token: 0x0400E8E1 RID: 59617
			public readonly int regionId;

			// Token: 0x0400E8E2 RID: 59618
			public readonly bool landmark;

			// Token: 0x0400E8E3 RID: 59619
			public readonly bool validForSettlement;

			// Token: 0x0400E8E4 RID: 59620
			public readonly bool comboLandmark;

			// Token: 0x0400E8E5 RID: 59621
			public readonly float3 center;

			// Token: 0x0400E8E6 RID: 59622
			public readonly Hilliness hilliness;

			// Token: 0x0400E8E7 RID: 59623
			public readonly bool passable;
		}

		// Token: 0x02003C29 RID: 15401
		[BurstCompile]
		private struct ComputeQueryJob : IJobParallelFor
		{
			// Token: 0x06014E4C RID: 85580 RVA: 0x00642248 File Offset: 0x00640448
			[BurstCompile]
			public void Execute(int i)
			{
				FastTileFinder.CachedTileData cachedTileData = this.tiles[i];
				if (this.countNormal > this.maximum && !this.closestResults.IsCreated)
				{
					return;
				}
				if (this.IsValidQuery(this.normal, cachedTileData))
				{
					int num = Interlocked.Increment(ref this.countNormal);
					if (num <= this.maximum)
					{
						this.normalResult[num - 1] = new PlanetTile(i, this.layerId);
					}
					if (this.closest)
					{
						this.CheckClosest(i);
					}
				}
				if (this.countDesperate > this.maximum || !this.desperate.Valid)
				{
					return;
				}
				if (this.IsValidQuery(this.desperate, cachedTileData))
				{
					int num2 = Interlocked.Increment(ref this.countDesperate);
					if (num2 <= this.maximum)
					{
						this.desperateResult[num2 - 1] = new PlanetTile(i, this.layerId);
					}
					if (this.closest)
					{
						this.CheckClosest(i);
					}
				}
			}

			// Token: 0x06014E4D RID: 85581 RVA: 0x00642338 File Offset: 0x00640538
			private void CheckClosest(int index)
			{
				float num = math.distancesq(this.tiles[index].center, this.vOrigin);
				if (this.closestResults[this.threadIndex] == -1 || num < this.closestSqrDists[this.threadIndex])
				{
					this.closestResults[this.threadIndex] = index;
					this.closestSqrDists[this.threadIndex] = num;
				}
			}

			// Token: 0x06014E4E RID: 85582 RVA: 0x006423B0 File Offset: 0x006405B0
			private bool IsValidQuery(in FastTileFinder.TileQueryParams query, in FastTileFinder.CachedTileData tile)
			{
				return (!query.reachable || this.region < 0 || tile.regionId == this.region) && (query.minHilliness == Hilliness.Undefined || tile.hilliness >= query.minHilliness) && (query.maxHilliness == Hilliness.Undefined || tile.hilliness <= query.maxHilliness) && (!query.validSettlement || tile.validForSettlement) && this.IsValidLandmark(query, tile) && (!query.checkBiome || this.IsValidBiome(tile)) && this.IsValidDistance(query, tile);
			}

			// Token: 0x06014E4F RID: 85583 RVA: 0x0064244C File Offset: 0x0064064C
			private bool IsValidDistance(in FastTileFinder.TileQueryParams query, in FastTileFinder.CachedTileData tile)
			{
				float num = FastTileFinder.ComputeQueryJob.SphericalDistance(this.vOrigin, tile.center);
				num = num * this.layerRadius / this.averageTileSize;
				return num >= query.minDistTiles && num <= query.maxDistTiles;
			}

			// Token: 0x06014E50 RID: 85584 RVA: 0x00642494 File Offset: 0x00640694
			private bool IsValidLandmark(in FastTileFinder.TileQueryParams query, in FastTileFinder.CachedTileData tile)
			{
				if (query.landmarkMode == FastTileFinder.LandmarkMode.Any)
				{
					return true;
				}
				if (!query.comboLandmarks && tile.comboLandmark)
				{
					return false;
				}
				if (this.landmarks.Length > 0)
				{
					bool flag = false;
					for (int i = 0; i < this.landmarks.Length; i++)
					{
						if (this.landmarks[i] == tile.landmarkHash)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return tile.landmark == (query.landmarkMode == FastTileFinder.LandmarkMode.Required);
			}

			// Token: 0x06014E51 RID: 85585 RVA: 0x00642514 File Offset: 0x00640714
			private bool IsValidBiome(in FastTileFinder.CachedTileData tile)
			{
				if (this.biomes.Length <= 0)
				{
					return true;
				}
				bool result = false;
				for (int i = 0; i < this.biomes.Length; i++)
				{
					if (this.biomes[i] == tile.biomeHash)
					{
						result = true;
						break;
					}
				}
				return result;
			}

			// Token: 0x06014E52 RID: 85586 RVA: 0x00642562 File Offset: 0x00640762
			[BurstCompile]
			private static float SphericalDistance(in float3 a, in float3 b)
			{
				return FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.Invoke(a, b);
			}

			// Token: 0x06014E53 RID: 85587 RVA: 0x0064256C File Offset: 0x0064076C
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static float SphericalDistance$BurstManaged(in float3 a, in float3 b)
			{
				if (math.lengthsq(a - b) < 0.0001f)
				{
					return 0f;
				}
				return math.acos(math.dot(math.normalize(a), math.normalize(b)));
			}

			// Token: 0x0400E8E9 RID: 59625
			[NativeSetThreadIndex]
			[ReadOnly]
			private int threadIndex;

			// Token: 0x0400E8EA RID: 59626
			[ReadOnly]
			public NativeArray<FastTileFinder.CachedTileData> tiles;

			// Token: 0x0400E8EB RID: 59627
			[ReadOnly]
			public NativeArray<ushort> biomes;

			// Token: 0x0400E8EC RID: 59628
			[ReadOnly]
			public NativeArray<ushort> landmarks;

			// Token: 0x0400E8ED RID: 59629
			[ReadOnly]
			public FastTileFinder.TileQueryParams normal;

			// Token: 0x0400E8EE RID: 59630
			[ReadOnly]
			public FastTileFinder.TileQueryParams desperate;

			// Token: 0x0400E8EF RID: 59631
			[ReadOnly]
			public int layerId;

			// Token: 0x0400E8F0 RID: 59632
			[ReadOnly]
			public float averageTileSize;

			// Token: 0x0400E8F1 RID: 59633
			[ReadOnly]
			public float layerRadius;

			// Token: 0x0400E8F2 RID: 59634
			[ReadOnly]
			public float3 vOrigin;

			// Token: 0x0400E8F3 RID: 59635
			[ReadOnly]
			public int region;

			// Token: 0x0400E8F4 RID: 59636
			[ReadOnly]
			public bool closest;

			// Token: 0x0400E8F5 RID: 59637
			public int countNormal;

			// Token: 0x0400E8F6 RID: 59638
			public int countDesperate;

			// Token: 0x0400E8F7 RID: 59639
			[ReadOnly]
			public int maximum;

			// Token: 0x0400E8F8 RID: 59640
			[NativeDisableParallelForRestriction]
			public NativeArray<PlanetTile> normalResult;

			// Token: 0x0400E8F9 RID: 59641
			[NativeDisableParallelForRestriction]
			public NativeArray<PlanetTile> desperateResult;

			// Token: 0x0400E8FA RID: 59642
			[NativeDisableParallelForRestriction]
			public NativeArray<int> closestResults;

			// Token: 0x0400E8FB RID: 59643
			[NativeDisableParallelForRestriction]
			public NativeArray<float> closestSqrDists;

			// Token: 0x02003C2A RID: 15402
			// (Invoke) Token: 0x06014E55 RID: 85589
			public delegate float SphericalDistance_00014E3B$PostfixBurstDelegate(in float3 a, in float3 b);

			// Token: 0x02003C2B RID: 15403
			internal static class SphericalDistance_00014E3B$BurstDirectCall
			{
				// Token: 0x06014E58 RID: 85592 RVA: 0x006425BC File Offset: 0x006407BC
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.Pointer == 0)
					{
						FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.DeferredCompilation, methodof(FastTileFinder.ComputeQueryJob.SphericalDistance$BurstManaged(float3*, float3*)).MethodHandle, typeof(FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.Pointer;
				}

				// Token: 0x06014E59 RID: 85593 RVA: 0x006425E8 File Offset: 0x006407E8
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x06014E5A RID: 85594 RVA: 0x00642600 File Offset: 0x00640800
				public unsafe static void Constructor()
				{
					FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(FastTileFinder.ComputeQueryJob.SphericalDistance(float3*, float3*)).MethodHandle);
				}

				// Token: 0x06014E5B RID: 85595 RVA: 0x000026EA File Offset: 0x000008EA
				public static void Initialize()
				{
				}

				// Token: 0x06014E5C RID: 85596 RVA: 0x00642611 File Offset: 0x00640811
				// Note: this type is marked as 'beforefieldinit'.
				static SphericalDistance_00014E3B$BurstDirectCall()
				{
					FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.Constructor();
				}

				// Token: 0x06014E5D RID: 85597 RVA: 0x00642618 File Offset: 0x00640818
				public static float Invoke(in float3 a, in float3 b)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = FastTileFinder.ComputeQueryJob.SphericalDistance_00014E3B$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Single(Unity.Mathematics.float3&,Unity.Mathematics.float3&), ref a, ref b, functionPointer);
						}
					}
					return FastTileFinder.ComputeQueryJob.SphericalDistance$BurstManaged(a, b);
				}

				// Token: 0x0400E8FC RID: 59644
				private static IntPtr Pointer;

				// Token: 0x0400E8FD RID: 59645
				private static IntPtr DeferredCompilation;
			}
		}
	}
}
