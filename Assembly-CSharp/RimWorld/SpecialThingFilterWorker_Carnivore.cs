using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036D5 RID: 14037
	public class SpecialThingFilterWorker_Carnivore : SpecialThingFilterWorker
	{
		// Token: 0x060137AB RID: 79787 RVA: 0x005C9136 File Offset: 0x005C7336
		public override bool Matches(Thing t)
		{
			return !FoodUtility.AcceptableVegetarian(t) && FoodUtility.AcceptableCarnivore(t) && !FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(t) && !FoodUtility.IsInsectCorpseOrInsectMeatIngredient(t);
		}

		// Token: 0x060137AC RID: 79788 RVA: 0x005C915B File Offset: 0x005C735B
		public override bool CanEverMatch(ThingDef def)
		{
			return FoodUtility.UnacceptableVegetarian(def) && FoodUtility.GetMeatSourceCategory(def) != MeatSourceCategory.Humanlike && FoodUtility.GetMeatSourceCategory(def) != MeatSourceCategory.Insect;
		}
	}
}
