using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C6C RID: 7276
	public static class MoveColonyUtility
	{
		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x0600A220 RID: 41504 RVA: 0x002E9393 File Offset: 0x002E7593
		public static bool TitleAndRoleRequirementsGracePeriodActive
		{
			get
			{
				return MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft > 0;
			}
		}

		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x0600A221 RID: 41505 RVA: 0x002E939D File Offset: 0x002E759D
		public static int TitleAndRoleRequirementGracePeriodTicksLeft
		{
			get
			{
				if (!Find.TickManager.HasSettledNewColony)
				{
					return 0;
				}
				return Mathf.Max(0, 600000 - Find.TickManager.TicksSinceSettle);
			}
		}

		// Token: 0x0600A222 RID: 41506 RVA: 0x002E93C4 File Offset: 0x002E75C4
		public static void PickNewColonyTile(Action<PlanetTile> targetChosen, Action noTileChosen = null)
		{
			Find.TilePicker.StartTargeting(delegate(PlanetTile tile)
			{
				MoveColonyUtility.cannotPlaceTileReason.Clear();
				if (!TileFinder.IsValidTileForNewSettlement(tile, MoveColonyUtility.cannotPlaceTileReason, false))
				{
					Messages.Message(MoveColonyUtility.cannotPlaceTileReason.ToString(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				return true;
			}, delegate(PlanetTile tile)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				targetChosen(tile);
			}, null, null, false, noTileChosen, "ChooseNextColonySite".Translate(), true, false, false, false, null);
		}

		// Token: 0x0600A223 RID: 41507 RVA: 0x002E942C File Offset: 0x002E762C
		public static Settlement MoveColonyAndReset(PlanetTile tile, IEnumerable<Thing> colonyThings, Faction takeoverFaction = null, WorldObjectDef worldObjectDef = null)
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.IsEndOnNewArchonexusSettlement())
				{
					quest.hidden = true;
					quest.End(QuestEndOutcome.Unknown, false, true);
				}
			}
			List<Pawn> list = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				if (!colonyThings.Contains(pawn))
				{
					pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
					if (pawn == pawn.Faction.leader)
					{
						pawn.Faction.Notify_LeaderLost();
					}
					if (pawn.IsCaravanMember())
					{
						pawn.GetCaravan().RemovePawn(pawn);
					}
					if (pawn.holdingOwner != null)
					{
						pawn.holdingOwner.Remove(pawn);
					}
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
					if (ModsConfig.BiotechActive && pawn.RaceProps.IsMechanoid)
					{
						Pawn overseer = pawn.GetOverseer();
						if (overseer != null && colonyThings.Contains(overseer))
						{
							overseer.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, pawn);
						}
					}
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = caravans.Count - 1; i >= 0; i--)
			{
				if (caravans[i].IsPlayerControlled)
				{
					caravans[i].RemoveAllPawns();
					caravans[i].Destroy();
				}
			}
			List<TravellingTransporters> travellingTransporters = Find.WorldObjects.TravellingTransporters;
			for (int j = travellingTransporters.Count - 1; j >= 0; j--)
			{
				travellingTransporters[j].Destroy();
			}
			foreach (Thing thing4 in colonyThings)
			{
				thing4.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (thing4.holdingOwner != null)
				{
					thing4.holdingOwner.Remove(thing4);
				}
			}
			List<MapParent> mapParents = Find.World.worldObjects.MapParents;
			for (int k = mapParents.Count - 1; k >= 0; k--)
			{
				mapParents[k].CheckRemoveMapNow();
			}
			MoveColonyUtility.playerSettlementsRemoved.Clear();
			List<Map> maps = Find.Maps;
			for (int l = maps.Count - 1; l >= 0; l--)
			{
				Map map = maps[l];
				if (map.IsPlayerHome)
				{
					MoveColonyUtility.playerSettlementsRemoved.Add(map.Tile);
					map.Parent.SetFaction(null);
					Current.Game.DeinitAndRemoveMap(map, false);
					map.Parent.Destroy();
					foreach (Pawn pawn2 in list)
					{
						if (pawn2.playerSettings != null)
						{
							pawn2.playerSettings.Notify_MapRemoved(map);
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				List<Site> sites = Find.WorldObjects.Sites;
				for (int m = sites.Count - 1; m >= 0; m--)
				{
					if (sites[m].parts.Any((SitePart p) => p.def == SitePartDefOf.AncientComplex))
					{
						Find.WorldObjects.Remove(sites[m]);
					}
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Find.Anomaly.ResetMonolith();
				Find.Anomaly.monolith = null;
			}
			if (ModsConfig.OdysseyActive)
			{
				Find.QuestManager.QuestsListForReading.RemoveAll((Quest q) => q.root == QuestScriptDefOf.MechanoidSignal);
			}
			Find.GameInfo.startingTile = tile;
			WorldObjectDef worldObjectDef2 = worldObjectDef ?? WorldObjectDefOf.Settlement;
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(worldObjectDef2);
			settlement.SetFaction(Faction.OfPlayer);
			settlement.Tile = tile;
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Faction.OfPlayer.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(settlement);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, worldObjectDef2, null);
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			List<List<Thing>> list2 = new List<List<Thing>>();
			List<Pawn> list3 = new List<Pawn>();
			foreach (Thing thing2 in colonyThings)
			{
				if (thing2 is Pawn)
				{
					list2.Add(new List<Thing>
					{
						thing2
					});
					list3.Add((Pawn)thing2);
				}
			}
			int num = 0;
			using (IEnumerator<Thing> enumerator3 = colonyThings.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Thing thing = enumerator3.Current;
					if (thing.def.CanHaveFaction && thing.Faction != Faction.OfPlayer)
					{
						Pawn pawn3 = thing as Pawn;
						if (pawn3 != null && pawn3.IsSlaveOfColony)
						{
							continue;
						}
						thing.SetFaction(Faction.OfPlayer, null);
					}
					if (!list2.Any((List<Thing> g) => g.Contains(thing)))
					{
						list2[num].Add(thing);
						num = (num + 1) % list2.Count;
					}
				}
			}
			foreach (Pawn pawn4 in list3)
			{
				pawn4.inventory.DestroyAll(DestroyMode.Vanish);
				pawn4.ownership.UnclaimThrone();
				pawn4.ownership.UnclaimDeathrestCasket();
				Pawn_GeneTracker genes = pawn4.genes;
				if (genes != null)
				{
					genes.Notify_NewColony();
				}
			}
			MoveColonyUtility.RemoveWeaponsAndUtilityItems(list3, colonyThings);
			foreach (Thing thing3 in MoveColonyUtility.GetAbandonedRelicsCarriedByPawns(colonyThings))
			{
				if (!thing3.DestroyedOrNull())
				{
					thing3.Destroy(DestroyMode.Vanish);
				}
			}
			DropPodUtility.DropThingGroupsNear(playerStartSpot, orGenerateMap, list2, 110, false, false, true, true, true, false, null);
			if (takeoverFaction != null)
			{
				foreach (PlanetTile tile2 in MoveColonyUtility.playerSettlementsRemoved)
				{
					SettleUtility.AddNewHome(tile2, takeoverFaction);
				}
			}
			MoveColonyUtility.playerSettlementsRemoved.Clear();
			List<ResearchProjectDef> list4 = (from proj in DefDatabase<ResearchProjectDef>.AllDefs
			where proj.IsFinished && proj.HasTag(ResearchProjectTagDefOf.ClassicStart)
			select proj).ToList<ResearchProjectDef>();
			Find.ResearchManager.ResetAllProgress();
			EntityCodex entityCodex = Find.EntityCodex;
			if (entityCodex != null)
			{
				entityCodex.Reset();
			}
			ResearchUtility.ApplyPlayerStartingResearch();
			foreach (ResearchProjectDef proj2 in list4)
			{
				Find.ResearchManager.FinishProject(proj2, false, null, false);
			}
			FactionUtility.ResetAllFactionRelations();
			if (ModsConfig.BiotechActive)
			{
				GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
				if (component != null)
				{
					component.ResetProgress();
				}
			}
			MoveColonyUtility.SetPawnThoughts(list3);
			MoveColonyUtility.ResetNeedLevels(list3);
			MoveColonyUtility.RemoveHediffs(list3);
			MoveColonyUtility.ResetStartingGracePeriods(list3);
			Find.FactionManager.OfPlayer.ideos.RecalculateIdeosBasedOnPlayerPawns();
			IdeoUtility.Notify_NewColonyStarted();
			return settlement;
		}

		// Token: 0x0600A224 RID: 41508 RVA: 0x002E9BF8 File Offset: 0x002E7DF8
		public static IEnumerable<Thing> GetStartingThingsForNewColony()
		{
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				ScenPart_StartingThing_Defined scenPart_StartingThing_Defined = scenPart as ScenPart_StartingThing_Defined;
				if (scenPart_StartingThing_Defined != null)
				{
					foreach (Thing thing in scenPart_StartingThing_Defined.PlayerStartingThings())
					{
						yield return thing;
					}
					IEnumerator<Thing> enumerator2 = null;
				}
			}
			IEnumerator<ScenPart> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600A225 RID: 41509 RVA: 0x002E9C01 File Offset: 0x002E7E01
		public static bool IsBringableItem(Thing t)
		{
			return !t.def.destroyOnDrop && !t.IsRelic() && (t.Map == null || !t.Position.Fogged(t.Map));
		}

		// Token: 0x0600A226 RID: 41510 RVA: 0x002E9C3C File Offset: 0x002E7E3C
		public static bool IsDistinctArchonexusItem(ThingDef td)
		{
			if ((!td.IsWeapon && !td.IsApparel) || !td.HasComp(typeof(CompQuality)))
			{
				bool? flag;
				bool flag2;
				if (td != null)
				{
					List<string> weaponTags = td.weaponTags;
					flag = ((weaponTags != null) ? new bool?(weaponTags.Contains("Gun")) : null);
					flag2 = true;
					if (flag.GetValueOrDefault() == flag2 & flag != null)
					{
						return true;
					}
				}
				if (td == null)
				{
					return false;
				}
				ApparelProperties apparel = td.apparel;
				bool? flag3;
				if (apparel == null)
				{
					flag3 = null;
				}
				else
				{
					ApparelLayerDef lastLayer = apparel.LastLayer;
					flag3 = ((lastLayer != null) ? new bool?(lastLayer.IsUtilityLayer) : null);
				}
				flag = flag3;
				flag2 = true;
				return flag.GetValueOrDefault() == flag2 & flag != null;
			}
			return true;
		}

		// Token: 0x0600A227 RID: 41511 RVA: 0x002E9CFC File Offset: 0x002E7EFC
		public static void RemoveWeaponsAndUtilityItems(List<Pawn> pawns, IEnumerable<Thing> selectedThings)
		{
			List<Thing> list = new List<Thing>();
			foreach (Pawn pawn in pawns)
			{
				if (pawn.equipment != null)
				{
					foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
					{
						if (!thingWithComps.DestroyedOrNull() && !selectedThings.Contains(thingWithComps))
						{
							list.Add(thingWithComps);
						}
					}
				}
				if (pawn.apparel != null)
				{
					foreach (Apparel apparel in pawn.apparel.WornApparel)
					{
						ThingDef def = apparel.def;
						bool flag;
						if (def == null)
						{
							flag = false;
						}
						else
						{
							ApparelProperties apparel2 = def.apparel;
							bool? flag2;
							if (apparel2 == null)
							{
								flag2 = null;
							}
							else
							{
								ApparelLayerDef lastLayer = apparel2.LastLayer;
								flag2 = ((lastLayer != null) ? new bool?(lastLayer.IsUtilityLayer) : null);
							}
							bool? flag3 = flag2;
							bool flag4 = true;
							flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
						}
						if (flag && !selectedThings.Contains(apparel))
						{
							list.Add(apparel);
						}
					}
				}
			}
			foreach (Thing thing in list)
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600A228 RID: 41512 RVA: 0x002E9EDC File Offset: 0x002E80DC
		public static List<Thing> GetAbandonedRelicsCarriedByPawns(IEnumerable<Thing> selectedThings)
		{
			List<Thing> list = new List<Thing>();
			List<Pawn> list2 = new List<Pawn>();
			foreach (Thing thing in selectedThings)
			{
				if (thing.IsRelic())
				{
					list.Add(thing);
				}
				if (thing is Pawn)
				{
					list2.Add((Pawn)thing);
				}
			}
			List<Thing> list3 = new List<Thing>();
			foreach (Pawn pawn in list2)
			{
				foreach (Thing thing2 in pawn.EquippedWornOrInventoryThings)
				{
					if (thing2.IsRelic() && !list.Contains(thing2))
					{
						list3.Add(thing2);
					}
				}
			}
			return list3;
		}

		// Token: 0x0600A229 RID: 41513 RVA: 0x002E9FE0 File Offset: 0x002E81E0
		private static void ResetStartingGracePeriods(List<Pawn> pawns)
		{
			Find.TickManager.ResetSettlementTicks();
			Find.StoryWatcher.watcherAdaptation.ResetAdaptDays();
			Find.StoryWatcher.watcherPopAdaptation.ResetAdaptDays();
			foreach (Pawn pawn in pawns)
			{
				if (pawn.IsColonist)
				{
					pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
				}
			}
		}

		// Token: 0x0600A22A RID: 41514 RVA: 0x002EA064 File Offset: 0x002E8264
		private static void SetPawnThoughts(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				MemoryThoughtHandler memoryThoughtHandler;
				if (needs == null)
				{
					memoryThoughtHandler = null;
				}
				else
				{
					Need_Mood mood = needs.mood;
					if (mood == null)
					{
						memoryThoughtHandler = null;
					}
					else
					{
						ThoughtHandler thoughts = mood.thoughts;
						memoryThoughtHandler = ((thoughts != null) ? thoughts.memories : null);
					}
				}
				MemoryThoughtHandler memoryThoughtHandler2 = memoryThoughtHandler;
				if (memoryThoughtHandler2 != null)
				{
					memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyOptimism);
					memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyHope);
					if (pawn.IsFreeNonSlaveColonist)
					{
						memoryThoughtHandler2.TryGainMemory(ThoughtDefOf.NewColonyOptimism, null, null);
					}
				}
			}
		}

		// Token: 0x0600A22B RID: 41515 RVA: 0x002EA104 File Offset: 0x002E8304
		private static void ResetNeedLevels(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Food food = needs.food;
					if (food != null)
					{
						food.SetInitialLevel();
					}
				}
				Pawn_NeedsTracker needs2 = pawn.needs;
				if (needs2 != null)
				{
					Need_Rest rest = needs2.rest;
					if (rest != null)
					{
						rest.SetInitialLevel();
					}
				}
			}
		}

		// Token: 0x0600A22C RID: 41516 RVA: 0x002EA184 File Offset: 0x002E8384
		private static void RemoveHediffs(List<Pawn> pawns)
		{
			List<Hediff> list = new List<Hediff>();
			foreach (Pawn pawn in pawns)
			{
				list.Clear();
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					if (hediff.def == HediffDefOf.Hypothermia || hediff.def == HediffDefOf.Heatstroke || hediff.def == HediffDefOf.Malnutrition)
					{
						list.Add(hediff);
					}
				}
				foreach (Hediff hediff2 in list)
				{
					pawn.health.RemoveHediff(hediff2);
				}
			}
		}

		// Token: 0x0400700B RID: 28683
		private static StringBuilder cannotPlaceTileReason = new StringBuilder();

		// Token: 0x0400700C RID: 28684
		public const int TitleAndRoleRequirementGracePeriodTicks = 600000;

		// Token: 0x0400700D RID: 28685
		private static List<PlanetTile> playerSettlementsRemoved = new List<PlanetTile>();
	}
}
