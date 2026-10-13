using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200331B RID: 13083
	public class FloatMenuOptionProvider_OpenThing : FloatMenuOptionProvider
	{
		// Token: 0x17002FD3 RID: 12243
		// (get) Token: 0x06012662 RID: 75362 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FD4 RID: 12244
		// (get) Token: 0x06012663 RID: 75363 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FD5 RID: 12245
		// (get) Token: 0x06012664 RID: 75364 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012665 RID: 75365 RVA: 0x0056C2A4 File Offset: 0x0056A4A4
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			IOpenable openable = clickedThing as IOpenable;
			if (openable == null || !openable.CanOpen)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotOpen".Translate(clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return new FloatMenuOption("CannotOpen".Translate(clickedThing) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.Drafted && clickedThing.Map.designationManager.DesignationOn(clickedThing, DesignationDefOf.Open) != null)
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Open".Translate(clickedThing), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.Open, clickedThing);
				job.ignoreDesignations = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
		}
	}
}
