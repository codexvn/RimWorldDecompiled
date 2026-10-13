using System;

namespace Verse
{
	// Token: 0x0200024E RID: 590
	public static class DietCategoryUtility
	{
		// Token: 0x06000F0A RID: 3850 RVA: 0x000465B4 File Offset: 0x000447B4
		public static string ToStringHuman(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00046648 File Offset: 0x00044848
		public static string ToStringHumanShort(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats_Short".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous_Short".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous_Short".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous_Short".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous_Short".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous_Short".Translate();
			default:
				return "error";
			}
		}
	}
}
