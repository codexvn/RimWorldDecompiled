using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200019E RID: 414
	[StaticConstructorOnStartup]
	public class WorldComponent_GravshipController : WorldComponent
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x000368B9 File Offset: 0x00034AB9
		public static bool CutsceneInProgress
		{
			get
			{
				return WorldComponent_GravshipController.cutsceneInProgress;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x000368C0 File Offset: 0x00034AC0
		public static bool GravshipRenderInProgess
		{
			get
			{
				return GravshipCapturer.IsGravshipRenderInProgress;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x000368C7 File Offset: 0x00034AC7
		public static CellRect GravshipRenderBounds
		{
			get
			{
				return GravshipCapturer.GravshipCaptureBounds;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x000368CE File Offset: 0x00034ACE
		public bool IsGravshipTravelling
		{
			get
			{
				return this.gravship != null;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x000368D9 File Offset: 0x00034AD9
		public bool LandingAreaConfirmationInProgress
		{
			get
			{
				return this.landingMarker != null;
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000368E4 File Offset: 0x00034AE4
		public WorldComponent_GravshipController(World world) : base(world)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.gravshipCapturer = new GravshipCapturer();
			this.gravshipRenderer = new GravshipRenderer();
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00036944 File Offset: 0x00034B44
		public void InitiateTakeoff(Building_GravEngine engine, PlanetTile targetTile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.map = engine.Map;
			this.takeoffTile = this.map.Tile;
			this.landingTile = targetTile;
			SectionLayer_GravshipMask.Engine = engine;
			this.RegenerateGravshipMask();
			WorldComponent_GravshipController.cutsceneInProgress = true;
			this.isTakeoff = true;
			this.gravshipRenderer.Init(this.map);
			this.gravshipCapturer.BeginGravshipRender(engine, new Action<Capture>(this.OnGravshipCaptureComplete));
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000369C0 File Offset: 0x00034BC0
		public static void DestroyTreesAroundSubstructure(Map map, HashSet<IntVec3> substructure, int radius = 2, HashSet<IntVec3> borderCells = null)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			if (borderCells == null)
			{
				borderCells = new HashSet<IntVec3>(substructure);
			}
			HashSet<IntVec3> hashSet = (radius > 1) ? new HashSet<IntVec3>() : null;
			foreach (IntVec3 root in borderCells)
			{
				foreach (IntVec3 intVec in GenAdjFast.AdjacentCells8Way(root))
				{
					if (!substructure.Contains(intVec) && intVec.InBounds(map))
					{
						if (hashSet != null)
						{
							hashSet.Add(intVec);
						}
						List<Thing> list = map.thingGrid.ThingsListAtFast(intVec);
						for (int i = list.Count - 1; i >= 0; i--)
						{
							Thing thing = list[i];
							if (thing.def.plant != null && (thing.def.plant.IsTree || Mathf.Max(thing.DrawSize.x, thing.DrawSize.y) > 1.5f))
							{
								list[i].DeSpawn(DestroyMode.Vanish);
							}
						}
					}
				}
			}
			if (radius > 0)
			{
				WorldComponent_GravshipController.DestroyTreesAroundSubstructure(map, substructure, radius - 1, hashSet);
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00036B28 File Offset: 0x00034D28
		private void RegenerateGravshipMask()
		{
			this.map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipMask));
			this.map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipHull));
			this.map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_SubstructureProps));
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00036B84 File Offset: 0x00034D84
		public void OnGravshipCaptureComplete(Capture capture)
		{
			this.zoomRange = this.GetCutsceneZoomRange(capture);
			Action <>9__2;
			Action <>9__1;
			this.PanIf(Find.CameraDriver.config.gravshipPanOnCutsceneStart, capture.captureCenter, this.zoomRange.min, 1f, delegate
			{
				float seconds = 0.5f;
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						Action action2;
						if ((action2 = <>9__2) == null)
						{
							action2 = (<>9__2 = delegate()
							{
								HashSet<IntVec3> validSubstructure = capture.engine.ValidSubstructure;
								LayerSubMesh item = SectionLayer_IndoorMask.BakeGravshipIndoorMesh(this.map, validSubstructure, validSubstructure.Count, WorldComponent_GravshipController.IndoorMaskGravship, capture.captureCenter);
								List<LayerSubMesh> collection = SectionLayer_GravshipHull.BakeGravshipIndoorMesh(this.map, capture.captureBounds, capture.captureCenter);
								this.gravship = this.RemoveGravshipFromMap(capture.engine);
								this.gravship.capture = capture;
								this.gravship.bakedIndoorMasks.Clear();
								this.gravship.bakedIndoorMasks.Add(item);
								this.gravship.bakedIndoorMasks.AddRange(collection);
								this.RegenerateGravshipMask();
								this.BeginTakeoffCutscene();
							});
						}
						LongEventHandler.QueueLongEvent(action2, "GeneratingGravship", false, null, true, false, null);
					});
				}
				Delay.AfterNSeconds(seconds, action);
			});
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00036BF4 File Offset: 0x00034DF4
		private void BeginTakeoffCutscene()
		{
			this.timeLeft = 10.5f;
			Find.CameraDriver.shaker.DoShake(0.2f, 120);
			this.gravshipRenderer.BeginCutscene(this.gravship, this.gravship.capture.captureCenter, this.gravship.originalPosition.ToVector3(), Rot4.North);
			this.gravshipAudio.BeginTakeoff();
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00036C64 File Offset: 0x00034E64
		public void InitiateLanding(Gravship gravship, Map map, IntVec3 landingPos, Rot4 landingRot)
		{
			WorldComponent_GravshipController.<>c__DisplayClass45_0 CS$<>8__locals1 = new WorldComponent_GravshipController.<>c__DisplayClass45_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.gravship = gravship;
			CS$<>8__locals1.landingPos = landingPos;
			CS$<>8__locals1.landingRot = landingRot;
			this.map = CS$<>8__locals1.map;
			this.gravship = null;
			WorldComponent_GravshipController.cutsceneInProgress = true;
			this.isTakeoff = false;
			Find.ScreenshotModeHandler.Active = true;
			int dist = Mathf.CeilToInt(Mathf.Max(15f, 15f));
			this.terrainCurtainBounds = CellRect.FromCellList(this.landingMarker.GravshipCells).ExpandedBy(dist).MovedBy(CS$<>8__locals1.landingPos.x, CS$<>8__locals1.landingPos.z);
			foreach (Pawn pawn in CS$<>8__locals1.map.mapPawns.AllPawnsSpawned)
			{
				if (this.terrainCurtainBounds.Contains(pawn.Position))
				{
					pawn.pather.ResetToCurrentPosition();
				}
			}
			HashSet<IntVec3> substructure = new HashSet<IntVec3>(from c in this.landingMarker.GravshipCells
			select c + CS$<>8__locals1.landingPos);
			WorldComponent_GravshipController.DestroyTreesAroundSubstructure(CS$<>8__locals1.map, substructure, 2, null);
			if (WorldComponent_GravshipController.BiomeHasTransparency(CS$<>8__locals1.map.Biome))
			{
				GravshipCapturer.CaptureWorldSkybox(delegate(Texture2D skybox)
				{
					GravshipRenderer.MatTerrainCurtain.SetTexture(WorldComponent_GravshipController.ShaderPropertyBackgroundTex, skybox);
					base.<InitiateLanding>g__CaptureAndBeginCutscene|0();
				});
				return;
			}
			CS$<>8__locals1.<InitiateLanding>g__CaptureAndBeginCutscene|0();
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00036DDC File Offset: 0x00034FDC
		private static bool BiomeHasTransparency(BiomeDef biome)
		{
			return biome == BiomeDefOf.Space || biome == BiomeDefOf.Orbit;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00036DF0 File Offset: 0x00034FF0
		private void BeginLandingCutscene(IntVec3 landingPos, Rot4 landingRot)
		{
			this.timeLeft = 10f;
			Find.CameraDriver.shaker.DoShake(0.2f, 120);
			Vector3 b = WorldComponent_GravshipController.CornerOffsetsPerRotation[landingRot.AsInt];
			Vector3 takeoffOrLandingCenter = landingPos.ToVector3() + landingRot.AsQuat * this.gravship.engineToCenter + b;
			this.gravshipRenderer.BeginCutscene(this.gravship, takeoffOrLandingCenter, landingPos.ToVector3(), landingRot);
			this.gravshipAudio.BeginLanding();
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00036E7F File Offset: 0x0003507F
		private Gravship RemoveGravshipFromMap(Building_GravEngine engine)
		{
			if (!engine.Spawned)
			{
				Log.Error("Tried to make a gravship out of an unspawned engine");
				return null;
			}
			return GravshipUtility.GenerateGravship(engine);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00036E9C File Offset: 0x0003509C
		private void TakeoffEnded()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			WorldComponent_GravshipController.cutsceneInProgress = false;
			Find.CameraDriver.shaker.StopAllShaking();
			if (!this.map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
			{
				GravshipUtility.AbandonMap(this.map);
			}
			GravshipUtility.TravelTo(this.gravship, this.takeoffTile, this.landingTile);
			Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
			this.gravshipAudio.EndTakeoff();
			this.map = null;
			this.ResetCutscene();
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00036F24 File Offset: 0x00035124
		public override void WorldComponentUpdate()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.gravshipCapturer.Update();
			float progress = Mathf.Clamp01(1f - this.timeLeft / 10f);
			this.gravshipAudio.Update(10f - this.timeLeft, progress, this.isTakeoff);
			if (this.forceDrawTerrainCurtain)
			{
				this.DrawTerrainCurtain(0f);
			}
			if (!this.gravshipCapturer.IsCaptureComplete || !WorldComponent_GravshipController.cutsceneInProgress || this.gravship == null)
			{
				return;
			}
			if (this.timeLeft > 0f && DebugViewSettings.playGravshipCutscenes)
			{
				this.timeLeft -= Time.deltaTime;
				if (!this.isTakeoff && !this.forceDrawTerrainCurtain)
				{
					this.DrawTerrainCurtain(progress);
				}
				this.DrawGravship(progress);
				return;
			}
			if (DebugSettings.loopGravshipCutscene)
			{
				this.BeginTakeoffCutscene();
				return;
			}
			if (this.isTakeoff)
			{
				this.TakeoffEnded();
				return;
			}
			this.LandingEnded();
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00037014 File Offset: 0x00035214
		private float EaseInOut(float x)
		{
			if (x >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * x + 2f, 3f) * 0.5f;
			}
			return 4f * x * x * x;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0003704D File Offset: 0x0003524D
		private void PanIf(bool panCondition, Vector3 loc, float size, float duration = 0.25f, PanCompletionCallback onComplete = null)
		{
			if (panCondition)
			{
				Find.CameraDriver.PanToMapLocAndSize(loc, size, duration, onComplete);
				return;
			}
			if (onComplete != null)
			{
				onComplete();
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00037070 File Offset: 0x00035270
		private FloatRange GetCutsceneZoomRange(Capture capture)
		{
			CameraMapConfig config = Find.CameraDriver.config;
			if (config.gravshipEnableOverrideSizeRange)
			{
				return config.gravshipOverrideSizeRange;
			}
			return new FloatRange(capture.minCameraSize, capture.maxCameraSize);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x000370A8 File Offset: 0x000352A8
		private void DrawTerrainCurtain(float progress)
		{
			GravshipRenderer.MatTerrainCurtain.mainTexture = (Texture2D)this.terrainCapture.capture;
			Color color = new Color(1f, 1f, 1f, progress.RemapClamped(0.9f, 1f, 1f, 0f));
			float num = WorldComponent_GravshipController.BiomeHasTransparency(this.map.Biome) ? 1f : 0f;
			Color value = (this.map.Biome != null && this.map.Biome.disableSkyLighting) ? new Color(1f, 1f, 1f, num) : this.map.skyManager.CurSky.colors.sky.WithAlpha(num);
			GravshipRenderer.MatTerrainCurtain.color = color;
			GravshipRenderer.MatTerrainCurtain.SetColor(WorldComponent_GravshipController.ShaderPropertyBackgroundColor, value);
			GenDraw.DrawCellRect(this.terrainCurtainBounds, Vector3.up * (AltitudeLayer.Skyfaller.AltitudeFor() - 0.03658537f), GravshipRenderer.MatTerrainCurtain, null, WorldComponent_GravshipController.LayerGravshipExclude);
			if (this.terrainCurtainIndoorMask != null)
			{
				Graphics.DrawMesh(this.terrainCurtainIndoorMask.mesh, Matrix4x4.TRS(this.terrainCurtainBounds.CenterVector3, Quaternion.identity, Vector3.one), this.terrainCurtainIndoorMask.material, 0);
			}
			if (this.terrainCurtainShadowMask != null)
			{
				Graphics.DrawMesh(this.terrainCurtainShadowMask.mesh, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one), this.terrainCurtainShadowMask.material, 0);
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00037234 File Offset: 0x00035434
		private void DrawGravship(float progress)
		{
			this.gravshipRenderer.BeginUpdate();
			FloatRange cutsceneZoomRange = this.GetCutsceneZoomRange(this.gravship.capture);
			float b;
			if (this.isTakeoff)
			{
				this.gravshipRenderer.UpdateTakeoff(progress);
				b = Mathf.Lerp(cutsceneZoomRange.min, cutsceneZoomRange.max, this.EaseInOut(progress.RemapClamped(0f, 0.7f, 0f, 1f)));
			}
			else
			{
				this.gravshipRenderer.UpdateLanding(progress);
				b = Mathf.Lerp(cutsceneZoomRange.max, cutsceneZoomRange.min, this.EaseInOut(progress.RemapClamped(0f, 0.8f, 0f, 1f)));
			}
			CameraDriver cameraDriver = Find.CameraDriver;
			if (!cameraDriver.config.gravshipFreeCam)
			{
				cameraDriver.SetRootSize(Mathf.Lerp(cameraDriver.RootSize, b, 5f * Time.deltaTime));
			}
			this.gravshipRenderer.EndUpdate();
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0003731F File Offset: 0x0003551F
		public override void FinalizeInit(bool fromLoad)
		{
			if (!fromLoad)
			{
				this.ResetCutscene();
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0003732A File Offset: 0x0003552A
		private void ResetCutscene()
		{
			Find.ScreenshotModeHandler.Active = false;
			WorldComponent_GravshipController.cutsceneInProgress = false;
			this.landingMap = null;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00037344 File Offset: 0x00035544
		public void Notify_LandingAreaConfirmationStarted(GravshipLandingMarker marker)
		{
			this.landingMarker = marker;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00037350 File Offset: 0x00035550
		private void LandingEnded()
		{
			SectionLayer_GravshipMask.Engine = this.gravship.Engine;
			SectionLayer_GravshipMask.OverrideMode = SectionLayer_GravshipMask.MaskOverrideMode.Gravship;
			this.RegenerateGravshipMask();
			SectionLayer_GravshipMask.OverrideMode = SectionLayer_GravshipMask.MaskOverrideMode.None;
			if (this.gravship.Engine.launchInfo.doNegativeOutcome)
			{
				DefDatabase<LandingOutcomeDef>.AllDefsListForReading.RandomElementByWeight((LandingOutcomeDef d) => d.weight).Worker.ApplyOutcome(this.gravship);
			}
			this.terrainCapture = null;
			this.gravship = null;
			Current.Game.Gravship = null;
			Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
			this.terrainCurtainIndoorMask = null;
			this.terrainCurtainShadowMask = null;
			this.map = null;
			this.moveDesignator = null;
			this.ResetCutscene();
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00037418 File Offset: 0x00035618
		private Building_GravEngine PlaceGravship(Gravship gravship, IntVec3 root, Map map)
		{
			List<Thing> gravshipThings;
			GravshipPlacementUtility.PlaceGravshipInMap(gravship, root, map, out gravshipThings);
			GravshipPlacementUtility.ApplyTemperatureVacuumFromBase(gravship, root, map);
			map.listerFilthInHomeArea.RebuildAll();
			map.resourceCounter.UpdateResourceCounts();
			map.wealthWatcher.ForceRecount(true);
			map.powerNetManager.UpdatePowerNetsAndConnections_First();
			GravshipPlacementUtility.PostSwapMap(gravship, gravshipThings);
			return gravship.Engine;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00037474 File Offset: 0x00035674
		public override void WorldComponentOnGUI()
		{
			if (!this.LandingAreaConfirmationInProgress || Find.ScreenshotModeHandler.Active || !WorldRendererUtility.DrawingMap)
			{
				return;
			}
			float num = 430f;
			if (this.landingMap != null)
			{
				num = 640f;
			}
			Rect rect = new Rect((float)UI.screenWidth / 2f - num / 2f, (float)UI.screenHeight - 150f - 70f, num, 70f);
			Widgets.DrawWindowBackground(rect);
			Rect rect2 = new Rect(rect.xMin + 10f, rect.yMin + 10f, 200f, 50f);
			if (this.landingMap != null)
			{
				if (Widgets.ButtonText(rect2, "AbortLandGravship".Translate(), true, true, true, null))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("AbortLandConfirmation".Translate(), new Action(this.AbortLanding), false, null, WindowLayer.Dialog));
				}
				TooltipHandler.TipRegion(rect2, "AbortLandGravshipDesc".Translate());
				rect2.x += 210f;
			}
			Designator_MoveGravship designator_MoveGravship = this.MoveDesignator();
			if (Widgets.ButtonText(rect2, "DesignatorMoveGravship".Translate(), true, true, true, null))
			{
				Find.DesignatorManager.Select(designator_MoveGravship);
			}
			TooltipHandler.TipRegion(rect2, "DesignatorMoveGravshipDesc".Translate());
			rect2.x += 210f;
			if (Widgets.ButtonText(rect2, "ConfirmLandGravship".Translate(), true, true, true, null))
			{
				if (this.landingMarker.Spawned && Find.DesignatorManager.SelectedDesignator != designator_MoveGravship)
				{
					this.landingMarker.BeginLanding(this);
					this.landingMarker = null;
					SoundDefOf.Gravship_Land.PlayOneShotOnCamera(null);
				}
				else
				{
					Messages.Message("GravshipLandingMarkerNotPlaced".Translate(), MessageTypeDefOf.RejectInput, true);
				}
			}
			TooltipHandler.TipRegion(rect2, "ConfirmLandGravshipDesc".Translate());
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0003767C File Offset: 0x0003587C
		private void AbortLanding()
		{
			PlanetLayer layer = this.landingMap.Tile.Layer;
			int num = 1;
			PlanetTile destinationTile = PlanetTile.Invalid;
			while (!destinationTile.Valid && num < 50)
			{
				FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(this.landingMap.Tile, 1f, (float)num, FastTileFinder.LandmarkMode.Any, false, Hilliness.Undefined, Hilliness.Undefined, true, true, true);
				destinationTile = layer.FastTileFinder.Query(query, null, null, default(FastTileFinder.TileQueryParams)).RandomElementWithFallback(PlanetTile.Invalid);
				num++;
			}
			if (!destinationTile.Valid)
			{
				Messages.Message("FailedToFindCrashSite".Translate(), MessageTypeDefOf.RejectInput, true);
				return;
			}
			this.landingMap = null;
			this.landingMarker.Destroy(DestroyMode.Vanish);
			this.landingMarker = null;
			Find.CurrentGravship.destinationTile = destinationTile;
			float quality = Find.CurrentGravship.Engine.launchInfo.quality;
			Find.CurrentGravship.Engine.launchInfo.doNegativeOutcome = Rand.Chance(GravshipUtility.NegativeLandingOutcomeFromQuality(quality) * 2f);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GravshipUtility.ArriveNewMap(Find.CurrentGravship);
			}, "GeneratingMap", false, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true, false, null);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000377BA File Offset: 0x000359BA
		public Designator_MoveGravship MoveDesignator()
		{
			if (this.moveDesignator == null)
			{
				this.moveDesignator = new Designator_MoveGravship(this.landingMap, this.landingMarker);
			}
			this.moveDesignator.marker = this.landingMarker;
			return this.moveDesignator;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000377F4 File Offset: 0x000359F4
		public override void ExposeData()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Scribe_Values.Look<bool>(ref WorldComponent_GravshipController.cutsceneInProgress, "cutsceneInProgress", false, false);
			Scribe_Values.Look<PlanetTile>(ref this.takeoffTile, "takeoffTile", PlanetTile.Invalid, false);
			Scribe_Values.Look<PlanetTile>(ref this.landingTile, "targetTile", PlanetTile.Invalid, false);
			Scribe_References.Look<Map>(ref this.map, "takeoffMap", false);
			Scribe_References.Look<Gravship>(ref this.gravship, "gravship", false);
			Scribe_References.Look<Map>(ref this.landingMap, "landingMap", false);
			Scribe_Values.Look<FloatRange>(ref this.zoomRange, "zoomRange", default(FloatRange), false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				GravshipLandingMarker gravshipLandingMarker = this.landingMarker;
				this.landingMarkerSpawned = (gravshipLandingMarker != null && gravshipLandingMarker.Spawned);
			}
			Scribe_Values.Look<bool>(ref this.landingMarkerSpawned, "landingMarkerSpawned", false, false);
			if (this.landingMarkerSpawned)
			{
				Scribe_References.Look<GravshipLandingMarker>(ref this.landingMarker, "landingMarker", false);
			}
			else
			{
				Scribe_Deep.Look<GravshipLandingMarker>(ref this.landingMarker, "landingMarker", Array.Empty<object>());
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.landingMarker != null && !this.landingMarkerSpawned)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					CameraJumper.TryJump(this.landingMap.Center, this.landingMap, CameraJumper.MovementMode.Cut);
				});
			}
		}

		// Token: 0x0400069C RID: 1692
		private Gravship gravship;

		// Token: 0x0400069D RID: 1693
		private Map map;

		// Token: 0x0400069E RID: 1694
		private PlanetTile takeoffTile = PlanetTile.Invalid;

		// Token: 0x0400069F RID: 1695
		private PlanetTile landingTile = PlanetTile.Invalid;

		// Token: 0x040006A0 RID: 1696
		private static bool cutsceneInProgress = false;

		// Token: 0x040006A1 RID: 1697
		private float timeLeft = -1000f;

		// Token: 0x040006A2 RID: 1698
		private bool isTakeoff;

		// Token: 0x040006A3 RID: 1699
		public Map landingMap;

		// Token: 0x040006A4 RID: 1700
		private GravshipLandingMarker landingMarker;

		// Token: 0x040006A5 RID: 1701
		private FloatRange zoomRange;

		// Token: 0x040006A6 RID: 1702
		private GravshipCapturer gravshipCapturer;

		// Token: 0x040006A7 RID: 1703
		private GravshipRenderer gravshipRenderer;

		// Token: 0x040006A8 RID: 1704
		private Capture terrainCapture;

		// Token: 0x040006A9 RID: 1705
		private CellRect terrainCurtainBounds;

		// Token: 0x040006AA RID: 1706
		private bool forceDrawTerrainCurtain;

		// Token: 0x040006AB RID: 1707
		private LayerSubMesh terrainCurtainIndoorMask;

		// Token: 0x040006AC RID: 1708
		private LayerSubMesh terrainCurtainShadowMask;

		// Token: 0x040006AD RID: 1709
		private GravshipAudio gravshipAudio = new GravshipAudio();

		// Token: 0x040006AE RID: 1710
		private const float InitialTime = 10f;

		// Token: 0x040006AF RID: 1711
		private const float CameraSizeLerpSpeed = 5f;

		// Token: 0x040006B0 RID: 1712
		private const float CameraInitialPanDuration = 1f;

		// Token: 0x040006B1 RID: 1713
		public const int PlantDestroyRadius = 2;

		// Token: 0x040006B2 RID: 1714
		private static readonly int LayerGravshipExclude = LayerMask.NameToLayer("GravshipExclude");

		// Token: 0x040006B3 RID: 1715
		private static readonly int ShaderPropertyBackgroundTex = Shader.PropertyToID("_BackgroundTex");

		// Token: 0x040006B4 RID: 1716
		private static readonly int ShaderPropertyBackgroundColor = Shader.PropertyToID("_BackgroundColor");

		// Token: 0x040006B5 RID: 1717
		private static readonly Material IndoorMaskGravship = MatLoader.LoadMat("Misc/IndoorMaskGravship", -1);

		// Token: 0x040006B6 RID: 1718
		private static readonly Material IndoorMaskTerrainCurtain = MatLoader.LoadMat("Misc/IndoorMaskTerrainCurtain", -1);

		// Token: 0x040006B7 RID: 1719
		private static readonly Material GravshipShadowMaskTerrainCurtain = MatLoader.LoadMat("Misc/GravshipMaskTerrainCurtain", -1);

		// Token: 0x040006B8 RID: 1720
		private static readonly Vector3[] CornerOffsetsPerRotation = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, 0f, 0f)
		};

		// Token: 0x040006B9 RID: 1721
		private Designator_MoveGravship moveDesignator;

		// Token: 0x040006BA RID: 1722
		private bool landingMarkerSpawned;
	}
}
