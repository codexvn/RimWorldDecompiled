using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000173 RID: 371
	public class GeneCategoryDef : Def
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x00032441 File Offset: 0x00030641
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (DefDatabase<GeneCategoryDef>.AllDefs.Any((GeneCategoryDef x) => x != this && x.displayPriorityInXenotype == this.displayPriorityInXenotype))
			{
				yield return "Multiple gene categories share the same displayPriorityInXenotype. This can cause display order issues.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040005A2 RID: 1442
		public float displayPriorityInXenotype;

		// Token: 0x040005A3 RID: 1443
		public float displayPriorityInGenepack;
	}
}
