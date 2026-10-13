using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFA RID: 6906
	public class StyleItemSpawningProperties : IExposable
	{
		// Token: 0x06009A43 RID: 39491 RVA: 0x002CDE6E File Offset: 0x002CC06E
		public void ExposeData()
		{
			Scribe_Values.Look<StyleItemFrequency>(ref this.frequency, "frequency", StyleItemFrequency.Never, false);
			Scribe_Values.Look<StyleGender>(ref this.gender, "gender", StyleGender.Male, false);
		}

		// Token: 0x04006A45 RID: 27205
		public StyleItemFrequency frequency;

		// Token: 0x04006A46 RID: 27206
		public StyleGender gender = StyleGender.Any;
	}
}
