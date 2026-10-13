using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200333E RID: 13118
	public class FloatMenuOptionProvider_Strip : FloatMenuOptionProvider
	{
		// Token: 0x17003002 RID: 12290
		// (get) Token: 0x060126FF RID: 75519 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003003 RID: 12291
		// (get) Token: 0x06012700 RID: 75520 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003004 RID: 12292
		// (get) Token: 0x06012701 RID: 75521 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003005 RID: 12293
		// (get) Token: 0x06012702 RID: 75522 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012703 RID: 75523 RVA: 0x0056F10C File Offset: 0x0056D30C
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Pawn pawn = clickedThing as Pawn;
			if (!StrippableUtility.CanBeStrippedByColony(clickedThing))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotStrip".Translate(clickedThing.LabelCap, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (pawn != null && pawn.HasExtraHomeFaction(null))
			{
				return new FloatMenuOption("CannotStrip".Translate(pawn.LabelCap, pawn) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Strip".Translate(clickedThing.LabelCap, clickedThing), delegate()
			{
				clickedThing.SetForbidden(false, false);
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Strip, clickedThing), new JobTag?(JobTag.Misc), false);
				StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(clickedThing);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
		}
	}
}
