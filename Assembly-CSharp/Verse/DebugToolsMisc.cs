using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020009DB RID: 2523
	public static class DebugToolsMisc
	{
		// Token: 0x06003F9A RID: 16282 RVA: 0x0014CC8C File Offset: 0x0014AE8C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AttachFire()
		{
			using (List<Thing>.Enumerator enumerator = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing t = enumerator.Current;
					if (t.CanEverAttachFire())
					{
						t.TryAttachFire(1f, null);
					}
					else
					{
						FireUtility.TryStartFireIn(UI.MouseCell(), Find.CurrentMap, 1.75f, null, null);
					}
				}
			}
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0014CD1C File Offset: 0x0014AF1C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode SetStuff()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			using (IEnumerator<ThingDef> enumerator = (from x in DefDatabase<ThingDef>.AllDefs
			where x.IsStuff
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef stuff = enumerator.Current;
					ThingDef stuffInner = stuff;
					debugActionNode.AddChild(new DebugActionNode(stuff.LabelCap, DebugActionType.ToolMap, delegate()
					{
						foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
						{
							if (thing.Stuff != null)
							{
								float num = (float)thing.HitPoints / (float)thing.MaxHitPoints;
								thing.SetStuffDirect(stuffInner);
								StatDefOf.MaxHitPoints.Worker.ClearCacheForThing(thing);
								thing.HitPoints = Mathf.CeilToInt((float)thing.MaxHitPoints * num);
								thing.Notify_ColorChanged();
								thing.DirtyMapMesh(thing.Map);
								UnfinishedThing unfinishedThing = thing as UnfinishedThing;
								if (unfinishedThing != null)
								{
									int num2 = 0;
									int num3 = 100;
									foreach (Thing thing2 in unfinishedThing.ingredients)
									{
										num2 += thing2.stackCount;
									}
									unfinishedThing.ingredients.Clear();
									while (num2 > 0 && num3-- > 0)
									{
										Thing thing3 = ThingMaker.MakeThing(stuff, null);
										thing3.stackCount = Mathf.Min(num2, stuff.stackLimit);
										unfinishedThing.ingredients.Add(thing3);
										num2 -= thing3.stackCount;
									}
								}
							}
						}
					}, null));
				}
			}
			return debugActionNode;
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0014CDCC File Offset: 0x0014AFCC
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static DebugActionNode SetQuality()
		{
			DebugActionNode debugActionNode = new DebugActionNode();
			foreach (object obj in Enum.GetValues(typeof(QualityCategory)))
			{
				QualityCategory qualityInner2 = (QualityCategory)obj;
				QualityCategory qualityInner = qualityInner2;
				debugActionNode.AddChild(new DebugActionNode(qualityInner.ToString(), DebugActionType.ToolMap, delegate()
				{
					foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap))
					{
						CompQuality compQuality = thing.TryGetComp<CompQuality>();
						if (compQuality != null)
						{
							compQuality.SetQuality(qualityInner, new ArtGenerationContext?(ArtGenerationContext.Outsider));
						}
					}
				}, null));
			}
			return debugActionNode;
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0014CE68 File Offset: 0x0014B068
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		public static void MeasureDrawSize()
		{
			Vector3 first = Vector3.zero;
			Action <>9__1;
			DebugTools.curMeasureTool = new DrawMeasureTool("first corner...", delegate()
			{
				first = UI.MouseMapPosition();
				string label = "second corner...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						Vector3 vector = UI.MouseMapPosition();
						Rect rect = default(Rect);
						rect.xMin = Mathf.Min(first.x, vector.x);
						rect.yMin = Mathf.Min(first.z, vector.z);
						rect.xMax = Mathf.Max(first.x, vector.x);
						rect.yMax = Mathf.Max(first.z, vector.z);
						string text = string.Format("Center: ({0},{1})", rect.center.x, rect.center.y);
						text += string.Format("\nSize: ({0},{1})", rect.size.x, rect.size.y);
						if (Find.Selector.SingleSelectedObject != null)
						{
							Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
							Vector3 drawPos = singleSelectedThing.DrawPos;
							Vector2 vector2 = rect.center - new Vector2(drawPos.x, drawPos.z);
							text += string.Format("\nOffset: ({0},{1})", vector2.x, vector2.y);
							Vector2 vector3 = vector2.RotatedBy(-singleSelectedThing.Rotation.AsAngle);
							text += string.Format("\nUnrotated offset: ({0},{1})", vector3.x, vector3.y);
						}
						Log.Message(text);
						DebugToolsMisc.MeasureDrawSize();
					});
				}
				DebugTools.curMeasureTool = new DrawMeasureTool(label, clickAction, first);
			}, null);
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x0014CEA4 File Offset: 0x0014B0A4
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		public static void MeasureWorldDistance()
		{
			Vector3 first = Vector3.zero;
			Action <>9__1;
			DebugTools.curMeasureTool = new MeasureWorldDistanceTool("First Point...", delegate()
			{
				first = UI.MouseMapPosition();
				string label = "Second Point...";
				Action clickAction;
				if ((clickAction = <>9__1) == null)
				{
					clickAction = (<>9__1 = delegate()
					{
						Vector3 vector = UI.MouseMapPosition() - first;
						Log.Message(string.Format("Vector: {0}, Distance:{1}", vector, vector.magnitude));
						DebugToolsMisc.MeasureWorldDistance();
					});
				}
				DebugTools.curMeasureTool = new MeasureWorldDistanceTool(label, clickAction, first);
			}, null);
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x0014CEE0 File Offset: 0x0014B0E0
		[DebugAction("General", "Draw Attach Points", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap)]
		public static void DrawAttachPoints()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				ThingWithComps thingWithComps = thing as ThingWithComps;
				if (thingWithComps != null)
				{
					CompAttachPoints comp = thingWithComps.GetComp<CompAttachPoints>();
					if (comp != null)
					{
						int ttl = 500;
						DebugTools.curTool = new DebugTool("attach point drawer", delegate()
						{
						}, delegate()
						{
							int ttl = ttl;
							ttl--;
							if (ttl <= 0)
							{
								DebugTools.curTool = null;
							}
							foreach (AttachPointType type in comp.points.PointTypes(0, int.MaxValue))
							{
								Vector3 worldPos = comp.points.GetWorldPos(type);
								GenMapUI.DrawText(new Vector2(worldPos.x, worldPos.z), type.ToString(), Color.red);
								GenDraw.DrawCircleOutline(worldPos, 0.4f);
							}
						});
					}
				}
			}
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x0014CFA8 File Offset: 0x0014B1A8
		[DebugAction("General", "Pollution +1%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void IncreasePollutionSmall()
		{
			PlanetTile root = GenWorld.MouseTile(false);
			if (root.Valid)
			{
				WorldPollutionUtility.PolluteWorldAtTile(root, 0.01f);
			}
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x0014CFD0 File Offset: 0x0014B1D0
		[DebugAction("General", "Pollution +25%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void IncreasePollutionLarge()
		{
			PlanetTile root = GenWorld.MouseTile(false);
			if (root.Valid)
			{
				WorldPollutionUtility.PolluteWorldAtTile(root, 0.25f);
			}
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x0014CFF8 File Offset: 0x0014B1F8
		[DebugAction("General", "Pollution -25%", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresBiotech = true)]
		private static void DecreasePollutionLarge()
		{
			PlanetTile root = GenWorld.MouseTile(false);
			if (root.Valid)
			{
				WorldPollutionUtility.PolluteWorldAtTile(root, -0.25f);
			}
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x0014D020 File Offset: 0x0014B220
		[DebugAction("General", "Set biome", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static List<DebugActionNode> SetBiome()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (BiomeDef biomeDef in DefDatabase<BiomeDef>.AllDefsListForReading)
			{
				BiomeDef local = biomeDef;
				if (biomeDef.implemented && biomeDef.generatesNaturally)
				{
					list.Add(new DebugActionNode(biomeDef.label, DebugActionType.ToolWorld, delegate()
					{
						PlanetTile planetTile = GenWorld.MouseTile(false);
						if (!planetTile.Valid || planetTile.Tile.PrimaryBiome == local)
						{
							return;
						}
						planetTile.Tile.PrimaryBiome = local;
						Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(PlanetLayer.Selected).RegenerateNow();
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0014D0B0 File Offset: 0x0014B2B0
		[DebugAction("General", "Set landmark", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresOdyssey = true)]
		private static List<DebugActionNode> SetLandmark()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (LandmarkDef landmarkDef in DefDatabase<LandmarkDef>.AllDefsListForReading)
			{
				LandmarkDef local = landmarkDef;
				list.Add(new DebugActionNode(landmarkDef.defName, DebugActionType.ToolWorld, delegate()
				{
					PlanetTile tile = GenWorld.MouseTile(false);
					if (!tile.Valid || !local.IsValidTile(tile, tile.Layer, false))
					{
						return;
					}
					Find.World.landmarks.AddLandmark(local, tile, null, false);
					Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(tile.Layer).RegenerateNow();
					Find.World.renderer.GetLayer<WorldDrawLayer_Landmarks>(Find.WorldGrid.Surface).RegenerateNow();
					Find.World.renderer.GetLayer<WorldDrawLayer_Hills>(Find.WorldGrid.Surface).RegenerateNow();
				}, null));
			}
			return list;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0014D130 File Offset: 0x0014B330
		[DebugAction("General", "Clear Landmark", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld, requiresOdyssey = true)]
		private static void ClearLandmark()
		{
			PlanetTile tile = GenWorld.MouseTile(false);
			if (tile.Valid)
			{
				Find.World.landmarks.RemoveLandmark(tile);
				Find.World.renderer.GetLayer<WorldDrawLayer_Terrain>(tile.Layer).RegenerateNow();
			}
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0014D178 File Offset: 0x0014B378
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresBiotech = true)]
		private static void ResetBossgroupCooldown()
		{
			Find.BossgroupManager.lastBossgroupCalled = Find.TickManager.TicksGame - 120000;
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0014D194 File Offset: 0x0014B394
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresBiotech = true)]
		private static void ResetBossgroupKilledPawns()
		{
			Find.BossgroupManager.DebugResetDefeatedPawns();
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0014D1A0 File Offset: 0x0014B3A0
		[DebugAction("Insect", "Spawn cocoon infestation", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, hideInSubMenu = true, requiresBiotech = true)]
		private static List<DebugActionNode> SpawnCocoonInfestationWithPoints()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (float localP2 in DebugActionsUtility.PointsOptions(false))
			{
				float localP = localP2;
				DebugActionNode item = new DebugActionNode(localP.ToString() + " points", DebugActionType.ToolMap, delegate()
				{
					CocoonInfestationUtility.SpawnCocoonInfestation(UI.MouseCell(), Find.CurrentMap, localP);
				}, null);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0014D22C File Offset: 0x0014B42C
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static void SpawnPitGate()
		{
			IntVec3 loc = UI.MouseMapPosition().ToIntVec3();
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.PitGateSpawner, null), loc, Find.CurrentMap, WipeMode.Vanish);
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x0014D25C File Offset: 0x0014B45C
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static void SpawnPitBurrow()
		{
			FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(UI.MouseMapPosition().ToIntVec3(), Find.CurrentMap, 200f, new IntRange(600, 600), new IntRange(60, 180), true);
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0014D294 File Offset: 0x0014B494
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static void SpawnFleshmassHeart()
		{
			IntVec3 loc = UI.MouseMapPosition().ToIntVec3();
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.FleshmassHeartSpawner, null), loc, Find.CurrentMap, WipeMode.Vanish);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0014D2C4 File Offset: 0x0014B4C4
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresAnomaly = true)]
		private static void DiscoverAllEntities()
		{
			Find.EntityCodex.Debug_DiscoverAll();
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0014D2D0 File Offset: 0x0014B4D0
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action)]
		private static void BenchmarkPerformance()
		{
			Messages.Message(string.Format("Running benchmark, results displayed in {0} seconds", 30f), MessageTypeDefOf.NeutralEvent, false);
			PerformanceBenchmarkUtility.StartBenchmark();
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0014D2F8 File Offset: 0x0014B4F8
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CompareLineOfSight(Pawn pawn)
		{
			foreach (IntVec3 c in GenRadial.RadialCellsAround(UI.MouseCell(), 50f, true))
			{
				if (c.InBounds(Find.CurrentMap))
				{
					Pawn firstPawn = c.GetFirstPawn(Find.CurrentMap);
					if (firstPawn != null)
					{
						bool flag = pawn.CanSee(firstPawn, null);
						bool flag2 = firstPawn.CanSee(pawn, null);
						if (flag != flag2)
						{
							Find.CurrentMap.debugDrawer.FlashCell(c, 1f, null, 50);
						}
					}
				}
			}
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0014D390 File Offset: 0x0014B590
		[DebugAction("General", "Enable wound debug draw", false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void WoundDebug()
		{
			IntVec3 c = UI.MouseCell();
			Pawn pawn = c.GetFirstPawn(Find.CurrentMap);
			if (pawn == null || pawn.def.race == null || pawn.def.race.body == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("All", DebugMenuOptionMode.Action, delegate()
			{
				pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = true;
				pawn.Drawer.renderer.WoundOverlays.ClearCache();
				PortraitsCache.SetDirty(pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
			}));
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord part = allParts[i];
				list.Add(new DebugMenuOption(part.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					pawn.Drawer.renderer.WoundOverlays.debugDrawPart = part;
					pawn.Drawer.renderer.WoundOverlays.ClearCache();
					PortraitsCache.SetDirty(pawn);
					GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0014D494 File Offset: 0x0014B694
		[DebugAction("General", "Wound debug export (non-humanlike)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap, hideInSubMenu = true)]
		private static void WoundDebugExport()
		{
			string text = Application.dataPath + "\\woundDump";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			HashSet<RaceProperties> hashSet = new HashSet<RaceProperties>();
			foreach (PawnKindDef pawnKindDef in from pkd in DefDatabase<PawnKindDef>.AllDefsListForReading
			where !pkd.RaceProps.Humanlike
			select pkd)
			{
				if (!hashSet.Contains(pawnKindDef.RaceProps))
				{
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null, null);
					for (int i = 0; i < 4; i++)
					{
						Rot4 rot = new Rot4((byte)i);
						RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 32, RenderTextureFormat.ARGB32);
						temporary.name = "WoundDebugExport";
						pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = true;
						pawn.Drawer.renderer.WoundOverlays.ClearCache();
						Find.PawnCacheRenderer.RenderPawn(pawn, temporary, Vector3.zero, 1f, 0f, rot, true, true, true, false, default(Vector3), null, null, false);
						pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = false;
						pawn.Drawer.renderer.WoundOverlays.ClearCache();
						Texture2D texture2D = new Texture2D(temporary.width, temporary.height, TextureFormat.ARGB32, 0, false);
						RenderTexture.active = temporary;
						texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0, true);
						RenderTexture.active = null;
						RenderTexture.ReleaseTemporary(temporary);
						string str = text + "\\" + pawn.def.LabelCap + "_";
						Rot4 rot2 = rot;
						File.WriteAllBytes(str + rot2.ToString() + ".png", texture2D.EncodeToPNG());
					}
					pawn.Destroy(DestroyMode.Vanish);
					hashSet.Add(pawnKindDef.RaceProps);
				}
			}
			Log.Message("Dumped to " + text);
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x0014D6F8 File Offset: 0x0014B8F8
		[DebugAction("Anomaly", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresAnomaly = true)]
		private static List<DebugActionNode> EmergeMetalhorrors()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 1; i <= 10; i++)
			{
				int delaySeconds = i;
				list.Add(new DebugActionNode(string.Format("{0}s delay", i), DebugActionType.Action, null, null)
				{
					action = delegate()
					{
						int delayTicks = delaySeconds * 60;
						DelayedMetalhorrorEmerger.Spawn(Find.CurrentMap, delayTicks);
					}
				});
			}
			return list;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0014D756 File Offset: 0x0014B956
		[DebugAction("Mechanoid", null, false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.IsCurrentlyOnMap, requiresOdyssey = true)]
		private static void DisableMechs()
		{
			if (Faction.OfMechanoids == null)
			{
				return;
			}
			Faction.OfMechanoids.deactivated = true;
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x0014D76C File Offset: 0x0014B96C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresOdyssey = true)]
		private static void AddTraitToUniqueWeapon()
		{
			CompUniqueWeapon comp = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault((Thing x) => x.HasComp<CompUniqueWeapon>()).TryGetComp<CompUniqueWeapon>();
			if (comp == null)
			{
				return;
			}
			DebugToolsMisc.tmpTraitDefs.Clear();
			DebugToolsMisc.tmpTraitDefs.AddRange(from trait in DefDatabase<WeaponTraitDef>.AllDefs
			where comp.CanAddTrait(trait)
			select trait);
			if (!DebugToolsMisc.tmpTraitDefs.Any<WeaponTraitDef>())
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WeaponTraitDef weaponTraitDef in DebugToolsMisc.tmpTraitDefs)
			{
				WeaponTraitDef localTrait = weaponTraitDef;
				list.Add(new DebugMenuOption(weaponTraitDef.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					comp.AddTrait(localTrait);
					comp.Setup(true);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0014D894 File Offset: 0x0014BA94
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, requiresOdyssey = true)]
		private static void RemoveTraitFromUniqueWeapon()
		{
			CompUniqueWeapon comp = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault((Thing x) => x.HasComp<CompUniqueWeapon>()).TryGetComp<CompUniqueWeapon>();
			if (comp == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WeaponTraitDef weaponTraitDef in comp.TraitsListForReading)
			{
				WeaponTraitDef localTrait = weaponTraitDef;
				list.Add(new DebugMenuOption(weaponTraitDef.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					comp.TraitsListForReading.Remove(localTrait);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x04002D62 RID: 11618
		private static List<WeaponTraitDef> tmpTraitDefs = new List<WeaponTraitDef>();
	}
}
