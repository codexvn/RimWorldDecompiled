using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.Utility;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BAA RID: 15274
	public class QuestNode_Root_VoidAwakening : QuestNode
	{
		// Token: 0x06014BDA RID: 84954 RVA: 0x00636608 File Offset: 0x00634808
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.AnomalyActive && QuestGen_Get.GetMap(false, null, false) != null && Find.Anomaly.monolith != null && Find.Anomaly.LevelDef == MonolithLevelDefOf.VoidAwakened;
		}

		// Token: 0x06014BDB RID: 84955 RVA: 0x00636654 File Offset: 0x00634854
		protected override void RunInt()
		{
			Building_VoidMonolith monolith = Find.Anomaly.monolith;
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = monolith.Map;
			float var = StorytellerUtility.DefaultThreatPointsNow(map);
			slate.Set<Map>("map", map, false);
			slate.Set<float>("points", var, false);
			List<CellRect> structureRects = new List<CellRect>();
			List<Thing> tmpStageStructures = new List<Thing>();
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("monolithMap");
			QuestUtility.AddQuestTag(ref map.Parent.questTags, questTagToAdd);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("monolithMap.MapRemoved");
			string questTagToAdd2 = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("monolith");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("monolith.Activated");
			QuestUtility.AddQuestTag(ref monolith.questTags, questTagToAdd2);
			string voidNodeTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("voidNode");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("voidNode.NodeClosed");
			string beginDarkeningSignal = QuestGen.GenerateNewSignal("BeginDarkening", true);
			string darknessEnvelopSignal = QuestGen.GenerateNewSignal("DarknessEnveloped", true);
			string entityArrivalSignal = QuestGen.GenerateNewSignal("EntityWave", true);
			StageSignals[] stages = new StageSignals[3];
			for (int i = 0; i < 3; i++)
			{
				string text2 = string.Format("stageStructure.{0}", i);
				stages[i] = new StageSignals
				{
					structureTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(text2),
					beganSignal = QuestGen.GenerateNewSignal(string.Format("StageBegan.{0}", i), true),
					structureActivatedSignal = QuestGenUtility.HardcodedSignalWithQuestID(text2 + ".Activated"),
					structureDestroyedSignal = QuestGenUtility.HardcodedSignalWithQuestID(text2 + ".Destroyed"),
					allStructuresActivatedSignal = QuestGenUtility.HardcodedSignalWithQuestID(string.Format("AllStructuresActivated.{0}", i))
				};
			}
			int num = 720;
			quest.AddPart(new QuestPart_Alert_StructuresArriving("[structuresArrivingAlertLabel]", "[structuresArrivingAlertExplanation]", beginDarkeningSignal, stages[0].beganSignal, num, null));
			quest.Delay(363, delegate
			{
				quest.SignalPass(null, null, beginDarkeningSignal);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Begin darkness delay";
			Quest quest3 = quest;
			GameConditionDef unnaturalDarkness = GameConditionDefOf.UnnaturalDarkness;
			int duration = -1;
			string beginDarkeningSignal2 = beginDarkeningSignal;
			quest3.GameCondition(unnaturalDarkness, duration, map.Parent, true, true, beginDarkeningSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.Delay(num, delegate
			{
				quest.SignalPass(null, null, stages[0].beganSignal);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Stage one delay";
			this.SpawnStructures(structureRects, stages[0], 2, ThingDefOf.VoidStructure, tmpStageStructures, VoidAwakeningUtility.EncodeWaveType(entityArrivalSignal, VoidAwakeningUtility.WaveType.Twisted, 1f));
			Action <>9__6;
			quest.SignalPass(delegate
			{
				Quest quest2 = quest;
				int randomInRange = QuestNode_Root_VoidAwakening.EntityArrivalDelayTicksRange.RandomInRange;
				Action inner;
				if ((inner = <>9__6) == null)
				{
					inner = (<>9__6 = delegate()
					{
						quest.SignalPass(null, null, VoidAwakeningUtility.EncodeWaveType(entityArrivalSignal, VoidAwakeningUtility.WaveType.Fleshbeast, 1f));
					});
				}
				quest2.Delay(randomInRange, inner, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			}, stages[0].beganSignal, null);
			quest.AddPart(new QuestPart_StructureActivated(2, stages[0].structureActivatedSignal, stages[0].structureDestroyedSignal, stages[0].allStructuresActivatedSignal, 1));
			int num2 = Mathf.RoundToInt(QuestNode_Root_VoidAwakening.InterStageDelayHoursRange.RandomInRange * 2500f);
			int num3 = num2 - 1800;
			int delayTicks = num3 - 2500;
			quest.AddPart(new QuestPart_Alert_StructuresArriving("[structuresArrivingAlertLabel]", "[structuresArrivingAlertExplanation]", stages[0].allStructuresActivatedSignal, stages[1].beganSignal, num2, null));
			quest.Delay(delayTicks, delegate
			{
				Quest quest2 = quest;
				LetterDef neutralEvent = LetterDefOf.NeutralEvent;
				string inSignal3 = null;
				string chosenPawnSignal = null;
				Faction relatedFaction = null;
				MapParent useColonistsOnMap = null;
				bool useColonistsFromCaravanArg = false;
				QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
				IEnumerable<object> lookTargets = null;
				bool filterDeadPawnsFromLookTargets = false;
				string label = "DarknessWarningLetterLabel".Translate();
				quest2.Letter(neutralEvent, inSignal3, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "DarknessWarningLetterText".Translate(), null, label, null, null);
			}, stages[0].allStructuresActivatedSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Darkness warning delay";
			quest.Delay(num3, delegate
			{
				quest.SignalPass(null, null, darknessEnvelopSignal);
				quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[darknessStartedLetterText]", null, "[darknessStartedLetterLabel]", null, null);
			}, stages[0].allStructuresActivatedSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Darkness delay";
			quest.AddPart(new QuestPart_DarkenMap(darknessEnvelopSignal, map.Parent));
			quest.Delay(num2, delegate
			{
				quest.SignalPass(null, null, stages[1].beganSignal);
			}, stages[0].allStructuresActivatedSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Stage two delay";
			this.SpawnStructures(structureRects, stages[1], 3, ThingDefOf.VoidStructure, tmpStageStructures, VoidAwakeningUtility.EncodeWaveType(entityArrivalSignal, VoidAwakeningUtility.WaveType.Twisted, 1.25f));
			quest.AddPart(new QuestPart_StructureActivated(3, stages[1].structureActivatedSignal, stages[1].structureDestroyedSignal, stages[1].allStructuresActivatedSignal, 2));
			quest.AddPart(new QuestPart_MonolithTwisting("[monolithTwistingAlertLabel]", "[monolithTwistingAlertExplanation]", stages[1].allStructuresActivatedSignal, stages[2].beganSignal, 10800, monolith, 10500));
			quest.Delay(10500, delegate
			{
				quest.Letter(LetterDefOf.ThreatBig, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Building_VoidMonolith>(monolith), false, "[gleamingMonolithWarningLetterText]", null, "[gleamingMonolithWarningLetterLabel]", null, null);
			}, stages[1].allStructuresActivatedSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Delay(10800, delegate
			{
				quest.SignalPass(null, null, stages[2].beganSignal);
			}, stages[1].allStructuresActivatedSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Stage three delay";
			quest.Delay(11100, delegate
			{
				quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Building_VoidMonolith>(monolith), false, "[gleamingMonolithLetterText]", null, "[gleamingMonolithLetterLabel]", null, null);
			}, stages[1].allStructuresActivatedSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.AddPart(new QuestPart_SetMonolithGleaming(monolith, stages[2].beganSignal));
			quest.AddPart(new QuestPart_StructureActivated(1, text, null, stages[2].allStructuresActivatedSignal, 3));
			quest.AddPart(new QuestPart_EntityArrival(entityArrivalSignal, map, monolith, Find.TickManager.TicksGame, structureRects));
			string text3 = QuestGen.GenerateNewSignal("WaveBegan", true);
			quest.SignalPassAny(null, new List<string>
			{
				stages[0].beganSignal,
				stages[1].beganSignal
			}, text3);
			quest.AddPart(new QuestPart_WanderingEntities(map, text3));
			quest.AddPart(new QuestPart_MetalHell(text, voidNodeTag));
			quest.AddPart(new QuestPart_CleanUpVoidAwakening(inSignal2, map));
			quest.End(QuestEndOutcome.Success, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, true, false);
		}

		// Token: 0x06014BDC RID: 84956 RVA: 0x00636CF8 File Offset: 0x00634EF8
		private void SpawnStructures(List<CellRect> structureRects, StageSignals stage, int numStructures, ThingDef structureDef, List<Thing> tmpStageStructures, string structureActivatedSignal)
		{
			QuestNode_Root_VoidAwakening.<>c__DisplayClass8_0 CS$<>8__locals1 = new QuestNode_Root_VoidAwakening.<>c__DisplayClass8_0();
			CS$<>8__locals1.structureActivatedSignal = structureActivatedSignal;
			tmpStageStructures.Clear();
			CS$<>8__locals1.quest = QuestGen.quest;
			CS$<>8__locals1.map = QuestGen.slate.Get<Map>("map", null, false);
			LargeBuildingSpawnParms largeBuildingSpawnParms = QuestNode_Root_VoidAwakening.StructureSpawnParms.ForThing(structureDef);
			LargeBuildingSpawnParms largeBuildingSpawnParms2 = QuestNode_Root_VoidAwakening.StructureSpawnParms.ForThing(structureDef);
			largeBuildingSpawnParms.minDistanceFromUsedRects = 25f;
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < numStructures; i++)
			{
				QuestNode_Root_VoidAwakening.<>c__DisplayClass8_1 CS$<>8__locals2 = new QuestNode_Root_VoidAwakening.<>c__DisplayClass8_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				QuestNode_Root_VoidAwakening.<>c__DisplayClass8_1 CS$<>8__locals3 = CS$<>8__locals2;
				Map map = CS$<>8__locals2.CS$<>8__locals1.map;
				LargeBuildingSpawnParms parms = largeBuildingSpawnParms;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = CS$<>8__locals2.CS$<>8__locals1.<>9__1) == null)
				{
					extraValidator = (CS$<>8__locals2.CS$<>8__locals1.<>9__1 = ((IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, CS$<>8__locals2.CS$<>8__locals1.map)));
				}
				if (!LargeBuildingCellFinder.TryFindCell(out CS$<>8__locals3.cell, map, parms, structureRects, extraValidator, false))
				{
					QuestNode_Root_VoidAwakening.<>c__DisplayClass8_1 CS$<>8__locals4 = CS$<>8__locals2;
					Map map2 = CS$<>8__locals2.CS$<>8__locals1.map;
					LargeBuildingSpawnParms parms2 = largeBuildingSpawnParms2;
					Predicate<IntVec3> extraValidator2;
					if ((extraValidator2 = CS$<>8__locals2.CS$<>8__locals1.<>9__2) == null)
					{
						extraValidator2 = (CS$<>8__locals2.CS$<>8__locals1.<>9__2 = ((IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, CS$<>8__locals2.CS$<>8__locals1.map)));
					}
					if (!LargeBuildingCellFinder.TryFindCell(out CS$<>8__locals4.cell, map2, parms2, structureRects, extraValidator2, false))
					{
						break;
					}
				}
				IntVec2 size = structureDef.Size;
				structureRects.Add(new CellRect(CS$<>8__locals2.cell.x - size.x / 2, CS$<>8__locals2.cell.z - size.z / 2, size.x, size.z));
				CS$<>8__locals2.structure = ThingMaker.MakeThing(structureDef, null);
				CS$<>8__locals2.structure.SetFaction(Faction.OfEntities, null);
				CS$<>8__locals2.structure.Position = CS$<>8__locals2.cell;
				QuestUtility.AddQuestTag(ref CS$<>8__locals2.structure.questTags, stage.structureTag);
				CS$<>8__locals2.CS$<>8__locals1.quest.SpawnThing(CS$<>8__locals2.CS$<>8__locals1.map, ThingMaker.MakeThing(ThingDefOf.VoidStructureIncoming, null), null, new IntVec3?(CS$<>8__locals2.cell), stage.beganSignal, false, false, null, null, true, null);
				CS$<>8__locals2.CS$<>8__locals1.quest.Delay(EffecterDefOf.VoidStructureIncoming.maintainTicks, delegate
				{
					CS$<>8__locals2.CS$<>8__locals1.quest.SpawnThing(CS$<>8__locals2.CS$<>8__locals1.map, CS$<>8__locals2.structure, null, new IntVec3?(CS$<>8__locals2.cell), null, false, false, null, null, true, EffecterDefOf.VoidStructureSpawningSkipSequence);
					CS$<>8__locals2.CS$<>8__locals1.quest.AddPart(new QuestPart_StructureSpawned(QuestGen.slate.Get<string>("inSignal", null, false), CS$<>8__locals2.structure));
				}, stage.beganSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
				tmpStageStructures.Add(CS$<>8__locals2.structure);
				list.Add(CS$<>8__locals2.structure);
				string text = stage.structureTag + "." + i.ToString();
				QuestUtility.AddQuestTag(ref CS$<>8__locals2.structure.questTags, QuestGenUtility.HardcodedTargetQuestTagWithQuestID(text));
				string inSignal = QuestGenUtility.HardcodedSignalWithQuestID(text + ".Activated");
				if (!CS$<>8__locals2.CS$<>8__locals1.structureActivatedSignal.NullOrEmpty())
				{
					Quest quest = CS$<>8__locals2.CS$<>8__locals1.quest;
					Action action;
					if ((action = CS$<>8__locals2.CS$<>8__locals1.<>9__4) == null)
					{
						action = (CS$<>8__locals2.CS$<>8__locals1.<>9__4 = delegate()
						{
							Quest quest2 = CS$<>8__locals2.CS$<>8__locals1.quest;
							int randomInRange = QuestNode_Root_VoidAwakening.EntityArrivalDelayTicksRange.RandomInRange;
							Action inner;
							if ((inner = CS$<>8__locals2.CS$<>8__locals1.<>9__5) == null)
							{
								inner = (CS$<>8__locals2.CS$<>8__locals1.<>9__5 = delegate()
								{
									CS$<>8__locals2.CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals2.CS$<>8__locals1.structureActivatedSignal);
								});
							}
							quest2.Delay(randomInRange, inner, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
						});
					}
					quest.SignalPass(action, inSignal, null);
				}
			}
			CS$<>8__locals1.quest.Letter(LetterDefOf.ThreatBig, stage.beganSignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, from s in tmpStageStructures
			select new GlobalTargetInfo(s.Position, CS$<>8__locals1.map, false), false, "[voidStructuresLetterText]", null, "[voidStructuresLetterLabel]", null, null);
			CS$<>8__locals1.quest.AddPart(new QuestPart_ActivateStructuresAlert(list, stage.beganSignal, stage.allStructuresActivatedSignal));
		}

		// Token: 0x0400E6E0 RID: 59104
		private const int StageOneNumStructures = 2;

		// Token: 0x0400E6E1 RID: 59105
		private const int StageTwoNumStructures = 3;

		// Token: 0x0400E6E2 RID: 59106
		private static readonly FloatRange InterStageDelayHoursRange = new FloatRange(24f, 48f);

		// Token: 0x0400E6E3 RID: 59107
		private static readonly IntRange EntityArrivalDelayTicksRange = new IntRange(480, 900);

		// Token: 0x0400E6E4 RID: 59108
		private const int GleamingMonolithDelayTicks = 10800;

		// Token: 0x0400E6E5 RID: 59109
		public static readonly LargeBuildingSpawnParms StructureSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};
	}
}
