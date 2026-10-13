using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001597 RID: 5527
	public class Workgiver_AdministerHemogen : WorkGiver_Scanner
	{
		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x060082D3 RID: 33491 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x060082D4 RID: 33492 RVA: 0x00075162 File Offset: 0x00073362
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060082D5 RID: 33493 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060082D6 RID: 33494 RVA: 0x00274C48 File Offset: 0x00272E48
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn)
			{
				return false;
			}
			Pawn_GeneTracker genes = pawn2.genes;
			Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen == null)
			{
				return false;
			}
			if (gene_Hemogen.ValuePercent >= 0.95f)
			{
				return false;
			}
			if (!forced && gene_Hemogen.Value >= 0.25f)
			{
				return false;
			}
			if (!FeedPatientUtility.ShouldBeFed(pawn2))
			{
				return false;
			}
			if (!gene_Hemogen.hemogenPacksAllowed)
			{
				return false;
			}
			if (!gene_Hemogen.ShouldConsumeHemogenNow())
			{
				JobFailReason.Is("NotAllowedHemogen".Translate(), null);
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.HemogenPack), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false) == null)
			{
				JobFailReason.Is("NoIngredient".Translate(ThingDefOf.HemogenPack), null);
				return false;
			}
			return true;
		}

		// Token: 0x060082D7 RID: 33495 RVA: 0x00274D70 File Offset: 0x00272F70
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn t2 = (Pawn)t;
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.HemogenPack), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.FeedPatient, thing, t2);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x040056A6 RID: 22182
		private const float MinLevelForFeedingHemogenUnforced = 0.25f;

		// Token: 0x040056A7 RID: 22183
		private const float HemogenPctMax = 0.95f;
	}
}
