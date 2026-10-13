using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002AAA RID: 10922
	[StaticConstructorOnStartup]
	public class MonumentMarker : Thing
	{
		// Token: 0x1700260E RID: 9742
		// (get) Token: 0x0600F356 RID: 62294 RVA: 0x0046A420 File Offset: 0x00468620
		public override CellRect? CustomRectForSelector
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return new CellRect?(this.sketch.OccupiedRect.MovedBy(base.Position));
			}
		}

		// Token: 0x1700260F RID: 9743
		// (get) Token: 0x0600F357 RID: 62295 RVA: 0x0046A460 File Offset: 0x00468660
		public bool AllDone
		{
			get
			{
				if (!base.Spawned)
				{
					return false;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawned(base.Position + sketchEntity.pos, base.Map))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17002610 RID: 9744
		// (get) Token: 0x0600F358 RID: 62296 RVA: 0x0046A4E4 File Offset: 0x004686E4
		public IntVec2 Size
		{
			get
			{
				return this.sketch.OccupiedSize;
			}
		}

		// Token: 0x17002611 RID: 9745
		// (get) Token: 0x0600F359 RID: 62297 RVA: 0x0046A4F4 File Offset: 0x004686F4
		public Thing FirstDisallowedBuilding
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				List<SketchTerrain> terrain = this.sketch.Terrain;
				for (int i = 0; i < terrain.Count; i++)
				{
					MonumentMarker.tmpAllowedBuildings.Clear();
					SketchThing sketchThing;
					List<SketchThing> list;
					this.sketch.ThingsAt(terrain[i].pos, out sketchThing, out list);
					if (sketchThing != null)
					{
						MonumentMarker.tmpAllowedBuildings.Add(sketchThing.def);
					}
					if (list != null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							MonumentMarker.tmpAllowedBuildings.Add(list[j].def);
						}
					}
					List<Thing> thingList = (terrain[i].pos + base.Position).GetThingList(base.Map);
					for (int k = 0; k < thingList.Count; k++)
					{
						if (thingList[k].def.IsBuildingArtificial && !thingList[k].def.IsBlueprint && !thingList[k].def.IsFrame && !thingList[k].def.building.isPowerConduit && !MonumentMarker.tmpAllowedBuildings.Contains(thingList[k].def))
						{
							return thingList[k];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17002612 RID: 9746
		// (get) Token: 0x0600F35A RID: 62298 RVA: 0x0046A653 File Offset: 0x00468853
		public bool AnyDisallowedBuilding
		{
			get
			{
				return this.FirstDisallowedBuilding != null;
			}
		}

		// Token: 0x17002613 RID: 9747
		// (get) Token: 0x0600F35B RID: 62299 RVA: 0x0046A660 File Offset: 0x00468860
		public SketchEntity FirstEntityWithMissingBlueprint
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawnedOrBlueprintOrFrame(base.Position + sketchEntity.pos, base.Map))
					{
						return sketchEntity;
					}
				}
				return null;
			}
		}

		// Token: 0x17002614 RID: 9748
		// (get) Token: 0x0600F35C RID: 62300 RVA: 0x0046A6E4 File Offset: 0x004688E4
		public bool DisallowedBuildingTicksExpired
		{
			get
			{
				return this.ticksSinceDisallowedBuilding >= 60000;
			}
		}

		// Token: 0x0600F35D RID: 62301 RVA: 0x0046A6F6 File Offset: 0x004688F6
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.sketch.Rotate(base.Rotation);
			}
			if (!ModLister.CheckRoyalty("Monument"))
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600F35E RID: 62302 RVA: 0x0046A728 File Offset: 0x00468928
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(177))
			{
				return;
			}
			bool allDone = this.AllDone;
			if (!this.complete && allDone)
			{
				this.complete = true;
				QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentCompleted", this.Named("SUBJECT"));
			}
			if (this.complete && !allDone)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentDestroyed", this.Named("SUBJECT"));
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				return;
			}
			if (allDone)
			{
				if (this.AnyDisallowedBuilding)
				{
					this.ticksSinceDisallowedBuilding += 177;
					if (this.DisallowedBuildingTicksExpired)
					{
						Messages.Message("MessageMonumentDestroyedBecauseOfDisallowedBuilding".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
						QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentDestroyed", this.Named("SUBJECT"));
						if (!base.Destroyed)
						{
							this.Destroy(DestroyMode.Vanish);
						}
						return;
					}
				}
				else
				{
					this.ticksSinceDisallowedBuilding = 0;
				}
			}
		}

		// Token: 0x0600F35F RID: 62303 RVA: 0x0046A83C File Offset: 0x00468A3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Sketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksSinceDisallowedBuilding, "ticksSinceDisallowedBuilding", 0, false);
			Scribe_Values.Look<bool>(ref this.complete, "complete", false, false);
		}

		// Token: 0x0600F360 RID: 62304 RVA: 0x0046A888 File Offset: 0x00468A88
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawGhost(drawLoc.ToIntVec3(), false, base.Rotation);
		}

		// Token: 0x0600F361 RID: 62305 RVA: 0x0046A8A0 File Offset: 0x00468AA0
		public void DrawGhost(IntVec3 at, bool placingMode, Rot4 rotation)
		{
			CellRect rect = this.sketch.OccupiedRect.MovedBy(at);
			Blueprint_Install thingToIgnore = this.FindMyBlueprint(rect, Find.CurrentMap);
			this.sketch.Rotate(rotation);
			Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null;
			if (placingMode)
			{
				validator = ((SketchEntity entity, IntVec3 offset, List<Thing> things, Map map) => MonumentMarkerUtility.GetFirstAdjacentBuilding(entity, offset, things, map) == null);
			}
			this.sketch.DrawGhost(at, Sketch.SpawnPosType.Unchanged, placingMode, thingToIgnore, validator);
		}

		// Token: 0x0600F362 RID: 62306 RVA: 0x0046A910 File Offset: 0x00468B10
		public Blueprint_Install FindMyBlueprint(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect)
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Blueprint_Install blueprint_Install = thingList[i] as Blueprint_Install;
						if (blueprint_Install != null && blueprint_Install.ThingToInstall == this)
						{
							return blueprint_Install;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600F363 RID: 62307 RVA: 0x0046A9A0 File Offset: 0x00468BA0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!this.AllDone)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelMonumentMarker".Translate(),
					defaultDesc = "CommandCancelMonumentMarkerDesc".Translate(),
					icon = MonumentMarker.CancelCommandTex,
					action = delegate()
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmCancelMonumentMarker".Translate(), delegate()
						{
							QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentCancelled", this.Named("SUBJECT"));
							this.RemovePossiblyRelatedBlueprints();
							this.Uninstall();
						}, true, null, WindowLayer.Dialog));
					}
				};
			}
			bool flag = false;
			foreach (SketchEntity sketchEntity in this.sketch.Entities)
			{
				SketchBuildable sketchBuildable = sketchEntity as SketchBuildable;
				if (sketchBuildable != null && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + base.Position, base.Map) && !sketchEntity.IsSpawningBlocked(sketchEntity.pos + base.Position, base.Map, null, false) && BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable, true) != null)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandPlaceBlueprints".Translate(),
					defaultDesc = "CommandPlaceBlueprintsDesc".Translate(),
					icon = MonumentMarker.PlaceBlueprintsCommandTex,
					action = delegate()
					{
						IEnumerable<ThingDef> enumerable = this.AllowedStuffs();
						if (!enumerable.Any<ThingDef>())
						{
							this.PlaceAllBlueprints(null);
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							return;
						}
						if (enumerable.Count<ThingDef>() == 1)
						{
							this.PlaceAllBlueprints(enumerable.First<ThingDef>());
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							return;
						}
						this.ListFloatMenuOptions(enumerable, delegate(ThingDef stuff)
						{
							this.PlaceAllBlueprints(stuff);
						});
					}
				};
			}
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				bool flag2 = false;
				foreach (SketchEntity sketchEntity2 in this.sketch.Entities)
				{
					if (!sketchEntity2.IsSameSpawned(sketchEntity2.pos + base.Position, base.Map) && !sketchEntity2.IsSpawningBlocked(sketchEntity2.pos + base.Position, base.Map, null, false))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Build all",
						action = delegate()
						{
							this.DebugBuildAll();
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					};
				}
				if (this.AllDone && this.AnyDisallowedBuilding && !this.DisallowedBuildingTicksExpired)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Disallowed building ticks +6 hours",
						action = delegate()
						{
							this.ticksSinceDisallowedBuilding += 15000;
						}
					};
				}
			}
			MonumentMarker.tmpUniqueBuildableDefs.Clear();
			foreach (SketchEntity sketchEntity3 in this.sketch.Entities)
			{
				SketchBuildable buildable = sketchEntity3 as SketchBuildable;
				if (buildable != null && !sketchEntity3.IsSameSpawnedOrBlueprintOrFrame(sketchEntity3.pos + base.Position, base.Map))
				{
					if (MonumentMarker.tmpUniqueBuildableDefs.Add(new Pair<BuildableDef, ThingDef>(buildable.Buildable, buildable.Stuff)))
					{
						SketchTerrain sketchTerrain = buildable as SketchTerrain;
						if (sketchTerrain != null && sketchTerrain.treatSimilarAsSame)
						{
							TerrainDef terrain = buildable.Buildable as TerrainDef;
							if (terrain.designatorDropdown != null)
							{
								Designator designator = BuildCopyCommandUtility.FindAllowedDesignatorRoot(buildable.Buildable, true);
								if (designator != null)
								{
									yield return designator;
								}
							}
							else
							{
								IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
								foreach (TerrainDef terrainDef in allDefs)
								{
									if (terrainDef.BuildableByPlayer && terrainDef.designatorDropdown == null)
									{
										bool flag3 = true;
										for (int i = 0; i < terrain.affordances.Count; i++)
										{
											if (!terrainDef.affordances.Contains(terrain.affordances[i]))
											{
												flag3 = false;
												break;
											}
										}
										if (flag3)
										{
											Command command = BuildCopyCommandUtility.BuildCommand(terrainDef, null, null, null, false, null, null, false, null);
											if (command != null)
											{
												yield return command;
											}
										}
									}
								}
								IEnumerator<TerrainDef> enumerator4 = null;
							}
							terrain = null;
						}
						else
						{
							Command command2 = BuildCopyCommandUtility.BuildCommand(buildable.Buildable, buildable.Stuff, null, null, false, sketchEntity3.LabelCap, buildable.Buildable.description, false, null);
							if (command2 != null)
							{
								yield return command2;
							}
						}
						Command_Action placeBlueprintsCommand = this.GetPlaceBlueprintsCommand(buildable);
						if (placeBlueprintsCommand != null)
						{
							yield return placeBlueprintsCommand;
						}
					}
					buildable = null;
				}
			}
			List<SketchEntity>.Enumerator enumerator3 = default(List<SketchEntity>.Enumerator);
			MonumentMarker.tmpUniqueBuildableDefs.Clear();
			yield break;
			yield break;
		}

		// Token: 0x0600F364 RID: 62308 RVA: 0x0046A9B0 File Offset: 0x00468BB0
		private Command_Action GetPlaceBlueprintsCommand(SketchBuildable buildable)
		{
			Action<ThingDef> <>9__1;
			return new Command_Action
			{
				defaultLabel = "CommandPlaceBlueprintsSpecific".Translate(buildable.Label).CapitalizeFirst(),
				defaultDesc = "CommandPlaceBlueprintsSpecificDesc".Translate(buildable.Label).CapitalizeFirst(),
				icon = MonumentMarker.PlaceBlueprintsCommandTex,
				Order = 2000f,
				action = delegate()
				{
					List<ThingDef> list = this.AllowedStuffsFor(buildable);
					if (!list.Any<ThingDef>())
					{
						this.PlaceBlueprintsSimilarTo(buildable, null);
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					if (list.Count<ThingDef>() == 1)
					{
						this.PlaceBlueprintsSimilarTo(buildable, list.First<ThingDef>());
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					MonumentMarker <>4__this = this;
					IEnumerable<ThingDef> allowedStuff = list;
					Action<ThingDef> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(ThingDef stuff)
						{
							this.PlaceBlueprintsSimilarTo(buildable, stuff);
						});
					}
					<>4__this.ListFloatMenuOptions(allowedStuff, action);
				}
			};
		}

		// Token: 0x0600F365 RID: 62309 RVA: 0x0046AA58 File Offset: 0x00468C58
		private void ListFloatMenuOptions(IEnumerable<ThingDef> allowedStuff, Action<ThingDef> action)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			bool flag = false;
			foreach (ThingDef def in allowedStuff)
			{
				if (base.Map.listerThings.ThingsOfDef(def).Count > 0)
				{
					flag = true;
					break;
				}
			}
			foreach (ThingDef thingDef in allowedStuff)
			{
				if (!flag || base.Map.listerThings.ThingsOfDef(thingDef).Count != 0)
				{
					ThingDef stuffLocal = thingDef;
					list.Add(new FloatMenuOption(stuffLocal.LabelCap, delegate()
					{
						action(stuffLocal);
					}, thingDef, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600F366 RID: 62310 RVA: 0x0046AB88 File Offset: 0x00468D88
		public void DebugBuildAll()
		{
			this.sketch.Spawn(base.Map, base.Position, Faction.OfPlayer, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, false, null, null, null);
		}

		// Token: 0x0600F367 RID: 62311 RVA: 0x0046ABBC File Offset: 0x00468DBC
		private void PlaceBlueprintsSimilarTo(SketchBuildable buildable, ThingDef preferredStuffIfNone)
		{
			bool flag = buildable is SketchTerrain;
			foreach (SketchBuildable sketchBuildable in this.sketch.Buildables)
			{
				if (flag)
				{
					SketchTerrain sketchTerrain = sketchBuildable as SketchTerrain;
					if (sketchTerrain != null && sketchTerrain.IsSameOrSimilar(buildable.Buildable))
					{
						goto IL_5E;
					}
				}
				if (flag)
				{
					continue;
				}
				SketchThing sketchThing = sketchBuildable as SketchThing;
				if (sketchThing == null || buildable.Buildable != sketchThing.def)
				{
					continue;
				}
				IL_5E:
				MonumentMarker.tmpBuildables.Add(sketchBuildable);
			}
			foreach (SketchBuildable entity in MonumentMarker.tmpBuildables)
			{
				this.PlaceBlueprint(entity, preferredStuffIfNone);
			}
			MonumentMarker.tmpBuildables.Clear();
		}

		// Token: 0x0600F368 RID: 62312 RVA: 0x0046ACAC File Offset: 0x00468EAC
		private void PlaceAllBlueprints(ThingDef preferredStuffIfNone)
		{
			foreach (SketchEntity entity in this.sketch.Entities)
			{
				this.PlaceBlueprint(entity, preferredStuffIfNone);
			}
		}

		// Token: 0x0600F369 RID: 62313 RVA: 0x0046AD08 File Offset: 0x00468F08
		private void PlaceBlueprint(SketchEntity entity, ThingDef preferredStuffIfNone)
		{
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable == null)
			{
				return;
			}
			if (entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + base.Position, base.Map))
			{
				return;
			}
			if (entity.IsSpawningBlocked(entity.pos + base.Position, base.Map, null, false))
			{
				return;
			}
			if (BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable, true) == null)
			{
				return;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null && sketchThing.def.MadeFromStuff && sketchThing.stuff == null && preferredStuffIfNone != null && preferredStuffIfNone.stuffProps.CanMake(sketchThing.def))
			{
				sketchThing.stuff = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, false, null, false, null);
				sketchThing.stuff = null;
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null && sketchTerrain.stuffForComparingSimilar == null && preferredStuffIfNone != null)
			{
				sketchTerrain.stuffForComparingSimilar = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, false, null, false, null);
				sketchTerrain.stuffForComparingSimilar = null;
				return;
			}
			entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, false, null, false, null);
		}

		// Token: 0x0600F36A RID: 62314 RVA: 0x0046AE54 File Offset: 0x00469054
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Quest quest = Find.QuestManager.QuestsListForReading.FirstOrDefault((Quest q) => q.QuestLookTargets.Contains(this));
			if (quest != null)
			{
				stringBuilder.Append("Quest".Translate() + ": " + quest.name);
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			if (base.Spawned && !this.AllDone)
			{
				MonumentMarker.tmpBuiltParts.Clear();
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					Pair<int, int> value;
					if (!MonumentMarker.tmpBuiltParts.TryGetValue(sketchEntity.LabelCap, out value))
					{
						value = default(Pair<int, int>);
					}
					if (sketchEntity.IsSameSpawned(sketchEntity.pos + base.Position, base.Map))
					{
						value = new Pair<int, int>(value.First + 1, value.Second + 1);
					}
					else
					{
						value = new Pair<int, int>(value.First, value.Second + 1);
					}
					MonumentMarker.tmpBuiltParts[sketchEntity.LabelCap] = value;
				}
				foreach (KeyValuePair<string, Pair<int, int>> keyValuePair in MonumentMarker.tmpBuiltParts)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(string.Concat(new string[]
					{
						keyValuePair.Key.CapitalizeFirst(),
						": ",
						keyValuePair.Value.First.ToString(),
						" / ",
						keyValuePair.Value.Second.ToString()
					}));
				}
				MonumentMarker.tmpBuiltParts.Clear();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600F36B RID: 62315 RVA: 0x0046B06C File Offset: 0x0046926C
		private void RemovePossiblyRelatedBlueprints()
		{
			if (!base.Spawned)
			{
				return;
			}
			foreach (SketchBuildable sketchBuildable in this.sketch.Buildables)
			{
				Blueprint blueprint = sketchBuildable.GetSpawnedBlueprintOrFrame(base.Position + sketchBuildable.pos, base.Map) as Blueprint;
				if (blueprint != null)
				{
					blueprint.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600F36C RID: 62316 RVA: 0x0046B0F4 File Offset: 0x004692F4
		public bool IsPart(Thing thing)
		{
			MonumentMarker.<>c__DisplayClass40_0 CS$<>8__locals1;
			CS$<>8__locals1.thing = thing;
			CS$<>8__locals1.<>4__this = this;
			if (!base.Spawned)
			{
				return false;
			}
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Contains(CS$<>8__locals1.thing.Position))
			{
				return false;
			}
			SketchThing sketchThing;
			List<SketchThing> list;
			this.sketch.ThingsAt(CS$<>8__locals1.thing.Position - base.Position, out sketchThing, out list);
			if (sketchThing != null && this.<IsPart>g__IsPartInternal|40_0(sketchThing, ref CS$<>8__locals1))
			{
				return true;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<IsPart>g__IsPartInternal|40_0(list[i], ref CS$<>8__locals1))
					{
						return true;
					}
				}
			}
			if (CS$<>8__locals1.thing.def.entityDefToBuild != null)
			{
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(CS$<>8__locals1.thing.Position - base.Position);
				if (sketchTerrain != null && this.<IsPart>g__IsPartInternal|40_0(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F36D RID: 62317 RVA: 0x0046B1F4 File Offset: 0x004693F4
		public bool AllowsPlacingBlueprint(BuildableDef buildable, IntVec3 pos, Rot4 rot, ThingDef stuff)
		{
			MonumentMarker.<>c__DisplayClass41_0 CS$<>8__locals1;
			CS$<>8__locals1.buildable = buildable;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.stuff = stuff;
			CS$<>8__locals1.rot = rot;
			if (!base.Spawned)
			{
				return true;
			}
			CS$<>8__locals1.newRect = GenAdj.OccupiedRect(pos, CS$<>8__locals1.rot, CS$<>8__locals1.buildable.Size);
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Overlaps(CS$<>8__locals1.newRect))
			{
				return true;
			}
			CS$<>8__locals1.collided = false;
			foreach (IntVec3 a in CS$<>8__locals1.newRect)
			{
				SketchThing sketchThing;
				List<SketchThing> list;
				this.sketch.ThingsAt(a - base.Position, out sketchThing, out list);
				if (sketchThing != null && this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(sketchThing, ref CS$<>8__locals1))
				{
					return true;
				}
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(list[i], ref CS$<>8__locals1))
						{
							return true;
						}
					}
				}
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(a - base.Position);
				if (sketchTerrain != null && this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return !CS$<>8__locals1.collided;
		}

		// Token: 0x0600F36E RID: 62318 RVA: 0x0046B364 File Offset: 0x00469564
		public IEnumerable<ThingDef> AllowedStuffs()
		{
			MonumentMarker.tmpStuffCategories.Clear();
			bool flag = true;
			List<SketchThing> things = this.sketch.Things;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.MadeFromStuff && things[i].stuff == null)
				{
					if (flag)
					{
						flag = false;
						MonumentMarker.tmpStuffCategories.AddRange(things[i].def.stuffCategories);
					}
					else
					{
						bool flag2 = false;
						for (int j = 0; j < things[i].def.stuffCategories.Count; j++)
						{
							if (MonumentMarker.tmpStuffCategories.Contains(things[i].def.stuffCategories[j]))
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							for (int k = MonumentMarker.tmpStuffCategories.Count - 1; k >= 0; k--)
							{
								if (!things[i].def.stuffCategories.Contains(MonumentMarker.tmpStuffCategories[k]))
								{
									MonumentMarker.tmpStuffCategories.RemoveAt(k);
								}
							}
						}
					}
				}
			}
			return GenStuff.AllowedStuffs(MonumentMarker.tmpStuffCategories, TechLevel.Undefined);
		}

		// Token: 0x0600F36F RID: 62319 RVA: 0x0046B494 File Offset: 0x00469694
		public List<ThingDef> AllowedStuffsFor(SketchBuildable buildable)
		{
			if (buildable.Buildable.MadeFromStuff && buildable.Stuff == null)
			{
				return GenStuff.AllowedStuffs(buildable.Buildable.stuffCategories, TechLevel.Undefined).ToList<ThingDef>();
			}
			SketchTerrain sketchTerrain = buildable as SketchTerrain;
			if (sketchTerrain != null)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
				{
					if (terrainDef.BuildableByPlayer && sketchTerrain.IsSameOrSimilar(terrainDef) && !terrainDef.CostList.NullOrEmpty<ThingDefCountClass>())
					{
						list.Add(terrainDef.CostList.First<ThingDefCountClass>().thingDef);
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x0600F37A RID: 62330 RVA: 0x0046B6BC File Offset: 0x004698BC
		[CompilerGenerated]
		private bool <IsPart>g__IsPartInternal|40_0(SketchBuildable b, ref MonumentMarker.<>c__DisplayClass40_0 A_2)
		{
			BuildableDef buildable = b.Buildable;
			if (A_2.thing.def != buildable && A_2.thing.def.entityDefToBuild != buildable)
			{
				return false;
			}
			if (b.GetSpawnedBlueprintOrFrame(b.pos + base.Position, base.Map) == A_2.thing)
			{
				return true;
			}
			SketchThing sketchThing = b as SketchThing;
			return sketchThing != null && sketchThing.GetSameSpawned(sketchThing.pos + base.Position, base.Map) == A_2.thing;
		}

		// Token: 0x0600F37B RID: 62331 RVA: 0x0046B74C File Offset: 0x0046994C
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__IsSameOrSimilar|41_0(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass41_0 A_2)
		{
			if (A_2.buildable != entity.Buildable)
			{
				SketchTerrain sketchTerrain = entity as SketchTerrain;
				if (sketchTerrain == null || !sketchTerrain.IsSameOrSimilar(A_2.buildable))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F37C RID: 62332 RVA: 0x0046B784 File Offset: 0x00469984
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__CheckEntity|41_1(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass41_0 A_2)
		{
			if (entity.IsSameSpawned(entity.pos + base.Position, base.Map))
			{
				return false;
			}
			if (entity.OccupiedRect.MovedBy(base.Position).Overlaps(A_2.newRect))
			{
				A_2.collided = true;
			}
			SketchThing sketchThing = entity as SketchThing;
			return entity.OccupiedRect.MovedBy(base.Position).Equals(A_2.newRect) && this.<AllowsPlacingBlueprint>g__IsSameOrSimilar|41_0(entity, ref A_2) && (A_2.stuff == null || entity.Stuff == null || A_2.stuff == entity.Stuff) && (sketchThing == null || sketchThing.rot == A_2.rot || sketchThing.rot == A_2.rot.Opposite || !sketchThing.def.rotatable);
		}

		// Token: 0x0400A04A RID: 41034
		public Sketch sketch = new Sketch();

		// Token: 0x0400A04B RID: 41035
		public int ticksSinceDisallowedBuilding;

		// Token: 0x0400A04C RID: 41036
		public bool complete;

		// Token: 0x0400A04D RID: 41037
		private static readonly Texture2D PlaceBlueprintsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);

		// Token: 0x0400A04E RID: 41038
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400A04F RID: 41039
		public const int DestroyAfterTicksSinceDisallowedBuilding = 60000;

		// Token: 0x0400A050 RID: 41040
		private const int MonumentCompletedCheckIntervalTicks = 177;

		// Token: 0x0400A051 RID: 41041
		private static List<ThingDef> tmpAllowedBuildings = new List<ThingDef>();

		// Token: 0x0400A052 RID: 41042
		private static HashSet<Pair<BuildableDef, ThingDef>> tmpUniqueBuildableDefs = new HashSet<Pair<BuildableDef, ThingDef>>();

		// Token: 0x0400A053 RID: 41043
		private static List<SketchBuildable> tmpBuildables = new List<SketchBuildable>();

		// Token: 0x0400A054 RID: 41044
		private static Dictionary<string, Pair<int, int>> tmpBuiltParts = new Dictionary<string, Pair<int, int>>();

		// Token: 0x0400A055 RID: 41045
		private static List<StuffCategoryDef> tmpStuffCategories = new List<StuffCategoryDef>();
	}
}
