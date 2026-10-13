using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200167F RID: 5759
	public class LordJob_TradeWithColony : LordJob
	{
		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x060087B7 RID: 34743 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060087B8 RID: 34744 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_TradeWithColony()
		{
		}

		// Token: 0x060087B9 RID: 34745 RVA: 0x00288649 File Offset: 0x00286849
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return p.RaceProps.FenceBlocked;
		}

		// Token: 0x060087BA RID: 34746 RVA: 0x0028865B File Offset: 0x0028685B
		public LordJob_TradeWithColony(Faction faction, IntVec3 chillSpot)
		{
			this.faction = faction;
			this.chillSpot = chillSpot;
		}

		// Token: 0x060087BB RID: 34747 RVA: 0x00288674 File Offset: 0x00286874
		public override StateGraph CreateGraph()
		{
			Pawn trader = TraderCaravanUtility.FindTrader(this.lord);
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.chillSpot);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan = new LordToil_DefendTraderCaravan();
			stateGraph.AddToil(lordToil_DefendTraderCaravan);
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan2 = new LordToil_DefendTraderCaravan(this.chillSpot);
			stateGraph.AddToil(lordToil_DefendTraderCaravan2);
			LordToil_ExitMapAndEscortCarriers lordToil_ExitMapAndEscortCarriers = new LordToil_ExitMapAndEscortCarriers();
			stateGraph.AddToil(lordToil_ExitMapAndEscortCarriers);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Walk, true, false);
			stateGraph.AddToil(lordToil_ExitMap2);
			LordToil_ExitMapTraderFighting lordToil_ExitMapTraderFighting = new LordToil_ExitMapTraderFighting();
			stateGraph.AddToil(lordToil_ExitMapTraderFighting);
			Transition transition = new Transition(lordToil_Travel, lordToil_ExitMapAndEscortCarriers, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2
			});
			transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Travel, lordToil_ExitMapAndEscortCarriers, false, true);
			transition2.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2
			});
			transition2.AddPreAction(new TransitionAction_Message("MessageVisitorsAnomalousWeather".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			transition2.AddTrigger(new Trigger_PawnExperiencingAnomalousWeather());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Travel, lordToil_ExitMapAndEscortCarriers, false, true);
			transition3.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2
			});
			transition3.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousConditions".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			Trigger_GameCondition trigger_GameCondition = new Trigger_GameCondition();
			trigger_GameCondition.gameConditions = DefDatabase<GameConditionDef>.AllDefsListForReading.FindAll((GameConditionDef x) => x.causesTraderCaravanExit);
			transition3.AddTrigger(trigger_GameCondition);
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_ExitMap2, false, true);
			transition4.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2,
				lordToil_ExitMapAndEscortCarriers,
				lordToil_ExitMap,
				lordToil_ExitMapTraderFighting
			});
			transition4.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			transition4.AddPostAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition4.AddPostAction(new TransitionAction_WakeAll());
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_ExitMap2, lordToil_ExitMapTraderFighting, false, true);
			transition5.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition5.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_Travel, lordToil_ExitMapTraderFighting, false, true);
			transition6.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2,
				lordToil_ExitMapAndEscortCarriers,
				lordToil_ExitMap
			});
			transition6.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition6.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_Travel, lordToil_DefendTraderCaravan, false, true);
			transition7.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			transition7.AddPreAction(new TransitionAction_SetDefendTrader());
			transition7.AddPostAction(new TransitionAction_WakeAll());
			transition7.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(lordToil_DefendTraderCaravan, lordToil_Travel, false, true);
			transition8.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = new Transition(lordToil_Travel, lordToil_DefendTraderCaravan2, false, true);
			transition9.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = new Transition(lordToil_DefendTraderCaravan2, lordToil_ExitMapAndEscortCarriers, false, true);
			transition10.AddTrigger(new Trigger_TicksPassed(DebugSettings.instantVisitorsGift ? 0 : Rand.Range(27000, 45000)));
			transition10.AddPreAction(new TransitionAction_CheckGiveGift());
			transition10.AddPreAction(new TransitionAction_Message("MessageTraderCaravanLeaving".Translate(this.faction.Name), null, 1f));
			transition10.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = new Transition(lordToil_DefendTraderCaravan2, lordToil_ExitMapAndEscortCarriers, false, true);
			transition11.AddSource(lordToil_Travel);
			transition11.AddTrigger(new Trigger_Custom(delegate(TriggerSignal s)
			{
				if (s.type == TriggerSignalType.Tick)
				{
					Pawn trader = trader;
					Pawn_MindState pawn_MindState = (trader != null) ? trader.mindState : null;
					return pawn_MindState != null && pawn_MindState.traderDismissed;
				}
				return false;
			}));
			transition11.AddPreAction(new TransitionAction_Message("MessageTraderCaravanDismissed".Translate(this.faction.Name), null, 1f));
			transition11.AddPostAction(new TransitionAction_WakeAll());
			transition11.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition11, false);
			Transition transition12 = new Transition(lordToil_ExitMapAndEscortCarriers, lordToil_ExitMapAndEscortCarriers, true, true);
			transition12.canMoveToSameState = true;
			transition12.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			transition12.AddTrigger(new Trigger_TickCondition(() => LordToil_ExitMapAndEscortCarriers.IsAnyDefendingPosition(this.lord.ownedPawns) && !GenHostility.AnyHostileActiveThreatTo(this.Map, this.faction, true, false), 60));
			stateGraph.AddTransition(transition12, false);
			Transition transition13 = new Transition(lordToil_ExitMapAndEscortCarriers, lordToil_ExitMap, false, true);
			transition13.AddTrigger(new Trigger_TicksPassed(60000));
			transition13.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition13, false);
			Transition transition14 = new Transition(lordToil_DefendTraderCaravan2, lordToil_ExitMapAndEscortCarriers, false, true);
			transition14.AddSources(new LordToil[]
			{
				lordToil_Travel,
				lordToil_DefendTraderCaravan
			});
			transition14.AddTrigger(new Trigger_ImportantTraderCaravanPeopleLost());
			transition14.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition14.AddPostAction(new TransitionAction_WakeAll());
			transition14.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition14, false);
			return stateGraph;
		}

		// Token: 0x060087BC RID: 34748 RVA: 0x00288C9C File Offset: 0x00286E9C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.chillSpot, "chillSpot", default(IntVec3), false);
		}

		// Token: 0x04005894 RID: 22676
		private Faction faction;

		// Token: 0x04005895 RID: 22677
		private IntVec3 chillSpot;
	}
}
