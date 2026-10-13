using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200248E RID: 9358
	public static class RestCategoryUtility
	{
		// Token: 0x0600CAF1 RID: 51953 RVA: 0x003B72E8 File Offset: 0x003B54E8
		public static string GetLabel(this RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return "HungerLevel_Rested".Translate();
			case RestCategory.Tired:
				return "HungerLevel_Tired".Translate();
			case RestCategory.VeryTired:
				return "HungerLevel_VeryTired".Translate();
			case RestCategory.Exhausted:
				return "HungerLevel_Exhausted".Translate();
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
