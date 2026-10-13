using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032BD RID: 12989
	public class FloatMenuOptionProvider_BringBabyToSafety : FloatMenuOptionProvider
	{
		// Token: 0x17002F35 RID: 12085
		// (get) Token: 0x06012491 RID: 74897 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F36 RID: 12086
		// (get) Token: 0x06012492 RID: 74898 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F37 RID: 12087
		// (get) Token: 0x06012493 RID: 74899 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F38 RID: 12088
		// (get) Token: 0x06012494 RID: 74900 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F39 RID: 12089
		// (get) Token: 0x06012495 RID: 74901 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanTargetDespawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012496 RID: 74902 RVA: 0x00563089 File Offset: 0x00561289
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x06012497 RID: 74903 RVA: 0x00563090 File Offset: 0x00561290
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!HealthAIUtility.CanRescueNow(context.FirstSelectedPawn, clickedPawn, true))
			{
				return null;
			}
			if (clickedPawn.IsPrisonerOfColony || clickedPawn.IsSlaveOfColony || clickedPawn.IsColonyMech)
			{
				return null;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(clickedPawn, out breastfeedFailReason))
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PutSomewhereSafe".Translate(clickedPawn.LabelCap, clickedPawn), delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(ChildcareUtility.MakeBringBabyToSafetyJob(context.FirstSelectedPawn, clickedPawn), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
