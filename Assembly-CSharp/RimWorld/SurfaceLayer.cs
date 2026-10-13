using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002837 RID: 10295
	public class SurfaceLayer : PlanetLayer
	{
		// Token: 0x17002258 RID: 8792
		// (get) Token: 0x0600E13B RID: 57659 RVA: 0x00420C31 File Offset: 0x0041EE31
		public bool HasWorldData
		{
			get
			{
				return this.tiles.Count > 0;
			}
		}

		// Token: 0x17002259 RID: 8793
		public SurfaceTile this[int tileID]
		{
			get
			{
				if ((ulong)tileID >= (ulong)((long)base.TilesCount))
				{
					return null;
				}
				return (SurfaceTile)this.tiles[tileID];
			}
		}

		// Token: 0x1700225A RID: 8794
		public SurfaceTile this[PlanetTile tile]
		{
			get
			{
				return this[tile.tileId];
			}
		}

		// Token: 0x0600E13E RID: 57662 RVA: 0x00420BC4 File Offset: 0x0041EDC4
		public SurfaceLayer()
		{
		}

		// Token: 0x0600E13F RID: 57663 RVA: 0x00420C70 File Offset: 0x0041EE70
		public SurfaceLayer(int layerId, PlanetLayerDef def, float radius, Vector3 origin, Vector3 viewCenter, float viewAngle, int subdivisions, float extraCameraAltitude, float backgroundWorldCameraOffset, float backgroundWorldCameraParallaxDistance) : base(layerId, def, radius, origin, viewCenter, viewAngle, subdivisions, extraCameraAltitude, backgroundWorldCameraOffset, backgroundWorldCameraParallaxDistance)
		{
		}

		// Token: 0x0600E140 RID: 57664 RVA: 0x00420C94 File Offset: 0x0041EE94
		internal override void ExposeBody()
		{
			base.ExposeBody();
			DataExposeUtility.LookByteArray(ref this.tileElevation, "tileElevation");
			DataExposeUtility.LookByteArray(ref this.tileHilliness, "tileHilliness");
			DataExposeUtility.LookByteArray(ref this.tileTemperature, "tileTemperature");
			DataExposeUtility.LookByteArray(ref this.tileRainfall, "tileRainfall");
			DataExposeUtility.LookByteArray(ref this.tileSwampiness, "tileSwampiness");
			DataExposeUtility.LookByteArray(ref this.tileFeature, "tileFeature");
			DataExposeUtility.LookByteArray(ref this.tilePollution, "tilePollution");
			DataExposeUtility.LookByteArray(ref this.tileRoadOrigins, "tileRoadOrigins");
			DataExposeUtility.LookByteArray(ref this.tileRoadAdjacency, "tileRoadAdjacency");
			DataExposeUtility.LookByteArray(ref this.tileRoadDef, "tileRoadDef");
			DataExposeUtility.LookByteArray(ref this.tileRiverOrigins, "tileRiverOrigins");
			DataExposeUtility.LookByteArray(ref this.tileRiverAdjacency, "tileRiverAdjacency");
			DataExposeUtility.LookByteArray(ref this.tileRiverDef, "tileRiverDef");
			DataExposeUtility.LookByteArray(ref this.tileRiverDistances, "tileRiverDistances");
			DataExposeUtility.LookByteArray(ref this.tileMutatorTiles, "tileMutatorTiles");
			DataExposeUtility.LookByteArray(ref this.tileMutatorDefs, "tileMutatorDefs");
		}

		// Token: 0x0600E141 RID: 57665 RVA: 0x00420DA8 File Offset: 0x0041EFA8
		protected override void TilesToRawData()
		{
			base.TilesToRawData();
			this.tileElevation = DataSerializeUtility.SerializeUshort(base.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((this.tiles[i].WaterCovered ? this.tiles[i].elevation : Mathf.Max(this.tiles[i].elevation, 1f)) + 8192f), 0, 65535));
			this.tileHilliness = DataSerializeUtility.SerializeByte(base.TilesCount, (int i) => (byte)this.tiles[i].hilliness);
			this.tileTemperature = DataSerializeUtility.SerializeUshort(base.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt((this.tiles[i].temperature + 300f) * 10f), 0, 65535));
			this.tileRainfall = DataSerializeUtility.SerializeUshort(base.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].rainfall), 0, 65535));
			this.tileSwampiness = DataSerializeUtility.SerializeByte(base.TilesCount, (int i) => (byte)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].swampiness * 255f), 0, 255));
			this.tileFeature = DataSerializeUtility.SerializeUshort(base.TilesCount, delegate(int i)
			{
				if (this.tiles[i].feature != null)
				{
					return (ushort)this.tiles[i].feature.uniqueID;
				}
				return ushort.MaxValue;
			});
			this.tilePollution = DataSerializeUtility.SerializeUshort(base.TilesCount, (int i) => (ushort)Mathf.Clamp(Mathf.RoundToInt(this.tiles[i].pollution * 65535f), 0, 65535));
			this.tileRiverDistances = DataSerializeUtility.SerializeByte(base.TilesCount, (int i) => (byte)this[i].riverDist);
			this.SerializeRoads();
			this.SerializeRivers();
			this.SerializeMutators();
		}

		// Token: 0x0600E142 RID: 57666 RVA: 0x00420EB8 File Offset: 0x0041F0B8
		private void SerializeMutators()
		{
			List<ushort> list = new List<ushort>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < base.TilesCount; i++)
			{
				List<TileMutatorDef> list3 = this.tiles[i].Mutators.ToList<TileMutatorDef>();
				for (int j = 0; j < list3.Count; j++)
				{
					list.Add(list3[j].shortHash);
					list2.Add(i);
				}
			}
			this.tileMutatorDefs = DataSerializeUtility.SerializeUshort(list.ToArray());
			this.tileMutatorTiles = DataSerializeUtility.SerializeInt(list2.ToArray());
		}

		// Token: 0x0600E143 RID: 57667 RVA: 0x00420F4C File Offset: 0x0041F14C
		private void SerializeRivers()
		{
			List<int> list = new List<int>();
			List<byte> list2 = new List<byte>();
			List<ushort> list3 = new List<ushort>();
			for (int i = 0; i < base.TilesCount; i++)
			{
				List<SurfaceTile.RiverLink> potentialRivers = this[i].potentialRivers;
				if (potentialRivers != null)
				{
					for (int j = 0; j < potentialRivers.Count; j++)
					{
						SurfaceTile.RiverLink riverLink = potentialRivers[j];
						if (riverLink.neighbor.tileId >= i)
						{
							int neighborId = base.GetNeighborId(i, riverLink.neighbor.tileId);
							if (neighborId < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for river piece", 81637014);
							}
							else
							{
								list.Add(i);
								list2.Add((byte)neighborId);
								list3.Add(riverLink.river.shortHash);
							}
						}
					}
				}
			}
			this.tileRiverOrigins = DataSerializeUtility.SerializeInt(list.ToArray());
			this.tileRiverAdjacency = DataSerializeUtility.SerializeByte(list2.ToArray());
			this.tileRiverDef = DataSerializeUtility.SerializeUshort(list3.ToArray());
		}

		// Token: 0x0600E144 RID: 57668 RVA: 0x00421044 File Offset: 0x0041F244
		private void SerializeRoads()
		{
			List<int> list = new List<int>();
			List<byte> list2 = new List<byte>();
			List<ushort> list3 = new List<ushort>();
			for (int i = 0; i < base.TilesCount; i++)
			{
				List<SurfaceTile.RoadLink> potentialRoads = this[i].potentialRoads;
				if (potentialRoads != null)
				{
					for (int j = 0; j < potentialRoads.Count; j++)
					{
						SurfaceTile.RoadLink roadLink = potentialRoads[j];
						if (roadLink.neighbor.tileId >= i)
						{
							int neighborId = base.GetNeighborId(i, roadLink.neighbor.tileId);
							if (neighborId < 0)
							{
								Log.ErrorOnce("Couldn't find valid neighbor for road piece", 81637014);
							}
							else
							{
								list.Add(i);
								list2.Add((byte)neighborId);
								list3.Add(roadLink.road.shortHash);
							}
						}
					}
				}
			}
			this.tileRoadOrigins = DataSerializeUtility.SerializeInt(list.ToArray());
			this.tileRoadAdjacency = DataSerializeUtility.SerializeByte(list2.ToArray());
			this.tileRoadDef = DataSerializeUtility.SerializeUshort(list3.ToArray());
		}

		// Token: 0x0600E145 RID: 57669 RVA: 0x0042113C File Offset: 0x0041F33C
		protected override void RawDataToTiles()
		{
			bool flag = this.tiles.Count == base.TilesCount;
			base.RawDataToTiles();
			if (flag)
			{
				for (int k = 0; k < base.TilesCount; k++)
				{
					this[k].potentialRoads = null;
					this[k].potentialRivers = null;
				}
			}
			DataSerializeUtility.LoadUshort(this.tileElevation, base.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].elevation = (float)(data - 8192);
			});
			DataSerializeUtility.LoadByte(this.tileHilliness, base.TilesCount, delegate(int i, byte data)
			{
				this.tiles[i].hilliness = (Hilliness)data;
			});
			DataSerializeUtility.LoadUshort(this.tileTemperature, base.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].temperature = (float)data / 10f - 300f;
			});
			DataSerializeUtility.LoadUshort(this.tileRainfall, base.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].rainfall = (float)data;
			});
			DataSerializeUtility.LoadByte(this.tileSwampiness, base.TilesCount, delegate(int i, byte data)
			{
				this.tiles[i].swampiness = (float)data / 255f;
			});
			DataSerializeUtility.LoadUshort(this.tilePollution, base.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].pollution = (float)data / 65535f;
			});
			DataSerializeUtility.LoadByte(this.tileRiverDistances, base.TilesCount, delegate(int i, byte data)
			{
				this[i].riverDist = (int)data;
			});
			for (int j = 0; j < base.TilesCount; j++)
			{
				this.tiles[j].tile = new PlanetTile(j, base.LayerID);
			}
			this.DeserializeRoads();
			this.DeserializeRivers();
			this.DeserializeMutators();
		}

		// Token: 0x0600E146 RID: 57670 RVA: 0x0042129C File Offset: 0x0041F49C
		private void DeserializeMutators()
		{
			if (this.tileMutatorDefs == null || this.tileMutatorTiles == null)
			{
				return;
			}
			ushort[] array = DataSerializeUtility.DeserializeUshort(this.tileMutatorDefs);
			int[] array2 = DataSerializeUtility.DeserializeInt(this.tileMutatorTiles);
			int num = -1;
			for (int i = 0; i < array.Length; i++)
			{
				TileMutatorDef byShortHash = DefDatabase<TileMutatorDef>.GetByShortHash(array[i]);
				int num2 = array2[i];
				if (byShortHash != null)
				{
					if (this.tiles[num2].mutatorsNullable == null)
					{
						this.tiles[num2].mutatorsNullable = new List<TileMutatorDef>();
					}
					if (num2 != num)
					{
						this.tiles[num2].mutatorsNullable.Clear();
					}
					this.tiles[num2].mutatorsNullable.Add(byShortHash);
				}
				num = num2;
			}
		}

		// Token: 0x0600E147 RID: 57671 RVA: 0x00421358 File Offset: 0x0041F558
		private void DeserializeRivers()
		{
			if (this.tileRiverOrigins == null || this.tileRiverAdjacency == null || this.tileRiverDef == null)
			{
				return;
			}
			int[] array = DataSerializeUtility.DeserializeInt(this.tileRiverOrigins);
			byte[] array2 = DataSerializeUtility.DeserializeByte(this.tileRiverAdjacency);
			ushort[] array3 = DataSerializeUtility.DeserializeUshort(this.tileRiverDef);
			for (int i = 0; i < array.Length; i++)
			{
				PlanetTile planetTile = new PlanetTile(array[i], base.LayerID);
				PlanetTile tileNeighbor = base.GetTileNeighbor(planetTile, (int)array2[i]);
				RiverDef byShortHash = DefDatabase<RiverDef>.GetByShortHash(array3[i]);
				if (byShortHash != null)
				{
					if (this[planetTile].potentialRivers == null)
					{
						this[planetTile].potentialRivers = new List<SurfaceTile.RiverLink>();
					}
					if (this[tileNeighbor].potentialRivers == null)
					{
						this[tileNeighbor].potentialRivers = new List<SurfaceTile.RiverLink>();
					}
					this[planetTile].potentialRivers.Add(new SurfaceTile.RiverLink
					{
						neighbor = tileNeighbor,
						river = byShortHash
					});
					this[tileNeighbor].potentialRivers.Add(new SurfaceTile.RiverLink
					{
						neighbor = planetTile,
						river = byShortHash
					});
				}
			}
		}

		// Token: 0x0600E148 RID: 57672 RVA: 0x00421484 File Offset: 0x0041F684
		private void DeserializeRoads()
		{
			if (this.tileRoadOrigins == null || this.tileRoadAdjacency == null || this.tileRoadDef == null)
			{
				return;
			}
			int[] array = DataSerializeUtility.DeserializeInt(this.tileRoadOrigins);
			byte[] array2 = DataSerializeUtility.DeserializeByte(this.tileRoadAdjacency);
			ushort[] array3 = DataSerializeUtility.DeserializeUshort(this.tileRoadDef);
			for (int i = 0; i < array.Length; i++)
			{
				PlanetTile planetTile = new PlanetTile(array[i], base.LayerID);
				PlanetTile tileNeighbor = base.GetTileNeighbor(planetTile, (int)array2[i]);
				RoadDef byShortHash = DefDatabase<RoadDef>.GetByShortHash(array3[i]);
				if (byShortHash != null)
				{
					if (this[planetTile].potentialRoads == null)
					{
						this[planetTile].potentialRoads = new List<SurfaceTile.RoadLink>();
					}
					if (this[tileNeighbor].potentialRoads == null)
					{
						this[tileNeighbor].potentialRoads = new List<SurfaceTile.RoadLink>();
					}
					this[planetTile].potentialRoads.Add(new SurfaceTile.RoadLink
					{
						neighbor = tileNeighbor,
						road = byShortHash
					});
					this[tileNeighbor].potentialRoads.Add(new SurfaceTile.RoadLink
					{
						neighbor = planetTile,
						road = byShortHash
					});
				}
			}
		}

		// Token: 0x0400976C RID: 38764
		private byte[] tileElevation;

		// Token: 0x0400976D RID: 38765
		private byte[] tileHilliness;

		// Token: 0x0400976E RID: 38766
		private byte[] tileTemperature;

		// Token: 0x0400976F RID: 38767
		private byte[] tileRainfall;

		// Token: 0x04009770 RID: 38768
		private byte[] tileSwampiness;

		// Token: 0x04009771 RID: 38769
		private byte[] tilePollution;

		// Token: 0x04009772 RID: 38770
		public byte[] tileFeature;

		// Token: 0x04009773 RID: 38771
		private byte[] tileRoadOrigins;

		// Token: 0x04009774 RID: 38772
		private byte[] tileRoadAdjacency;

		// Token: 0x04009775 RID: 38773
		private byte[] tileRoadDef;

		// Token: 0x04009776 RID: 38774
		private byte[] tileRiverOrigins;

		// Token: 0x04009777 RID: 38775
		private byte[] tileRiverAdjacency;

		// Token: 0x04009778 RID: 38776
		private byte[] tileRiverDef;

		// Token: 0x04009779 RID: 38777
		private byte[] tileRiverDistances;

		// Token: 0x0400977A RID: 38778
		private byte[] tileMutatorTiles;

		// Token: 0x0400977B RID: 38779
		private byte[] tileMutatorDefs;

		// Token: 0x0400977C RID: 38780
		private const int ElevationOffset = 8192;

		// Token: 0x0400977D RID: 38781
		private const int TemperatureOffset = 300;

		// Token: 0x0400977E RID: 38782
		private const float TemperatureMultiplier = 10f;
	}
}
