using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D5F RID: 11615
	public class CompMelter : ThingComp
	{
		// Token: 0x06010519 RID: 66841 RVA: 0x004B6308 File Offset: 0x004B4508
		public override void CompTickRare()
		{
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature < 0f)
			{
				return;
			}
			int num = GenMath.RoundRandom(0.15f * (ambientTemperature / 10f));
			if (num > 0)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x0400AA3F RID: 43583
		private const float MeltPerIntervalPer10Degrees = 0.15f;
	}
}
