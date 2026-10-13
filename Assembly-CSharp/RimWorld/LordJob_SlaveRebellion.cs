using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001677 RID: 5751
	public class LordJob_SlaveRebellion : LordJob
	{
		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x06008780 RID: 34688 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x06008781 RID: 34689 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x06008782 RID: 34690 RVA: 0x00287688 File Offset: 0x00285888
		public bool IsAggressiveRebellion
		{
			get
			{
				return !this.passive;
			}
		}

		// Token: 0x06008783 RID: 34691 RVA: 0x00287693 File Offset: 0x00285893
		public LordJob_SlaveRebellion()
		{
		}

		// Token: 0x06008784 RID: 34692 RVA: 0x002876A2 File Offset: 0x002858A2
		public LordJob_SlaveRebellion(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID, bool passive = true)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
			this.passive = passive;
		}

		// Token: 0x06008785 RID: 34693 RVA: 0x002876D0 File Offset: 0x002858D0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckIdeology("Slave rebellion"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.groupUpLoc);
			lordToil_Travel.maxDanger = Danger.Deadly;
			lordToil_Travel.useAvoidGrid = true;
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_AssaultColonyPrisoners lordToil_AssaultColonyPrisoners = new LordToil_AssaultColonyPrisoners();
			lordToil_AssaultColonyPrisoners.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_AssaultColonyPrisoners);
			LordToil_PrisonerEscape lordToil_PrisonerEscape = new LordToil_PrisonerEscape(this.exitPoint, this.sapperThingID);
			lordToil_PrisonerEscape.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_PrisonerEscape);
			LordToil_ExitMapFighting lordToil_ExitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Jog, true, false);
			lordToil_ExitMapFighting.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMapFighting);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, true, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, false, false);
			lordToil_ExitMap2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap2);
			if (!this.passive)
			{
				Transition transition = new Transition(lordToil_Travel, lordToil_AssaultColonyPrisoners, false, true);
				transition.AddTrigger(new Trigger_Memo("TravelArrived"));
				transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
				stateGraph.AddTransition(transition, false);
				Transition transition2 = new Transition(lordToil_AssaultColonyPrisoners, lordToil_ExitMapFighting, false, true);
				transition2.AddTrigger(new Trigger_FractionColonyDamageTaken(LordJob_SlaveRebellion.DesiredDamageRange.RandomInRange, 900f));
				transition2.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition2, false);
				Transition transition3 = new Transition(lordToil_AssaultColonyPrisoners, lordToil_ExitMapFighting, false, true);
				transition3.AddTrigger(new Trigger_TicksPassed(LordJob_SlaveRebellion.AssaultTimeBeforeGiveUp.RandomInRange));
				transition3.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition3, false);
			}
			else
			{
				Transition transition4 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
				transition4.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition4, false);
				Transition transition5 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
				transition5.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
				stateGraph.AddTransition(transition5, false);
				Transition transition6 = new Transition(lordToil_Travel, lordToil_ExitMap, false, true);
				transition6.AddSources(new LordToil[]
				{
					lordToil_AssaultColonyPrisoners,
					lordToil_PrisonerEscape,
					lordToil_ExitMapFighting
				});
				transition6.AddTrigger(new Trigger_PawnCannotReachMapEdge());
				stateGraph.AddTransition(transition6, false);
				Transition transition7 = new Transition(lordToil_ExitMap, lordToil_PrisonerEscape, false, true);
				transition7.AddTrigger(new Trigger_PawnCanReachMapEdge());
				transition7.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition7, false);
			}
			Transition transition8 = new Transition(lordToil_PrisonerEscape, lordToil_ExitMap2, false, true);
			transition8.AddSource(lordToil_ExitMapFighting);
			transition8.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition8, false);
			return stateGraph;
		}

		// Token: 0x06008786 RID: 34694 RVA: 0x00287940 File Offset: 0x00285B40
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
			Scribe_Values.Look<bool>(ref this.passive, "passive", false, false);
		}

		// Token: 0x06008787 RID: 34695 RVA: 0x002863B9 File Offset: 0x002845B9
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06008788 RID: 34696 RVA: 0x002863B9 File Offset: 0x002845B9
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06008789 RID: 34697 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x0600878A RID: 34698 RVA: 0x002879AC File Offset: 0x00285BAC
		public override bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn == null)
			{
				return true;
			}
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			return mentalStateDef == null || !mentalStateDef.escapingPrisonersIgnore;
		}

		// Token: 0x0400587A RID: 22650
		private IntVec3 groupUpLoc;

		// Token: 0x0400587B RID: 22651
		private IntVec3 exitPoint;

		// Token: 0x0400587C RID: 22652
		private bool passive;

		// Token: 0x0400587D RID: 22653
		private int sapperThingID = -1;

		// Token: 0x0400587E RID: 22654
		private static FloatRange DesiredDamageRange = new FloatRange(0.25f, 0.35f);

		// Token: 0x0400587F RID: 22655
		private const float MinDamage = 900f;

		// Token: 0x04005880 RID: 22656
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);
	}
}
