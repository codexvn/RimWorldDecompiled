using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Unity.Profiling;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200365D RID: 13917
	public static class GravshipUtility
	{
		// Token: 0x170031D2 RID: 12754
		// (get) Token: 0x0601352D RID: 79149 RVA: 0x005BEEF8 File Offset: 0x005BD0F8
		public static bool ShowConnectedSubstructure
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return false;
				}
				Designator selectedDesignator = Find.DesignatorManager.SelectedDesignator;
				if (selectedDesignator is Designator_Install)
				{
					return false;
				}
				Designator_Place designator_Place = selectedDesignator as Designator_Place;
				if (designator_Place != null)
				{
					TerrainDef terrainDef = designator_Place.PlacingDef as TerrainDef;
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						return true;
					}
					ThingDef thingDef = designator_Place.PlacingDef as ThingDef;
					if (thingDef != null && thingDef.GetCompProperties<CompProperties_SubstructureFootprint>() != null)
					{
						return true;
					}
				}
				foreach (object obj in Find.Selector.SelectedObjects)
				{
					Thing thing = obj as Thing;
					CompSubstructureFootprint compSubstructureFootprint;
					if (thing != null && thing.TryGetComp(out compSubstructureFootprint) && compSubstructureFootprint.Valid && compSubstructureFootprint.DisplaySubstructureOverlay)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0601352E RID: 79150 RVA: 0x005BEFD4 File Offset: 0x005BD1D4
		public static bool PlayerHasGravEngine()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (Find.CurrentGravship != null)
			{
				return true;
			}
			bool result;
			using (GravshipUtility.PlayerHasGravEngineMarker.Auto())
			{
				using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (GravshipUtility.PlayerHasGravEngine(enumerator.Current))
						{
							return true;
						}
					}
				}
				foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
				{
					if (worldObject.Faction != null && worldObject.Faction.IsPlayer && !(worldObject is MapParent))
					{
						IThingHolder thingHolder = worldObject as IThingHolder;
						if (thingHolder != null)
						{
							using (List<Thing>.Enumerator enumerator3 = ThingOwnerUtility.GetAllThingsRecursively(thingHolder, true).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									if (enumerator3.Current.GetInnerIfMinified().def == ThingDefOf.GravEngine)
									{
										return true;
									}
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0601352F RID: 79151 RVA: 0x005BF130 File Offset: 0x005BD330
		public static Thing GetPlayerGravEngine(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			ThingDef def = ThingDefOf.GravEngine;
			Thing thing = map.listerThings.ThingsOfDef(def).FirstOrDefault<Thing>();
			if (thing == null)
			{
				thing = map.listerThings.ThingsOfDef(def.minifiedDef).FirstOrDefault<Thing>().GetInnerIfMinified();
			}
			if (thing == null)
			{
				List<Thing> list = new List<Thing>();
				ThingOwnerUtility.GetAllThingsRecursively<Thing>(map, ThingRequest.ForDef(def.minifiedDef), list, true, null, false);
				Thing thing2 = list.Find(delegate(Thing t)
				{
					Thing innerIfMinified = t.GetInnerIfMinified();
					return ((innerIfMinified != null) ? innerIfMinified.def : null) == def;
				});
				if (thing2 != null)
				{
					thing = thing2.GetInnerIfMinified();
				}
			}
			return thing;
		}

		// Token: 0x06013530 RID: 79152 RVA: 0x005BF1D3 File Offset: 0x005BD3D3
		public static bool PlayerHasGravEngine(Map map)
		{
			return ModsConfig.OdysseyActive && GravshipUtility.GetPlayerGravEngine(map) != null;
		}

		// Token: 0x06013531 RID: 79153 RVA: 0x005BF1E8 File Offset: 0x005BD3E8
		public static bool TryGetNameOfGravshipOnMap(Map map, out string name)
		{
			name = null;
			if (!ModsConfig.OdysseyActive || map == null || !GravshipUtility.PlayerHasGravEngine(map))
			{
				return false;
			}
			Building_GravEngine building_GravEngine = GravshipUtility.GetPlayerGravEngine(map) as Building_GravEngine;
			if (building_GravEngine == null)
			{
				return false;
			}
			if (building_GravEngine.nameHidden)
			{
				return false;
			}
			name = building_GravEngine.RenamableLabel;
			return true;
		}

		// Token: 0x06013532 RID: 79154 RVA: 0x005BF230 File Offset: 0x005BD430
		public static void GetConnectedSubstructure(Building_GravEngine engine, HashSet<IntVec3> cells, int maxCells, bool requireInsideFootprint = true)
		{
			cells.Clear();
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (!engine.Spawned)
			{
				Log.Error("Tried to get connected substructure for an unspawned engine.");
				return;
			}
			List<Thing> footprintMakers = engine.Map.listerThings.ThingsInGroup(ThingRequestGroup.SubstructureFootprint);
			engine.Map.floodFiller.FloodFill(engine.Position, delegate(IntVec3 x)
			{
				if (x.InBounds(engine.Map))
				{
					TerrainDef terrainDef = engine.Map.terrainGrid.FoundationAt(x);
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						return !requireInsideFootprint || GravshipUtility.InsideFootprint(x, engine.Map, footprintMakers, null);
					}
				}
				return false;
			}, delegate(IntVec3 x)
			{
				cells.Add(x);
				return false;
			}, maxCells, false, null);
		}

		// Token: 0x06013533 RID: 79155 RVA: 0x005BF2DC File Offset: 0x005BD4DC
		public static bool InsideFootprint(IntVec3 loc, Map map, List<Thing> footprintMakers = null, Thing thingToExclude = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (footprintMakers == null)
			{
				footprintMakers = map.listerThings.ThingsInGroup(ThingRequestGroup.SubstructureFootprint);
			}
			foreach (Thing thing in footprintMakers)
			{
				CompSubstructureFootprint compSubstructureFootprint;
				if (thing.Spawned && thing.TryGetComp(out compSubstructureFootprint) && compSubstructureFootprint.Valid && loc.InHorDistOf(thing.Position, compSubstructureFootprint.Props.radius) && (thingToExclude == null || thing != thingToExclude))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013534 RID: 79156 RVA: 0x005BF380 File Offset: 0x005BD580
		public static void PreLaunchConfirmation(Building_GravEngine engine, Action launchAction)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			TaggedString taggedString = "ConfirmGravEngineLaunch".Translate();
			List<Pawn> list = new List<Pawn>();
			List<Pawn> list2 = new List<Pawn>();
			List<Thing> list3 = new List<Thing>();
			List<Pawn> list4 = new List<Pawn>();
			foreach (Pawn pawn in engine.Map.mapPawns.AllPawns)
			{
				if (pawn.Faction == Faction.OfPlayer && (!engine.ValidSubstructureAt(pawn.PositionHeld) || pawn.ParentHolder is ActiveTransporterInfo))
				{
					if (pawn.RaceProps.Humanlike)
					{
						list.Add(pawn);
					}
					else
					{
						list2.Add(pawn);
					}
				}
				if (GravshipUtility.ConsideredKidnapped(engine, pawn))
				{
					list4.Add(pawn);
				}
			}
			foreach (Pawn pawn2 in engine.Map.mapPawns.PrisonersOfColony)
			{
				if (!engine.ValidSubstructureAt(pawn2.PositionHeld) && !list.Contains(pawn2))
				{
					list.Add(pawn2);
				}
			}
			if (list.Count > 0)
			{
				TaggedString t = taggedString;
				string[] array = new string[5];
				array[0] = "\n\n";
				array[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array[2] = "PeopleWillBeLeftBehind".Translate().Resolve();
				array[3] = ":\n";
				array[4] = (from p in list
				select p.NameFullColored.Resolve()).ToLineList("  - ", true);
				taggedString = t + string.Concat(array);
			}
			if (list2.Count > 0)
			{
				TaggedString t2 = taggedString;
				string[] array2 = new string[5];
				array2[0] = "\n\n";
				array2[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array2[2] = "AnimalsWillBeLeftBehind".Translate().Resolve();
				array2[3] = ":\n";
				array2[4] = (from p in list2
				select p.NameFullColored.Resolve()).ToLineList("  - ", true);
				taggedString = t2 + string.Concat(array2);
			}
			if (list4.Count > 0)
			{
				TaggedString t3 = taggedString;
				string[] array3 = new string[5];
				array3[0] = "\n\n";
				array3[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array3[2] = "PawnsKidnappedOnGravship".Translate().Resolve();
				array3[3] = ":\n";
				array3[4] = (from p in list4
				select p.NameFullColored.Resolve()).ToLineList("  - ", true);
				taggedString = t3 + string.Concat(array3);
			}
			foreach (IntVec3 c in engine.AllConnectedSubstructure)
			{
				Building edifice = c.GetEdifice(engine.Map);
				if (edifice != null && edifice.def.Size != IntVec2.One && !engine.OnValidSubstructure(edifice) && !list3.Contains(edifice))
				{
					list3.Add(edifice);
				}
			}
			if (list3.Count > 0)
			{
				TaggedString t4 = taggedString;
				string[] array4 = new string[5];
				array4[0] = "\n\n";
				array4[1] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				array4[2] = "BuildingsWillBeLeftBehind".Translate().Resolve();
				array4[3] = ":\n";
				array4[4] = (from b in list3
				select b.LabelShort).ToLineList("  - ", true);
				taggedString = t4 + string.Concat(array4);
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (IntVec3 intVec in engine.Map.AllCells)
			{
				TerrainDef terrainDef = engine.Map.terrainGrid.FoundationAt(intVec);
				if (terrainDef != null && terrainDef.IsSubstructure)
				{
					if (!GravshipUtility.InsideFootprint(intVec, engine.Map, null, null))
					{
						hashSet.Add("SubstructureOutsideFootprint".Translate());
					}
					if (!engine.ValidSubstructureAt(intVec))
					{
						hashSet.Add("DisconnectedSubstructure".Translate());
					}
					RoofDef roofDef = engine.Map.roofGrid.RoofAt(intVec);
					if (roofDef != null && roofDef.isThickRoof)
					{
						hashSet.Add("SubstructureUnderRockRoof".Translate());
					}
				}
			}
			if (hashSet.Count > 0)
			{
				taggedString += "\n\n" + ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable) + "SubstructureWillBeLeftBehind".Translate() + ":\n" + hashSet.ToLineList("  - ", true);
			}
			Find.WindowStack.Add(new Dialog_MessageBox(taggedString, null, launchAction, "Cancel".Translate(), delegate()
			{
			}, null, true, launchAction, null, WindowLayer.Dialog));
		}

		// Token: 0x06013535 RID: 79157 RVA: 0x005BF94C File Offset: 0x005BDB4C
		public static void TravelTo(Gravship gravship, PlanetTile oldTile, PlanetTile newTile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			gravship.SetFaction(gravship.Engine.Faction);
			if (oldTile.Layer != newTile.Layer)
			{
				oldTile = newTile.Layer.GetClosestTile(oldTile);
			}
			gravship.Tile = oldTile;
			gravship.destinationTile = newTile;
			Find.WorldObjects.Add(gravship);
			CameraJumper.TryJump(gravship, CameraJumper.MovementMode.Pan);
		}

		// Token: 0x06013536 RID: 79158 RVA: 0x005BF9B8 File Offset: 0x005BDBB8
		public static Gravship GenerateGravship(Building_GravEngine engine)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			GravshipUtility.generatingGravship = true;
			Map map = engine.Map;
			IntVec3[] array = engine.ValidSubstructure.ToArray<IntVec3>();
			Gravship gravship = new Gravship(engine);
			gravship = (WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Gravship, gravship) as Gravship);
			Current.Game.Gravship = gravship;
			List<Thing> list = gravship.Things.ToList<Thing>();
			list.SortByDescending(new Func<Thing, int>(GravshipUtility.ThingSpawnPriority));
			foreach (Thing thing in list)
			{
				thing.PreSwapMap();
			}
			foreach (Thing thing2 in list)
			{
				if (thing2.Spawned)
				{
					thing2.DeSpawn(DestroyMode.WillReplace);
				}
			}
			foreach (Pawn pawn in gravship.Pawns)
			{
				pawn.PreSwapMap();
				if (pawn.Spawned)
				{
					pawn.DeSpawn(DestroyMode.WillReplace);
				}
			}
			ulong dirtyFlags = MapMeshFlagDefOf.Terrain | MapMeshFlagDefOf.Roofs | MapMeshFlagDefOf.Snow | MapMeshFlagDefOf.Sand | MapMeshFlagDefOf.Gas;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = array[i];
				int index = map.cellIndices.CellToIndex(intVec);
				map.terrainGrid.RemoveGravshipTerrainUnsafe(intVec, index);
				map.roofGrid.RemoveRoofUnsafe(index);
				map.areaManager.BuildRoof[index] = false;
				map.areaManager.NoRoof[index] = false;
				map.glowGrid.DirtyCell(intVec);
				map.snowGrid.DepthGrid_Unsafe[index] = 0f;
				map.sandGrid.DepthGrid_Unsafe[index] = 0f;
				map.gasGrid.ClearCellUnsafe(intVec);
				foreach (Designation des in map.designationManager.AllDesignationsAt(intVec))
				{
					map.designationManager.RemoveDesignation(des);
				}
				IntVec2 cell2D = intVec.ToIntVec2;
				Predicate<IFleck> <>9__0;
				foreach (FleckSystem fleckSystem in map.flecks.Systems)
				{
					Predicate<IFleck> shouldRemove;
					if ((shouldRemove = <>9__0) == null)
					{
						shouldRemove = (<>9__0 = ((IFleck fleck) => fleck.GetPosition().ToIntVec2() == cell2D));
					}
					fleckSystem.RemoveAllFlecks(shouldRemove);
				}
				map.mapDrawer.MapMeshDirty(intVec, dirtyFlags, true, false);
				map.pathing.RecalculatePerceivedPathCostAt(intVec);
			}
			map.roofGrid.Drawer.SetDirty();
			GravshipUtility.generatingGravship = false;
			return gravship;
		}

		// Token: 0x06013537 RID: 79159 RVA: 0x005BFD00 File Offset: 0x005BDF00
		public static void AbandonMap(Map map)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			List<Pawn> list = map.mapPawns.AllPawnsSpawned.ToList<Pawn>();
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(list, PawnDiedOrDownedThoughtsKind.Lost);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				list[i].Notify_LeftBehind();
				if (list[i].Spawned)
				{
					list[i].DeSpawn(DestroyMode.Vanish);
				}
				if (!list[i].Destroyed)
				{
					list[i].DestroyOrPassToWorld(DestroyMode.Vanish);
				}
			}
			map.Parent.Abandon(true);
		}

		// Token: 0x06013538 RID: 79160 RVA: 0x005BFD94 File Offset: 0x005BDF94
		public static void ArriveExistingMap(Gravship gravship)
		{
			GravshipUtility.<>c__DisplayClass18_0 CS$<>8__locals1 = new GravshipUtility.<>c__DisplayClass18_0();
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			GravshipUtility.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			MapParent mapParent = Find.WorldObjects.MapParentAt(gravship.destinationTile);
			CS$<>8__locals2.map = ((mapParent != null) ? mapParent.Map : null);
			if (CS$<>8__locals1.map == null)
			{
				Log.Error(string.Format("Tried to arrive gravship {0} at tile {1} but no map exists there.", gravship.Label, gravship.destinationTile));
				return;
			}
			Find.GravshipController.landingMap = CS$<>8__locals1.map;
			GravshipLandingMarker gravshipLandingMarker = ThingMaker.MakeThing(ThingDefOf.GravshipLandingMarker, null) as GravshipLandingMarker;
			gravshipLandingMarker.gravship = gravship;
			gravshipLandingMarker.CacheCells();
			Find.GravshipController.Notify_LandingAreaConfirmationStarted(gravshipLandingMarker);
			Find.WorldObjects.Remove(gravship);
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (Prefs.PauseOnLoad)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				}
				CameraJumper.TryJump(CS$<>8__locals1.map.Center, CS$<>8__locals1.map, CameraJumper.MovementMode.Cut);
				Find.DesignatorManager.Select(Find.GravshipController.MoveDesignator());
			});
		}

		// Token: 0x06013539 RID: 79161 RVA: 0x005BFE50 File Offset: 0x005BE050
		public static void ArriveNewMap(Gravship gravship)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			PlanetTile destinationTile = gravship.destinationTile;
			MapParent mapParent = Find.WorldObjects.MapParentAt(destinationTile);
			if (mapParent == null)
			{
				if (destinationTile.LayerDef.DefaultWorldObject == WorldObjectDefOf.Settlement)
				{
					mapParent = SettleUtility.AddNewHome(destinationTile, Faction.OfPlayer);
				}
				else
				{
					mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(destinationTile.LayerDef.DefaultWorldObject);
					mapParent.Tile = destinationTile;
					Find.WorldObjects.Add(mapParent);
				}
				if (mapParent.def.canHaveFaction && mapParent.Faction == null)
				{
					mapParent.SetFaction(Faction.OfPlayer);
				}
				mapParent.Tile = destinationTile;
				Settlement settlement = mapParent as Settlement;
				if (settlement != null)
				{
					settlement.Name = "GravshipLandingSite".Translate(gravship.Label).CapitalizeFirst();
					settlement.namedByPlayer = true;
				}
			}
			else if (mapParent.def.canHaveFaction && mapParent.Faction == null)
			{
				mapParent.SetFaction(Faction.OfPlayer);
			}
			IntVec3 intVec = Find.World.info.initialMapSize;
			if (mapParent.def.overrideMapSize != null)
			{
				intVec = mapParent.def.overrideMapSize.Value;
			}
			else if (mapParent.def.canResizeToGravship)
			{
				intVec.x = Mathf.RoundToInt(Mathf.Ceil((float)gravship.Bounds.maxX / 100f) * 100f);
				intVec.z = Mathf.RoundToInt(Mathf.Ceil((float)gravship.Bounds.maxZ / 100f) * 100f);
				intVec.y = 1;
			}
			Site site = mapParent as Site;
			if (site != null)
			{
				IntVec3 preferredMapSize = site.PreferredMapSize;
				intVec = new IntVec3(Mathf.Max(intVec.x, preferredMapSize.x), Mathf.Max(intVec.y, preferredMapSize.y), Mathf.Max(intVec.z, preferredMapSize.z));
				if (site.MainSitePartDef.minMapSize != null)
				{
					IntVec3 value = site.MainSitePartDef.minMapSize.Value;
					intVec = new IntVec3(Mathf.Max(intVec.x, value.x), Mathf.Max(intVec.y, value.y), Mathf.Max(intVec.z, value.z));
				}
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(destinationTile, intVec, mapParent.def, GravshipUtility.GetGenSteps(gravship), false);
			bool flag;
			if (!(orGenerateMap.Parent is Settlement))
			{
				Site site2 = orGenerateMap.Parent as Site;
				if (site2 != null)
				{
					flag = site2.parts.All((SitePart part) => part.def.considerEnteringAsAttack);
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && orGenerateMap.Parent.Faction != null && orGenerateMap.Parent.Faction != Faction.OfPlayer)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				TaggedString label = "LetterLabelGravshipEnteredEnemyBase".Translate();
				TaggedString text = "LetterGravshipEnteredEnemyBase".Translate(orGenerateMap.Parent.Label.ApplyTag(TagType.Settlement, orGenerateMap.Parent.Faction.GetUniqueLoadID())).CapitalizeFirst();
				SettlementUtility.AffectRelationsOnAttacked(orGenerateMap.Parent, ref text);
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(from p in orGenerateMap.mapPawns.AllPawns
				where !gravship.ContainsPawn(p)
				select p, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, null, 0, true);
				Find.GoodwillSituationManager.RecalculateAll(true);
			}
			if (gravship.Spawned)
			{
				Find.WorldObjects.Remove(gravship);
			}
			TaleRecorder.RecordTale(TaleDefOf.TileSettled, Array.Empty<object>()).customLabel = "NewSettlement".Translate();
			if (Find.IdeoManager != null)
			{
				Find.IdeoManager.lastResettledTick = GenTicks.TicksGame;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (Prefs.PauseOnLoad)
				{
					Find.TickManager.DoSingleTick();
					Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
				}
				CameraJumper.TryJump(gravship.Engine, CameraJumper.MovementMode.Cut);
				Find.CameraDriver.shaker.DoShake(0.1f, 180);
			});
		}

		// Token: 0x0601353A RID: 79162 RVA: 0x005C0290 File Offset: 0x005BE490
		public static int ThingSpawnPriority(Thing a)
		{
			if (!ModLister.CheckOdyssey("Gravship thing spawn priority"))
			{
				return 0;
			}
			if (a is Building_Bed)
			{
				return 20;
			}
			BuildingProperties building = a.def.building;
			if (building != null && building.isAttachment)
			{
				return 10;
			}
			if (a.def.category == ThingCategory.Item)
			{
				return 20;
			}
			return a.def.gravshipSpawnPriority;
		}

		// Token: 0x0601353B RID: 79163 RVA: 0x005C02F0 File Offset: 0x005BE4F0
		public static bool TryGetPathFuelCost(PlanetTile from, PlanetTile to, out float cost, out int distance, float fuelPerTile = 10f, float fuelFactor = 1f)
		{
			cost = 0f;
			distance = 0;
			if (!ModLister.CheckOdyssey("Gravship path fuel cost"))
			{
				return false;
			}
			if (GravshipUtility.cachedOrigin == from && GravshipUtility.cachedDest == to)
			{
				cost = GravshipUtility.cachedCost;
				distance = GravshipUtility.cachedDistance;
				return GravshipUtility.cachedResult;
			}
			GravshipUtility.cachedOrigin = from;
			GravshipUtility.cachedDest = to;
			GravshipUtility.cachedCost = (cost = 0f);
			GravshipUtility.cachedDistance = (distance = 0);
			if (from.Layer != to.Layer)
			{
				if (GravshipUtility.cachedOriginLayer == from.Layer && GravshipUtility.cachedDestLayer == to.Layer)
				{
					cost += GravshipUtility.cachedLayerCost;
				}
				else
				{
					float num;
					if (!from.Layer.TryGetPath(to.Layer, GravshipUtility.connections, out num, 2147483647))
					{
						return GravshipUtility.cachedResult = false;
					}
					GravshipUtility.cachedOriginLayer = to.Layer;
					GravshipUtility.cachedDestLayer = from.Layer;
					cost += (GravshipUtility.cachedLayerCost = num);
					GravshipUtility.connections.Clear();
				}
				from = to.Layer.GetClosestTile(from);
			}
			GravshipUtility.cachedDistance = (distance = Find.WorldGrid.TraversalDistanceBetween(from, to, true, int.MaxValue, false));
			cost += (float)distance * to.LayerDef.rangeDistanceFactor * (fuelPerTile * fuelFactor);
			GravshipUtility.cachedCost = (cost = Mathf.Max(cost, 50f));
			return GravshipUtility.cachedResult = true;
		}

		// Token: 0x0601353C RID: 79164 RVA: 0x005C0460 File Offset: 0x005BE660
		public static int MaxDistForFuel(float fuel, PlanetLayer fromLayer, PlanetLayer toLayer, float fuelPerTile = 10f, float fuelFactor = 1f)
		{
			if (fromLayer != toLayer)
			{
				if (GravshipUtility.cachedOriginLayer == fromLayer && GravshipUtility.cachedDestLayer == toLayer)
				{
					fuel -= GravshipUtility.cachedLayerCost;
				}
				else
				{
					float num;
					if (!fromLayer.TryGetPath(toLayer, GravshipUtility.connections, out num, 2147483647))
					{
						return 0;
					}
					GravshipUtility.cachedOriginLayer = toLayer;
					GravshipUtility.cachedDestLayer = fromLayer;
					fuel -= (GravshipUtility.cachedLayerCost = num);
					GravshipUtility.connections.Clear();
				}
			}
			return Mathf.FloorToInt(fuel / (toLayer.Def.rangeDistanceFactor * (fuelPerTile * fuelFactor)));
		}

		// Token: 0x0601353D RID: 79165 RVA: 0x005C04DB File Offset: 0x005BE6DB
		private static IEnumerable<GenStepWithParams> GetGenSteps(Gravship gravship)
		{
			yield return new GenStepWithParams
			{
				def = GenStepDefOf.ReserveGravshipArea,
				parms = new GenStepParams
				{
					gravship = gravship
				}
			};
			yield return new GenStepWithParams
			{
				def = GenStepDefOf.GravshipMarker,
				parms = new GenStepParams
				{
					gravship = gravship
				}
			};
			yield break;
		}

		// Token: 0x0601353E RID: 79166 RVA: 0x005C04EC File Offset: 0x005BE6EC
		public static void SettleTile(Map map)
		{
			if (!ModLister.CheckOdyssey("Gravship settle tile"))
			{
				return;
			}
			MapParent parent = map.Parent;
			Settlement settlement = SettleUtility.AddNewHome(map.Tile, Faction.OfPlayer);
			map.info.parent = settlement;
			if (parent != null)
			{
				settlement.questTags = parent.questTags;
				parent.Notify_MyMapSettled(map);
				parent.Destroy();
			}
			settlement.Notify_MyMapSettled(map);
			Settlement settlement2 = parent as Settlement;
			if (settlement2 != null)
			{
				TaggedString taggedString = "";
				SettlementUtility.AffectRelationsOnAttacked(settlement2, ref taggedString);
			}
		}

		// Token: 0x0601353F RID: 79167 RVA: 0x005C056C File Offset: 0x005BE76C
		public static bool ConsideredKidnapped(Building_GravEngine engine, Pawn pawn)
		{
			return ModsConfig.OdysseyActive && engine.ValidSubstructureAt(pawn.PositionHeld) && pawn.Faction != Faction.OfPlayer && !pawn.IsPlayerControlled && !pawn.IsPrisonerOfColony && !pawn.IsSlaveOfColony && !pawn.Faction.HostileTo(Faction.OfPlayer) && pawn.Faction != null;
		}

		// Token: 0x06013540 RID: 79168 RVA: 0x005C05D6 File Offset: 0x005BE7D6
		public static float LaunchCooldownFromQuality(float quality)
		{
			return GravshipUtility.LaunchCooldownFromQualityCurve.Evaluate(quality);
		}

		// Token: 0x06013541 RID: 79169 RVA: 0x005C05E3 File Offset: 0x005BE7E3
		public static float NegativeLandingOutcomeFromQuality(float quality)
		{
			return GravshipUtility.NegativeLandingOutcomeFromQualityCurve.Evaluate(quality);
		}

		// Token: 0x06013542 RID: 79170 RVA: 0x005C05F0 File Offset: 0x005BE7F0
		public static bool IsOnboardGravship(IntVec3 cell, Building_GravEngine engine, Pawn pawn = null, bool desperate = false)
		{
			return ModsConfig.OdysseyActive && (pawn == null || cell.InAllowedArea(pawn)) && (desperate || !cell.GetRoom(engine.Map).PsychologicallyOutdoors) && engine.ValidSubstructure.Contains(cell);
		}

		// Token: 0x06013543 RID: 79171 RVA: 0x005C0630 File Offset: 0x005BE830
		public static bool TryFindSpotOnGravship(Pawn pawn, Building_GravEngine engine, out IntVec3 spot)
		{
			GravshipUtility.<>c__DisplayClass40_0 CS$<>8__locals1 = new GravshipUtility.<>c__DisplayClass40_0();
			CS$<>8__locals1.engine = engine;
			spot = IntVec3.Invalid;
			bool desperate = false;
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			CS$<>8__locals1.tmpWorkingCellList = new List<IntVec3>();
			Region region = CS$<>8__locals1.<TryFindSpotOnGravship>g__ClosestShipRegion|0(pawn.Position, pawn.MapHeld, pawn, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), false);
			if (region == null)
			{
				desperate = true;
				region = CS$<>8__locals1.<TryFindSpotOnGravship>g__ClosestShipRegion|0(pawn.Position, pawn.MapHeld, pawn, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), true);
			}
			return region != null && CS$<>8__locals1.<TryFindSpotOnGravship>g__TryGetAllowedCellInRegion|1(region, pawn, out spot, desperate);
		}

		// Token: 0x0400D66F RID: 54895
		private static List<ThingDef> allShipComponents;

		// Token: 0x0400D670 RID: 54896
		public static bool generatingGravship = false;

		// Token: 0x0400D671 RID: 54897
		public static readonly Color ConnectedSubstructureColor = ColorLibrary.Cyan;

		// Token: 0x0400D672 RID: 54898
		public static readonly Color DisconnectedSubstructureColor = ColorLibrary.Red;

		// Token: 0x0400D673 RID: 54899
		private const float MinFuelCost = 50f;

		// Token: 0x0400D674 RID: 54900
		private static readonly ProfilerMarker PlayerHasGravEngineMarker = new ProfilerMarker("PlayerHasGravEngine()");

		// Token: 0x0400D675 RID: 54901
		private static readonly List<PlanetLayerConnection> connections = new List<PlanetLayerConnection>();

		// Token: 0x0400D676 RID: 54902
		private static PlanetTile cachedOrigin;

		// Token: 0x0400D677 RID: 54903
		private static PlanetTile cachedDest;

		// Token: 0x0400D678 RID: 54904
		private static float cachedCost;

		// Token: 0x0400D679 RID: 54905
		private static float cachedLayerCost;

		// Token: 0x0400D67A RID: 54906
		private static PlanetLayer cachedOriginLayer;

		// Token: 0x0400D67B RID: 54907
		private static PlanetLayer cachedDestLayer;

		// Token: 0x0400D67C RID: 54908
		private static bool cachedResult;

		// Token: 0x0400D67D RID: 54909
		private static int cachedDistance;

		// Token: 0x0400D67E RID: 54910
		private static readonly SimpleCurve LaunchCooldownFromQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 600000f),
				true
			},
			{
				new CurvePoint(0.25f, 300000f),
				true
			},
			{
				new CurvePoint(0.5f, 120000f),
				true
			},
			{
				new CurvePoint(0.8f, 30000f),
				true
			},
			{
				new CurvePoint(1f, 20000f),
				true
			}
		};

		// Token: 0x0400D67F RID: 54911
		private static readonly SimpleCurve NegativeLandingOutcomeFromQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 0.25f),
				true
			},
			{
				new CurvePoint(1f, 0.02f),
				true
			}
		};
	}
}
