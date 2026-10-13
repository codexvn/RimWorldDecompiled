using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036D6 RID: 14038
	public class SpecialThingFilterWorker_Cannibal : SpecialThingFilterWorker
	{
		// Token: 0x060137AE RID: 79790 RVA: 0x005C917C File Offset: 0x005C737C
		public override bool Matches(Thing t)
		{
			return FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(t);
		}

		// Token: 0x060137AF RID: 79791 RVA: 0x005C9184 File Offset: 0x005C7384
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsCorpse || def.HasComp(typeof(CompIngredients)) || FoodUtility.GetMeatSourceCategory(def) == MeatSourceCategory.Humanlike;
		}
	}
}
