using System;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002F6D RID: 12141
	public class CompUseEffect_PlayWarmupSound : CompUseEffect
	{
		// Token: 0x17002C3B RID: 11323
		// (get) Token: 0x060111A5 RID: 70053 RVA: 0x004E9048 File Offset: 0x004E7248
		private CompProperties_UseEffectPlayWarmupSound Props
		{
			get
			{
				return (CompProperties_UseEffectPlayWarmupSound)this.props;
			}
		}

		// Token: 0x060111A6 RID: 70054 RVA: 0x004E9058 File Offset: 0x004E7258
		public override void PrepareTick()
		{
			if (this.Props.warmupSound != null && this.sustainer == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
				this.sustainer = this.Props.warmupSound.TrySpawnSustainer(info);
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
		}

		// Token: 0x0400B185 RID: 45445
		private Sustainer sustainer;
	}
}
