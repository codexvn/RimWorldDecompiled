using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003321 RID: 13089
	public class FloatMenuOptionProvider_PrisonerBloodfeed : FloatMenuOptionProvider
	{
		// Token: 0x17002FDB RID: 12251
		// (get) Token: 0x0601267D RID: 75389 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FDC RID: 12252
		// (get) Token: 0x0601267E RID: 75390 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FDD RID: 12253
		// (get) Token: 0x0601267F RID: 75391 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012680 RID: 75392 RVA: 0x0056CE67 File Offset: 0x0056B067
		protected override bool AppliesInt(FloatMenuContext context)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!context.FirstSelectedPawn.IsBloodfeeder())
			{
				return false;
			}
			Pawn_GeneTracker genes = context.FirstSelectedPawn.genes;
			return ((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) != null;
		}

		// Token: 0x06012681 RID: 75393 RVA: 0x0056CEA0 File Offset: 0x0056B0A0
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsBloodfeeder())
			{
				return null;
			}
			if (!clickedPawn.IsPrisonerOfColony || !clickedPawn.guest.PrisonerIsSecure || clickedPawn.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Bloodfeed))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotBloodfeedOn".Translate(clickedPawn.Named("PAWN")) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			AcceptanceReport acceptanceReport = JobGiver_GetHemogen.CanFeedOnPrisoner(context.FirstSelectedPawn, clickedPawn);
			if (acceptanceReport.Accepted)
			{
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("BloodfeedOn".Translate(clickedPawn.Named("PAWN")), delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.PrisonerBloodfeed, clickedPawn);
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			if (acceptanceReport.Reason.NullOrEmpty())
			{
				return null;
			}
			return new FloatMenuOption("CannotBloodfeedOn".Translate(clickedPawn.Named("PAWN")) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
		}
	}
}
