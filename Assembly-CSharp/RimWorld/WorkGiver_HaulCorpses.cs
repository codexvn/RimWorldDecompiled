using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D4 RID: 5588
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		// Token: 0x06008402 RID: 33794 RVA: 0x00279910 File Offset: 0x00277B10
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Corpse))
			{
				return null;
			}
			Pawn pawn2 = pawn.Map.physicalInteractionReservationManager.FirstReserverOf(t);
			if (pawn2 != null && pawn2.RaceProps.Animal && pawn2.Faction != Faction.OfPlayer)
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}

		// Token: 0x06008403 RID: 33795 RVA: 0x00279968 File Offset: 0x00277B68
		public override string PostProcessedGerund(Job job)
		{
			if (job.GetTarget(TargetIndex.B).Thing is Building_Grave)
			{
				return "Burying".Translate();
			}
			return base.PostProcessedGerund(job);
		}
	}
}
