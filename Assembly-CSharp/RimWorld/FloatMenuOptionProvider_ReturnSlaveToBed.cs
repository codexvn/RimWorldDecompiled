using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003336 RID: 13110
	public class FloatMenuOptionProvider_ReturnSlaveToBed : FloatMenuOptionProvider
	{
		// Token: 0x17002FF6 RID: 12278
		// (get) Token: 0x060126D9 RID: 75481 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FF7 RID: 12279
		// (get) Token: 0x060126DA RID: 75482 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FF8 RID: 12280
		// (get) Token: 0x060126DB RID: 75483 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FF9 RID: 12281
		// (get) Token: 0x060126DC RID: 75484 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060126DD RID: 75485 RVA: 0x0056E558 File Offset: 0x0056C758
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsSlaveOfColony || clickedPawn.InMentalState)
			{
				return null;
			}
			if (!clickedPawn.Downed)
			{
				return null;
			}
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ReturnToSlaveBed".Translate(), delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, new GuestStatus?(GuestStatus.Slave));
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, new GuestStatus?(GuestStatus.Slave));
				}
				if (building_Bed == null)
				{
					Messages.Message(string.Format("{0}: {1}", "CannotRescue".Translate(), "NoSlaveBed".Translate()), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Rescue, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			string cannot = string.Format("{0}: {1}", "CannotRescue".Translate(), "NoSlaveBed".Translate());
			FloatMenuUtility.ValidateTakeToBedOption(context.FirstSelectedPawn, clickedPawn, floatMenuOption, cannot, new GuestStatus?(GuestStatus.Slave));
			return floatMenuOption;
		}
	}
}
