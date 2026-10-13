using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200159B RID: 5531
	public class WorkGiver_Breastfeed : WorkGiver_Scanner
	{
		// Token: 0x060082DF RID: 33503 RVA: 0x00274EA6 File Offset: 0x002730A6
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x060082E0 RID: 33504 RVA: 0x00274EB8 File Offset: 0x002730B8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced)
		{
			if (forced)
			{
				return null;
			}
			Pawn pawn2 = t as Pawn;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanMomAutoBreastfeedBabyNow(pawn, pawn2, forced, out breastfeedFailReason))
			{
				return null;
			}
			if (pawn2.mindState.AutofeedSetting(pawn) != AutofeedMode.Childcare)
			{
				return null;
			}
			return ChildcareUtility.MakeBreastfeedJob(pawn2, null);
		}
	}
}
