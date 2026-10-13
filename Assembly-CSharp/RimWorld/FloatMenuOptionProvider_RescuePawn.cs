using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003334 RID: 13108
	public class FloatMenuOptionProvider_RescuePawn : FloatMenuOptionProvider
	{
		// Token: 0x17002FF2 RID: 12274
		// (get) Token: 0x060126D1 RID: 75473 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FF3 RID: 12275
		// (get) Token: 0x060126D2 RID: 75474 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FF4 RID: 12276
		// (get) Token: 0x060126D3 RID: 75475 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FF5 RID: 12277
		// (get) Token: 0x060126D4 RID: 75476 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060126D5 RID: 75477 RVA: 0x0056E224 File Offset: 0x0056C424
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!HealthAIUtility.CanRescueNow(context.FirstSelectedPawn, clickedPawn, true))
			{
				return null;
			}
			if (clickedPawn.mindState.WillJoinColonyIfRescued)
			{
				return null;
			}
			if (clickedPawn.IsPrisonerOfColony || clickedPawn.IsSlaveOfColony || clickedPawn.IsColonyMech)
			{
				return null;
			}
			if (clickedPawn.Faction != null && clickedPawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return null;
			}
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			bool flag = ChildcareUtility.CanSuckle(clickedPawn, out breastfeedFailReason);
			bool flag2 = HealthAIUtility.ShouldSeekMedicalRest(clickedPawn) || !clickedPawn.ageTracker.CurLifeStage.alwaysDowned;
			if (flag || !flag2)
			{
				return null;
			}
			Pawn_PlayerSettings playerSettings = clickedPawn.playerSettings;
			if (playerSettings != null && playerSettings.medCare == MedicalCareCategory.NoCare)
			{
				return new FloatMenuOption("CannotRescuePawn".Translate(clickedPawn.Named("PAWN")) + ": " + "MedicalCareDisabled".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Rescue".Translate(clickedPawn.LabelCap, clickedPawn), delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, null);
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, null);
				}
				if (building_Bed == null)
				{
					string t;
					if (clickedPawn.RaceProps.Animal)
					{
						t = "NoAnimalBed".Translate();
					}
					else
					{
						t = "NoNonPrisonerBed".Translate();
					}
					Messages.Message("CannotRescue".Translate() + ": " + t, clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Rescue, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
			}, MenuOptionPriority.RescueOrCapture, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			string key = clickedPawn.RaceProps.Animal ? "NoAnimalBed" : "NoNonPrisonerBed";
			string cannot = string.Format("{0}: {1}", "CannotRescue".Translate(), key.Translate().CapitalizeFirst());
			FloatMenuUtility.ValidateTakeToBedOption(context.FirstSelectedPawn, clickedPawn, floatMenuOption, cannot, null);
			return floatMenuOption;
		}
	}
}
