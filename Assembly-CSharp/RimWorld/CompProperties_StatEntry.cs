using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002147 RID: 8519
	public class CompProperties_StatEntry : CompProperties
	{
		// Token: 0x0600BA1E RID: 47646 RVA: 0x0035B3FF File Offset: 0x003595FF
		public CompProperties_StatEntry()
		{
			this.compClass = typeof(CompStatEntry);
		}

		// Token: 0x04008052 RID: 32850
		public StatCategoryDef statCategoryDef;

		// Token: 0x04008053 RID: 32851
		[MustTranslate]
		public string statLabel;

		// Token: 0x04008054 RID: 32852
		[MustTranslate]
		public string reportText;

		// Token: 0x04008055 RID: 32853
		public string valueString;

		// Token: 0x04008056 RID: 32854
		public Func<CompStatEntry, string> valueFunc;

		// Token: 0x04008057 RID: 32855
		public int displayPriorityInCategory;
	}
}
