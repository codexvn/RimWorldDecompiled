using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032CD RID: 13005
	public class FloatMenuOptionProvider_CarryMechToCharger : FloatMenuOptionProvider
	{
		// Token: 0x17002F4E RID: 12110
		// (get) Token: 0x060124DF RID: 74975 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F4F RID: 12111
		// (get) Token: 0x060124E0 RID: 74976 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F50 RID: 12112
		// (get) Token: 0x060124E1 RID: 74977 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F51 RID: 12113
		// (get) Token: 0x060124E2 RID: 74978 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060124E3 RID: 74979 RVA: 0x00563089 File Offset: 0x00561289
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x060124E4 RID: 74980 RVA: 0x00564AA8 File Offset: 0x00562CA8
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn == context.FirstSelectedPawn)
			{
				return null;
			}
			if (!clickedPawn.IsSelfShutdown())
			{
				return null;
			}
			Building_MechCharger charger = JobGiver_GetEnergy_Charger.GetClosestCharger(clickedPawn, context.FirstSelectedPawn, false);
			if (charger == null)
			{
				charger = JobGiver_GetEnergy_Charger.GetClosestCharger(clickedPawn, context.FirstSelectedPawn, true);
			}
			if (charger == null)
			{
				return new FloatMenuOption("CannotCarryToRecharger".Translate(clickedPawn.Named("PAWN")) + ": " + "CannotCarryToRechargerNoneAvailable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(charger, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotCarryToRecharger".Translate(clickedPawn.Named("PAWN")) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToRechargerOrdered".Translate(clickedPawn.Named("PAWN")), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.HaulMechToCharger, clickedPawn, charger, charger.InteractionCell);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(clickedPawn), "ReservedBy", null);
		}
	}
}
