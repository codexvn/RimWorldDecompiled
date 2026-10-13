using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D1 RID: 5585
	public class WorkGiver_Hack : WorkGiver_Scanner
	{
		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x060083F7 RID: 33783 RVA: 0x002797F8 File Offset: 0x002779F8
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Hackable);
			}
		}

		// Token: 0x060083F8 RID: 33784 RVA: 0x00279804 File Offset: 0x00277A04
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompHackable compHackable = t.TryGetComp<CompHackable>();
			if (compHackable == null)
			{
				return false;
			}
			if (!compHackable.Autohack && !forced)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = compHackable.CanHackNow(pawn);
			if (!acceptanceReport.Accepted && !acceptanceReport.Reason.NullOrEmpty())
			{
				JobFailReason.Is(acceptanceReport.Reason, null);
			}
			return (forced || pawn.CanReserve(t, 1, -1, null, false)) && (!forced || !ModsConfig.IdeologyActive || t.def != ThingDefOf.AncientEnemyTerminal) && acceptanceReport.Accepted;
		}

		// Token: 0x060083F9 RID: 33785 RVA: 0x0027988D File Offset: 0x00277A8D
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.TryGetComp<CompHackable>() == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Hack, t);
			job.playerForced = forced;
			return job;
		}
	}
}
