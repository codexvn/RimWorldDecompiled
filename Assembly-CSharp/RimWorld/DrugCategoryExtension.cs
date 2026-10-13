using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199C RID: 6556
	public static class DrugCategoryExtension
	{
		// Token: 0x06009463 RID: 37987 RVA: 0x002B9411 File Offset: 0x002B7611
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}

		// Token: 0x06009464 RID: 37988 RVA: 0x002B941C File Offset: 0x002B761C
		public static string GetLabel(this DrugCategory category)
		{
			switch (category)
			{
			case DrugCategory.None:
				return "DrugCategory_None".Translate();
			case DrugCategory.Medical:
				return "DrugCategory_Medical".Translate();
			case DrugCategory.Social:
				return "DrugCategory_Social".Translate();
			case DrugCategory.Hard:
				return "DrugCategory_Hard".Translate();
			}
			return "DrugCategory_Any".Translate();
		}
	}
}
