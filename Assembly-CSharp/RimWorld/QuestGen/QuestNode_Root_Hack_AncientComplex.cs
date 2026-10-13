using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B4C RID: 15180
	public class QuestNode_Root_Hack_AncientComplex : QuestNode_Root_AncientComplex
	{
		// Token: 0x06014A20 RID: 84512 RVA: 0x00629394 File Offset: 0x00627594
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Ancient complex rescue"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float num = slate.Get<float>("points", 0f, false);
			Precept_Relic precept_Relic = slate.Get<Precept_Relic>("relic", null, false);
			PlanetTile tile;
			this.TryFindSiteTile(out tile, false);
			Faction faction;
			this.TryFindEnemyFaction(out faction);
			if (precept_Relic == null)
			{
				precept_Relic = Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>().RandomElement<Precept_Relic>();
				Log.Warning("Ancient Complex quest requires relic from parent quest. None found so picking random player relic");
			}
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminals.Destroyed");
			string inSignal2 = QuestGen.GenerateNewSignal("TerminalHacked", true);
			string inSignal3 = QuestGen.GenerateNewSignal("AllTerminalsHacked", true);
			QuestGen.GenerateNewSignal("RaidArrives", true);
			string inSignal4 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			QuestNode_Root_Hack_AncientComplex.AddQuestRelicParts(precept_Relic, quest);
			LayoutStructureSketch layoutStructureSketch = this.QuestSetupComplex(quest, num);
			float num2 = Find.Storyteller.difficulty.allowViolentQuests ? QuestNode_Root_AncientComplex.ThreatPointsOverPointsCurve.Evaluate(num) : 0f;
			SitePartParams parms = new SitePartParams
			{
				ancientLayoutStructureSketch = layoutStructureSketch,
				ancientComplexRewardMaker = ThingSetMakerDefOf.MapGen_AncientComplexRoomLoot_Default,
				threatPoints = num2
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.AncientComplex, parms)), tile, Faction.OfAncients, false, null, null);
			quest.SpawnWorldObject(site, null, null);
			TimedDetectionRaids component = site.GetComponent<TimedDetectionRaids>();
			if (component != null)
			{
				component.alertRaidsArrivingIn = true;
			}
			quest.Message("[terminalHackedMessage]", null, true, null, null, inSignal2);
			quest.Message("[allTerminalsHackedMessage]", MessageTypeDefOf.PositiveEvent, false, null, null, inSignal3);
			if (Find.Storyteller.difficulty.allowViolentQuests && Rand.Chance(0.5f))
			{
				quest.RandomRaid(site, QuestNode_Root_Hack_AncientComplex.RandomRaidPointsFactorRange * num2, faction, inSignal3, PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, null, null, false);
			}
			QuestPart_Filter_Hacked questPart_Filter_Hacked = new QuestPart_Filter_Hacked();
			questPart_Filter_Hacked.inSignal = inSignal;
			questPart_Filter_Hacked.outSignalElse = QuestGen.GenerateNewSignal("FailQuestTerminalDestroyed", true);
			quest.AddPart(questPart_Filter_Hacked);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal4, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, questPart_Filter_Hacked.outSignalElse, QuestPart.SignalListenMode.OngoingOnly, true, false);
			slate.Set<List<Thing>>("terminals", layoutStructureSketch.thingsToSpawn, false);
			slate.Set<int>("terminalCount", layoutStructureSketch.thingsToSpawn.Count, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x06014A21 RID: 84513 RVA: 0x00629618 File Offset: 0x00627818
		public override LayoutStructureSketch QuestSetupComplex(Quest quest, float points)
		{
			Precept_Relic orAddRelicFromQuest = QuestNode_Root_Hack_AncientComplex.GetOrAddRelicFromQuest(quest);
			LayoutStructureSketch layoutStructureSketch = this.GenerateStructureSketch(points, true);
			layoutStructureSketch.thingDiscoveredMessage = "MessageAncientTerminalDiscovered".Translate(orAddRelicFromQuest.Label);
			string item = QuestGen.GenerateNewSignal("AllTerminalsHacked", false);
			string outSignalAny = QuestGen.GenerateNewSignal("TerminalHacked", false);
			List<string> list = new List<string>();
			for (int i = 0; i < layoutStructureSketch.thingsToSpawn.Count; i++)
			{
				Thing thing = layoutStructureSketch.thingsToSpawn[i];
				string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("terminal" + i.ToString());
				QuestUtility.AddQuestTag(thing, text);
				string item2 = QuestGenUtility.HardcodedSignalWithQuestID(text + ".Hacked");
				list.Add(item2);
				thing.TryGetComp<CompHackable>().defence = (float)(Rand.Chance(0.5f) ? QuestNode_Root_Hack_AncientComplex.HackDefenceRange.min : QuestNode_Root_Hack_AncientComplex.HackDefenceRange.max);
			}
			QuestPart_PassAllActivable questPart_PassAllActivable;
			if (!quest.TryGetFirstPartOfType<QuestPart_PassAllActivable>(out questPart_PassAllActivable))
			{
				questPart_PassAllActivable = quest.AddPart<QuestPart_PassAllActivable>();
				questPart_PassAllActivable.inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
				questPart_PassAllActivable.expiryInfoPartKey = "TerminalsHacked";
			}
			questPart_PassAllActivable.inSignals = list;
			questPart_PassAllActivable.outSignalsCompleted.Add(item);
			questPart_PassAllActivable.outSignalAny = outSignalAny;
			return layoutStructureSketch;
		}

		// Token: 0x06014A22 RID: 84514 RVA: 0x0062975C File Offset: 0x0062795C
		private static void AddQuestRelicParts(Precept_Relic relic, Quest quest)
		{
			Reward_RelicInfo item = new Reward_RelicInfo
			{
				relic = relic,
				quest = quest
			};
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(item);
			questPart_Choice.choices.Add(choice);
		}

		// Token: 0x06014A23 RID: 84515 RVA: 0x006297A4 File Offset: 0x006279A4
		private static Precept_Relic GetOrAddRelicFromQuest(Quest quest)
		{
			foreach (QuestPart_Choice.Choice choice in quest.GetFirstOrAddPart<QuestPart_Choice>().choices)
			{
				foreach (Reward reward in choice.rewards)
				{
					Reward_RelicInfo reward_RelicInfo = reward as Reward_RelicInfo;
					if (reward_RelicInfo != null)
					{
						return reward_RelicInfo.relic;
					}
				}
			}
			Log.Error("Failed to get relic from quest part, quest = " + quest.ToStringSafe<Quest>());
			return null;
		}

		// Token: 0x06014A24 RID: 84516 RVA: 0x00629858 File Offset: 0x00627A58
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, QuestNode_Root_AncientComplex.AllowedLandmarks, 0.5f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, null);
		}

		// Token: 0x06014A25 RID: 84517 RVA: 0x0062987F File Offset: 0x00627A7F
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			enemyFaction = Find.FactionManager.RandomRaidableEnemyFaction(false, false, true, TechLevel.Undefined);
			return enemyFaction != null;
		}

		// Token: 0x06014A26 RID: 84518 RVA: 0x00629898 File Offset: 0x00627A98
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			Faction faction;
			return this.TryFindSiteTile(out planetTile, true) && this.TryFindEnemyFaction(out faction);
		}

		// Token: 0x0400E520 RID: 58656
		private const int MinDistanceFromColony = 2;

		// Token: 0x0400E521 RID: 58657
		private const int MaxDistanceFromColony = 10;

		// Token: 0x0400E522 RID: 58658
		private static IntRange HackDefenceRange = new IntRange(300, 1800);

		// Token: 0x0400E523 RID: 58659
		private const float MinMaxHackDefenceChance = 0.5f;

		// Token: 0x0400E524 RID: 58660
		private static readonly FloatRange RandomRaidPointsFactorRange = new FloatRange(0.25f, 0.35f);

		// Token: 0x0400E525 RID: 58661
		private const float ChanceToSpawnAllTerminalsHackedRaid = 0.5f;
	}
}
