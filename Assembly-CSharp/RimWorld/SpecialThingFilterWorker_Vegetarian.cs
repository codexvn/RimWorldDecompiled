using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036D4 RID: 14036
	public class SpecialThingFilterWorker_Vegetarian : SpecialThingFilterWorker
	{
		// Token: 0x060137A8 RID: 79784 RVA: 0x005C9119 File Offset: 0x005C7319
		public override bool Matches(Thing t)
		{
			return FoodUtility.AcceptableVegetarian(t) && !FoodUtility.AcceptableCarnivore(t);
		}

		// Token: 0x060137A9 RID: 79785 RVA: 0x005C912E File Offset: 0x005C732E
		public override bool CanEverMatch(ThingDef def)
		{
			return FoodUtility.UnacceptableCarnivore(def);
		}
	}
}
