using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200164B RID: 5707
	public class LordJob_DefendAndExpandHive : LordJob
	{
		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x06008691 RID: 34449 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x06008692 RID: 34450 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008693 RID: 34451 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_DefendAndExpandHive()
		{
		}

		// Token: 0x06008694 RID: 34452 RVA: 0x00282EBC File Offset: 0x002810BC
		public LordJob_DefendAndExpandHive(SpawnedPawnParams parms)
		{
			this.aggressive = parms.aggressive;
		}

		// Token: 0x06008695 RID: 34453 RVA: 0x00282ED0 File Offset: 0x002810D0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendAndExpandHive lordToil_DefendAndExpandHive = new LordToil_DefendAndExpandHive();
			lordToil_DefendAndExpandHive.distToHiveToAttack = 10f;
			stateGraph.StartingToil = lordToil_DefendAndExpandHive;
			LordToil_DefendHiveAggressively lordToil_DefendHiveAggressively = new LordToil_DefendHiveAggressively();
			lordToil_DefendHiveAggressively.distToHiveToAttack = 40f;
			stateGraph.AddToil(lordToil_DefendHiveAggressively);
			LordToil lordToil = new LordToil_AssaultColony(false, false);
			if (base.Map.generatorDef == MapGeneratorDefOf.InsectLair)
			{
				lordToil = new LordToil_NestAssault(false, false);
			}
			stateGraph.AddToil(lordToil);
			Transition transition = new Transition(lordToil_DefendAndExpandHive, this.aggressive ? lordToil : lordToil_DefendHiveAggressively, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null, null, null));
			transition.AddTrigger(new Trigger_PawnLostViolently(false));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoAttackedByEnemy));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoBurnedBadly));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoDestroyedNonRoofCollapse));
			transition.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendAndExpandHive, lordToil, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction, null, null));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendHiveAggressively, lordToil, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction, null, null));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_DefendAndExpandHive, lordToil_DefendAndExpandHive, true, true);
			transition4.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_DefendHiveAggressively, lordToil_DefendHiveAggressively, true, true);
			transition5.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil, lordToil_DefendAndExpandHive, false, true);
			transition6.AddSource(lordToil_DefendHiveAggressively);
			transition6.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1200, new string[]
			{
				Hive.MemoAttackedByEnemy,
				Hive.MemoBurnedBadly,
				Hive.MemoDestroyedNonRoofCollapse,
				Hive.MemoDeSpawned,
				HediffGiver_Heat.MemoPawnBurnedByAir
			}));
			transition6.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x06008696 RID: 34454 RVA: 0x00283118 File Offset: 0x00281318
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}

		// Token: 0x06008697 RID: 34455 RVA: 0x00283124 File Offset: 0x00281324
		public override bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			if (!base.ValidateAttackTarget(searcher, target))
			{
				return false;
			}
			MentalStateDef mentalStateDef = searcher.MentalStateDef;
			return mentalStateDef == null || !mentalStateDef.blocksDefendAndExpandHive;
		}

		// Token: 0x06008698 RID: 34456 RVA: 0x00283152 File Offset: 0x00281352
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
		}

		// Token: 0x040057EB RID: 22507
		private bool aggressive;
	}
}
