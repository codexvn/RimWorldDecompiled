using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EF5 RID: 12021
	public abstract class CompProperties_GrayStatue : CompProperties
	{
		// Token: 0x0400B089 RID: 45193
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400B08A RID: 45194
		[MustTranslate]
		public string letterText;

		// Token: 0x0400B08B RID: 45195
		public LetterDef letterDef;

		// Token: 0x0400B08C RID: 45196
		public FloatRange triggerRadiusRange = new FloatRange(3f, 8f);
	}
}
