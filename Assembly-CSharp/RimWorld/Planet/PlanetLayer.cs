using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CDC RID: 15580
	[BurstCompile]
	public class PlanetLayer : IExposable, ILoadReferenceable, IDisposable
	{
		// Token: 0x060152EB RID: 86763 RVA: 0x00656FCB File Offset: 0x006551CB
		public string GetUniqueLoadID()
		{
			return string.Format("PlanetLayer_{0}", this.layerId);
		}

		// Token: 0x170034C9 RID: 13513
		// (get) Token: 0x060152EC RID: 86764 RVA: 0x00656FE2 File Offset: 0x006551E2
		public List<WorldDrawLayer> WorldDrawLayers { get; } = new List<WorldDrawLayer>();

		// Token: 0x170034CA RID: 13514
		// (get) Token: 0x060152ED RID: 86765 RVA: 0x00656FEA File Offset: 0x006551EA
		public FastTileFinder FastTileFinder
		{
			get
			{
				return this.finder;
			}
		}

		// Token: 0x170034CB RID: 13515
		// (get) Token: 0x060152EE RID: 86766 RVA: 0x00656FF2 File Offset: 0x006551F2
		public WorldPathing Pather
		{
			get
			{
				return this.pather;
			}
		}

		// Token: 0x170034CC RID: 13516
		// (get) Token: 0x060152EF RID: 86767 RVA: 0x00656FFA File Offset: 0x006551FA
		public WorldFloodFiller Filler
		{
			get
			{
				return this.filler;
			}
		}

		// Token: 0x170034CD RID: 13517
		// (get) Token: 0x060152F0 RID: 86768 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170034CE RID: 13518
		// (get) Token: 0x060152F1 RID: 86769 RVA: 0x00657002 File Offset: 0x00655202
		public bool IsSelected
		{
			get
			{
				return PlanetLayer.Selected == this;
			}
		}

		// Token: 0x170034CF RID: 13519
		// (get) Token: 0x060152F2 RID: 86770 RVA: 0x0065700C File Offset: 0x0065520C
		public int LayerID
		{
			get
			{
				return this.layerId;
			}
		}

		// Token: 0x170034D0 RID: 13520
		// (get) Token: 0x060152F3 RID: 86771 RVA: 0x00657014 File Offset: 0x00655214
		public PlanetLayerDef Def
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x170034D1 RID: 13521
		// (get) Token: 0x060152F4 RID: 86772 RVA: 0x0065701C File Offset: 0x0065521C
		public List<Tile> Tiles
		{
			get
			{
				return this.tiles;
			}
		}

		// Token: 0x170034D2 RID: 13522
		// (get) Token: 0x060152F5 RID: 86773 RVA: 0x00657024 File Offset: 0x00655224
		public int TilesCount
		{
			get
			{
				return this.tileIDToNeighbors_offsets.Length;
			}
		}

		// Token: 0x170034D3 RID: 13523
		// (get) Token: 0x060152F6 RID: 86774 RVA: 0x00657031 File Offset: 0x00655231
		public bool IsRootSurface
		{
			get
			{
				return this.isRootSurface;
			}
		}

		// Token: 0x170034D4 RID: 13524
		// (get) Token: 0x060152F7 RID: 86775 RVA: 0x00657039 File Offset: 0x00655239
		public NativeArray<Vector3> UnsafeVerts
		{
			get
			{
				return this.verts;
			}
		}

		// Token: 0x170034D5 RID: 13525
		// (get) Token: 0x060152F8 RID: 86776 RVA: 0x00657041 File Offset: 0x00655241
		public NativeArray<int> UnsafeTileIDToVerts_offsets
		{
			get
			{
				return this.tileIDToVerts_offsets;
			}
		}

		// Token: 0x170034D6 RID: 13526
		// (get) Token: 0x060152F9 RID: 86777 RVA: 0x00657049 File Offset: 0x00655249
		public NativeArray<int> UnsafeTileIDToNeighbors_offsets
		{
			get
			{
				return this.tileIDToNeighbors_offsets;
			}
		}

		// Token: 0x170034D7 RID: 13527
		// (get) Token: 0x060152FA RID: 86778 RVA: 0x00657051 File Offset: 0x00655251
		public NativeArray<PlanetTile> UnsafeTileIDToNeighbors_values
		{
			get
			{
				return this.tileIDToNeighbors_values;
			}
		}

		// Token: 0x170034D8 RID: 13528
		// (get) Token: 0x060152FB RID: 86779 RVA: 0x00657059 File Offset: 0x00655259
		public virtual Vector3 Origin
		{
			get
			{
				return this.origin;
			}
		}

		// Token: 0x170034D9 RID: 13529
		// (get) Token: 0x060152FC RID: 86780 RVA: 0x00657061 File Offset: 0x00655261
		public Vector3 NorthPolePos
		{
			get
			{
				return new Vector3(0f, this.Radius, 0f);
			}
		}

		// Token: 0x170034DA RID: 13530
		// (get) Token: 0x060152FD RID: 86781 RVA: 0x00657078 File Offset: 0x00655278
		public float AverageTileSize
		{
			get
			{
				return this.averageTileSize;
			}
		}

		// Token: 0x170034DB RID: 13531
		// (get) Token: 0x060152FE RID: 86782 RVA: 0x00657080 File Offset: 0x00655280
		public float Radius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x170034DC RID: 13532
		// (get) Token: 0x060152FF RID: 86783 RVA: 0x00657088 File Offset: 0x00655288
		public float ExtraCameraAltitude
		{
			get
			{
				return this.extraCameraAltitude;
			}
		}

		// Token: 0x170034DD RID: 13533
		// (get) Token: 0x06015300 RID: 86784 RVA: 0x00657090 File Offset: 0x00655290
		public float BackgroundWorldCameraOffset
		{
			get
			{
				return this.backgroundWorldCameraOffset;
			}
		}

		// Token: 0x170034DE RID: 13534
		// (get) Token: 0x06015301 RID: 86785 RVA: 0x00657098 File Offset: 0x00655298
		public float BackgroundWorldCameraParallaxDistancePer100Cells
		{
			get
			{
				return this.backgroundWorldCameraParallaxDistancePer100Cells;
			}
		}

		// Token: 0x170034DF RID: 13535
		// (get) Token: 0x06015302 RID: 86786 RVA: 0x006570A0 File Offset: 0x006552A0
		public Vector3 ViewCenter
		{
			get
			{
				return this.viewCenter;
			}
		}

		// Token: 0x170034E0 RID: 13536
		// (get) Token: 0x06015303 RID: 86787 RVA: 0x006570A8 File Offset: 0x006552A8
		public float ViewAngle
		{
			get
			{
				return this.viewAngle;
			}
		}

		// Token: 0x170034E1 RID: 13537
		public Tile this[int tileID]
		{
			get
			{
				if ((ulong)tileID >= (ulong)((long)this.TilesCount))
				{
					return null;
				}
				return this.tiles[tileID];
			}
		}

		// Token: 0x170034E2 RID: 13538
		public Tile this[PlanetTile tile]
		{
			get
			{
				return this[tile.tileId];
			}
		}

		// Token: 0x06015306 RID: 86790 RVA: 0x006570D9 File Offset: 0x006552D9
		public PlanetTile PlanetTileForID(int tileID)
		{
			return new PlanetTile(tileID, this.layerId);
		}

		// Token: 0x170034E3 RID: 13539
		// (get) Token: 0x06015307 RID: 86791 RVA: 0x006570E7 File Offset: 0x006552E7
		public virtual bool Raycastable
		{
			get
			{
				return this.def.alwaysRaycastable || PlanetLayer.Selected == this;
			}
		}

		// Token: 0x170034E4 RID: 13540
		// (get) Token: 0x06015308 RID: 86792 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanReachLayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170034E5 RID: 13541
		// (get) Token: 0x06015309 RID: 86793 RVA: 0x00657100 File Offset: 0x00655300
		// (set) Token: 0x0601530A RID: 86794 RVA: 0x00657108 File Offset: 0x00655308
		public string ScenarioTag
		{
			get
			{
				return this.scenarioTag;
			}
			set
			{
				this.scenarioTag = value;
			}
		}

		// Token: 0x170034E6 RID: 13542
		// (get) Token: 0x0601530B RID: 86795 RVA: 0x00657111 File Offset: 0x00655311
		// (set) Token: 0x0601530C RID: 86796 RVA: 0x0065711D File Offset: 0x0065531D
		public static PlanetLayer Selected
		{
			get
			{
				return Find.WorldSelector.SelectedLayer;
			}
			set
			{
				Find.WorldSelector.SelectedLayer = value;
			}
		}

		// Token: 0x0601530D RID: 86797 RVA: 0x0065712A File Offset: 0x0065532A
		public PlanetLayer()
		{
		}

		// Token: 0x0601530E RID: 86798 RVA: 0x0065715C File Offset: 0x0065535C
		public PlanetLayer(int layerId, PlanetLayerDef def, float radius, Vector3 origin, Vector3 viewCenter, float viewAngle, int subdivisions, float extraCameraAltitude, float backgroundWorldCameraOffset, float backgroundWorldCameraParallaxDistancePer100Cells)
		{
			this.layerId = layerId;
			this.def = def;
			this.radius = radius;
			this.origin = origin;
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.subdivisions = subdivisions;
			this.extraCameraAltitude = extraCameraAltitude;
			this.backgroundWorldCameraOffset = backgroundWorldCameraOffset;
			this.backgroundWorldCameraParallaxDistancePer100Cells = backgroundWorldCameraParallaxDistancePer100Cells;
		}

		// Token: 0x0601530F RID: 86799 RVA: 0x006571E4 File Offset: 0x006553E4
		public void MarkRootSurface()
		{
			this.isRootSurface = true;
		}

		// Token: 0x06015310 RID: 86800 RVA: 0x006571F0 File Offset: 0x006553F0
		public void AddConnection(PlanetLayer target, float cost)
		{
			if (this.connections.ContainsKey(target))
			{
				Log.Error(string.Format("Attempted to add a layer connection ({0}) which already exists.", target));
				return;
			}
			this.connections[target] = new PlanetLayerConnection
			{
				origin = this,
				target = target,
				fuelCost = cost
			};
		}

		// Token: 0x06015311 RID: 86801 RVA: 0x00657242 File Offset: 0x00655442
		public bool HasConnectionFromTo(PlanetLayer layer)
		{
			return this.connections.ContainsKey(layer);
		}

		// Token: 0x06015312 RID: 86802 RVA: 0x00657250 File Offset: 0x00655450
		public PlanetLayerConnection GetConnectionFromTo(PlanetLayer layer)
		{
			return this.connections[layer];
		}

		// Token: 0x06015313 RID: 86803 RVA: 0x0065725E File Offset: 0x0065545E
		public bool TryGetConnectionFromTo(PlanetLayer layer, out PlanetLayerConnection connection)
		{
			return this.connections.TryGetValue(layer, out connection);
		}

		// Token: 0x06015314 RID: 86804 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport CanSelectLayer()
		{
			return true;
		}

		// Token: 0x06015315 RID: 86805 RVA: 0x0065726D File Offset: 0x0065546D
		public void RunWorldGeneration()
		{
			this.RunWorldGeneration(Find.World.info.seedString, Rand.Int);
		}

		// Token: 0x06015316 RID: 86806 RVA: 0x00657289 File Offset: 0x00655489
		public void RunWorldGeneration(int seed)
		{
			this.RunWorldGeneration(Find.World.info.seedString, seed);
		}

		// Token: 0x06015317 RID: 86807 RVA: 0x006572A1 File Offset: 0x006554A1
		public void RunWorldGeneration(string seedString, int seed)
		{
			WorldGenerator.GeneratePlanetLayer(this, seedString, seed);
		}

		// Token: 0x06015318 RID: 86808 RVA: 0x006572AC File Offset: 0x006554AC
		public void InitializeLayer()
		{
			PlanetLayer.tmp_tris.Clear();
			PlanetLayer.tmp_verts.Clear();
			this.Dispose();
			IcosahedronGenerator.GenerateIcosahedron(PlanetLayer.tmp_verts, PlanetLayer.tmp_tris, this.Radius, this.viewCenter, this.viewAngle);
			for (int i = 0; i < this.subdivisions + 1; i++)
			{
				bool lastPass = i == this.subdivisions;
				this.Subdivide(lastPass);
			}
			this.verts = NativeArrayUtility.GetNativeArrayCopy<Vector3>(PlanetLayer.tmp_generatedVerts, Allocator.Persistent, false);
			this.tileIDToVerts_offsets = NativeArrayUtility.GetNativeArrayCopy<int>(PlanetLayer.tmp_generatedVertOffsets, Allocator.Persistent, false);
			this.CalculateTileNeighbors();
			PlanetLayer.ClearAndDeallocateWorkingLists();
			this.averageTileSize = PlanetLayer.CalculateAverageTileSize(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, this.tileIDToVerts_offsets, this.verts);
			this.WorldDrawLayers.Clear();
			foreach (Type type in this.def.worldDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = (WorldDrawLayer)Activator.CreateInstance(type);
				worldDrawLayer.Initialize(this);
				this.WorldDrawLayers.Add(worldDrawLayer);
			}
			this.finder = new FastTileFinder(this);
			this.pather = new WorldPathing(this);
			this.filler = new WorldFloodFiller(this);
		}

		// Token: 0x06015319 RID: 86809 RVA: 0x006573FC File Offset: 0x006555FC
		private void Subdivide(bool lastPass)
		{
			PackedListOfLists.GenerateVertToTrisPackedList(PlanetLayer.tmp_verts, PlanetLayer.tmp_tris, PlanetLayer.tmp_vertToTris_offsets, PlanetLayer.tmp_vertToTris_values);
			int count = PlanetLayer.tmp_verts.Count;
			int i = 0;
			int count2 = PlanetLayer.tmp_tris.Count;
			while (i < count2)
			{
				TriangleIndices triangleIndices = PlanetLayer.tmp_tris[i];
				Vector3 vector = (PlanetLayer.tmp_verts[triangleIndices.v1] + PlanetLayer.tmp_verts[triangleIndices.v2] + PlanetLayer.tmp_verts[triangleIndices.v3]) / 3f;
				PlanetLayer.tmp_verts.Add(vector.normalized * this.Radius);
				i++;
			}
			PlanetLayer.tmp_generatedTris.Clear();
			if (lastPass)
			{
				PlanetLayer.tmp_vertToTileIDs_offsets.Clear();
				PlanetLayer.tmp_vertToTileIDs_values.Clear();
				PlanetLayer.tmp_tileIDToVerts_offsets.Clear();
				PlanetLayer.tmp_tileIDToVerts_values.Clear();
				int j = 0;
				int count3 = PlanetLayer.tmp_verts.Count;
				while (j < count3)
				{
					PlanetLayer.tmp_vertToTileIDs_offsets.Add(PlanetLayer.tmp_vertToTileIDs_values.Count);
					if (j >= count)
					{
						for (int k = 0; k < 6; k++)
						{
							PlanetLayer.tmp_vertToTileIDs_values.Add(-1);
						}
					}
					j++;
				}
			}
			for (int l = 0; l < count; l++)
			{
				PackedListOfLists.GetList<int>(PlanetLayer.tmp_vertToTris_offsets, PlanetLayer.tmp_vertToTris_values, l, PlanetLayer.tmp_adjacentTris);
				int count4 = PlanetLayer.tmp_adjacentTris.Count;
				if (!lastPass)
				{
					for (int m = 0; m < count4; m++)
					{
						int num = PlanetLayer.tmp_adjacentTris[m];
						int v = count + num;
						int nextOrderedVertex = PlanetLayer.tmp_tris[num].GetNextOrderedVertex(l);
						int num2 = -1;
						for (int n = 0; n < count4; n++)
						{
							if (m != n)
							{
								TriangleIndices triangleIndices2 = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[n]];
								if (triangleIndices2.v1 == nextOrderedVertex || triangleIndices2.v2 == nextOrderedVertex || triangleIndices2.v3 == nextOrderedVertex)
								{
									num2 = PlanetLayer.tmp_adjacentTris[n];
									break;
								}
							}
						}
						if (num2 >= 0)
						{
							int v2 = count + num2;
							PlanetLayer.tmp_generatedTris.Add(new TriangleIndices(l, v2, v));
						}
					}
				}
				else if (count4 == 5 || count4 == 6)
				{
					int num3 = 0;
					int nextOrderedVertex2 = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[num3]].GetNextOrderedVertex(l);
					int num4 = num3;
					int currentTriangleVertex = nextOrderedVertex2;
					PlanetLayer.tmp_generatedTileVerts.Clear();
					for (int num5 = 0; num5 < count4; num5++)
					{
						int item = count + PlanetLayer.tmp_adjacentTris[num4];
						PlanetLayer.tmp_generatedTileVerts.Add(item);
						int nextAdjacentTriangle = this.GetNextAdjacentTriangle(num4, currentTriangleVertex);
						int nextOrderedVertex3 = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[nextAdjacentTriangle]].GetNextOrderedVertex(l);
						num4 = nextAdjacentTriangle;
						currentTriangleVertex = nextOrderedVertex3;
					}
					this.FinalizeGeneratedTile(PlanetLayer.tmp_generatedTileVerts);
				}
			}
			PlanetLayer.tmp_tris.Clear();
			PlanetLayer.tmp_tris.AddRange(PlanetLayer.tmp_generatedTris);
		}

		// Token: 0x0601531A RID: 86810 RVA: 0x00657708 File Offset: 0x00655908
		private void FinalizeGeneratedTile(List<int> generatedTileVertsList)
		{
			if ((generatedTileVertsList.Count != 5 && generatedTileVertsList.Count != 6) || generatedTileVertsList.Count > 6)
			{
				Log.Error(string.Format("Planet shape generation internal error: generated a tile with {0} vertices. Only 5 and 6 are allowed.", generatedTileVertsList.Count));
				return;
			}
			if (this.ShouldDiscardGeneratedTile(generatedTileVertsList))
			{
				return;
			}
			int count = PlanetLayer.tmp_generatedVertOffsets.Count;
			PlanetLayer.tmp_generatedVertOffsets.Add(PlanetLayer.tmp_generatedVerts.Count);
			int i = 0;
			int count2 = generatedTileVertsList.Count;
			while (i < count2)
			{
				int index = generatedTileVertsList[i];
				PlanetLayer.tmp_generatedVerts.Add(PlanetLayer.tmp_verts[index]);
				PlanetLayer.tmp_vertToTileIDs_values[PlanetLayer.tmp_vertToTileIDs_values.IndexOf(-1, PlanetLayer.tmp_vertToTileIDs_offsets[index])] = count;
				i++;
			}
			PackedListOfLists.AddList<int>(PlanetLayer.tmp_tileIDToVerts_offsets, PlanetLayer.tmp_tileIDToVerts_values, generatedTileVertsList);
		}

		// Token: 0x0601531B RID: 86811 RVA: 0x006577D8 File Offset: 0x006559D8
		private bool ShouldDiscardGeneratedTile(List<int> generatedTileVertsList)
		{
			Vector3 a = Vector3.zero;
			int i = 0;
			int count = generatedTileVertsList.Count;
			while (i < count)
			{
				a += PlanetLayer.tmp_verts[generatedTileVertsList[i]];
				i++;
			}
			return !MeshUtility.VisibleForWorldgen(a / (float)generatedTileVertsList.Count, this.Radius, this.viewCenter, this.viewAngle);
		}

		// Token: 0x0601531C RID: 86812 RVA: 0x00657840 File Offset: 0x00655A40
		private void CalculateTileNeighbors()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<PlanetTile> list3 = new List<PlanetTile>();
			int i = 0;
			int count = PlanetLayer.tmp_tileIDToVerts_offsets.Count;
			while (i < count)
			{
				PlanetLayer.tmp_neighborsToAdd.Clear();
				PackedListOfLists.GetList<int>(PlanetLayer.tmp_tileIDToVerts_offsets, PlanetLayer.tmp_tileIDToVerts_values, i, PlanetLayer.tmp_neighbourVerts);
				int j = 0;
				int count2 = PlanetLayer.tmp_neighbourVerts.Count;
				while (j < count2)
				{
					PackedListOfLists.GetList<int>(PlanetLayer.tmp_vertToTileIDs_offsets, PlanetLayer.tmp_vertToTileIDs_values, PlanetLayer.tmp_neighbourVerts[j], PlanetLayer.tmp_tileIDs);
					PackedListOfLists.GetList<int>(PlanetLayer.tmp_vertToTileIDs_offsets, PlanetLayer.tmp_vertToTileIDs_values, PlanetLayer.tmp_neighbourVerts[(j + 1) % PlanetLayer.tmp_neighbourVerts.Count], list);
					int k = 0;
					int count3 = PlanetLayer.tmp_tileIDs.Count;
					while (k < count3)
					{
						int num = PlanetLayer.tmp_tileIDs[k];
						if (num != i && num != -1 && list.Contains(num))
						{
							PlanetLayer.tmp_neighborsToAdd.Add(new PlanetTile(num, this.layerId));
						}
						k++;
					}
					j++;
				}
				PackedListOfLists.AddList<PlanetTile>(list2, list3, PlanetLayer.tmp_neighborsToAdd);
				i++;
			}
			this.tileIDToNeighbors_offsets = NativeArrayUtility.GetNativeArrayCopy<int>(list2, Allocator.Persistent, false);
			this.tileIDToNeighbors_values = NativeArrayUtility.GetNativeArrayCopy<PlanetTile>(list3, Allocator.Persistent, false);
		}

		// Token: 0x0601531D RID: 86813 RVA: 0x00657988 File Offset: 0x00655B88
		private int GetNextAdjacentTriangle(int currentAdjTriangleIndex, int currentTriangleVertex)
		{
			int i = 0;
			int count = PlanetLayer.tmp_adjacentTris.Count;
			while (i < count)
			{
				if (currentAdjTriangleIndex != i)
				{
					TriangleIndices triangleIndices = PlanetLayer.tmp_tris[PlanetLayer.tmp_adjacentTris[i]];
					if (triangleIndices.v1 == currentTriangleVertex || triangleIndices.v2 == currentTriangleVertex || triangleIndices.v3 == currentTriangleVertex)
					{
						return i;
					}
				}
				i++;
			}
			Log.Error("Planet shape generation internal error: could not find next adjacent triangle.");
			return -1;
		}

		// Token: 0x0601531E RID: 86814 RVA: 0x006579EE File Offset: 0x00655BEE
		[BurstCompile(CompileSynchronously = true)]
		private static float CalculateAverageTileSize(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts)
		{
			return PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.Invoke(tileIDToNeighbors_offsets, tileIDToNeighbors_values, tileIDToVerts_offsets, verts);
		}

		// Token: 0x0601531F RID: 86815 RVA: 0x006579F9 File Offset: 0x00655BF9
		public float GetTileSize(PlanetTile tile)
		{
			return PlanetLayer.IntGetTileSize(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, this.tileIDToVerts_offsets, this.verts, tile);
		}

		// Token: 0x06015320 RID: 86816 RVA: 0x00657A1A File Offset: 0x00655C1A
		[BurstCompile]
		private static float IntGetTileSize(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, in PlanetTile tile)
		{
			return PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.Invoke(tileIDToNeighbors_offsets, tileIDToNeighbors_values, tileIDToVerts_offsets, verts, tile);
		}

		// Token: 0x06015321 RID: 86817 RVA: 0x00657A28 File Offset: 0x00655C28
		public Vector3 GetTileCenter(PlanetTile tile)
		{
			if (tile.Layer != this)
			{
				Log.Error(string.Format("Attempted to get the center of a tile that is not the same planet layer ({0}, ID: {1}): {2} (layer: {3})", new object[]
				{
					this.def.label,
					this.layerId,
					tile,
					tile.LayerDef.label
				}));
				return Vector3.zero;
			}
			return this.GetTileCenter(tile.tileId);
		}

		// Token: 0x06015322 RID: 86818 RVA: 0x00657A9C File Offset: 0x00655C9C
		public Vector3 GetTileCenter(int tileID)
		{
			if (tileID < 0 || tileID >= this.TilesCount)
			{
				Log.Error(string.Format("Attempted to access a tile with ID {0}, but it is out of range (count: {1})", tileID, this.TilesCount));
				return Vector3.zero;
			}
			Vector3 result;
			PlanetLayer.IntGetTileCenter(this.tileIDToVerts_offsets, this.verts, tileID, out result);
			return result;
		}

		// Token: 0x06015323 RID: 86819 RVA: 0x00657AF1 File Offset: 0x00655CF1
		[BurstCompile]
		private static void IntGetTileCenter(in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, int tileID, out Vector3 center)
		{
			PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.Invoke(tileIDToVerts_offsets, verts, tileID, out center);
		}

		// Token: 0x06015324 RID: 86820 RVA: 0x00657AFC File Offset: 0x00655CFC
		public float GetHeadingFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return this.GetHeadingFromTo(fromTile.tileId, toTile.tileId);
		}

		// Token: 0x06015325 RID: 86821 RVA: 0x00657B10 File Offset: 0x00655D10
		public float GetHeadingFromTo(int fromTileID, int toTileID)
		{
			if (fromTileID == toTileID)
			{
				return 0f;
			}
			Vector3 tileCenter = this.GetTileCenter(fromTileID);
			Vector3 tileCenter2 = this.GetTileCenter(toTileID);
			return this.GetHeadingFromTo(tileCenter, tileCenter2);
		}

		// Token: 0x06015326 RID: 86822 RVA: 0x00657B40 File Offset: 0x00655D40
		public float GetHeadingFromTo(Vector3 from, Vector3 to)
		{
			if (from == to)
			{
				return 0f;
			}
			Vector3 northPolePos = this.NorthPolePos;
			Vector3 from2;
			Vector3 rhs;
			WorldRendererUtility.GetTangentialVectorFacing(from, northPolePos, out from2, out rhs);
			Vector3 vector;
			Vector3 vector2;
			WorldRendererUtility.GetTangentialVectorFacing(from, to, out vector, out vector2);
			float num = Vector3.Angle(from2, vector);
			if (Vector3.Dot(vector, rhs) < 0f)
			{
				num = 360f - num;
			}
			return num;
		}

		// Token: 0x06015327 RID: 86823 RVA: 0x00657B9C File Offset: 0x00655D9C
		public bool InBounds(PlanetTile tile)
		{
			return this.InBounds(tile.tileId);
		}

		// Token: 0x06015328 RID: 86824 RVA: 0x00657BAA File Offset: 0x00655DAA
		public bool InBounds(int tileID)
		{
			return (ulong)tileID < (ulong)((long)this.TilesCount);
		}

		// Token: 0x06015329 RID: 86825 RVA: 0x00657BB7 File Offset: 0x00655DB7
		public Vector2 LongLatOf(PlanetTile tile)
		{
			return this.LongLatOf(tile.tileId);
		}

		// Token: 0x0601532A RID: 86826 RVA: 0x00657BC8 File Offset: 0x00655DC8
		public Vector2 LongLatOf(int tileID)
		{
			Vector3 tileCenter = this.GetTileCenter(tileID);
			float x = Mathf.Atan2(tileCenter.x, -tileCenter.z) * 57.29578f;
			float y = Mathf.Asin(tileCenter.y / this.Radius) * 57.29578f;
			return new Vector2(x, y);
		}

		// Token: 0x0601532B RID: 86827 RVA: 0x00657C14 File Offset: 0x00655E14
		public Direction8Way GetDirection8WayFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return this.GetDirection8WayFromTo(fromTile.tileId, toTile.tileId);
		}

		// Token: 0x0601532C RID: 86828 RVA: 0x00657C28 File Offset: 0x00655E28
		public Direction8Way GetDirection8WayFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (headingFromTo >= 337.5f || headingFromTo < 22.5f)
			{
				return Direction8Way.North;
			}
			if (headingFromTo < 67.5f)
			{
				return Direction8Way.NorthEast;
			}
			if (headingFromTo < 112.5f)
			{
				return Direction8Way.East;
			}
			if (headingFromTo < 157.5f)
			{
				return Direction8Way.SouthEast;
			}
			if (headingFromTo < 202.5f)
			{
				return Direction8Way.South;
			}
			if (headingFromTo < 247.5f)
			{
				return Direction8Way.SouthWest;
			}
			if (headingFromTo < 292.5f)
			{
				return Direction8Way.West;
			}
			return Direction8Way.NorthWest;
		}

		// Token: 0x0601532D RID: 86829 RVA: 0x00657C8D File Offset: 0x00655E8D
		public Rot4 GetRotFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return this.GetRotFromTo(fromTile.tileId, toTile.tileId);
		}

		// Token: 0x0601532E RID: 86830 RVA: 0x00657CA4 File Offset: 0x00655EA4
		public Rot4 GetRotFromTo(int fromTileID, int toTileID)
		{
			float headingFromTo = this.GetHeadingFromTo(fromTileID, toTileID);
			if (headingFromTo >= 315f || headingFromTo < 45f)
			{
				return Rot4.North;
			}
			if (headingFromTo < 135f)
			{
				return Rot4.East;
			}
			if (headingFromTo < 225f)
			{
				return Rot4.South;
			}
			return Rot4.West;
		}

		// Token: 0x0601532F RID: 86831 RVA: 0x00657CF1 File Offset: 0x00655EF1
		public void GetTileVertices(PlanetTile tile, List<Vector3> outVerts)
		{
			this.GetTileVertices(tile.tileId, outVerts);
		}

		// Token: 0x06015330 RID: 86832 RVA: 0x00657D00 File Offset: 0x00655F00
		public void GetTileVertices(int tileID, List<Vector3> outVerts)
		{
			PackedListOfLists.GetList<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVerts);
		}

		// Token: 0x06015331 RID: 86833 RVA: 0x00657D15 File Offset: 0x00655F15
		public void GetTileVerticesIndices(PlanetTile tile, List<int> outVertsIndices)
		{
			this.GetTileVerticesIndices(tile.tileId, outVertsIndices);
		}

		// Token: 0x06015332 RID: 86834 RVA: 0x00657D24 File Offset: 0x00655F24
		public void GetTileVerticesIndices(int tileID, List<int> outVertsIndices)
		{
			PackedListOfLists.GetListValuesIndices<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID, outVertsIndices);
		}

		// Token: 0x06015333 RID: 86835 RVA: 0x00657D39 File Offset: 0x00655F39
		public void GetTileNeighbors(PlanetTile tile, List<PlanetTile> outNeighbors)
		{
			this.GetTileNeighbors(tile.tileId, outNeighbors);
		}

		// Token: 0x06015334 RID: 86836 RVA: 0x00657D48 File Offset: 0x00655F48
		public void GetTileNeighbors(int tileID, List<PlanetTile> outNeighbors)
		{
			PackedListOfLists.GetList<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID, outNeighbors);
		}

		// Token: 0x06015335 RID: 86837 RVA: 0x00657D5D File Offset: 0x00655F5D
		public int GetTileNeighborCount(PlanetTile tile)
		{
			return this.GetTileNeighborCount(tile.tileId);
		}

		// Token: 0x06015336 RID: 86838 RVA: 0x00657D6B File Offset: 0x00655F6B
		public int GetTileNeighborCount(int tileID)
		{
			return PackedListOfLists.GetListCount<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
		}

		// Token: 0x06015337 RID: 86839 RVA: 0x00657D7F File Offset: 0x00655F7F
		public int GetMaxTileNeighborCountEver(PlanetTile tile)
		{
			return this.GetMaxTileNeighborCountEver(tile.tileId);
		}

		// Token: 0x06015338 RID: 86840 RVA: 0x00657D8D File Offset: 0x00655F8D
		public int GetMaxTileNeighborCountEver(int tileID)
		{
			return PackedListOfLists.GetListCount<Vector3>(this.tileIDToVerts_offsets, this.verts, tileID);
		}

		// Token: 0x06015339 RID: 86841 RVA: 0x00657DA1 File Offset: 0x00655FA1
		public bool IsNeighbor(PlanetTile tileA, PlanetTile tileB)
		{
			return this.IsNeighbor(tileA.tileId, tileB.tileId);
		}

		// Token: 0x0601533A RID: 86842 RVA: 0x00657DB8 File Offset: 0x00655FB8
		public bool IsNeighbor(int tileA, int tileB)
		{
			ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileA);
			int item = listIndexes.Item1;
			int item2 = listIndexes.Item2;
			for (int i = item; i < item2; i++)
			{
				if (this.tileIDToNeighbors_values[i].tileId == tileB)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601533B RID: 86843 RVA: 0x00657E07 File Offset: 0x00656007
		public int GetNeighborId(PlanetTile tileA, PlanetTile tileB)
		{
			return this.GetNeighborId(tileA.tileId, tileB.tileId);
		}

		// Token: 0x0601533C RID: 86844 RVA: 0x00657E1C File Offset: 0x0065601C
		public int GetNeighborId(int tileA, int tileB)
		{
			ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileA);
			int item = listIndexes.Item1;
			int item2 = listIndexes.Item2;
			int i = item;
			int num = 0;
			while (i < item2)
			{
				if (this.tileIDToNeighbors_values[i].tileId == tileB)
				{
					return num;
				}
				i++;
				num++;
			}
			return -1;
		}

		// Token: 0x0601533D RID: 86845 RVA: 0x00657E71 File Offset: 0x00656071
		public PlanetTile GetTileNeighbor(PlanetTile tile, int adjacentId)
		{
			return this.GetTileNeighbor(tile.tileId, adjacentId);
		}

		// Token: 0x0601533E RID: 86846 RVA: 0x00657E80 File Offset: 0x00656080
		public PlanetTile GetTileNeighbor(int tileID, int adjacentId)
		{
			ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(this.tileIDToNeighbors_offsets, this.tileIDToNeighbors_values, tileID);
			int item = listIndexes.Item1;
			int item2 = listIndexes.Item2;
			if (item + adjacentId >= item2)
			{
				Log.Error(string.Format("Attempted to get tile neighbour {0} with invalid adjacent ID: {1}, adjacencies: {2}", tileID, adjacentId, item2 - item));
			}
			return this.tileIDToNeighbors_values[item + adjacentId];
		}

		// Token: 0x0601533F RID: 86847 RVA: 0x00657EE3 File Offset: 0x006560E3
		public bool IsNeighborOrSame(PlanetTile tileA, PlanetTile tileB)
		{
			return this.IsNeighborOrSame(tileA.tileId, tileB.tileId);
		}

		// Token: 0x06015340 RID: 86848 RVA: 0x00657EF7 File Offset: 0x006560F7
		public bool IsNeighborOrSame(int tileA, int tileB)
		{
			return tileA == tileB || this.IsNeighbor(tileA, tileB);
		}

		// Token: 0x06015341 RID: 86849 RVA: 0x00657F07 File Offset: 0x00656107
		public float TileRadiusToAngle(float radii)
		{
			return this.DistOnSurfaceToAngle(radii * this.averageTileSize);
		}

		// Token: 0x06015342 RID: 86850 RVA: 0x00657F17 File Offset: 0x00656117
		public float DistOnSurfaceToAngle(float dist)
		{
			return dist / (6.2831855f * this.Radius) * 360f;
		}

		// Token: 0x06015343 RID: 86851 RVA: 0x00657F2D File Offset: 0x0065612D
		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return sphericalDistance * this.Radius / this.averageTileSize;
		}

		// Token: 0x06015344 RID: 86852 RVA: 0x00657F3E File Offset: 0x0065613E
		public float DistanceFromEquatorNormalized(PlanetTile tile)
		{
			return this.DistanceFromEquatorNormalized(tile.tileId);
		}

		// Token: 0x06015345 RID: 86853 RVA: 0x00657F4C File Offset: 0x0065614C
		public float DistanceFromEquatorNormalized(int tile)
		{
			return Mathf.Abs(this.GetTileCenter(tile).y / this.Radius);
		}

		// Token: 0x06015346 RID: 86854 RVA: 0x00657F66 File Offset: 0x00656166
		public float ApproxDistanceInTiles(PlanetTile tileA, PlanetTile tileB)
		{
			return this.ApproxDistanceInTiles(tileA.tileId, tileB.tileId);
		}

		// Token: 0x06015347 RID: 86855 RVA: 0x00657F7C File Offset: 0x0065617C
		public float ApproxDistanceInTiles(int tileA, int tileB)
		{
			Vector3 tileCenter = this.GetTileCenter(tileA);
			Vector3 tileCenter2 = this.GetTileCenter(tileB);
			return this.ApproxDistanceInTiles(GenMath.SphericalDistance(tileCenter.normalized, tileCenter2.normalized));
		}

		// Token: 0x06015348 RID: 86856 RVA: 0x00657FB4 File Offset: 0x006561B4
		private static void ClearAndDeallocateWorkingLists()
		{
			PlanetLayer.ClearAndDeallocate<TriangleIndices>(ref PlanetLayer.tmp_tris);
			PlanetLayer.ClearAndDeallocate<Vector3>(ref PlanetLayer.tmp_verts);
			PlanetLayer.ClearAndDeallocate<Vector3>(ref PlanetLayer.tmp_generatedVerts);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_generatedVertOffsets);
			PlanetLayer.ClearAndDeallocate<TriangleIndices>(ref PlanetLayer.tmp_generatedTris);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_generatedTileVerts);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_adjacentTris);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_tileIDs);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_neighbourVerts);
			PlanetLayer.ClearAndDeallocate<PlanetTile>(ref PlanetLayer.tmp_neighborsToAdd);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTris_offsets);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTris_values);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTileIDs_offsets);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_vertToTileIDs_values);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_tileIDToVerts_offsets);
			PlanetLayer.ClearAndDeallocate<int>(ref PlanetLayer.tmp_tileIDToVerts_values);
		}

		// Token: 0x06015349 RID: 86857 RVA: 0x00658061 File Offset: 0x00656261
		private static void ClearAndDeallocate<T>(ref List<T> list)
		{
			list.Clear();
			list.TrimExcess();
			list = new List<T>();
		}

		// Token: 0x0601534A RID: 86858 RVA: 0x00658078 File Offset: 0x00656278
		public void ExposeData()
		{
			Scribe_Defs.Look<PlanetLayerDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.layerId, "layerId", -1, false);
			Scribe_Values.Look<int>(ref this.subdivisions, "subdivisions", 0, false);
			Scribe_Values.Look<string>(ref this.scenarioTag, "scenarioTag", null, false);
			Scribe_Values.Look<bool>(ref this.isRootSurface, "isRootSurface", false, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraOffset, "backgroundWorldCameraOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraParallaxDistancePer100Cells, "backgroundWorldCameraParallaxDistance", 0f, false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<float>(ref this.viewAngle, "viewAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.extraCameraAltitude, "extraCameraAltitude", 0f, false);
			Scribe_Values.Look<Vector3>(ref this.viewCenter, "viewCenter", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_References.Look<PlanetLayer>(ref this.zoomInToLayer, "zoomInToLayer", false);
			Scribe_References.Look<PlanetLayer>(ref this.zoomOutToLayer, "zoomOutToLayer", false);
			Scribe_Collections.Look<PlanetLayer, PlanetLayerConnection>(ref this.connections, "connections", LookMode.Reference, LookMode.Deep, ref this.tmpLayers, ref this.tmpConnections, true, false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeLayer();
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.TilesToRawData();
			}
			this.ExposeBody();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.RawDataToTiles();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.finder.RegenerateCache();
			}
		}

		// Token: 0x0601534B RID: 86859 RVA: 0x00658205 File Offset: 0x00656405
		public virtual void Standardize()
		{
			if (this.tiles.Any<Tile>())
			{
				this.TilesToRawData();
			}
			this.RawDataToTiles();
			this.finder.RegenerateCache();
		}

		// Token: 0x0601534C RID: 86860 RVA: 0x0065822B File Offset: 0x0065642B
		internal virtual void ExposeBody()
		{
			DataExposeUtility.LookByteArray(ref this.tileBiome, "tileBiome");
		}

		// Token: 0x0601534D RID: 86861 RVA: 0x0065823D File Offset: 0x0065643D
		protected virtual void TilesToRawData()
		{
			this.tileBiome = DataSerializeUtility.SerializeUshort(this.TilesCount, (int i) => this.tiles[i].PrimaryBiome.shortHash);
		}

		// Token: 0x0601534E RID: 86862 RVA: 0x0065825C File Offset: 0x0065645C
		protected virtual void RawDataToTiles()
		{
			if (this.tiles.Count != this.TilesCount)
			{
				this.tiles.Clear();
				for (int j = 0; j < this.TilesCount; j++)
				{
					Tile tile = (Tile)Activator.CreateInstance(this.def.tileType, new object[]
					{
						new PlanetTile(j, this)
					});
					this.tiles.Add(tile);
					tile.PrimaryBiome = this.def.DefaultBiome;
					tile.elevation = this.Radius;
				}
			}
			DataSerializeUtility.LoadUshort(this.tileBiome, this.TilesCount, delegate(int i, ushort data)
			{
				this.tiles[i].PrimaryBiome = (DefDatabase<BiomeDef>.GetByShortHash(data) ?? this.def.DefaultBiome);
			});
		}

		// Token: 0x0601534F RID: 86863 RVA: 0x0065830C File Offset: 0x0065650C
		public void SetAllLayersDirty()
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				worldDrawLayer.SetDirty();
			}
		}

		// Token: 0x06015350 RID: 86864 RVA: 0x0065835C File Offset: 0x0065655C
		public void SetDirty<T>() where T : WorldDrawLayer
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				if (worldDrawLayer is T)
				{
					worldDrawLayer.SetDirty();
				}
			}
		}

		// Token: 0x06015351 RID: 86865 RVA: 0x006583B8 File Offset: 0x006565B8
		public T GetLayer<T>() where T : WorldDrawLayer
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				T t = worldDrawLayer as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06015352 RID: 86866 RVA: 0x00658428 File Offset: 0x00656628
		public void RegenerateAllLayersNow()
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				if (worldDrawLayer.Visible)
				{
					worldDrawLayer.RegenerateNow();
				}
			}
		}

		// Token: 0x06015353 RID: 86867 RVA: 0x00658484 File Offset: 0x00656684
		public bool LineIntersects(Vector3 A, Vector3 B, float radiusFactor = 1f)
		{
			Vector3 a = this.Origin;
			Vector3 lhs = a - A;
			Vector3 vector = B - A;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Vector3.Dot(lhs, vector) / sqrMagnitude;
			if (num < 0f)
			{
				return false;
			}
			if (num > 1f)
			{
				return false;
			}
			Vector3 b = A + vector * num;
			return Vector3.Distance(a, b) < this.Radius * radiusFactor;
		}

		// Token: 0x06015354 RID: 86868 RVA: 0x006584F0 File Offset: 0x006566F0
		public PlanetTile GetClosestTile(PlanetTile other)
		{
			if (other.Layer == this)
			{
				return other;
			}
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(other, 0f, float.MaxValue, FastTileFinder.LandmarkMode.Any, true, Hilliness.Undefined, Hilliness.Undefined, true, true, true);
			PlanetTile result;
			if (!this.FastTileFinder.Closest(query, out result, null, null, default(FastTileFinder.TileQueryParams)))
			{
				return PlanetTile.Invalid;
			}
			return result;
		}

		// Token: 0x06015355 RID: 86869 RVA: 0x00657242 File Offset: 0x00655442
		public bool DirectConnectionTo(PlanetLayer other)
		{
			return this.connections.ContainsKey(other);
		}

		// Token: 0x06015356 RID: 86870 RVA: 0x00658544 File Offset: 0x00656744
		public bool HasConnectionPathTo(PlanetLayer other)
		{
			bool result = this.IsConnectedToRecursive(other);
			PlanetLayer.TmpVisited.Clear();
			return result;
		}

		// Token: 0x06015357 RID: 86871 RVA: 0x00658558 File Offset: 0x00656758
		private bool IsConnectedToRecursive(PlanetLayer other)
		{
			PlanetLayer.TmpVisited.Add(this);
			if (this.connections.ContainsKey(other))
			{
				return true;
			}
			foreach (KeyValuePair<PlanetLayer, PlanetLayerConnection> keyValuePair in this.connections)
			{
				PlanetLayer planetLayer;
				PlanetLayerConnection planetLayerConnection;
				keyValuePair.Deconstruct(out planetLayer, out planetLayerConnection);
				PlanetLayer planetLayer2 = planetLayer;
				if (!PlanetLayer.TmpVisited.Contains(planetLayer2) && planetLayer2.IsConnectedToRecursive(other))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06015358 RID: 86872 RVA: 0x006585EC File Offset: 0x006567EC
		private static void ResetPathVars()
		{
			PlanetLayer.openSet.Clear();
			PlanetLayer.cameFrom.Clear();
			PlanetLayer.gScore.Clear();
			PlanetLayer.toEnqueue.Clear();
		}

		// Token: 0x06015359 RID: 86873 RVA: 0x00658616 File Offset: 0x00656816
		public bool TryGetPath(PlanetLayer goal, List<PlanetLayerConnection> path, out float cost, int max = 2147483647)
		{
			return PlanetLayer.TryGetPath(this, goal, path, out cost, max);
		}

		// Token: 0x0601535A RID: 86874 RVA: 0x00658624 File Offset: 0x00656824
		public static bool TryGetPath(PlanetLayer start, PlanetLayer goal, List<PlanetLayerConnection> path, out float cost, int max = 2147483647)
		{
			PlanetLayer.ResetPathVars();
			PlanetLayer.gScore.Add(start, 0);
			PlanetLayer.openSet.Enqueue(start, PlanetLayer.gScore[start]);
			while (PlanetLayer.openSet.Count != 0)
			{
				PlanetLayer planetLayer = PlanetLayer.openSet.Dequeue();
				if (planetLayer == goal)
				{
					PlanetLayer.ReconstructPath(path, PlanetLayer.cameFrom, planetLayer);
					cost = path.Sum((PlanetLayerConnection x) => x.fuelCost);
					PlanetLayer.ResetPathVars();
					return true;
				}
				PlanetLayer.toEnqueue.Clear();
				foreach (KeyValuePair<PlanetLayer, PlanetLayerConnection> keyValuePair in planetLayer.connections)
				{
					PlanetLayer planetLayer2;
					PlanetLayerConnection planetLayerConnection;
					keyValuePair.Deconstruct(out planetLayer2, out planetLayerConnection);
					PlanetLayer planetLayer3 = planetLayer2;
					PlanetLayerConnection planetLayerConnection2 = planetLayerConnection;
					if (planetLayer3.CanReachLayer)
					{
						if (planetLayer3 == goal)
						{
							PlanetLayer.cameFrom[planetLayer3] = planetLayer;
							PlanetLayer.ReconstructPath(path, PlanetLayer.cameFrom, planetLayer3);
							cost = path.Sum((PlanetLayerConnection x) => x.fuelCost);
							PlanetLayer.ResetPathVars();
							return true;
						}
						int num = PlanetLayer.gScore[planetLayer] + (int)planetLayerConnection2.fuelCost;
						if (num > max)
						{
							break;
						}
						if (!PlanetLayer.gScore.ContainsKey(planetLayer3) || num < PlanetLayer.gScore[planetLayer3])
						{
							PlanetLayer.cameFrom[planetLayer3] = planetLayer;
							PlanetLayer.gScore[planetLayer3] = num;
							PlanetLayer.openSet.Enqueue(planetLayer3, PlanetLayer.gScore[planetLayer3]);
							PlanetLayer.toEnqueue.Add(planetLayer3);
						}
					}
				}
				foreach (PlanetLayer planetLayer4 in PlanetLayer.toEnqueue)
				{
					PlanetLayer.openSet.Enqueue(planetLayer4, PlanetLayer.gScore[planetLayer4]);
				}
			}
			cost = 0f;
			PlanetLayer.ResetPathVars();
			return false;
		}

		// Token: 0x0601535B RID: 86875 RVA: 0x00658848 File Offset: 0x00656A48
		private static void ReconstructPath(List<PlanetLayerConnection> path, Dictionary<PlanetLayer, PlanetLayer> from, PlanetLayer current)
		{
			path.Clear();
			path.Add(from[current].GetConnectionFromTo(current));
			while (from.ContainsKey(from[current]))
			{
				current = from[current];
				path.Add(from[current].GetConnectionFromTo(current));
			}
			path.Reverse();
		}

		// Token: 0x0601535C RID: 86876 RVA: 0x006588A4 File Offset: 0x00656AA4
		public void Dispose()
		{
			foreach (WorldDrawLayer worldDrawLayer in this.WorldDrawLayers)
			{
				worldDrawLayer.Dispose();
			}
			FastTileFinder fastTileFinder = this.finder;
			if (fastTileFinder != null)
			{
				fastTileFinder.Dispose();
			}
			WorldPathing worldPathing = this.pather;
			if (worldPathing != null)
			{
				worldPathing.Dispose();
			}
			ref this.verts.EnsureDisposed<Vector3>();
			ref this.tileIDToVerts_offsets.EnsureDisposed<int>();
			ref this.tileIDToNeighbors_offsets.EnsureDisposed<int>();
			ref this.tileIDToNeighbors_values.EnsureDisposed<PlanetTile>();
		}

		// Token: 0x06015360 RID: 86880 RVA: 0x00658A64 File Offset: 0x00656C64
		[BurstCompile(CompileSynchronously = true)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalculateAverageTileSize$BurstManaged(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts)
		{
			NativeArray<int> nativeArray = tileIDToNeighbors_offsets;
			if (nativeArray.Length == 0)
			{
				return 1f;
			}
			nativeArray = tileIDToNeighbors_offsets;
			int length = nativeArray.Length;
			double num = 0.0;
			int num2 = 0;
			for (int i = 0; i < length; i++)
			{
				Vector3 a;
				PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, i, out a);
				ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<PlanetTile>(tileIDToNeighbors_offsets, tileIDToNeighbors_values, i);
				int item = listIndexes.Item1;
				int item2 = listIndexes.Item2;
				for (int j = item; j < item2; j++)
				{
					NativeArray<PlanetTile> nativeArray2 = tileIDToNeighbors_values;
					int tileId = nativeArray2[j].tileId;
					Vector3 b;
					PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, tileId, out b);
					num += (double)Vector3.Distance(a, b);
					num2++;
				}
			}
			return (float)(num / (double)num2);
		}

		// Token: 0x06015361 RID: 86881 RVA: 0x00658B2C File Offset: 0x00656D2C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float IntGetTileSize$BurstManaged(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, in PlanetTile tile)
		{
			double num = 0.0;
			int num2 = 0;
			Vector3 a;
			PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, tile, out a);
			int num3 = tile + 1;
			NativeArray<int> nativeArray = tileIDToNeighbors_offsets;
			int num4;
			if (num3 >= nativeArray.Length)
			{
				NativeArray<PlanetTile> nativeArray2 = tileIDToNeighbors_values;
				num4 = nativeArray2.Length;
			}
			else
			{
				nativeArray = tileIDToNeighbors_offsets;
				num4 = nativeArray[tile + 1];
			}
			int num5 = num4;
			nativeArray = tileIDToNeighbors_offsets;
			for (int i = nativeArray[tile]; i < num5; i++)
			{
				NativeArray<PlanetTile> nativeArray2 = tileIDToNeighbors_values;
				int tileId = nativeArray2[i].tileId;
				Vector3 b;
				PlanetLayer.IntGetTileCenter(tileIDToVerts_offsets, verts, tileId, out b);
				num += (double)Vector3.Distance(a, b);
				num2++;
			}
			return (float)(num / (double)num2);
		}

		// Token: 0x06015362 RID: 86882 RVA: 0x00658C0C File Offset: 0x00656E0C
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntGetTileCenter$BurstManaged(in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, int tileID, out Vector3 center)
		{
			if (tileID >= 0)
			{
				NativeArray<int> nativeArray = tileIDToVerts_offsets;
				if (tileID < nativeArray.Length)
				{
					nativeArray = tileIDToVerts_offsets;
					if (!nativeArray.IsCreated)
					{
						center = Vector3.zero;
						return;
					}
					ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<Vector3>(tileIDToVerts_offsets, verts, tileID);
					int item = listIndexes.Item1;
					int item2 = listIndexes.Item2;
					Vector3 vector = Vector3.zero;
					int num = 0;
					for (int i = item; i < item2; i++)
					{
						Vector3 a = vector;
						NativeArray<Vector3> nativeArray2 = verts;
						vector = a + nativeArray2[i];
						num++;
					}
					center = vector / (float)num;
					return;
				}
			}
			center = Vector3.zero;
		}

		// Token: 0x0400EC55 RID: 60501
		private PlanetLayerDef def;

		// Token: 0x0400EC56 RID: 60502
		private string scenarioTag;

		// Token: 0x0400EC57 RID: 60503
		private bool isRootSurface;

		// Token: 0x0400EC58 RID: 60504
		private int layerId = -1;

		// Token: 0x0400EC59 RID: 60505
		private int subdivisions;

		// Token: 0x0400EC5A RID: 60506
		private float backgroundWorldCameraOffset;

		// Token: 0x0400EC5B RID: 60507
		private float backgroundWorldCameraParallaxDistancePer100Cells;

		// Token: 0x0400EC5C RID: 60508
		private float radius;

		// Token: 0x0400EC5D RID: 60509
		private float viewAngle;

		// Token: 0x0400EC5E RID: 60510
		private float extraCameraAltitude;

		// Token: 0x0400EC5F RID: 60511
		private Vector3 viewCenter;

		// Token: 0x0400EC60 RID: 60512
		private Vector3 origin;

		// Token: 0x0400EC61 RID: 60513
		public PlanetLayer zoomOutToLayer;

		// Token: 0x0400EC62 RID: 60514
		public PlanetLayer zoomInToLayer;

		// Token: 0x0400EC63 RID: 60515
		private Dictionary<PlanetLayer, PlanetLayerConnection> connections = new Dictionary<PlanetLayer, PlanetLayerConnection>();

		// Token: 0x0400EC64 RID: 60516
		protected readonly List<Tile> tiles = new List<Tile>();

		// Token: 0x0400EC65 RID: 60517
		private FastTileFinder finder;

		// Token: 0x0400EC66 RID: 60518
		private WorldPathing pather;

		// Token: 0x0400EC67 RID: 60519
		private WorldFloodFiller filler;

		// Token: 0x0400EC68 RID: 60520
		private byte[] tileBiome;

		// Token: 0x0400EC69 RID: 60521
		private NativeArray<Vector3> verts;

		// Token: 0x0400EC6A RID: 60522
		private NativeArray<int> tileIDToVerts_offsets;

		// Token: 0x0400EC6B RID: 60523
		private NativeArray<int> tileIDToNeighbors_offsets;

		// Token: 0x0400EC6C RID: 60524
		private NativeArray<PlanetTile> tileIDToNeighbors_values;

		// Token: 0x0400EC6D RID: 60525
		private float averageTileSize;

		// Token: 0x0400EC6E RID: 60526
		private static List<TriangleIndices> tmp_tris = new List<TriangleIndices>();

		// Token: 0x0400EC6F RID: 60527
		private static List<Vector3> tmp_verts = new List<Vector3>();

		// Token: 0x0400EC70 RID: 60528
		private static List<Vector3> tmp_generatedVerts = new List<Vector3>();

		// Token: 0x0400EC71 RID: 60529
		private static List<int> tmp_generatedVertOffsets = new List<int>();

		// Token: 0x0400EC72 RID: 60530
		private static List<TriangleIndices> tmp_generatedTris = new List<TriangleIndices>();

		// Token: 0x0400EC73 RID: 60531
		private static List<int> tmp_generatedTileVerts = new List<int>();

		// Token: 0x0400EC74 RID: 60532
		private static List<int> tmp_adjacentTris = new List<int>();

		// Token: 0x0400EC75 RID: 60533
		private static List<int> tmp_tileIDs = new List<int>();

		// Token: 0x0400EC76 RID: 60534
		private static List<int> tmp_neighbourVerts = new List<int>();

		// Token: 0x0400EC77 RID: 60535
		private static List<PlanetTile> tmp_neighborsToAdd = new List<PlanetTile>();

		// Token: 0x0400EC78 RID: 60536
		private static List<int> tmp_vertToTris_offsets = new List<int>();

		// Token: 0x0400EC79 RID: 60537
		private static List<int> tmp_vertToTris_values = new List<int>();

		// Token: 0x0400EC7A RID: 60538
		private static List<int> tmp_vertToTileIDs_offsets = new List<int>();

		// Token: 0x0400EC7B RID: 60539
		private static List<int> tmp_vertToTileIDs_values = new List<int>();

		// Token: 0x0400EC7C RID: 60540
		private static List<int> tmp_tileIDToVerts_offsets = new List<int>();

		// Token: 0x0400EC7D RID: 60541
		private static List<int> tmp_tileIDToVerts_values = new List<int>();

		// Token: 0x0400EC7E RID: 60542
		private const int MaxTileVertices = 6;

		// Token: 0x0400EC80 RID: 60544
		private List<PlanetLayer> tmpLayers;

		// Token: 0x0400EC81 RID: 60545
		private List<PlanetLayerConnection> tmpConnections;

		// Token: 0x0400EC82 RID: 60546
		private static readonly HashSet<PlanetLayer> TmpVisited = new HashSet<PlanetLayer>();

		// Token: 0x0400EC83 RID: 60547
		private static readonly PriorityQueue<PlanetLayer, int> openSet = new PriorityQueue<PlanetLayer, int>();

		// Token: 0x0400EC84 RID: 60548
		private static readonly Dictionary<PlanetLayer, PlanetLayer> cameFrom = new Dictionary<PlanetLayer, PlanetLayer>();

		// Token: 0x0400EC85 RID: 60549
		private static readonly Dictionary<PlanetLayer, int> gScore = new Dictionary<PlanetLayer, int>();

		// Token: 0x0400EC86 RID: 60550
		private static readonly List<PlanetLayer> toEnqueue = new List<PlanetLayer>();

		// Token: 0x02003CDE RID: 15582
		// (Invoke) Token: 0x06015368 RID: 86888
		public delegate float CalculateAverageTileSize_000152FC$PostfixBurstDelegate(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts);

		// Token: 0x02003CDF RID: 15583
		internal static class CalculateAverageTileSize_000152FC$BurstDirectCall
		{
			// Token: 0x0601536B RID: 86891 RVA: 0x00658CCD File Offset: 0x00656ECD
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.Pointer == 0)
				{
					PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.DeferredCompilation, methodof(PlanetLayer.CalculateAverageTileSize$BurstManaged(NativeArray<int>*, NativeArray<PlanetTile>*, NativeArray<int>*, NativeArray<Vector3>*)).MethodHandle, typeof(PlanetLayer.CalculateAverageTileSize_000152FC$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.Pointer;
			}

			// Token: 0x0601536C RID: 86892 RVA: 0x00658CFC File Offset: 0x00656EFC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x0601536D RID: 86893 RVA: 0x00658D14 File Offset: 0x00656F14
			public unsafe static void Constructor()
			{
				PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PlanetLayer.CalculateAverageTileSize(NativeArray<int>*, NativeArray<PlanetTile>*, NativeArray<int>*, NativeArray<Vector3>*)).MethodHandle);
			}

			// Token: 0x0601536E RID: 86894 RVA: 0x000026EA File Offset: 0x000008EA
			public static void Initialize()
			{
			}

			// Token: 0x0601536F RID: 86895 RVA: 0x00658D25 File Offset: 0x00656F25
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateAverageTileSize_000152FC$BurstDirectCall()
			{
				PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.Constructor();
			}

			// Token: 0x06015370 RID: 86896 RVA: 0x00658D2C File Offset: 0x00656F2C
			public static float Invoke(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeArray`1<System.Int32>&,Unity.Collections.NativeArray`1<RimWorld.Planet.PlanetTile>&,Unity.Collections.NativeArray`1<System.Int32>&,Unity.Collections.NativeArray`1<UnityEngine.Vector3>&), ref tileIDToNeighbors_offsets, ref tileIDToNeighbors_values, ref tileIDToVerts_offsets, ref verts, functionPointer);
					}
				}
				return PlanetLayer.CalculateAverageTileSize$BurstManaged(tileIDToNeighbors_offsets, tileIDToNeighbors_values, tileIDToVerts_offsets, verts);
			}

			// Token: 0x0400EC8A RID: 60554
			private static IntPtr Pointer;

			// Token: 0x0400EC8B RID: 60555
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02003CE0 RID: 15584
		// (Invoke) Token: 0x06015372 RID: 86898
		public delegate float IntGetTileSize_000152FE$PostfixBurstDelegate(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, in PlanetTile tile);

		// Token: 0x02003CE1 RID: 15585
		internal static class IntGetTileSize_000152FE$BurstDirectCall
		{
			// Token: 0x06015375 RID: 86901 RVA: 0x00658D63 File Offset: 0x00656F63
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.Pointer == 0)
				{
					PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.DeferredCompilation, methodof(PlanetLayer.IntGetTileSize$BurstManaged(NativeArray<int>*, NativeArray<PlanetTile>*, NativeArray<int>*, NativeArray<Vector3>*, PlanetTile*)).MethodHandle, typeof(PlanetLayer.IntGetTileSize_000152FE$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.Pointer;
			}

			// Token: 0x06015376 RID: 86902 RVA: 0x00658D90 File Offset: 0x00656F90
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06015377 RID: 86903 RVA: 0x00658DA8 File Offset: 0x00656FA8
			public unsafe static void Constructor()
			{
				PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PlanetLayer.IntGetTileSize(NativeArray<int>*, NativeArray<PlanetTile>*, NativeArray<int>*, NativeArray<Vector3>*, PlanetTile*)).MethodHandle);
			}

			// Token: 0x06015378 RID: 86904 RVA: 0x000026EA File Offset: 0x000008EA
			public static void Initialize()
			{
			}

			// Token: 0x06015379 RID: 86905 RVA: 0x00658DB9 File Offset: 0x00656FB9
			// Note: this type is marked as 'beforefieldinit'.
			static IntGetTileSize_000152FE$BurstDirectCall()
			{
				PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.Constructor();
			}

			// Token: 0x0601537A RID: 86906 RVA: 0x00658DC0 File Offset: 0x00656FC0
			public static float Invoke(in NativeArray<int> tileIDToNeighbors_offsets, in NativeArray<PlanetTile> tileIDToNeighbors_values, in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, in PlanetTile tile)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeArray`1<System.Int32>&,Unity.Collections.NativeArray`1<RimWorld.Planet.PlanetTile>&,Unity.Collections.NativeArray`1<System.Int32>&,Unity.Collections.NativeArray`1<UnityEngine.Vector3>&,RimWorld.Planet.PlanetTile&), ref tileIDToNeighbors_offsets, ref tileIDToNeighbors_values, ref tileIDToVerts_offsets, ref verts, ref tile, functionPointer);
					}
				}
				return PlanetLayer.IntGetTileSize$BurstManaged(tileIDToNeighbors_offsets, tileIDToNeighbors_values, tileIDToVerts_offsets, verts, tile);
			}

			// Token: 0x0400EC8C RID: 60556
			private static IntPtr Pointer;

			// Token: 0x0400EC8D RID: 60557
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02003CE2 RID: 15586
		// (Invoke) Token: 0x0601537C RID: 86908
		public delegate void IntGetTileCenter_00015301$PostfixBurstDelegate(in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, int tileID, out Vector3 center);

		// Token: 0x02003CE3 RID: 15587
		internal static class IntGetTileCenter_00015301$BurstDirectCall
		{
			// Token: 0x0601537F RID: 86911 RVA: 0x00658DFB File Offset: 0x00656FFB
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.Pointer == 0)
				{
					PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.DeferredCompilation, methodof(PlanetLayer.IntGetTileCenter$BurstManaged(NativeArray<int>*, NativeArray<Vector3>*, int, Vector3*)).MethodHandle, typeof(PlanetLayer.IntGetTileCenter_00015301$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.Pointer;
			}

			// Token: 0x06015380 RID: 86912 RVA: 0x00658E28 File Offset: 0x00657028
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06015381 RID: 86913 RVA: 0x00658E40 File Offset: 0x00657040
			public unsafe static void Constructor()
			{
				PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PlanetLayer.IntGetTileCenter(NativeArray<int>*, NativeArray<Vector3>*, int, Vector3*)).MethodHandle);
			}

			// Token: 0x06015382 RID: 86914 RVA: 0x000026EA File Offset: 0x000008EA
			public static void Initialize()
			{
			}

			// Token: 0x06015383 RID: 86915 RVA: 0x00658E51 File Offset: 0x00657051
			// Note: this type is marked as 'beforefieldinit'.
			static IntGetTileCenter_00015301$BurstDirectCall()
			{
				PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.Constructor();
			}

			// Token: 0x06015384 RID: 86916 RVA: 0x00658E58 File Offset: 0x00657058
			public static void Invoke(in NativeArray<int> tileIDToVerts_offsets, in NativeArray<Vector3> verts, int tileID, out Vector3 center)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeArray`1<System.Int32>&,Unity.Collections.NativeArray`1<UnityEngine.Vector3>&,System.Int32,UnityEngine.Vector3&), ref tileIDToVerts_offsets, ref verts, tileID, ref center, functionPointer);
						return;
					}
				}
				PlanetLayer.IntGetTileCenter$BurstManaged(tileIDToVerts_offsets, verts, tileID, out center);
			}

			// Token: 0x0400EC8E RID: 60558
			private static IntPtr Pointer;

			// Token: 0x0400EC8F RID: 60559
			private static IntPtr DeferredCompilation;
		}
	}
}
