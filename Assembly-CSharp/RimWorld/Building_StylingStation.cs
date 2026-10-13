using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002940 RID: 10560
	public class Building_StylingStation : Building
	{
		// Token: 0x0600E734 RID: 59188 RVA: 0x004360F2 File Offset: 0x004342F2
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (ModLister.IdeologyInstalled)
			{
				if (!selPawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return new FloatMenuOption("CannotUseReason".Translate("NoPath".Translate().CapitalizeFirst()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ChangeStyle".Translate().CapitalizeFirst(), delegate()
					{
						selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OpenStylingStationDialog, this), new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
				}
				Job job;
				if (JobGiver_OptimizeApparel.TryCreateRecolorJob(selPawn, out job, true))
				{
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RecolorApparel".Translate().CapitalizeFirst(), delegate()
					{
						Job job2;
						JobGiver_OptimizeApparel.TryCreateRecolorJob(selPawn, out job2, false);
						selPawn.jobs.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
				}
			}
			yield break;
			yield break;
		}
	}
}
