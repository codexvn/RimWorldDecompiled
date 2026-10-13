using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001682 RID: 5762
	public class LordJob_Venerate : LordJob
	{
		// Token: 0x060087C3 RID: 34755 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_Venerate()
		{
		}

		// Token: 0x060087C4 RID: 34756 RVA: 0x00288D59 File Offset: 0x00286F59
		public LordJob_Venerate(Thing target, int venerateDurationTicks, string outSignalVenerationCompleted = null, string forceExitSignal = null)
		{
			this.target = target;
			this.venerateDurationTicks = venerateDurationTicks;
			this.outSignalVenerationCompleted = outSignalVenerationCompleted;
			this.forceExitSignal = forceExitSignal;
		}

		// Token: 0x060087C5 RID: 34757 RVA: 0x00288D80 File Offset: 0x00286F80
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.target.InteractionCell);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_Venerate lordToil_Venerate = new LordToil_Venerate(this.target);
			stateGraph.AddToil(lordToil_Venerate);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMapAndDefendSelf toil = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(toil);
			Transition transition = new Transition(lordToil_Travel, lordToil_Venerate, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Venerate, lordToil_ExitMap, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(this.venerateDurationTicks));
			if (!this.outSignalVenerationCompleted.NullOrEmpty())
			{
				transition2.AddPostAction(new TransitionAction_Custom(delegate()
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalVenerationCompleted, false));
				}));
			}
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Venerate, toil, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddSource(lordToil_ExitMap);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_ExitMap, false, true);
			transition4.AddSource(lordToil_Venerate);
			transition4.AddTrigger(new Trigger_AnyThingDamageTaken(new List<Thing>
			{
				this.target
			}, 1f));
			if (this.forceExitSignal != null)
			{
				transition4.AddTrigger(new Trigger_Signal(this.forceExitSignal));
			}
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition4, false);
			return stateGraph;
		}

		// Token: 0x060087C6 RID: 34758 RVA: 0x00288F00 File Offset: 0x00287100
		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<int>(ref this.venerateDurationTicks, "venerateDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalVenerationCompleted, "outSignalVenerationCompleted", null, false);
			Scribe_Values.Look<string>(ref this.forceExitSignal, "forceExitSignal", null, false);
		}

		// Token: 0x0400589A RID: 22682
		private Thing target;

		// Token: 0x0400589B RID: 22683
		private int venerateDurationTicks;

		// Token: 0x0400589C RID: 22684
		private string outSignalVenerationCompleted;

		// Token: 0x0400589D RID: 22685
		private string forceExitSignal;
	}
}
