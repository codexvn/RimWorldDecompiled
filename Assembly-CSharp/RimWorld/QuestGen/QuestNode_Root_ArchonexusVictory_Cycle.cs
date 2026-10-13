using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B2C RID: 15148
	public abstract class QuestNode_Root_ArchonexusVictory_Cycle : QuestNode
	{
		// Token: 0x1700335C RID: 13148
		// (get) Token: 0x0601499C RID: 84380
		protected abstract int ArchonexusCycle { get; }

		// Token: 0x0601499D RID: 84381 RVA: 0x006257A8 File Offset: 0x006239A8
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			this.map = QuestGen_Get.GetMap(false, null, true);
			string text = QuestGen.GenerateNewSignal("PlayerWealthSatisfied", true);
			string text2 = QuestGen.GenerateNewSignal("SendLetterReminder", true);
			QuestGen.GenerateNewSignal("ActivateLetterReminderSignal", true);
			QuestPart_RequirementsToAcceptPlayerWealth questPart_RequirementsToAcceptPlayerWealth = new QuestPart_RequirementsToAcceptPlayerWealth();
			questPart_RequirementsToAcceptPlayerWealth.requiredPlayerWealth = 350000f;
			quest.AddPart(questPart_RequirementsToAcceptPlayerWealth);
			QuestPart_PlayerWealth questPart_PlayerWealth = new QuestPart_PlayerWealth();
			questPart_PlayerWealth.inSignalEnable = quest.AddedSignal;
			questPart_PlayerWealth.playerWealth = 350000f;
			questPart_PlayerWealth.outSignalsCompleted.Add(text);
			questPart_PlayerWealth.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
			quest.AddPart(questPart_PlayerWealth);
			QuestPart_PassOutInterval questPart_PassOutInterval = new QuestPart_PassOutInterval();
			questPart_PassOutInterval.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
			questPart_PassOutInterval.inSignalEnable = text;
			questPart_PassOutInterval.ticksInterval = new IntRange(3600000, 3600000);
			questPart_PassOutInterval.outSignals.Add(text2);
			quest.AddPart(questPart_PassOutInterval);
			QuestPart_Filter_PlayerWealth questPart_Filter_PlayerWealth = new QuestPart_Filter_PlayerWealth();
			questPart_Filter_PlayerWealth.minPlayerWealth = 350000f;
			questPart_Filter_PlayerWealth.inSignal = text2;
			questPart_Filter_PlayerWealth.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted", true);
			questPart_Filter_PlayerWealth.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
			quest.AddPart(questPart_Filter_PlayerWealth);
			quest.CanAcceptQuest(delegate
			{
				QuestNode_ResolveQuestName.Resolve();
				string value = slate.Get<string>("resolvedQuestName", null, false);
				Quest quest = quest;
				LetterDef positiveEvent = LetterDefOf.PositiveEvent;
				string inSignal = null;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "LetterLabelArchonexusWealthReached".Translate(value);
				quest.Letter(positiveEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextArchonexusWealthReached".Translate(value), null, label, null, null);
			}, null, questPart_Filter_PlayerWealth.outSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly);
			Reward_ArchonexusMap reward_ArchonexusMap = new Reward_ArchonexusMap();
			reward_ArchonexusMap.currentPart = this.ArchonexusCycle;
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_ArchonexusMap);
			questPart_Choice.choices.Add(choice);
			List<MapParent> list = new List<MapParent>();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					list.Add(map.Parent);
				}
			}
			slate.Set<List<MapParent>>("playerSettlements", list, false);
			slate.Set<int>("playerSettlementsCount", list.Count, false);
			slate.Set<int>("colonistsAllowed", 5, false);
			slate.Set<int>("animalsAllowed", 5, false);
			slate.Set<float>("requiredWealth", 350000f, false);
			slate.Set<Map>("map", this.map, false);
			slate.Set<MapParent>("mapParent", this.map.Parent, false);
		}

		// Token: 0x0601499E RID: 84382 RVA: 0x00625A3C File Offset: 0x00623C3C
		protected void PickNewColony(Faction takeOverFaction, WorldObjectDef worldObjectDef, SoundDef colonyStartSounDef, int maxRelics = 1)
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			string text = QuestGen.GenerateNewSignal("NewColonyCreated", true);
			string text2 = QuestGen.GenerateNewSignal("NewColonyCancelled", true);
			quest.AddPart(new QuestPart_NewColony
			{
				inSignal = slate.Get<string>("inSignal", null, false),
				otherFaction = takeOverFaction,
				outSignalCompleted = text,
				outSignalCancelled = text2,
				worldObjectDef = worldObjectDef,
				maxRelics = maxRelics
			});
			quest.SetQuestNotYetAccepted(text2, true);
			quest.End(QuestEndOutcome.Success, 0, null, text, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0601499F RID: 84383 RVA: 0x00625AC8 File Offset: 0x00623CC8
		protected void TryAddStudyRequirement(Quest quest, Slate slate, ThingDef buildingToStudyDef)
		{
			Thing thing = this.map.listerThings.ThingsOfDef(buildingToStudyDef).FirstOrDefault<Thing>();
			if (thing != null)
			{
				slate.Set<Thing>("archonexusMajorStructure", thing, false);
				slate.Set<bool>("studyRequirement", true, false);
				LetterDef positiveEvent = LetterDefOf.PositiveEvent;
				string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("archonexusMajorStructure.Researched");
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "LetterLabelArchonexusStructureResearched".Translate(thing);
				quest.Letter(positiveEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextArchonexusStructureResearched".Translate(thing), null, label, null, null);
				quest.AddPart(new QuestPart_RequirementsToAcceptThingStudied_ArchotechStructures
				{
					thing = thing
				});
				return;
			}
			slate.Set<bool>("studyRequirement", false, false);
		}

		// Token: 0x060149A0 RID: 84384 RVA: 0x00625B78 File Offset: 0x00623D78
		protected override bool TestRunInt(Slate slate)
		{
			return QuestGen_Get.GetMap(false, null, true) != null;
		}

		// Token: 0x0400E4B6 RID: 58550
		public const int ColonistsAllowed = 5;

		// Token: 0x0400E4B7 RID: 58551
		public const int AnimalsAllowed = 5;

		// Token: 0x0400E4B8 RID: 58552
		public const float RequiredWealth = 350000f;

		// Token: 0x0400E4B9 RID: 58553
		public const int LetterReminderInterval = 3600000;

		// Token: 0x0400E4BA RID: 58554
		protected Map map;
	}
}
