using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001687 RID: 5767
	public class LordJob_WaitForEscort : LordJob
	{
		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x060087D5 RID: 34773 RVA: 0x002895E7 File Offset: 0x002877E7
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x060087D6 RID: 34774 RVA: 0x002895EF File Offset: 0x002877EF
		public LordJob_WaitForEscort()
		{
		}

		// Token: 0x060087D7 RID: 34775 RVA: 0x002895FE File Offset: 0x002877FE
		public LordJob_WaitForEscort(IntVec3 point, bool addFleeToil = true)
		{
			this.point = point;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x060087D8 RID: 34776 RVA: 0x0028961C File Offset: 0x0028781C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckRoyalty("Shuttle crash rescue"))
			{
				return stateGraph;
			}
			LordToil_WanderClose lordToil_WanderClose = new LordToil_WanderClose(this.point);
			stateGraph.AddToil(lordToil_WanderClose);
			stateGraph.StartingToil = lordToil_WanderClose;
			return stateGraph;
		}

		// Token: 0x060087D9 RID: 34777 RVA: 0x00289658 File Offset: 0x00287858
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x040058A7 RID: 22695
		public IntVec3 point;

		// Token: 0x040058A8 RID: 22696
		private bool addFleeToil = true;
	}
}
