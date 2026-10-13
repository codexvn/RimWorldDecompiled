using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200334E RID: 13134
	public class FloatMenuOptionProvider_Xenogerm : FloatMenuOptionProvider
	{
		// Token: 0x1700301F RID: 12319
		// (get) Token: 0x0601275E RID: 75614 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003020 RID: 12320
		// (get) Token: 0x0601275F RID: 75615 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003021 RID: 12321
		// (get) Token: 0x06012760 RID: 75616 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012761 RID: 75617 RVA: 0x00571003 File Offset: 0x0056F203
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive && GeneUtility.CanAbsorbXenogerm(context.FirstSelectedPawn);
		}

		// Token: 0x06012762 RID: 75618 RVA: 0x00571020 File Offset: 0x0056F220
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!context.FirstSelectedPawn.CanReserveAndReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (context.FirstSelectedPawn.IsQuestLodger())
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "TemporaryFactionMember".Translate(context.FirstSelectedPawn.Named("PAWN")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (GeneUtility.SameXenotype(context.FirstSelectedPawn, clickedPawn))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "SameXenotype".Translate(context.FirstSelectedPawn.Named("PAWN")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (clickedPawn.health.hediffSet.HasHediff(HediffDefOf.XenogermLossShock, false))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "XenogermLossShockPresent".Translate(clickedPawn.Named("PAWN")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!CompAbilityEffect_ReimplantXenogerm.PawnIdeoCanAcceptReimplant(clickedPawn, context.FirstSelectedPawn))
			{
				return new FloatMenuOption("CannotAbsorbXenogerm".Translate(clickedPawn.Named("PAWN")) + ": " + "IdeoligionForbids".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action <>9__1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("AbsorbXenogerm".Translate(clickedPawn.Named("PAWN")), delegate()
			{
				if (clickedPawn.IsPrisonerOfColony && !clickedPawn.Downed)
				{
					Messages.Message("MessageTargetMustBeDownedToForceReimplant".Translate(clickedPawn.Named("PAWN")), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				if (GeneUtility.PawnWouldDieFromReimplanting(clickedPawn))
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text = "WarningPawnWillDieFromReimplanting".Translate(clickedPawn.Named("PAWN"));
					Action confirmedAct;
					if ((confirmedAct = <>9__1) == null)
					{
						confirmedAct = (<>9__1 = delegate()
						{
							GeneUtility.GiveReimplantJob(context.FirstSelectedPawn, clickedPawn);
						});
					}
					windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
					return;
				}
				GeneUtility.GiveReimplantJob(context.FirstSelectedPawn, clickedPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
