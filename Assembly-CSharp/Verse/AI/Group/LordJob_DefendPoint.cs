using System;

namespace Verse.AI.Group
{
	// Token: 0x02001094 RID: 4244
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x0600683B RID: 26683 RVA: 0x0020FC9A File Offset: 0x0020DE9A
		public override bool IsCaravanSendable
		{
			get
			{
				return this.isCaravanSendable;
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x0600683C RID: 26684 RVA: 0x0020FCA2 File Offset: 0x0020DEA2
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x0020FCB2 File Offset: 0x0020DEB2
		public LordJob_DefendPoint(IntVec3 point, float? wanderRadius = null, float? defendRadius = null, bool isCaravanSendable = false, bool addFleeToil = true)
		{
			this.point = point;
			this.wanderRadius = wanderRadius;
			this.defendRadius = defendRadius;
			this.isCaravanSendable = isCaravanSendable;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x0600683F RID: 26687 RVA: 0x0020FCE0 File Offset: 0x0020DEE0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			IntVec3 defendPoint = this.point;
			float? num = this.wanderRadius;
			stateGraph.AddToil(new LordToil_DefendPoint(defendPoint, this.defendRadius, num));
			return stateGraph;
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x0020FD14 File Offset: 0x0020DF14
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<float?>(ref this.defendRadius, "defendRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04004A19 RID: 18969
		private IntVec3 point;

		// Token: 0x04004A1A RID: 18970
		private float? wanderRadius;

		// Token: 0x04004A1B RID: 18971
		private float? defendRadius;

		// Token: 0x04004A1C RID: 18972
		private bool isCaravanSendable;

		// Token: 0x04004A1D RID: 18973
		private bool addFleeToil;
	}
}
