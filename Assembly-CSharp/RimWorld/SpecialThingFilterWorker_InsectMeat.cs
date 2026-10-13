using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036D7 RID: 14039
	public class SpecialThingFilterWorker_InsectMeat : SpecialThingFilterWorker
	{
		// Token: 0x060137B1 RID: 79793 RVA: 0x005C91AB File Offset: 0x005C73AB
		public override bool Matches(Thing t)
		{
			return FoodUtility.IsInsectCorpseOrInsectMeatIngredient(t);
		}

		// Token: 0x060137B2 RID: 79794 RVA: 0x005C91B3 File Offset: 0x005C73B3
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsCorpse || def.HasComp(typeof(CompIngredients)) || FoodUtility.GetMeatSourceCategory(def) == MeatSourceCategory.Insect;
		}
	}
}
