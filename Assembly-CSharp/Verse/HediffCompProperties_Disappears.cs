using System;

namespace Verse
{
	// Token: 0x0200069E RID: 1694
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x06002A9E RID: 10910 RVA: 0x000E00A9 File Offset: 0x000DE2A9
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x04002282 RID: 8834
		public IntRange disappearsAfterTicks;

		// Token: 0x04002283 RID: 8835
		public bool showRemainingTime;

		// Token: 0x04002284 RID: 8836
		public bool canUseDecimalsShortForm;

		// Token: 0x04002285 RID: 8837
		public MentalStateDef requiredMentalState;

		// Token: 0x04002286 RID: 8838
		[MustTranslate]
		public string messageOnDisappear;

		// Token: 0x04002287 RID: 8839
		[MustTranslate]
		public string letterTextOnDisappear;

		// Token: 0x04002288 RID: 8840
		[MustTranslate]
		public string letterLabelOnDisappear;

		// Token: 0x04002289 RID: 8841
		public bool sendLetterOnDisappearIfDead = true;

		// Token: 0x0400228A RID: 8842
		public bool leaveFreshWounds = true;
	}
}
