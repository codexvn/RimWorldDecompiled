using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BA0 RID: 15264
	public class QuestNode_Root_UnnaturalDarkness : QuestNode
	{
		// Token: 0x06014BA8 RID: 84904 RVA: 0x0063535C File Offset: 0x0063355C
		protected override bool TestRunInt(Slate slate)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = QuestGen_Get.GetMap(false, null, false);
			if (map == null)
			{
				return false;
			}
			LargeBuildingSpawnParms parms = QuestNode_Root_UnnaturalDarkness.NoctolithSpawnParms.ForThing(ThingDefOf.Noctolith);
			IntVec3 intVec;
			return LargeBuildingCellFinder.AnyCellFast(map, parms) || LargeBuildingCellFinder.TryFindCell(out intVec, map, parms, null, null, false);
		}

		// Token: 0x06014BA9 RID: 84905 RVA: 0x006353B4 File Offset: 0x006335B4
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			slate.Set<Map>("map", map, false);
			float points = slate.Get<float>("points", 0f, false);
			List<Thing> list = new List<Thing>();
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("noctolith");
			string mainPhaseBeganSignal = QuestGen.GenerateNewSignal("MainPhaseBegan", true);
			string mainPhaseEndedSignal = QuestGen.GenerateNewSignal("MainPhaseEnded", true);
			string text = QuestGen.GenerateNewSignal("NoctolRaid", true);
			string inSignalNoctolithDamaged = QuestGenUtility.HardcodedSignalWithQuestID("noctolith.TookDamage");
			string inSignalNoctolithKilled = QuestGenUtility.HardcodedSignalWithQuestID("noctolith.Destroyed");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("map.MapRemoved");
			int num = Mathf.RoundToInt(QuestNode_Root_UnnaturalDarkness.InitialPhaseDurationDaysRange.RandomInRange * 60000f);
			int delayTicks = Mathf.RoundToInt(QuestNode_Root_UnnaturalDarkness.MainPhaseDurationDaysRange.RandomInRange * 60000f);
			quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, list, false, "[initialPhaseLetterText]", null, "[initialPhaseLetterLabel]", null, null);
			quest.GameCondition(GameConditionDefOf.UnnaturalDarkness, -1, map.Parent, true, true, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Delay(num - 10000, delegate
			{
				Quest quest = quest;
				LetterDef neutralEvent = LetterDefOf.NeutralEvent;
				string inSignal2 = null;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "DarknessWarningLetterLabel".Translate();
				quest.Letter(neutralEvent, inSignal2, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "DarknessWarningLetterText".Translate(), null, label, null, null);
			}, null, mainPhaseBeganSignal, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Warning letter delay";
			quest.Delay(num, delegate
			{
				quest.SignalPass(null, null, mainPhaseBeganSignal);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Main phase delay";
			quest.AddPart(new QuestPart_DarkenMap(mainPhaseBeganSignal, map.Parent));
			quest.Letter(LetterDefOf.ThreatBig, mainPhaseBeganSignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[mainPhaseLetterText]", null, "[mainPhaseLetterLabel]", null, null);
			List<CellRect> list2 = new List<CellRect>();
			LargeBuildingSpawnParms parms = QuestNode_Root_UnnaturalDarkness.NoctolithSpawnParms.ForThing(ThingDefOf.Noctolith);
			int num2 = 0;
			IntVec3 intVec;
			while (num2 < 3 && LargeBuildingCellFinder.TryFindCell(out intVec, map, parms, list2, null, false))
			{
				list2.Add(CellRect.CenteredOn(intVec, ThingDefOf.Noctolith.Size));
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Noctolith, null);
				thing.SetFaction(Faction.OfEntities, null);
				QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				list.Add(thing);
				quest.SpawnSkyfaller(map, ThingDefOf.NoctolithIncoming, Gen.YieldSingle<Thing>(thing), null, new IntVec3?(intVec), mainPhaseBeganSignal, false, false, null, null);
				num2++;
			}
			quest.AddPart(new QuestPart_Noctoliths(map.Parent, list, points, inSignalNoctolithDamaged, inSignalNoctolithKilled));
			quest.AddPart(new QuestPart_RandomWaves(mainPhaseBeganSignal, text, 36f, 36f, float.MaxValue));
			quest.SignalPass(delegate
			{
				Quest quest = quest;
				MapParent parent = map.Parent;
				float points;
				points *= 0.5f;
				Faction ofEntities = Faction.OfEntities;
				string inSignalLeave = null;
				PawnsArrivalModeDef edgeWalkInDarkness = PawnsArrivalModeDefOf.EdgeWalkInDarkness;
				RaidStrategyDef immediateAttack = RaidStrategyDefOf.ImmediateAttack;
				PawnGroupKindDef noctols = PawnGroupKindDefOf.Noctols;
				quest.Raid(parent, points, ofEntities, inSignalLeave, "NoctolAttackLetterLabel".Translate(), "NoctolAttackLetterText".Translate(), null, null, null, null, null, "root", edgeWalkInDarkness, immediateAttack, noctols, false, false, false, false);
			}, text, null);
			quest.Delay(delayTicks, delegate
			{
				quest.SignalPass(null, null, mainPhaseEndedSignal);
			}, mainPhaseBeganSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Main phase end delay";
			quest.Letter(LetterDefOf.PositiveEvent, mainPhaseEndedSignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[darknessLiftedLetterText]", null, "[darknessLiftedLetterLabel]", null, null);
			quest.AddPart(new QuestPart_DestroyAllThingsOfDef(mainPhaseEndedSignal, map.Parent, new List<ThingDef>
			{
				ThingDefOf.Noctolith
			}));
			quest.AddPart(new QuestPart_GiveMemoryToHumansOnMap
			{
				memory = ThoughtDefOf.DarknessLifted,
				inSignal = mainPhaseEndedSignal,
				mapParent = map.Parent
			});
			quest.SignalPass(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, mainPhaseEndedSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, mainPhaseEndedSignal, null);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			Pawn pawn = map.mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
			if (pawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.UnnaturalDarkness, new object[]
				{
					pawn
				});
			}
		}

		// Token: 0x0400E6AE RID: 59054
		public const int MaxNoctoliths = 3;

		// Token: 0x0400E6AF RID: 59055
		private static readonly FloatRange InitialPhaseDurationDaysRange = new FloatRange(0.5f, 0.75f);

		// Token: 0x0400E6B0 RID: 59056
		private static readonly FloatRange MainPhaseDurationDaysRange = new FloatRange(6f, 8f);

		// Token: 0x0400E6B1 RID: 59057
		private const float NoctolRaidPointsFactor = 0.5f;

		// Token: 0x0400E6B2 RID: 59058
		public static readonly LargeBuildingSpawnParms NoctolithSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};
	}
}
