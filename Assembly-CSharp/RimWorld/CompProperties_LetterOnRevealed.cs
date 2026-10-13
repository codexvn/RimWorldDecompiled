using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D1C RID: 11548
	public class CompProperties_LetterOnRevealed : CompProperties
	{
		// Token: 0x060103A4 RID: 66468 RVA: 0x004B166F File Offset: 0x004AF86F
		public CompProperties_LetterOnRevealed()
		{
			this.compClass = typeof(CompLetterOnRevealed);
		}

		// Token: 0x0400A977 RID: 43383
		[MustTranslate]
		public string label;

		// Token: 0x0400A978 RID: 43384
		[MustTranslate]
		public string text;

		// Token: 0x0400A979 RID: 43385
		public LetterDef letterDef;
	}
}
