using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA5 RID: 6821
	public class NegativeFishingOutcomeDef : Def
	{
		// Token: 0x04006790 RID: 26512
		public ThingDef fishType;

		// Token: 0x04006791 RID: 26513
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04006792 RID: 26514
		[MustTranslate]
		public string letterText;

		// Token: 0x04006793 RID: 26515
		public LetterDef letterDef;

		// Token: 0x04006794 RID: 26516
		public DamageDef damageDef;

		// Token: 0x04006795 RID: 26517
		public IntRange damageAmountRange;

		// Token: 0x04006796 RID: 26518
		public HediffDef addsHediff;

		// Token: 0x04006797 RID: 26519
		public float hediffSeverity;
	}
}
