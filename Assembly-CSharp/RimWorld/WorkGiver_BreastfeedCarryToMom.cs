using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200159C RID: 5532
	public class WorkGiver_BreastfeedCarryToMom : WorkGiver_FeedBabyManually
	{
		// Token: 0x060082E2 RID: 33506 RVA: 0x00274EF8 File Offset: 0x002730F8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> freeColonistsAndPrisonersSpawned = pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < freeColonistsAndPrisonersSpawned.Count; i++)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if ((freeColonistsAndPrisonersSpawned[i].Downed || freeColonistsAndPrisonersSpawned[i].IsPrisoner) && ChildcareUtility.CanBreastfeedNow(freeColonistsAndPrisonersSpawned[i], out breastfeedFailReason))
				{
					flag2 = true;
				}
				if (ChildcareUtility.CanSuckle(freeColonistsAndPrisonersSpawned[i], out breastfeedFailReason))
				{
					flag = true;
				}
				if (flag2 && flag)
				{
					return false;
				}
				if (flag && forced)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060082E3 RID: 33507 RVA: 0x00274F7C File Offset: 0x0027317C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.CanCreateManualFeedingJob(pawn, t, forced))
			{
				return null;
			}
			Pawn baby = (Pawn)t;
			Pawn pawn2;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.ImmobileBreastfeederAvailable(pawn, baby, forced, out pawn2, out breastfeedFailReason))
			{
				if (forced && pawn2 != pawn)
				{
					JobFailReason.Is(breastfeedFailReason.Value.Translate(pawn, pawn2, baby), null);
				}
				return null;
			}
			return ChildcareUtility.MakeBreastfeedCarryToMomJob(baby, pawn2);
		}
	}
}
