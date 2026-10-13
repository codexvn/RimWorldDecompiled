using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BFD RID: 7165
	public class LordJob_WaitForDurationThenExit : LordJob
	{
		// Token: 0x06009F7E RID: 40830 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_WaitForDurationThenExit()
		{
		}

		// Token: 0x06009F7F RID: 40831 RVA: 0x002E0A6B File Offset: 0x002DEC6B
		public LordJob_WaitForDurationThenExit(IntVec3 point, int durationTicks)
		{
			this.point = point;
			this.durationTicks = durationTicks;
		}

		// Token: 0x06009F80 RID: 40832 RVA: 0x002E0A84 File Offset: 0x002DEC84
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_WanderClose lordToil_WanderClose = new LordToil_WanderClose(this.point);
			stateGraph.AddToil(lordToil_WanderClose);
			stateGraph.StartingToil = lordToil_WanderClose;
			LordToil_ExitMapRandom lordToil_ExitMapRandom = new LordToil_ExitMapRandom();
			stateGraph.AddToil(lordToil_ExitMapRandom);
			Transition transition = new Transition(lordToil_WanderClose, lordToil_ExitMapRandom, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(this.durationTicks));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06009F81 RID: 40833 RVA: 0x002E0AEC File Offset: 0x002DECEC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
		}

		// Token: 0x04006E57 RID: 28247
		public IntVec3 point;

		// Token: 0x04006E58 RID: 28248
		public int durationTicks;
	}
}
