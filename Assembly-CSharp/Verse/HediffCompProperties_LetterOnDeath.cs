using System;

namespace Verse
{
	// Token: 0x020006DA RID: 1754
	public class HediffCompProperties_LetterOnDeath : HediffCompProperties
	{
		// Token: 0x06002B7C RID: 11132 RVA: 0x000E344C File Offset: 0x000E164C
		public HediffCompProperties_LetterOnDeath()
		{
			this.compClass = typeof(HediffComp_LetterOnDeath);
		}

		// Token: 0x04002300 RID: 8960
		public LetterDef letterDef;

		// Token: 0x04002301 RID: 8961
		[MustTranslate]
		public string letterText;

		// Token: 0x04002302 RID: 8962
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04002303 RID: 8963
		public bool onlyIfNoMechanitorDied;
	}
}
