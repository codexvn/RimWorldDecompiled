using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032C2 RID: 12994
	public class FloatMenuOptionProvider_CapturePawn : FloatMenuOptionProvider
	{
		// Token: 0x17002F40 RID: 12096
		// (get) Token: 0x060124AF RID: 74927 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F41 RID: 12097
		// (get) Token: 0x060124B0 RID: 74928 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F42 RID: 12098
		// (get) Token: 0x060124B1 RID: 74929 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F43 RID: 12099
		// (get) Token: 0x060124B2 RID: 74930 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060124B3 RID: 74931 RVA: 0x00563768 File Offset: 0x00561968
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.CanBeCaptured())
			{
				return null;
			}
			if (!HealthAIUtility.CanRescueNow(context.FirstSelectedPawn, clickedPawn, true))
			{
				return null;
			}
			TaggedString taggedString = "Capture".Translate(clickedPawn.LabelCap, clickedPawn);
			if (!clickedPawn.guest.Recruitable)
			{
				taggedString += " (" + "Unrecruitable".Translate() + ")";
			}
			if (clickedPawn.Faction != null && clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.Faction.Hidden && !clickedPawn.Faction.HostileTo(Faction.OfPlayer) && !clickedPawn.IsPrisonerOfColony)
			{
				taggedString += ": " + "AngersFaction".Translate().CapitalizeFirst();
			}
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, new GuestStatus?(GuestStatus.Prisoner));
				}
				if (building_Bed == null)
				{
					Messages.Message("CannotCapture".Translate() + ": " + "NoPrisonerBed".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Capture, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Capturing, KnowledgeAmount.Total);
				if (clickedPawn.Faction != null && clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.Faction.Hidden && !clickedPawn.Faction.HostileTo(Faction.OfPlayer) && !clickedPawn.IsPrisonerOfColony)
				{
					Messages.Message("MessageCapturingWillAngerFaction".Translate(clickedPawn.Named("PAWN")).AdjustedFor(clickedPawn, "PAWN", true), clickedPawn, MessageTypeDefOf.CautionInput, false);
				}
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			string cannot = string.Format("{0}: {1}", "CannotCapture".Translate(), "NoPrisonerBed".Translate());
			FloatMenuUtility.ValidateTakeToBedOption(context.FirstSelectedPawn, clickedPawn, floatMenuOption, cannot, new GuestStatus?(GuestStatus.Prisoner));
			return floatMenuOption;
		}
	}
}
