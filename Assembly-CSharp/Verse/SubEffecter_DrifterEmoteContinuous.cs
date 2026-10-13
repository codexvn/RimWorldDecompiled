using System;

namespace Verse
{
	// Token: 0x020001B4 RID: 436
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x06000C3B RID: 3131 RVA: 0x0003A2E7 File Offset: 0x000384E7
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0003A2F1 File Offset: 0x000384F1
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, -1);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x04000706 RID: 1798
		private int ticksUntilMote;
	}
}
