using System;

namespace Verse
{
	// Token: 0x02000CC0 RID: 3264
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x0600537B RID: 21371 RVA: 0x001AE9BE File Offset: 0x001ACBBE
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilMote = def.initialDelayTicks;
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x001AE9D4 File Offset: 0x001ACBD4
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.moteCount >= this.def.maxMoteCount)
			{
				return;
			}
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B, -1);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
				this.moteCount++;
			}
		}

		// Token: 0x04003C3F RID: 15423
		private int ticksUntilMote;

		// Token: 0x04003C40 RID: 15424
		private int moteCount;
	}
}
