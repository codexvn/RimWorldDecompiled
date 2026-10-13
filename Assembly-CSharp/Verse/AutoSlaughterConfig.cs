using System;

namespace Verse
{
	// Token: 0x020003CD RID: 973
	public class AutoSlaughterConfig : IExposable
	{
		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x0007DFA0 File Offset: 0x0007C1A0
		public bool AnyLimit
		{
			get
			{
				return this.maxTotal != -1 || this.maxMales != -1 || this.maxFemales != -1 || this.maxMalesYoung != -1 || this.maxFemalesYoung != -1 || !this.allowSlaughterPregnant || !this.allowSlaughterBonded;
			}
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0007DFF0 File Offset: 0x0007C1F0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.animal, "animal");
			Scribe_Values.Look<int>(ref this.maxTotal, "maxTotal", -1, false);
			Scribe_Values.Look<int>(ref this.maxMales, "maxMales", -1, false);
			Scribe_Values.Look<int>(ref this.maxMalesYoung, "maxMalesYoung", -1, false);
			Scribe_Values.Look<int>(ref this.maxFemales, "maxFemales", -1, false);
			Scribe_Values.Look<int>(ref this.maxFemalesYoung, "maxFemalesYoung", -1, false);
			Scribe_Values.Look<bool>(ref this.allowSlaughterPregnant, "allowSlaughterPregnant", false, false);
			Scribe_Values.Look<bool>(ref this.allowSlaughterBonded, "allowSlaughterBonded", false, false);
		}

		// Token: 0x04001610 RID: 5648
		public ThingDef animal;

		// Token: 0x04001611 RID: 5649
		public int maxTotal = -1;

		// Token: 0x04001612 RID: 5650
		public int maxMales = -1;

		// Token: 0x04001613 RID: 5651
		public int maxMalesYoung = -1;

		// Token: 0x04001614 RID: 5652
		public int maxFemales = -1;

		// Token: 0x04001615 RID: 5653
		public int maxFemalesYoung = -1;

		// Token: 0x04001616 RID: 5654
		public bool allowSlaughterPregnant;

		// Token: 0x04001617 RID: 5655
		public bool allowSlaughterBonded;

		// Token: 0x04001618 RID: 5656
		public string uiMaxTotalBuffer;

		// Token: 0x04001619 RID: 5657
		public string uiMaxMalesBuffer;

		// Token: 0x0400161A RID: 5658
		public string uiMaxMalesYoungBuffer;

		// Token: 0x0400161B RID: 5659
		public string uiMaxFemalesBuffer;

		// Token: 0x0400161C RID: 5660
		public string uiMaxFemalesYoungBuffer;

		// Token: 0x0400161D RID: 5661
		public const int NoLimit = -1;
	}
}
