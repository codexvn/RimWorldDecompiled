using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B5D RID: 15197
	public class QuestNode_Root_MechanitorShip : QuestNode
	{
		// Token: 0x06014A67 RID: 84583 RVA: 0x0062BCE4 File Offset: 0x00629EE4
		protected override void RunInt()
		{
			QuestNode_Root_MechanitorShip.<>c__DisplayClass7_0 CS$<>8__locals1 = new QuestNode_Root_MechanitorShip.<>c__DisplayClass7_0();
			if (!ModLister.CheckBiotech("Mechanitor ship"))
			{
				return;
			}
			CS$<>8__locals1.quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			QuestNode_Root_MechanitorShip.<>c__DisplayClass7_0 CS$<>8__locals2 = CS$<>8__locals1;
			Map map;
			if ((map = slate.Get<Map>("map", null, false)) == null)
			{
				map = QuestGen_Get.GetMap(false, null, false);
			}
			CS$<>8__locals2.map = map;
			float num = slate.Get<float>("points", 0f, false);
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("MechanitorShip");
			CS$<>8__locals1.defendTimeoutSignal = QuestGen.GenerateNewSignal("DefendTimeout", true);
			CS$<>8__locals1.beginAssaultSignal = QuestGen.GenerateNewSignal("BeginAssault", true);
			CS$<>8__locals1.assaultBeganSignal = QuestGen.GenerateNewSignal("AssaultBegan", true);
			CS$<>8__locals1.attackedSignal = QuestGenUtility.HardcodedSignalWithQuestID("mechs.TookDamageFromPlayer");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("mechs.Destroyed");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("mechs.Despawned");
			slate.Set<Map>("map", CS$<>8__locals1.map, false);
			CS$<>8__locals1.shuttleContents = new List<Thing>();
			CS$<>8__locals1.mechs = new List<Pawn>();
			this.GeneratePawns(this.mechGroups.RandomElement<ShipPawnGroup>(), num * this.combatPointsFactor, CS$<>8__locals1.quest, CS$<>8__locals1.mechs);
			CS$<>8__locals1.shuttleContents.AddRange(CS$<>8__locals1.mechs);
			string var = (from m in CS$<>8__locals1.mechs
			group m by m.kindDef into g
			select GenLabel.BestKindLabel(g.Key, Gender.None, false, -1) + " x" + g.Count<Pawn>().ToString()).ToLineList(" - ", true);
			slate.Set<string>("mechList", var, false);
			slate.Set<List<Pawn>>("mechs", CS$<>8__locals1.mechs, false);
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Mechanitor_Basic, Faction.OfAncients, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			CS$<>8__locals1.mechanitor = CS$<>8__locals1.quest.GeneratePawn(request, false);
			CS$<>8__locals1.mechanitor.Corpse.Age = QuestNode_Root_MechanitorShip.RandomMechanitorCorpseAge.RandomInRange;
			CS$<>8__locals1.mechanitor.relations.hidePawnRelations = true;
			CS$<>8__locals1.mechanitor.Corpse.GetComp<CompRottable>().RotProgress += (float)CS$<>8__locals1.mechanitor.Corpse.Age;
			CS$<>8__locals1.mechanitor.Corpse.SetForbidden(true, true);
			CS$<>8__locals1.shuttleContents.Add(CS$<>8__locals1.mechanitor.Corpse);
			slate.Set<Pawn>("mechanitor", CS$<>8__locals1.mechanitor, false);
			Thing thing = ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed_Exitable_Mechanitor, null);
			CS$<>8__locals1.quest.SetFaction(Gen.YieldSingle<Thing>(thing), Faction.OfAncients, null);
			this.TryFindShuttleCrashPosition(CS$<>8__locals1.map, thing.def.size, out CS$<>8__locals1.shuttleCrashPosition);
			CS$<>8__locals1.transportShip = CS$<>8__locals1.quest.GenerateTransportShip(TransportShipDefOf.Ship_ShuttleCrashing_Mechanitor, CS$<>8__locals1.shuttleContents, thing, null).transportShip;
			CS$<>8__locals1.quest.AddShipJob_WaitTime(CS$<>8__locals1.transportShip, 60, false, null, null).showGizmos = false;
			CS$<>8__locals1.quest.AddShipJob_Unload(CS$<>8__locals1.transportShip, ShipJobStartMode.Queue, false, null);
			QuestUtility.AddQuestTag(ref CS$<>8__locals1.transportShip.questTags, questTagToAdd);
			CS$<>8__locals1.quest.Delay(180, delegate
			{
				CS$<>8__locals1.quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, CS$<>8__locals1.shuttleContents, false, "[mechanitorShuttleCrashedLetterText]", null, "[mechanitorShuttleCrashedLetterLabel]", null, null);
				CS$<>8__locals1.quest.AddShipJob_Arrive(CS$<>8__locals1.transportShip, CS$<>8__locals1.map.Parent, null, new IntVec3?(CS$<>8__locals1.shuttleCrashPosition), ShipJobStartMode.Force_DelayCurrent, Faction.OfMechanoids, null);
				CS$<>8__locals1.quest.DefendPoint(CS$<>8__locals1.map.Parent, CS$<>8__locals1.mechanitor, CS$<>8__locals1.shuttleCrashPosition, CS$<>8__locals1.mechs, Faction.OfMechanoids, null, null, new float?((float)5), false, true);
				Quest quest = CS$<>8__locals1.quest;
				int delayTicks = 5000;
				Action inner;
				if ((inner = CS$<>8__locals1.<>9__4) == null)
				{
					inner = (CS$<>8__locals1.<>9__4 = delegate()
					{
						CS$<>8__locals1.quest.SignalPass(null, null, CS$<>8__locals1.attackedSignal);
					});
				}
				quest.Delay(delayTicks, inner, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Assault delay";
				CS$<>8__locals1.quest.AnySignal(new string[]
				{
					CS$<>8__locals1.attackedSignal,
					CS$<>8__locals1.defendTimeoutSignal
				}, null, Gen.YieldSingle<string>(CS$<>8__locals1.beginAssaultSignal), QuestPart.SignalListenMode.OngoingOnly);
				Quest quest2 = CS$<>8__locals1.quest;
				Action action;
				if ((action = CS$<>8__locals1.<>9__5) == null)
				{
					action = (CS$<>8__locals1.<>9__5 = delegate()
					{
						Quest quest3 = CS$<>8__locals1.quest;
						IEnumerable<Pawn> mechs = CS$<>8__locals1.mechs;
						Action action2;
						if ((action2 = CS$<>8__locals1.<>9__6) == null)
						{
							action2 = (CS$<>8__locals1.<>9__6 = delegate()
							{
								QuestPart_AssaultColony questPart_AssaultColony = CS$<>8__locals1.quest.AssaultColony(Faction.OfMechanoids, CS$<>8__locals1.map.Parent, CS$<>8__locals1.mechs, null, null);
								questPart_AssaultColony.canKidnap = false;
								questPart_AssaultColony.canSteal = false;
								questPart_AssaultColony.canTimeoutOrFlee = false;
								CS$<>8__locals1.quest.Letter(LetterDefOf.ThreatSmall, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, CS$<>8__locals1.mechs, false, "[assaultBeginLetterText]", null, "[assaultBeginLetterLabel]", null, null);
							});
						}
						quest3.AnyPawnInCombatShape(mechs, action2, null, null, CS$<>8__locals1.assaultBeganSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
					});
				}
				quest2.SignalPassActivable(action, null, CS$<>8__locals1.beginAssaultSignal, null, null, CS$<>8__locals1.assaultBeganSignal, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Arrival delay";
			CS$<>8__locals1.quest.AnySignal(new string[]
			{
				text,
				text2
			}, delegate
			{
				Quest quest = CS$<>8__locals1.quest;
				IEnumerable<Pawn> mechs = CS$<>8__locals1.mechs;
				Action action;
				if ((action = CS$<>8__locals1.<>9__7) == null)
				{
					action = (CS$<>8__locals1.<>9__7 = delegate()
					{
						Quest quest2 = CS$<>8__locals1.quest;
						IEnumerable<Pawn> pawns = Gen.YieldSingle<Pawn>(CS$<>8__locals1.mechanitor);
						HediffDef mechlinkImplant = HediffDefOf.MechlinkImplant;
						Action action2;
						if ((action2 = CS$<>8__locals1.<>9__8) == null)
						{
							action2 = (CS$<>8__locals1.<>9__8 = delegate()
							{
								CS$<>8__locals1.quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle<Corpse>(CS$<>8__locals1.mechanitor.Corpse), false, "[mechlinkAvailableLetterText]", null, "[mechlinkAvailableLetterLabel]", null, null);
							});
						}
						quest2.AnyPawnHasHediff(pawns, mechlinkImplant, action2, null, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
						CS$<>8__locals1.quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
					});
				}
				quest.AllPawnsDespawned(mechs, action, null, null, null, null, null, QuestPart.SignalListenMode.OngoingOnly);
			}, null, QuestPart.SignalListenMode.OngoingOnly);
		}

		// Token: 0x06014A68 RID: 84584 RVA: 0x0062C0D8 File Offset: 0x0062A2D8
		private void GeneratePawns(ShipPawnGroup group, float points, Quest quest, List<Pawn> outPawns)
		{
			foreach (ShipPawnOption shipPawnOption in from o in @group.options
			where o.requireOneOf
			select o)
			{
				outPawns.Add(quest.GeneratePawn(shipPawnOption.pawnKind, Faction.OfMechanoids, true, null, 0f, true, null, 0f, 0f, false, false, DevelopmentalStage.Adult, false));
				points -= shipPawnOption.pawnKind.combatPower;
			}
			Func<ShipPawnOption, bool> <>9__1;
			for (;;)
			{
				IEnumerable<ShipPawnOption> options = group.options;
				Func<ShipPawnOption, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ShipPawnOption o) => o.pawnKind.combatPower < points));
				}
				ShipPawnOption shipPawnOption2;
				if (!options.Where(predicate).TryRandomElementByWeight((ShipPawnOption o) => o.weight, out shipPawnOption2))
				{
					break;
				}
				outPawns.Add(quest.GeneratePawn(shipPawnOption2.pawnKind, Faction.OfMechanoids, true, null, 0f, true, null, 0f, 0f, false, false, DevelopmentalStage.Adult, false));
				points -= shipPawnOption2.pawnKind.combatPower;
			}
		}

		// Token: 0x06014A69 RID: 84585 RVA: 0x0062C238 File Offset: 0x0062A438
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, false);
			IntVec3 intVec;
			return map != null && Faction.OfMechanoids != null && this.TryFindShuttleCrashPosition(map, ThingDefOf.ShuttleCrashed_Exitable_Mechanitor.size, out intVec);
		}

		// Token: 0x06014A6A RID: 84586 RVA: 0x0062C27C File Offset: 0x0062A47C
		private bool TryFindShuttleCrashPosition(Map map, IntVec2 size, out IntVec3 spot)
		{
			return DropCellFinder.FindSafeLandingSpot(out spot, Faction.OfMechanoids, map, 55, 15, 25, new IntVec2?(size), new IntVec3?(ThingDefOf.ShuttleCrashed_Exitable_Mechanitor.interactionCellOffset));
		}

		// Token: 0x0400E56B RID: 58731
		private const string QuestTag = "MechanitorShip";

		// Token: 0x0400E56C RID: 58732
		private const int TicksToShuttleArrival = 180;

		// Token: 0x0400E56D RID: 58733
		private const int TicksToBeginAssault = 5000;

		// Token: 0x0400E56E RID: 58734
		private static readonly IntRange RandomMechanitorCorpseAge = new IntRange(50, 360000000);

		// Token: 0x0400E56F RID: 58735
		private const int LandingSpotDistanceToHostiles = 55;

		// Token: 0x0400E570 RID: 58736
		public float combatPointsFactor = 1f;

		// Token: 0x0400E571 RID: 58737
		public List<ShipPawnGroup> mechGroups;
	}
}
