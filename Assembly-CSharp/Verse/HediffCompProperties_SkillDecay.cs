using System;

namespace Verse
{
	// Token: 0x0200070B RID: 1803
	public class HediffCompProperties_SkillDecay : HediffCompProperties
	{
		// Token: 0x06002C10 RID: 11280 RVA: 0x000E50CB File Offset: 0x000E32CB
		public HediffCompProperties_SkillDecay()
		{
			this.compClass = typeof(HediffComp_SkillDecay);
		}

		// Token: 0x04002368 RID: 9064
		public SimpleCurve decayPerDayPercentageLevelCurve;
	}
}
