using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032D1 RID: 13009
	public class FloatMenuOptionProvider_CarryPawnToExit : FloatMenuOptionProvider
	{
		// Token: 0x17002F56 RID: 12118
		// (get) Token: 0x060124F1 RID: 74993 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F57 RID: 12119
		// (get) Token: 0x060124F2 RID: 74994 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F58 RID: 12120
		// (get) Token: 0x060124F3 RID: 74995 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060124F4 RID: 74996 RVA: 0x00564EB1 File Offset: 0x005630B1
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.map.IsPlayerHome && context.map.exitMapGrid.MapUsesExitGrid;
		}

		// Token: 0x060124F5 RID: 74997 RVA: 0x00564ED4 File Offset: 0x005630D4
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.Downed)
			{
				return null;
			}
			if (clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.IsPrisonerOfColony && !CaravanUtility.ShouldAutoCapture(clickedPawn, Faction.OfPlayer))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotCarryToExit".Translate(clickedPawn.Label, clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (context.FirstSelectedPawn.Map.IsPocketMap)
			{
				Thing portal;
				if (!RCellFinder.TryFindExitPortal(context.FirstSelectedPawn, out portal))
				{
					return new FloatMenuOption("CannotCarryToExit".Translate(clickedPawn.Label, clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption((clickedPawn.Faction == Faction.OfPlayer || clickedPawn.IsPrisonerOfColony) ? "CarryToExit".Translate(clickedPawn.Label, clickedPawn) : "CarryToExitAndCapture".Translate(clickedPawn.Label, clickedPawn), delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnToPortal, portal, clickedPawn);
					job.count = 1;
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			else
			{
				IntVec3 exitSpot;
				if (!RCellFinder.TryFindBestExitSpot(context.FirstSelectedPawn, out exitSpot, TraverseMode.ByPawn, true))
				{
					return new FloatMenuOption("CannotCarryToExit".Translate(clickedPawn.Label, clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption((clickedPawn.Faction == Faction.OfPlayer || clickedPawn.IsPrisonerOfColony) ? "CarryToExit".Translate(clickedPawn.Label, clickedPawn) : "CarryToExitAndCapture".Translate(clickedPawn.Label, clickedPawn), delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnToExit, clickedPawn, exitSpot);
					job.count = 1;
					job.failIfCantJoinOrCreateCaravan = true;
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
		}
	}
}
