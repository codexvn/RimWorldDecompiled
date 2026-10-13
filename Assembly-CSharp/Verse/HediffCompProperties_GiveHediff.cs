using System;

namespace Verse
{
	// Token: 0x020006BC RID: 1724
	public class HediffCompProperties_GiveHediff : HediffCompProperties
	{
		// Token: 0x06002B10 RID: 11024 RVA: 0x000E1891 File Offset: 0x000DFA91
		public HediffCompProperties_GiveHediff()
		{
			this.compClass = typeof(HediffComp_GiveHediff);
		}

		// Token: 0x040022BE RID: 8894
		public HediffDef hediffDef;

		// Token: 0x040022BF RID: 8895
		public bool skipIfAlreadyExists;

		// Token: 0x040022C0 RID: 8896
		public float atSeverity = 1f;

		// Token: 0x040022C1 RID: 8897
		public bool disappearsAfterGiving;

		// Token: 0x040022C2 RID: 8898
		public LetterDef letterDef;

		// Token: 0x040022C3 RID: 8899
		[MustTranslate]
		public string letterLabel;

		// Token: 0x040022C4 RID: 8900
		[MustTranslate]
		public string letterText;
	}
}
