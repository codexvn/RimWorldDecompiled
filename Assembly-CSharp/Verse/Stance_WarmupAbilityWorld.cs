using System;

namespace Verse
{
	// Token: 0x020007F3 RID: 2035
	public class Stance_WarmupAbilityWorld : Stance_Warmup
	{
		// Token: 0x060033FF RID: 13311 RVA: 0x00110F7C File Offset: 0x0010F17C
		public Stance_WarmupAbilityWorld()
		{
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x00110F84 File Offset: 0x0010F184
		public Stance_WarmupAbilityWorld(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x00110F8F File Offset: 0x0010F18F
		protected override void Expire()
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}
	}
}
