using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003319 RID: 13081
	public class FloatMenuOptionProvider_OfferHelp : FloatMenuOptionProvider
	{
		// Token: 0x17002FD0 RID: 12240
		// (get) Token: 0x0601265B RID: 75355 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FD1 RID: 12241
		// (get) Token: 0x0601265C RID: 75356 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FD2 RID: 12242
		// (get) Token: 0x0601265D RID: 75357 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601265E RID: 75358 RVA: 0x0056C16C File Offset: 0x0056A36C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn.Dead || !clickedPawn.mindState.WillJoinColonyIfRescued)
			{
				return null;
			}
			TaggedString taggedString = clickedPawn.IsPrisoner ? "FreePrisoner".Translate() : "OfferHelp".Translate();
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(taggedString + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OfferHelp, clickedPawn), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
