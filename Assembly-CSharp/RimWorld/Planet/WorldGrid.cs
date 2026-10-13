using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D0B RID: 15627
	public class WorldGrid : IExposable, IDisposable
	{
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0601541C RID: 87068 RVA: 0x0065C24C File Offset: 0x0065A44C
		// (remove) Token: 0x0601541D RID: 87069 RVA: 0x0065C284 File Offset: 0x0065A484
		public event Action<PlanetLayer> OnPlanetLayerAdded;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0601541E RID: 87070 RVA: 0x0065C2BC File Offset: 0x0065A4BC
		// (remove) Token: 0x0601541F RID: 87071 RVA: 0x0065C2F4 File Offset: 0x0065A4F4
		public event Action<PlanetLayer> OnPlanetLayerRemoved;

		// Token: 0x170034F5 RID: 13557
		// (get) Token: 0x06015420 RID: 87072 RVA: 0x0065C329 File Offset: 0x0065A529
		public SurfaceLayer Surface
		{
			get
			{
				return this.surface;
			}
		}

		// Token: 0x170034F6 RID: 13558
		// (get) Token: 0x06015421 RID: 87073 RVA: 0x0065C331 File Offset: 0x0065A531
		public PlanetLayer Orbit
		{
			get
			{
				return this.orbit;
			}
		}

		// Token: 0x170034F7 RID: 13559
		// (get) Token: 0x06015422 RID: 87074 RVA: 0x0065C339 File Offset: 0x0065A539
		public IReadOnlyList<WorldDrawLayerBase> GlobalLayers
		{
			get
			{
				return this.globalLayers;
			}
		}

		// Token: 0x170034F8 RID: 13560
		// (get) Token: 0x06015423 RID: 87075 RVA: 0x0065C341 File Offset: 0x0065A541
		public float SurfaceViewAngle
		{
			get
			{
				return this.surfaceViewAngle;
			}
		}

		// Token: 0x170034F9 RID: 13561
		// (get) Token: 0x06015424 RID: 87076 RVA: 0x0065C349 File Offset: 0x0065A549
		public Vector3 SurfaceViewCenter
		{
			get
			{
				return this.surfaceViewCenter;
			}
		}

		// Token: 0x170034FA RID: 13562
		// (get) Token: 0x06015425 RID: 87077 RVA: 0x0065C351 File Offset: 0x0065A551
		public int TilesCount
		{
			get
			{
				return this.surface.TilesCount;
			}
		}

		// Token: 0x170034FB RID: 13563
		// (get) Token: 0x06015426 RID: 87078 RVA: 0x0065C35E File Offset: 0x0065A55E
		public IEnumerable<SurfaceTile> Tiles
		{
			get
			{
				return this.surface.Tiles.Cast<SurfaceTile>();
			}
		}

		// Token: 0x170034FC RID: 13564
		// (get) Token: 0x06015427 RID: 87079 RVA: 0x0065C370 File Offset: 0x0065A570
		public IReadOnlyDictionary<int, PlanetLayer> PlanetLayers
		{
			get
			{
				return this.planetLayers;
			}
		}

		// Token: 0x170034FD RID: 13565
		// (get) Token: 0x06015428 RID: 87080 RVA: 0x0065C378 File Offset: 0x0065A578
		public NativeArray<Vector3> UnsafeVerts
		{
			get
			{
				return this.surface.UnsafeVerts;
			}
		}

		// Token: 0x170034FE RID: 13566
		// (get) Token: 0x06015429 RID: 87081 RVA: 0x0065C385 File Offset: 0x0065A585
		public NativeArray<int> UnsafeTileIDToVerts_offsets
		{
			get
			{
				return this.surface.UnsafeTileIDToVerts_offsets;
			}
		}

		// Token: 0x170034FF RID: 13567
		// (get) Token: 0x0601542A RID: 87082 RVA: 0x0065C392 File Offset: 0x0065A592
		public NativeArray<int> UnsafeTileIDToNeighbors_offsets
		{
			get
			{
				return this.surface.UnsafeTileIDToNeighbors_offsets;
			}
		}

		// Token: 0x17003500 RID: 13568
		// (get) Token: 0x0601542B RID: 87083 RVA: 0x0065C39F File Offset: 0x0065A59F
		public NativeArray<PlanetTile> UnsafeTileIDToNeighbors_values
		{
			get
			{
				return this.surface.UnsafeTileIDToNeighbors_values;
			}
		}

		// Token: 0x17003501 RID: 13569
		// (get) Token: 0x0601542C RID: 87084 RVA: 0x0065C3AC File Offset: 0x0065A5AC
		public Vector3 NorthPolePos
		{
			get
			{
				return this.surface.NorthPolePos;
			}
		}

		// Token: 0x17003502 RID: 13570
		public SurfaceTile this[int tileID]
		{
			get
			{
				return this.surface[tileID];
			}
		}

		// Token: 0x17003503 RID: 13571
		public Tile this[PlanetTile tile]
		{
			get
			{
				return tile.Layer.Tiles[tile.tileId];
			}
		}

		// Token: 0x17003504 RID: 13572
		// (get) Token: 0x0601542F RID: 87087 RVA: 0x0065C3E0 File Offset: 0x0065A5E0
		public bool HasWorldData
		{
			get
			{
				return this.surface.HasWorldData;
			}
		}

		// Token: 0x17003505 RID: 13573
		// (get) Token: 0x06015430 RID: 87088 RVA: 0x0065C3ED File Offset: 0x0065A5ED
		public float AverageTileSize
		{
			get
			{
				return this.surface.AverageTileSize;
			}
		}

		// Token: 0x06015431 RID: 87089 RVA: 0x0065C3FA File Offset: 0x0065A5FA
		public void GenerateFreshWorld()
		{
			this.CalculateViewCenterAndAngle();
			this.CreateRequiredLayers();
		}

		// Token: 0x06015432 RID: 87090 RVA: 0x0065C408 File Offset: 0x0065A608
		private void InitializeGlobalLayers()
		{
			this.globalLayers.Clear();
			foreach (GlobalWorldDrawLayerDef globalWorldDrawLayerDef in DefDatabase<GlobalWorldDrawLayerDef>.AllDefs)
			{
				this.globalLayers.Add((WorldDrawLayerBase)Activator.CreateInstance(globalWorldDrawLayerDef.worldDrawLayer));
			}
		}

		// Token: 0x06015433 RID: 87091 RVA: 0x0065C474 File Offset: 0x0065A674
		private void CreateRequiredLayers()
		{
			this.InitializeGlobalLayers();
			Dictionary<PlanetLayer, List<LayerConnection>> dictionary = new Dictionary<PlanetLayer, List<LayerConnection>>();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
				if (scenPart_PlanetLayer != null)
				{
					bool flag = false;
					foreach (PlanetLayer planetLayer in this.planetLayers.Values)
					{
						if (!(planetLayer.ScenarioTag != scenPart_PlanetLayer.tag))
						{
							dictionary[planetLayer] = scenPart_PlanetLayer.connections;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						PlanetLayer planetLayer2;
						if (scenPart_PlanetLayer == Find.Scenario.surfaceLayer)
						{
							PlanetLayerDef layer = scenPart_PlanetLayer.layer;
							PlanetLayerSettings settings = scenPart_PlanetLayer.Settings;
							Vector3? overrideViewCenter = new Vector3?(this.surfaceViewCenter);
							float? overrideViewAngle = new float?(this.surfaceViewAngle);
							planetLayer2 = (this.surface = (SurfaceLayer)this.RegisterPlanetLayer(layer, settings, null, null, overrideViewCenter, overrideViewAngle));
							this.surface.MarkRootSurface();
						}
						else
						{
							planetLayer2 = this.RegisterPlanetLayer(scenPart_PlanetLayer.layer, scenPart_PlanetLayer.Settings, null, null, null, null);
						}
						if (ModsConfig.OdysseyActive && scenPart_PlanetLayer.layer == PlanetLayerDefOf.Orbit)
						{
							this.orbit = planetLayer2;
						}
						planetLayer2.ScenarioTag = scenPart_PlanetLayer.tag;
						dictionary[planetLayer2] = scenPart_PlanetLayer.connections;
					}
				}
			}
			foreach (KeyValuePair<PlanetLayer, List<LayerConnection>> keyValuePair in dictionary)
			{
				PlanetLayer planetLayer3;
				List<LayerConnection> list;
				keyValuePair.Deconstruct(out planetLayer3, out list);
				PlanetLayer planetLayer4 = planetLayer3;
				foreach (LayerConnection layerConnection in list)
				{
					PlanetLayer planetLayer5;
					if (this.TryGetLayerByTag(layerConnection.tag, out planetLayer5) && !planetLayer4.HasConnectionFromTo(planetLayer5))
					{
						planetLayer4.AddConnection(planetLayer5, layerConnection.fuelCost);
						if (layerConnection.zoomMode == LayerConnection.ZoomMode.ZoomIn)
						{
							planetLayer4.zoomInToLayer = planetLayer5;
						}
						else if (layerConnection.zoomMode == LayerConnection.ZoomMode.ZoomOut)
						{
							planetLayer4.zoomOutToLayer = planetLayer5;
						}
					}
				}
			}
		}

		// Token: 0x06015434 RID: 87092 RVA: 0x0065C738 File Offset: 0x0065A938
		public bool TryGetLayerByTag(string tag, out PlanetLayer layer)
		{
			for (int i = 0; i < this.planetLayers.Count; i++)
			{
				if (this.planetLayers[i].ScenarioTag == tag)
				{
					layer = this.planetLayers[i];
					return true;
				}
			}
			layer = null;
			return false;
		}

		// Token: 0x06015435 RID: 87093 RVA: 0x0065C788 File Offset: 0x0065A988
		public Vector2 LongLatOf(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				if (Find.AnyPlayerHomeMap == null)
				{
					return Vector2.zero;
				}
				tile = Find.AnyPlayerHomeMap.Tile;
			}
			return tile.Layer.LongLatOf(tile);
		}

		// Token: 0x06015436 RID: 87094 RVA: 0x0065C7BC File Offset: 0x0065A9BC
		public PlanetLayer RegisterPlanetLayer(PlanetLayerDef layer, PlanetLayerSettings settings, float? overrideRadius = null, Vector3? overrideOrigin = null, Vector3? overrideViewCenter = null, float? overrideViewAngle = null)
		{
			float radius = overrideRadius ?? settings.radius;
			Vector3 value = overrideOrigin ?? settings.origin;
			Vector3 value2 = overrideViewCenter ?? this.surfaceViewCenter;
			float viewAngle = overrideViewAngle ?? (settings.useSurfaceViewAngle ? this.surfaceViewAngle : settings.viewAngle);
			return this.RegisterPlanetLayer(layer, new Vector3?(value), radius, viewAngle, settings.extraCameraAltitude, settings.subdivisions, settings.backgroundWorldCameraOffset, settings.backgroundWorldCameraParallaxDistancePer100Cells, new Vector3?(value2));
		}

		// Token: 0x06015437 RID: 87095 RVA: 0x0065C87C File Offset: 0x0065AA7C
		public PlanetLayer RegisterPlanetLayer(PlanetLayerDef def, Vector3? origin = null, float radius = 100f, float viewAngle = 180f, float extraCameraAltitude = 0f, int subdivisions = 10, float backgroundWorldCameraOffset = 0f, float backgroundWorldCameraParallaxDistancePer100Cells = 0f, Vector3? overrideViewCenter = null)
		{
			int num = this.nextLayerId;
			this.nextLayerId = num + 1;
			int num2 = num;
			Vector3 vector = origin ?? Vector3.zero;
			Vector3 vector2 = overrideViewCenter ?? this.surfaceViewCenter;
			PlanetLayer planetLayer = this.planetLayers[num2] = (PlanetLayer)Activator.CreateInstance(def.layerType, new object[]
			{
				num2,
				def,
				radius,
				vector,
				vector2,
				viewAngle,
				subdivisions,
				extraCameraAltitude,
				backgroundWorldCameraOffset,
				backgroundWorldCameraParallaxDistancePer100Cells
			});
			planetLayer.InitializeLayer();
			Action<PlanetLayer> onPlanetLayerAdded = this.OnPlanetLayerAdded;
			if (onPlanetLayerAdded != null)
			{
				onPlanetLayerAdded(planetLayer);
			}
			return planetLayer;
		}

		// Token: 0x06015438 RID: 87096 RVA: 0x0065C973 File Offset: 0x0065AB73
		public void RemovePlanetLayer(PlanetLayer layer)
		{
			if (this.planetLayers.Remove(layer.LayerID))
			{
				Action<PlanetLayer> onPlanetLayerRemoved = this.OnPlanetLayerRemoved;
				if (onPlanetLayerRemoved == null)
				{
					return;
				}
				onPlanetLayerRemoved(layer);
			}
		}

		// Token: 0x06015439 RID: 87097 RVA: 0x0065C999 File Offset: 0x0065AB99
		public bool TryGetFirstLayerOfDef(PlanetLayerDef def, out PlanetLayer layer)
		{
			layer = this.FirstLayerOfDef(def);
			return layer != null;
		}

		// Token: 0x0601543A RID: 87098 RVA: 0x0065C9AC File Offset: 0x0065ABAC
		public bool TryGetFirstAdjacentLayerOfDef(PlanetTile origin, PlanetLayerDef def, out PlanetLayer layer)
		{
			if (origin.LayerDef == def)
			{
				layer = origin.Layer;
				return true;
			}
			for (int i = 0; i < this.planetLayers.Count; i++)
			{
				if (this.planetLayers[i].Def == def && this.planetLayers[i].DirectConnectionTo(origin.Layer))
				{
					layer = this.planetLayers[i];
					return true;
				}
			}
			layer = null;
			return false;
		}

		// Token: 0x0601543B RID: 87099 RVA: 0x0065CA28 File Offset: 0x0065AC28
		public PlanetLayer FirstLayerOfDef(PlanetLayerDef def)
		{
			if (def == null)
			{
				return this.surface;
			}
			for (int i = 0; i < this.planetLayers.Count; i++)
			{
				if (this.planetLayers[i].Def == def)
				{
					return this.planetLayers[i];
				}
			}
			return null;
		}

		// Token: 0x0601543C RID: 87100 RVA: 0x0065CA77 File Offset: 0x0065AC77
		public Vector3 GetTileCenter(PlanetTile tile)
		{
			return tile.Layer.GetTileCenter(tile);
		}

		// Token: 0x0601543D RID: 87101 RVA: 0x0065CA86 File Offset: 0x0065AC86
		public bool InBounds(PlanetTile tile)
		{
			return tile.Layer.InBounds(tile);
		}

		// Token: 0x0601543E RID: 87102 RVA: 0x0065CA95 File Offset: 0x0065AC95
		public float GetHeadingFromTo(Vector3 from, Vector3 to)
		{
			return this.GetHeadingFromTo(this.surface, from, to);
		}

		// Token: 0x0601543F RID: 87103 RVA: 0x0065CAA5 File Offset: 0x0065ACA5
		public float GetHeadingFromTo(PlanetLayer layer, Vector3 from, Vector3 to)
		{
			return layer.GetHeadingFromTo(from, to);
		}

		// Token: 0x06015440 RID: 87104 RVA: 0x0065CAAF File Offset: 0x0065ACAF
		public float GetHeadingFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return fromTile.Layer.GetHeadingFromTo(fromTile, toTile);
		}

		// Token: 0x06015441 RID: 87105 RVA: 0x0065CABF File Offset: 0x0065ACBF
		public Direction8Way GetDirection8WayFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return fromTile.Layer.GetDirection8WayFromTo(fromTile, toTile);
		}

		// Token: 0x06015442 RID: 87106 RVA: 0x0065CACF File Offset: 0x0065ACCF
		public Rot4 GetRotFromTo(PlanetTile fromTile, PlanetTile toTile)
		{
			return fromTile.Layer.GetRotFromTo(fromTile, toTile);
		}

		// Token: 0x06015443 RID: 87107 RVA: 0x0065CADF File Offset: 0x0065ACDF
		public void GetTileVertices(PlanetTile tile, List<Vector3> outVerts)
		{
			tile.Layer.GetTileVertices(tile, outVerts);
		}

		// Token: 0x06015444 RID: 87108 RVA: 0x0065CAEF File Offset: 0x0065ACEF
		public void GetTileVerticesIndices(PlanetTile tile, List<int> outVertsIndices)
		{
			tile.Layer.GetTileVerticesIndices(tile, outVertsIndices);
		}

		// Token: 0x06015445 RID: 87109 RVA: 0x0065CAFF File Offset: 0x0065ACFF
		public void GetTileNeighbors(PlanetTile tile, List<PlanetTile> outNeighbors)
		{
			tile.Layer.GetTileNeighbors(tile, outNeighbors);
		}

		// Token: 0x06015446 RID: 87110 RVA: 0x0065CB0F File Offset: 0x0065AD0F
		public int GetTileNeighborCount(PlanetTile tile)
		{
			return tile.Layer.GetTileNeighborCount(tile);
		}

		// Token: 0x06015447 RID: 87111 RVA: 0x0065CB1E File Offset: 0x0065AD1E
		public int GetMaxTileNeighborCountEver(PlanetTile tile)
		{
			return tile.Layer.GetMaxTileNeighborCountEver(tile);
		}

		// Token: 0x06015448 RID: 87112 RVA: 0x0065CB2D File Offset: 0x0065AD2D
		public bool IsNeighbor(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.IsNeighbor(tileA, tileB);
		}

		// Token: 0x06015449 RID: 87113 RVA: 0x0065CB3D File Offset: 0x0065AD3D
		public bool IsNeighborOrSame(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.IsNeighborOrSame(tileA, tileB);
		}

		// Token: 0x0601544A RID: 87114 RVA: 0x0065CB4D File Offset: 0x0065AD4D
		public int GetNeighborId(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.GetNeighborId(tileA, tileB);
		}

		// Token: 0x0601544B RID: 87115 RVA: 0x0065CB5D File Offset: 0x0065AD5D
		public PlanetTile GetTileNeighbor(PlanetTile tile, int adjacentId)
		{
			return tile.Layer.GetTileNeighbor(tile, adjacentId);
		}

		// Token: 0x0601544C RID: 87116 RVA: 0x0065CB6D File Offset: 0x0065AD6D
		public float TileRadiusToAngle(float radius)
		{
			return this.TileRadiusToAngle(this.surface, radius);
		}

		// Token: 0x0601544D RID: 87117 RVA: 0x0065CB7C File Offset: 0x0065AD7C
		public float TileRadiusToAngle(PlanetLayer layer, float radius)
		{
			return layer.TileRadiusToAngle(radius);
		}

		// Token: 0x0601544E RID: 87118 RVA: 0x0065CB85 File Offset: 0x0065AD85
		public float DistOnSurfaceToAngle(float dist)
		{
			return this.DistOnSurfaceToAngle(this.surface, dist);
		}

		// Token: 0x0601544F RID: 87119 RVA: 0x0065CB94 File Offset: 0x0065AD94
		public float DistOnSurfaceToAngle(PlanetLayer layer, float dist)
		{
			return layer.DistOnSurfaceToAngle(dist);
		}

		// Token: 0x06015450 RID: 87120 RVA: 0x0065CB9D File Offset: 0x0065AD9D
		public float DistanceFromEquatorNormalized(PlanetTile tile)
		{
			return tile.Layer.DistanceFromEquatorNormalized(tile);
		}

		// Token: 0x06015451 RID: 87121 RVA: 0x0065CBAC File Offset: 0x0065ADAC
		public float ApproxDistanceInTiles(float sphericalDistance)
		{
			return this.ApproxDistanceInTiles(this.surface, sphericalDistance);
		}

		// Token: 0x06015452 RID: 87122 RVA: 0x0065CBBB File Offset: 0x0065ADBB
		public float ApproxDistanceInTiles(PlanetLayer layer, float sphericalDistance)
		{
			return layer.ApproxDistanceInTiles(sphericalDistance);
		}

		// Token: 0x06015453 RID: 87123 RVA: 0x0065CBC4 File Offset: 0x0065ADC4
		public float ApproxDistanceInTiles(PlanetTile tileA, PlanetTile tileB)
		{
			return tileA.Layer.ApproxDistanceInTiles(tileA, tileB);
		}

		// Token: 0x06015454 RID: 87124 RVA: 0x0065CBD4 File Offset: 0x0065ADD4
		public void OverlayRoad(PlanetTile fromTile, PlanetTile toTile, RoadDef roadDef)
		{
			if (roadDef == null)
			{
				Log.ErrorOnce("Attempted to remove road with overlayRoad; not supported", 90292249);
				return;
			}
			RoadDef roadDef2 = this.GetRoadDef(fromTile, toTile, false);
			if (roadDef2 == roadDef)
			{
				return;
			}
			object obj = this[fromTile];
			Tile obj2 = this[toTile];
			SurfaceTile surfaceTile;
			SurfaceTile surfaceTile2;
			if (obj.Isnt(out surfaceTile) || obj2.Isnt(out surfaceTile2))
			{
				return;
			}
			if (roadDef2 != null)
			{
				if (roadDef2.priority >= roadDef.priority)
				{
					return;
				}
				surfaceTile.potentialRoads.RemoveAll((SurfaceTile.RoadLink rl) => rl.neighbor == toTile);
				surfaceTile2.potentialRoads.RemoveAll((SurfaceTile.RoadLink rl) => rl.neighbor == fromTile);
			}
			if (surfaceTile.potentialRoads == null)
			{
				surfaceTile.potentialRoads = new List<SurfaceTile.RoadLink>();
			}
			if (surfaceTile2.potentialRoads == null)
			{
				surfaceTile2.potentialRoads = new List<SurfaceTile.RoadLink>();
			}
			surfaceTile.potentialRoads.Add(new SurfaceTile.RoadLink
			{
				neighbor = toTile,
				road = roadDef
			});
			surfaceTile2.potentialRoads.Add(new SurfaceTile.RoadLink
			{
				neighbor = fromTile,
				road = roadDef
			});
		}

		// Token: 0x06015455 RID: 87125 RVA: 0x0065CD10 File Offset: 0x0065AF10
		public RoadDef GetRoadDef(PlanetTile fromTile, PlanetTile toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find road information between non-neighboring tiles", 12390444);
				return null;
			}
			SurfaceTile surfaceTile;
			if (this[fromTile].Isnt(out surfaceTile))
			{
				return null;
			}
			List<SurfaceTile.RoadLink> list = visibleOnly ? surfaceTile.Roads : surfaceTile.potentialRoads;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].neighbor == toTile)
				{
					return list[i].road;
				}
			}
			return null;
		}

		// Token: 0x06015456 RID: 87126 RVA: 0x0065CD94 File Offset: 0x0065AF94
		public void OverlayRiver(PlanetTile fromTile, PlanetTile toTile, RiverDef riverDef)
		{
			if (riverDef == null)
			{
				Log.ErrorOnce("Attempted to remove river with overlayRiver; not supported", 90292250);
				return;
			}
			RiverDef riverDef2 = this.GetRiverDef(fromTile, toTile, false);
			if (riverDef2 == riverDef)
			{
				return;
			}
			object obj = this[fromTile];
			Tile obj2 = this[toTile];
			SurfaceTile surfaceTile;
			SurfaceTile surfaceTile2;
			if (obj.Isnt(out surfaceTile) || obj2.Isnt(out surfaceTile2))
			{
				return;
			}
			if (riverDef2 != null)
			{
				if (riverDef2.degradeThreshold >= riverDef.degradeThreshold)
				{
					return;
				}
				surfaceTile.potentialRivers.RemoveAll((SurfaceTile.RiverLink rl) => rl.neighbor == toTile);
				surfaceTile2.potentialRivers.RemoveAll((SurfaceTile.RiverLink rl) => rl.neighbor == fromTile);
			}
			SurfaceTile surfaceTile3 = surfaceTile;
			if (surfaceTile3.potentialRivers == null)
			{
				surfaceTile3.potentialRivers = new List<SurfaceTile.RiverLink>();
			}
			surfaceTile3 = surfaceTile2;
			if (surfaceTile3.potentialRivers == null)
			{
				surfaceTile3.potentialRivers = new List<SurfaceTile.RiverLink>();
			}
			surfaceTile.potentialRivers.Add(new SurfaceTile.RiverLink
			{
				neighbor = toTile,
				river = riverDef
			});
			surfaceTile2.potentialRivers.Add(new SurfaceTile.RiverLink
			{
				neighbor = fromTile,
				river = riverDef
			});
			surfaceTile2.riverDist = Mathf.Max(surfaceTile2.riverDist, surfaceTile.riverDist + 1);
		}

		// Token: 0x06015457 RID: 87127 RVA: 0x0065CEF4 File Offset: 0x0065B0F4
		public RiverDef GetRiverDef(PlanetTile fromTile, PlanetTile toTile, bool visibleOnly = true)
		{
			if (!this.IsNeighbor(fromTile, toTile))
			{
				Log.ErrorOnce("Tried to find river information between non-neighboring tiles", 12390444);
				return null;
			}
			SurfaceTile surfaceTile = (SurfaceTile)this[fromTile];
			List<SurfaceTile.RiverLink> list = visibleOnly ? surfaceTile.Rivers : surfaceTile.potentialRivers;
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].neighbor == toTile)
				{
					return list[i].river;
				}
			}
			return null;
		}

		// Token: 0x06015458 RID: 87128 RVA: 0x0065CF74 File Offset: 0x0065B174
		public float GetRoadMovementDifficultyMultiplier(PlanetTile fromTile, PlanetTile toTile, StringBuilder explanation = null)
		{
			List<SurfaceTile.RoadLink> roads = ((SurfaceTile)this[fromTile]).Roads;
			if (roads == null)
			{
				return 1f;
			}
			if (!toTile.Valid)
			{
				toTile = this.FindMostReasonableAdjacentTileForDisplayedPathCost(fromTile);
			}
			for (int i = 0; i < roads.Count; i++)
			{
				if (roads[i].neighbor == toTile)
				{
					float movementCostMultiplier = roads[i].road.movementCostMultiplier;
					if (explanation != null)
					{
						if (explanation.Length > 0)
						{
							explanation.AppendLine();
						}
						explanation.Append(string.Format("{0}: {1}", roads[i].road.LabelCap, movementCostMultiplier.ToStringPercent()));
					}
					return movementCostMultiplier;
				}
			}
			return 1f;
		}

		// Token: 0x06015459 RID: 87129 RVA: 0x0065D02C File Offset: 0x0065B22C
		public PlanetTile FindMostReasonableAdjacentTileForDisplayedPathCost(PlanetTile fromTile)
		{
			SurfaceTile surfaceTile = (SurfaceTile)this[fromTile];
			float num = 1f;
			PlanetTile planetTile = PlanetTile.Invalid;
			List<SurfaceTile.RoadLink> roads = surfaceTile.Roads;
			if (roads != null)
			{
				for (int i = 0; i < roads.Count; i++)
				{
					float movementCostMultiplier = roads[i].road.movementCostMultiplier;
					if (movementCostMultiplier < num && !Find.World.Impassable(roads[i].neighbor))
					{
						num = movementCostMultiplier;
						planetTile = roads[i].neighbor;
					}
				}
			}
			if (planetTile != PlanetTile.Invalid)
			{
				return planetTile;
			}
			this.tmpNeighbors.Clear();
			this.GetTileNeighbors(fromTile, this.tmpNeighbors);
			for (int j = 0; j < this.tmpNeighbors.Count; j++)
			{
				if (!Find.World.Impassable(this.tmpNeighbors[j]))
				{
					return this.tmpNeighbors[j];
				}
			}
			return fromTile;
		}

		// Token: 0x0601545A RID: 87130 RVA: 0x0065D114 File Offset: 0x0065B314
		public int TraversalDistanceBetween(PlanetTile start, PlanetTile end, bool passImpassable = true, int maxDist = 2147483647, bool canTraverseLayers = false)
		{
			if (start == end)
			{
				return 0;
			}
			if (!start.Valid || !end.Valid)
			{
				return int.MaxValue;
			}
			PlanetTile planetTile = start;
			if (((this.cachedTraversalDistanceForStart == start && this.cachedTraversalDistanceForEnd == end && this.cachedLayer == end.Layer) & passImpassable) && maxDist == 2147483647)
			{
				return this.cachedTraversalDistance;
			}
			if (start.Layer != end.Layer)
			{
				if (!canTraverseLayers)
				{
					return int.MaxValue;
				}
				this.cachedLayer = end.Layer;
				start = end.Layer.GetClosestTile(start);
			}
			if (!passImpassable && !Find.WorldReachability.CanReach(start, end))
			{
				return int.MaxValue;
			}
			int finalDist = int.MaxValue;
			int maxTilesToProcess = (maxDist == int.MaxValue) ? int.MaxValue : this.TilesNumWithinTraversalDistance(maxDist + 1);
			start.Layer.Filler.FloodFill(start, (PlanetTile x) => passImpassable || !Find.World.Impassable(x), delegate(PlanetTile tile, int dist)
			{
				if (tile == end)
				{
					finalDist = dist;
					return true;
				}
				return false;
			}, maxTilesToProcess, null);
			if (passImpassable && maxDist == 2147483647)
			{
				this.cachedTraversalDistance = finalDist;
				this.cachedTraversalDistanceForStart = planetTile;
				this.cachedTraversalDistanceForEnd = end;
			}
			return finalDist;
		}

		// Token: 0x0601545B RID: 87131 RVA: 0x0065D29E File Offset: 0x0065B49E
		public int TilesNumWithinTraversalDistance(int traversalDist)
		{
			if (traversalDist < 0)
			{
				return 0;
			}
			return 3 * traversalDist * (traversalDist + 1) + 1;
		}

		// Token: 0x0601545C RID: 87132 RVA: 0x0065D2AF File Offset: 0x0065B4AF
		public bool IsOnEdge(PlanetTile tileID)
		{
			return this.InBounds(tileID) && this.GetTileNeighborCount(tileID) < this.GetMaxTileNeighborCountEver(tileID);
		}

		// Token: 0x0601545D RID: 87133 RVA: 0x0065D2CC File Offset: 0x0065B4CC
		private void CalculateViewCenterAndAngle()
		{
			this.surfaceViewAngle = Find.World.PlanetCoverage * 180f;
			this.surfaceViewCenter = Vector3.back;
			float angle = 45f;
			if (this.surfaceViewAngle > 45f)
			{
				angle = Mathf.Max(90f - this.surfaceViewAngle, 0f);
			}
			this.surfaceViewCenter = Quaternion.AngleAxis(angle, Vector3.right) * this.surfaceViewCenter;
		}

		// Token: 0x0601545E RID: 87134 RVA: 0x0065D340 File Offset: 0x0065B540
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextLayerId, "nextLayerId", 0, false);
			Scribe_Collections.Look<int, PlanetLayer>(ref this.planetLayers, "layers", LookMode.Value, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.EnsureNextLayerIDValid();
				this.CalculateViewCenterAndAngle();
				if (this.planetLayers.NullOrEmpty<int, PlanetLayer>())
				{
					this.planetLayers = new Dictionary<int, PlanetLayer>();
					this.CreateRequiredLayers();
					this.surface.ExposeBody();
					this.StandardizeTileData();
				}
				else
				{
					foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
					{
						int num;
						PlanetLayer planetLayer;
						keyValuePair.Deconstruct(out num, out planetLayer);
						int item = num;
						PlanetLayer planetLayer2 = planetLayer;
						if (planetLayer2.Def == null)
						{
							WorldGrid.tmpLayerIds.Add(item);
						}
						else
						{
							if (planetLayer2.IsRootSurface)
							{
								this.surface = (SurfaceLayer)planetLayer2;
							}
							if (ModsConfig.OdysseyActive && planetLayer2.Def == PlanetLayerDefOf.Orbit)
							{
								this.orbit = planetLayer2;
							}
						}
					}
					if (this.surface == null)
					{
						foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
						{
							int num;
							PlanetLayer planetLayer;
							keyValuePair.Deconstruct(out num, out planetLayer);
							PlanetLayer planetLayer3 = planetLayer;
							if (planetLayer3.Def == PlanetLayerDefOf.Surface)
							{
								this.surface = (SurfaceLayer)planetLayer3;
								this.surface.MarkRootSurface();
								break;
							}
						}
					}
					foreach (int key in WorldGrid.tmpLayerIds)
					{
						this.planetLayers.Remove(key);
					}
					WorldGrid.tmpLayerIds.Clear();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CreateRequiredLayers();
				this.StandardizeTileData();
			}
		}

		// Token: 0x0601545F RID: 87135 RVA: 0x0065D534 File Offset: 0x0065B734
		private void EnsureNextLayerIDValid()
		{
			if (this.planetLayers.NullOrEmpty<int, PlanetLayer>())
			{
				return;
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				int num2 = num;
				if (num2 >= this.nextLayerId)
				{
					this.nextLayerId = num2 + 1;
				}
			}
		}

		// Token: 0x06015460 RID: 87136 RVA: 0x0065D5B0 File Offset: 0x0065B7B0
		public void StandardizeTileData()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (planetLayer2.Tiles.Empty<Tile>())
				{
					planetLayer2.Standardize();
				}
			}
		}

		// Token: 0x06015461 RID: 87137 RVA: 0x0065D620 File Offset: 0x0065B820
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (Current.ProgramState == ProgramState.Entry)
			{
				yield break;
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
			{
				int num;
				PlanetLayer layer2;
				keyValuePair.Deconstruct(out num, out layer2);
				PlanetLayer layer = layer2;
				if (!layer.IsSelected && (!layer.Def.viewGizmoOnlyVisibleWithDirectConnection || (Find.WorldSelector.SelectedLayer != null && Find.WorldSelector.SelectedLayer.HasConnectionFromTo(layer))))
				{
					AcceptanceReport acceptanceReport = layer.CanSelectLayer();
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "WorldSelectLayer".Translate(layer.Def.Named("LAYER")),
						hotKey = KeyBindingDefOf.Misc1,
						defaultDesc = layer.Def.viewGizmoTooltip,
						icon = layer.Def.ViewGizmoTexture,
						action = delegate()
						{
							PlanetLayer.Selected = layer;
						}
					};
					if (!acceptanceReport.Accepted)
					{
						command_Action.Disable(acceptanceReport.Reason);
					}
					yield return command_Action;
				}
			}
			Dictionary<int, PlanetLayer>.Enumerator enumerator = default(Dictionary<int, PlanetLayer>.Enumerator);
			Gizmo gizmo;
			if (!Find.WindowStack.IsOpen<WorldInspectPane>() && WorldGizmoUtility.TryGetCaravanGizmo(out gizmo))
			{
				yield return gizmo;
			}
			yield return WorldGizmoUtility.GetJumpToGizmo();
			yield break;
			yield break;
		}

		// Token: 0x06015462 RID: 87138 RVA: 0x0065D630 File Offset: 0x0065B830
		public void Dispose()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in this.planetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				planetLayer.Dispose();
			}
		}

		// Token: 0x0400ED16 RID: 60694
		private Dictionary<int, PlanetLayer> planetLayers = new Dictionary<int, PlanetLayer>();

		// Token: 0x0400ED17 RID: 60695
		private Vector3 surfaceViewCenter;

		// Token: 0x0400ED18 RID: 60696
		private float surfaceViewAngle;

		// Token: 0x0400ED19 RID: 60697
		private int nextLayerId;

		// Token: 0x0400ED1A RID: 60698
		private SurfaceLayer surface;

		// Token: 0x0400ED1B RID: 60699
		private PlanetLayer orbit;

		// Token: 0x0400ED1C RID: 60700
		private readonly List<WorldDrawLayerBase> globalLayers = new List<WorldDrawLayerBase>();

		// Token: 0x0400ED1D RID: 60701
		private readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x0400ED1E RID: 60702
		public const int SubdivisionsCount = 10;

		// Token: 0x0400ED1F RID: 60703
		public const float PlanetRadius = 100f;

		// Token: 0x0400ED22 RID: 60706
		private int cachedTraversalDistance = -1;

		// Token: 0x0400ED23 RID: 60707
		private PlanetTile cachedTraversalDistanceForStart = PlanetTile.Invalid;

		// Token: 0x0400ED24 RID: 60708
		private PlanetTile cachedTraversalDistanceForEnd = PlanetTile.Invalid;

		// Token: 0x0400ED25 RID: 60709
		private PlanetLayer cachedLayer;

		// Token: 0x0400ED26 RID: 60710
		private static readonly List<int> tmpLayerIds = new List<int>();
	}
}
