using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001685 RID: 5765
	public class LordJob_VoidAwakeningDefendStructure : LordJob
	{
		// Token: 0x060087CF RID: 34767 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_VoidAwakeningDefendStructure()
		{
		}

		// Token: 0x060087D0 RID: 34768 RVA: 0x002894D4 File Offset: 0x002876D4
		public LordJob_VoidAwakeningDefendStructure(IntVec3 defendSpot, float defendRadius)
		{
			this.defendSpot = defendSpot;
			this.defendRadius = defendRadius;
		}

		// Token: 0x060087D1 RID: 34769 RVA: 0x002894EC File Offset: 0x002876EC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
		}

		// Token: 0x060087D2 RID: 34770 RVA: 0x0028952C File Offset: 0x0028772C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defendSpot, new float?(this.defendRadius), null);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(Faction.OfEntities, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_DefendPoint, startingToil, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.33f, false, null, null, null));
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x040058A5 RID: 22693
		private IntVec3 defendSpot;

		// Token: 0x040058A6 RID: 22694
		private float defendRadius;
	}
}
