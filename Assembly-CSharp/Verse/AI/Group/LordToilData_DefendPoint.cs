using System;

namespace Verse.AI.Group
{
	// Token: 0x0200109E RID: 4254
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x0600688E RID: 26766 RVA: 0x00210D24 File Offset: 0x0020EF24
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
		}

		// Token: 0x04004A31 RID: 18993
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04004A32 RID: 18994
		public float defendRadius = 28f;

		// Token: 0x04004A33 RID: 18995
		public float? wanderRadius;
	}
}
