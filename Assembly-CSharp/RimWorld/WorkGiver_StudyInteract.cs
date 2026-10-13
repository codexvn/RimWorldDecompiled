using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001614 RID: 5652
	public class WorkGiver_StudyInteract : WorkGiver_StudyBase
	{
		// Token: 0x0600855C RID: 34140 RVA: 0x0027E03C File Offset: 0x0027C23C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (t == pawn)
			{
				return false;
			}
			CompStudiable compStudiable = t.TryGetComp<CompStudiable>();
			if (compStudiable == null || compStudiable.KnowledgeCategory != null)
			{
				return false;
			}
			if (!compStudiable.EverStudiable())
			{
				JobFailReason.IsSilent();
				return false;
			}
			if (!compStudiable.CurrentlyStudiable())
			{
				if (compStudiable.Props.frequencyTicks > 0 && compStudiable.TicksTilNextStudy > 0)
				{
					JobFailReason.Is("CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst(), null);
				}
				else
				{
					JobFailReason.IsSilent();
				}
				return false;
			}
			IntVec3 intVec;
			if (!SocialInteractionUtility.TryGetAdjacentInteractionCell(pawn, t, forced, out intVec))
			{
				JobFailReason.Is("CannotStandNear".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600855D RID: 34141 RVA: 0x0027E104 File Offset: 0x0027C304
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.StudyInteract, t);
		}
	}
}
