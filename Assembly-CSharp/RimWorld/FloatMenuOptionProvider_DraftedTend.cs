using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032EF RID: 13039
	public class FloatMenuOptionProvider_DraftedTend : FloatMenuOptionProvider
	{
		// Token: 0x17002F86 RID: 12166
		// (get) Token: 0x06012586 RID: 75142 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F87 RID: 12167
		// (get) Token: 0x06012587 RID: 75143 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F88 RID: 12168
		// (get) Token: 0x06012588 RID: 75144 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F89 RID: 12169
		// (get) Token: 0x06012589 RID: 75145 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F8A RID: 12170
		// (get) Token: 0x0601258A RID: 75146 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F8B RID: 12171
		// (get) Token: 0x0601258B RID: 75147 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0601258C RID: 75148 RVA: 0x00567DE7 File Offset: 0x00565FE7
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsMutant || context.FirstSelectedPawn.mutant.Def.canTend;
		}

		// Token: 0x0601258D RID: 75149 RVA: 0x00567E0D File Offset: 0x0056600D
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_DraftedTend.<>c__DisplayClass13_0 CS$<>8__locals1 = new FloatMenuOptionProvider_DraftedTend.<>c__DisplayClass13_0();
			CS$<>8__locals1.clickedPawn = clickedPawn;
			CS$<>8__locals1.context = context;
			if (!this.IsValidTendTarget(CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedPawn))
			{
				yield break;
			}
			if (!CS$<>8__locals1.clickedPawn.health.HasHediffsNeedingTend(false))
			{
				if (CS$<>8__locals1.context.FirstSelectedPawn != CS$<>8__locals1.clickedPawn)
				{
					yield return new FloatMenuOption("CannotTend".Translate(CS$<>8__locals1.clickedPawn) + ": " + "TendingNotRequired".Translate(CS$<>8__locals1.clickedPawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				yield break;
			}
			if (CS$<>8__locals1.context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				yield return new FloatMenuOption("CannotTend".Translate(CS$<>8__locals1.clickedPawn) + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Doctor.gerundLabel), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (!CS$<>8__locals1.context.FirstSelectedPawn.CanReach(CS$<>8__locals1.clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotTend".Translate(CS$<>8__locals1.clickedPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (CS$<>8__locals1.clickedPawn == CS$<>8__locals1.context.FirstSelectedPawn && CS$<>8__locals1.context.FirstSelectedPawn.playerSettings != null && !CS$<>8__locals1.context.FirstSelectedPawn.playerSettings.selfTend)
			{
				yield return new FloatMenuOption("CannotTend".Translate(CS$<>8__locals1.clickedPawn) + ": " + "SelfTendDisabled".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (CS$<>8__locals1.clickedPawn.InAggroMentalState && !CS$<>8__locals1.clickedPawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				yield return new FloatMenuOption("CannotTend".Translate(CS$<>8__locals1.clickedPawn) + ": " + "PawnIsInAggroMentalState".Translate(CS$<>8__locals1.clickedPawn).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			CS$<>8__locals1.medicine = HealthAIUtility.FindBestMedicine(CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedPawn, true);
			TaggedString taggedString = "Tend".Translate(CS$<>8__locals1.clickedPawn);
			if (CS$<>8__locals1.medicine == null)
			{
				taggedString += " (" + "WithoutMedicine".Translate() + ")";
			}
			yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, new Action(CS$<>8__locals1.<GetOptionsFor>g__Action|0), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), CS$<>8__locals1.context.FirstSelectedPawn, CS$<>8__locals1.clickedPawn, "ReservedBy", null);
			if (CS$<>8__locals1.medicine != null && CS$<>8__locals1.context.FirstSelectedPawn.CanReserve(CS$<>8__locals1.clickedPawn, 1, -1, null, false) && CS$<>8__locals1.clickedPawn.Spawned)
			{
				yield return new FloatMenuOption("Tend".Translate(CS$<>8__locals1.clickedPawn) + " (" + "WithoutMedicine".Translate() + ")", delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.TendPatient, CS$<>8__locals1.clickedPawn, null);
					job.count = 1;
					job.draftedTend = true;
					CS$<>8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x0601258E RID: 75150 RVA: 0x00567E2C File Offset: 0x0056602C
		private bool IsValidTendTarget(Pawn doctor, Pawn patient)
		{
			return (doctor.Drafted || patient == doctor) && (patient.Downed || (!patient.HostileTo(doctor.Faction) && (patient.IsColonist || patient.IsQuestLodger() || patient.IsPrisonerOfColony || patient.IsSlaveOfColony || (patient.Faction == Faction.OfPlayer && patient.IsAnimal) || (patient.IsColonySubhuman && patient.mutant.Def.entitledToMedicalCare))));
		}

		// Token: 0x0601258F RID: 75151 RVA: 0x00567EB8 File Offset: 0x005660B8
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_HoldingPlatform holdingPlatform = clickedThing as Building_HoldingPlatform;
			if (holdingPlatform == null)
			{
				return null;
			}
			Pawn heldPawn = holdingPlatform.HeldPawn;
			if (heldPawn == null)
			{
				return null;
			}
			if (!HealthAIUtility.ShouldBeTendedNowByPlayer(heldPawn))
			{
				return null;
			}
			if (context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				return new FloatMenuOption("CannotTend".Translate(heldPawn) + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Doctor.gerundLabel), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(heldPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotTend".Translate(heldPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Thing medicine = HealthAIUtility.FindBestMedicine(context.FirstSelectedPawn, heldPawn, false);
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Tend".Translate(heldPawn.LabelShort), delegate()
			{
				JobDef tendEntity = JobDefOf.TendEntity;
				LocalTargetInfo targetA = holdingPlatform;
				Thing medicine = medicine;
				Job job = JobMaker.MakeJob(tendEntity, targetA, (medicine != null) ? medicine : LocalTargetInfo.Invalid);
				job.count = 1;
				job.draftedTend = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, holdingPlatform, "ReservedBy", null);
		}
	}
}
