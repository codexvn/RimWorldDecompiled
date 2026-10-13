using System;

namespace Verse.AI.Group
{
	// Token: 0x02001096 RID: 4246
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x06006845 RID: 26693 RVA: 0x0020FE97 File Offset: 0x0020E097
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x06006846 RID: 26694 RVA: 0x0020FEA6 File Offset: 0x0020E0A6
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x06006847 RID: 26695 RVA: 0x0020FEDC File Offset: 0x0020E0DC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMapNear lordToil_ExitMapNear = new LordToil_ExitMapNear(this.near, this.radius, this.locomotion, this.canDig);
			if (this.useAvoidGridSmart)
			{
				lordToil_ExitMapNear.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil_ExitMapNear);
			return stateGraph;
		}

		// Token: 0x06006848 RID: 26696 RVA: 0x0020FF24 File Offset: 0x0020E124
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.near, "near", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
		}

		// Token: 0x04004A21 RID: 18977
		private IntVec3 near;

		// Token: 0x04004A22 RID: 18978
		private float radius;

		// Token: 0x04004A23 RID: 18979
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04004A24 RID: 18980
		private bool canDig;

		// Token: 0x04004A25 RID: 18981
		private bool useAvoidGridSmart;

		// Token: 0x04004A26 RID: 18982
		public const float DefaultRadius = 12f;
	}
}
