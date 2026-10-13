using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014AB RID: 5291
	public class JobGiver_NociosphereDepart : ThinkNode_JobGiver
	{
		// Token: 0x06007EEB RID: 32491 RVA: 0x00262D9C File Offset: 0x00260F9C
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompActivity compActivity = pawn.TryGetComp<CompActivity>();
			CompNociosphere compNociosphere = pawn.TryGetComp<CompNociosphere>();
			if (compActivity == null)
			{
				Log.ErrorOnce("JobGiver_NociosphereDepart tried to execute on a pawn with no CompActivity component.", 1562529);
				return null;
			}
			if (compNociosphere == null)
			{
				Log.ErrorOnce("JobGiver_NociosphereDepart tried to execute on a pawn with no CompNociosphere component.", 83712521);
				return null;
			}
			if (!compActivity.ShouldGoPassive())
			{
				return null;
			}
			if (!compNociosphere.IsUnstable)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.NociosphereDepart);
		}
	}
}
