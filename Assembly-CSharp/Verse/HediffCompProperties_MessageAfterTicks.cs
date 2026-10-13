using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006E0 RID: 1760
	public class HediffCompProperties_MessageAfterTicks : HediffCompProperties
	{
		// Token: 0x06002B8F RID: 11151 RVA: 0x000E38AC File Offset: 0x000E1AAC
		public HediffCompProperties_MessageAfterTicks()
		{
			this.compClass = typeof(HediffComp_MessageAfterTicks);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000E38C4 File Offset: 0x000E1AC4
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.ticks <= 0)
			{
				yield return "ticks must be a positive value";
			}
			yield break;
		}

		// Token: 0x0400230F RID: 8975
		public int ticks;

		// Token: 0x04002310 RID: 8976
		public MessageTypeDef messageType;

		// Token: 0x04002311 RID: 8977
		public LetterDef letterType;

		// Token: 0x04002312 RID: 8978
		[MustTranslate]
		public string message;

		// Token: 0x04002313 RID: 8979
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04002314 RID: 8980
		[MustTranslate]
		public string letterText;
	}
}
