using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200162B RID: 5675
	public static class WorkGiverUtility
	{
		// Token: 0x060085D3 RID: 34259 RVA: 0x0027EFF0 File Offset: 0x0027D1F0
		public static Job HaulStuffOffBillGiverJob(Pawn pawn, IBillGiver giver, Thing thingToIgnore)
		{
			foreach (IntVec3 c in giver.IngredientStackCells)
			{
				Thing thing = pawn.Map.thingGrid.ThingAt(c, ThingCategory.Item);
				if (thing != null && thing != thingToIgnore)
				{
					return HaulAIUtility.HaulAsideJobFor(pawn, thing);
				}
			}
			return null;
		}
	}
}
