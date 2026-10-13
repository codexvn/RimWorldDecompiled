using System;

namespace Verse
{
	// Token: 0x02000722 RID: 1826
	public class HediffCompProperties_SeverityPerDay : HediffCompProperties
	{
		// Token: 0x06002C7F RID: 11391 RVA: 0x000E6A02 File Offset: 0x000E4C02
		public HediffCompProperties_SeverityPerDay()
		{
			this.compClass = typeof(HediffComp_SeverityPerDay);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000E6A30 File Offset: 0x000E4C30
		public float CalculateSeverityPerDay()
		{
			float num = this.severityPerDay + this.severityPerDayRange.RandomInRange;
			if (Rand.Chance(this.reverseSeverityChangeChance))
			{
				num *= -1f;
			}
			return num;
		}

		// Token: 0x040023A3 RID: 9123
		public float severityPerDay;

		// Token: 0x040023A4 RID: 9124
		public bool showDaysToRecover;

		// Token: 0x040023A5 RID: 9125
		public bool showHoursToRecover;

		// Token: 0x040023A6 RID: 9126
		public float mechanitorFactor = 1f;

		// Token: 0x040023A7 RID: 9127
		public float reverseSeverityChangeChance;

		// Token: 0x040023A8 RID: 9128
		public FloatRange severityPerDayRange = FloatRange.Zero;

		// Token: 0x040023A9 RID: 9129
		public float minAge;
	}
}
