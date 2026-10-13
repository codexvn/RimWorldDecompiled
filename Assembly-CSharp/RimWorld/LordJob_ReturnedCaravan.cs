using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166C RID: 5740
	public class LordJob_ReturnedCaravan : LordJob
	{
		// Token: 0x0600874F RID: 34639 RVA: 0x0028686B File Offset: 0x00284A6B
		public LordJob_ReturnedCaravan(IntVec3 entryPoint)
		{
			this.entryPoint = entryPoint;
		}

		// Token: 0x06008750 RID: 34640 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_ReturnedCaravan()
		{
		}

		// Token: 0x06008751 RID: 34641 RVA: 0x0028687C File Offset: 0x00284A7C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ReturnedCaravan_PenAnimals lordToil_ReturnedCaravan_PenAnimals = new LordToil_ReturnedCaravan_PenAnimals(this.entryPoint);
			stateGraph.AddToil(lordToil_ReturnedCaravan_PenAnimals);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_ReturnedCaravan_PenAnimals, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_Memo("RepenningFinished"));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06008752 RID: 34642 RVA: 0x002868D0 File Offset: 0x00284AD0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.entryPoint, "entryPoint", default(IntVec3), false);
		}

		// Token: 0x0400585F RID: 22623
		private IntVec3 entryPoint;
	}
}
