using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000903 RID: 2307
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06003B8F RID: 15247 RVA: 0x0013A68D File Offset: 0x0013888D
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0013A69A File Offset: 0x0013889A
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0013A6B4 File Offset: 0x001388B4
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0013A6BC File Offset: 0x001388BC
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}
	}
}
