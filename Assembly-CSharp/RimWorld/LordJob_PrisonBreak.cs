using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001668 RID: 5736
	public class LordJob_PrisonBreak : LordJob
	{
		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x06008739 RID: 34617 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x0600873A RID: 34618 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600873B RID: 34619 RVA: 0x0028619D File Offset: 0x0028439D
		public LordJob_PrisonBreak()
		{
		}

		// Token: 0x0600873C RID: 34620 RVA: 0x002861AC File Offset: 0x002843AC
		public LordJob_PrisonBreak(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
		}

		// Token: 0x0600873D RID: 34621 RVA: 0x002861D0 File Offset: 0x002843D0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.groupUpLoc);
			lordToil_Travel.maxDanger = Danger.Deadly;
			lordToil_Travel.useAvoidGrid = true;
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_PrisonerEscape lordToil_PrisonerEscape = new LordToil_PrisonerEscape(this.exitPoint, this.sapperThingID);
			lordToil_PrisonerEscape.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_PrisonerEscape);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, false);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, true, false);
			stateGraph.AddToil(lordToil_ExitMap2);
			Transition transition = new Transition(lordToil_Travel, lordToil_ExitMap2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_PrisonerEscape,
				lordToil_ExitMap
			});
			transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_ExitMap2, lordToil_ExitMap, false, true);
			transition2.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition3.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
			transition4.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_PrisonerEscape, lordToil_PrisonerEscape, true, true);
			transition5.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			transition5.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_PrisonerEscape, lordToil_ExitMap, false, true);
			transition6.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x0600873E RID: 34622 RVA: 0x00286360 File Offset: 0x00284560
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
		}

		// Token: 0x0600873F RID: 34623 RVA: 0x002863B9 File Offset: 0x002845B9
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06008740 RID: 34624 RVA: 0x002863B9 File Offset: 0x002845B9
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06008741 RID: 34625 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x06008742 RID: 34626 RVA: 0x002863C4 File Offset: 0x002845C4
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

		// Token: 0x04005852 RID: 22610
		private IntVec3 groupUpLoc;

		// Token: 0x04005853 RID: 22611
		private IntVec3 exitPoint;

		// Token: 0x04005854 RID: 22612
		private int sapperThingID = -1;
	}
}
