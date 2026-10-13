using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B63 RID: 15203
	public class QuestNode_Root_MechanoidSignal : QuestNode
	{
		// Token: 0x06014A82 RID: 84610 RVA: 0x0062C890 File Offset: 0x0062AA90
		protected override void RunInt()
		{
			QuestNode_Root_MechanoidSignal.<>c__DisplayClass4_0 CS$<>8__locals1 = new QuestNode_Root_MechanoidSignal.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.quest = QuestGen.quest;
			CS$<>8__locals1.map = QuestGen_Get.GetMap(false, null, false);
			CS$<>8__locals1.dropPodsSpawnedSignal = QuestGenUtility.HardcodedSignalWithQuestID("dropPodsSpawned");
			CS$<>8__locals1.gravEngine = ThingMaker.MakeThing(ThingDefOf.GravEngine, null);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("gravEngine.Inspected");
			QuestUtility.AddQuestTag(CS$<>8__locals1.gravEngine, QuestGenUtility.HardcodedTargetQuestTagWithQuestID("gravEngine"));
			CS$<>8__locals1.quest.Delay(300, delegate
			{
				QuestNode_Root_MechanoidSignal.<>c__DisplayClass4_1 CS$<>8__locals2 = new QuestNode_Root_MechanoidSignal.<>c__DisplayClass4_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.spots = new List<IntVec3>();
				List<PawnKindDef> mechTypes = new List<PawnKindDef>
				{
					PawnKindDefOf.Mech_Pikeman,
					PawnKindDefOf.Mech_Scyther
				};
				int num = QuestNode_Root_MechanoidSignal.ChunkCountRange.RandomInRange + 1;
				int gravlitePanelCount = QuestNode_Root_MechanoidSignal.GravlitePanelCountTotal.RandomInRange / (num - 1);
				IntVec3 root;
				CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, CS$<>8__locals2.CS$<>8__locals1.map, false, false, true) && CS$<>8__locals2.CS$<>8__locals1.<>4__this.CanLandHere(c, CS$<>8__locals2.CS$<>8__locals1.map, true, CS$<>8__locals2.spots), out root);
				int i;
				int j;
				for (i = 0; i < num; i = j + 1)
				{
					Skyfaller skyfaller = SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShipChunkIncoming_SmallExplosion, CS$<>8__locals1.<>4__this.ChunkContents(CS$<>8__locals1.quest, CS$<>8__locals1.gravEngine, i, mechTypes, gravlitePanelCount));
					skyfaller.contentsCanOverlap = false;
					skyfaller.moveAside = true;
					QuestPart_SpawnThing questPart_SpawnThing = new QuestPart_SpawnThing();
					questPart_SpawnThing.thing = skyfaller;
					questPart_SpawnThing.mapParentOfPawn = CS$<>8__locals1.map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
					questPart_SpawnThing.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
					questPart_SpawnThing.cell = CellFinder.RandomClosewalkCellNear(root, CS$<>8__locals1.map, 15, (IntVec3 x) => CS$<>8__locals2.CS$<>8__locals1.<>4__this.CanLandHere(x, CS$<>8__locals2.CS$<>8__locals1.map, i == 0, CS$<>8__locals2.spots));
					questPart_SpawnThing.questLookTarget = false;
					CS$<>8__locals2.spots.Add(questPart_SpawnThing.cell);
					CS$<>8__locals1.quest.AddPart(questPart_SpawnThing);
					j = i;
				}
				CS$<>8__locals1.quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Thing>(CS$<>8__locals1.gravEngine), false, "[gravEngineSpawnedLetterText]", null, "[gravEngineSpawnedLetterLabel]", null, null);
				CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.dropPodsSpawnedSignal);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			CS$<>8__locals1.quest.LookTargets(Gen.YieldSingle<GlobalTargetInfo>(new GlobalTargetInfo(CS$<>8__locals1.gravEngine)));
			QuestPart_Choice questPart_Choice = CS$<>8__locals1.quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef
			{
				thingDef = ThingDefOf.GravEngine
			});
			questPart_Choice.choices.Add(choice);
			CS$<>8__locals1.quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x06014A83 RID: 84611 RVA: 0x0062C9A0 File Offset: 0x0062ABA0
		private bool CanLandHere(IntVec3 cell, Map map, bool engine, List<IntVec3> spots)
		{
			for (int i = 0; i < spots.Count; i++)
			{
				if (cell.InHorDistOf(spots[i], 3f))
				{
					return false;
				}
			}
			if (engine)
			{
				IntVec2 size = ThingDefOf.GravEngine.Size;
				CellRect cellRect = new CellRect(cell.x, cell.z, size.x, size.z);
				foreach (IntVec3 c in cellRect)
				{
					if (!c.InBounds(map))
					{
						return false;
					}
					if (!DropCellFinder.CanPhysicallyDropInto(c, map, false, false))
					{
						return false;
					}
					if (!GenConstruct.CanBuildOnTerrain(ThingDefOf.GravEngine, c, map, ThingDefOf.GravEngine.defaultPlacingRot, null, null))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06014A84 RID: 84612 RVA: 0x0062CA88 File Offset: 0x0062AC88
		private List<Thing> ChunkContents(Quest quest, Thing gravEngine, int index, List<PawnKindDef> mechTypes, int gravlitePanelCount)
		{
			List<Thing> list = new List<Thing>();
			if (index == 0)
			{
				list.Add(gravEngine);
				return list;
			}
			list.Add(ThingMaker.MakeThing(ThingDefOf.ShipChunk_Mech, null));
			int randomInRange = QuestNode_Root_MechanoidSignal.DeadMechCountPerChunk.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn pawn = quest.GeneratePawn(new PawnGenerationRequest(mechTypes.RandomElement<PawnKindDef>(), Faction.OfMechanoids, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false), false);
				Corpse corpse = (pawn != null) ? pawn.Corpse : null;
				if (corpse != null)
				{
					corpse.SetForbidden(true, false);
					list.Add(corpse);
				}
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.GravlitePanel, null);
			thing.stackCount = gravlitePanelCount;
			thing.SetForbidden(true, false);
			list.Add(thing);
			return list;
		}

		// Token: 0x06014A85 RID: 84613 RVA: 0x0062CBC4 File Offset: 0x0062ADC4
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive && QuestGen_Get.GetMap(false, null, false) != null && !GravshipUtility.PlayerHasGravEngine();
		}

		// Token: 0x0400E58F RID: 58767
		private const int SkyfallerDelayTicks = 300;

		// Token: 0x0400E590 RID: 58768
		private static readonly IntRange ChunkCountRange = new IntRange(4, 6);

		// Token: 0x0400E591 RID: 58769
		private static readonly IntRange DeadMechCountPerChunk = new IntRange(0, 2);

		// Token: 0x0400E592 RID: 58770
		private static readonly IntRange GravlitePanelCountTotal = new IntRange(350, 375);
	}
}
