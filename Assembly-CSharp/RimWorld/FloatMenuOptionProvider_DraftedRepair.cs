using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032ED RID: 13037
	public class FloatMenuOptionProvider_DraftedRepair : FloatMenuOptionProvider
	{
		// Token: 0x17002F81 RID: 12161
		// (get) Token: 0x0601257C RID: 75132 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F82 RID: 12162
		// (get) Token: 0x0601257D RID: 75133 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F83 RID: 12163
		// (get) Token: 0x0601257E RID: 75134 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F84 RID: 12164
		// (get) Token: 0x0601257F RID: 75135 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F85 RID: 12165
		// (get) Token: 0x06012580 RID: 75136 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012581 RID: 75137 RVA: 0x00567C80 File Offset: 0x00565E80
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.skills != null && !context.FirstSelectedPawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled;
		}

		// Token: 0x06012582 RID: 75138 RVA: 0x00567CB0 File Offset: 0x00565EB0
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			if (!RepairUtility.PawnCanRepairNow(context.FirstSelectedPawn, clickedThing))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotRepair".Translate(clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RepairThing".Translate(clickedThing), delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Repair, clickedThing), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
		}
	}
}
