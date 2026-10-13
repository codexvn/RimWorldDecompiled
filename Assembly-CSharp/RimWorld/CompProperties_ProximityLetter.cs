using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DC3 RID: 11715
	public class CompProperties_ProximityLetter : CompProperties
	{
		// Token: 0x060107A1 RID: 67489 RVA: 0x004C0D67 File Offset: 0x004BEF67
		public CompProperties_ProximityLetter()
		{
			this.compClass = typeof(CompProximityLetter);
		}

		// Token: 0x0400ABDC RID: 43996
		public int radius = 4;

		// Token: 0x0400ABDD RID: 43997
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400ABDE RID: 43998
		[MustTranslate]
		public string letterText;

		// Token: 0x0400ABDF RID: 43999
		public LetterDef letterDef;
	}
}
