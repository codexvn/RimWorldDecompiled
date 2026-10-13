using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032CF RID: 13007
	public class FloatMenuOptionProvider_CarryPawn : FloatMenuOptionProvider
	{
		// Token: 0x17002F52 RID: 12114
		// (get) Token: 0x060124E8 RID: 74984 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F53 RID: 12115
		// (get) Token: 0x060124E9 RID: 74985 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F54 RID: 12116
		// (get) Token: 0x060124EA RID: 74986 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F55 RID: 12117
		// (get) Token: 0x060124EB RID: 74987 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060124EC RID: 74988 RVA: 0x00564CB7 File Offset: 0x00562EB7
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsMutant || !context.FirstSelectedPawn.mutant.Def.canCarryPawns;
		}

		// Token: 0x060124ED RID: 74989 RVA: 0x00564CE0 File Offset: 0x00562EE0
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.Downed && !clickedPawn.IsSelfShutdown())
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotCarry".Translate(clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return new FloatMenuOption("CannotCarry".Translate(context.FirstSelectedPawn) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Carry".Translate(clickedPawn), delegate()
			{
				clickedPawn.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnDrafted, clickedPawn);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
