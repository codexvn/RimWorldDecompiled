using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E8 RID: 5096
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		// Token: 0x06007C64 RID: 31844 RVA: 0x002575D2 File Offset: 0x002557D2
		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x002575DC File Offset: 0x002557DC
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing result;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out result, out thingDef, false, true, false, true, true, true, false, false, false, false, false, FoodPreferability.RawTasty))
			{
				return result;
			}
			return null;
		}

		// Token: 0x040053E9 RID: 21481
		private const int BaseIngestInterval = 1100;
	}
}
