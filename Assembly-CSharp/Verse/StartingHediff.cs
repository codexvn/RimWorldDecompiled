using System;

namespace Verse
{
	// Token: 0x020002B7 RID: 695
	public class StartingHediff
	{
		// Token: 0x06001107 RID: 4359 RVA: 0x0004FDBF File Offset: 0x0004DFBF
		public bool HasHediff(Pawn p)
		{
			return p.health.hediffSet.HasHediff(this.def, false);
		}

		// Token: 0x04000E7E RID: 3710
		public HediffDef def;

		// Token: 0x04000E7F RID: 3711
		public float? severity;

		// Token: 0x04000E80 RID: 3712
		public float? chance;

		// Token: 0x04000E81 RID: 3713
		public IntRange? durationTicksRange;
	}
}
