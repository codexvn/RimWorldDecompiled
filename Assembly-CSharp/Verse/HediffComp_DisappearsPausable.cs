using System;

namespace Verse
{
	// Token: 0x020006A0 RID: 1696
	public class HediffComp_DisappearsPausable : HediffComp_Disappears
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002AB1 RID: 10929 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool Paused
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002AB2 RID: 10930 RVA: 0x000E04CF File Offset: 0x000DE6CF
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Paused)
				{
					return base.CompLabelInBracketsExtra;
				}
				return null;
			}
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x000E04E1 File Offset: 0x000DE6E1
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.Paused)
			{
				this.ticksToDisappear -= this.TicksLostPerTick;
			}
		}
	}
}
