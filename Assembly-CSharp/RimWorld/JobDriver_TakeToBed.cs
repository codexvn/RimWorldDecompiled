using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001330 RID: 4912
	public class JobDriver_TakeToBed : JobDriver
	{
		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x060078A1 RID: 30881 RVA: 0x00248208 File Offset: 0x00246408
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x060078A2 RID: 30882 RVA: 0x00248230 File Offset: 0x00246430
		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x060078A3 RID: 30883 RVA: 0x00248258 File Offset: 0x00246458
		private bool TakeeRescued
		{
			get
			{
				return this.Takee.RaceProps.Humanlike && this.job.def != JobDefOf.Arrest && !this.Takee.IsPrisonerOfColony && (!this.Takee.ageTracker.CurLifeStage.alwaysDowned || HealthAIUtility.ShouldSeekMedicalRest(this.Takee));
			}
		}

		// Token: 0x060078A4 RID: 30884 RVA: 0x002482BC File Offset: 0x002464BC
		public override string GetReport()
		{
			if (this.job.def == JobDefOf.Rescue && !this.TakeeRescued)
			{
				return "TakingToBed".Translate(this.Takee);
			}
			return base.GetReport();
		}

		// Token: 0x060078A5 RID: 30885 RVA: 0x002482FC File Offset: 0x002464FC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.Takee.ClearAllReservations(true);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DropBed, this.job, this.DropBed.SleepingSlotsCount, 0, null, errorOnFailed, false);
		}

		// Token: 0x060078A6 RID: 30886 RVA: 0x00248365 File Offset: 0x00246565
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			this.FailOn(delegate()
			{
				if (this.job.def.makeTargetPrisoner)
				{
					if (!this.DropBed.ForPrisoners)
					{
						return true;
					}
				}
				else if (this.DropBed.ForPrisoners != this.Takee.IsPrisoner)
				{
					return true;
				}
				return false;
			});
			yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.B, TargetIndex.A);
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				if (this.job.def.makeTargetPrisoner && this.Takee.ownership.OwnedBed == this.DropBed && this.Takee.Position != RestUtility.GetBedSleepingSlotPosFor(this.Takee, this.DropBed))
				{
					this.Takee.ownership.UnclaimBed();
				}
				if (this.pawn.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Direct, out thing, null);
				}
			});
			Toil goToTakee = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.job.def == JobDefOf.Arrest && !this.Takee.CanBeArrestedBy(this.pawn)).FailOn(() => !this.pawn.CanReach(this.DropBed, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOn(() => (this.job.def == JobDefOf.Rescue || this.job.def == JobDefOf.Capture) && !this.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil checkArrestResistance = ToilMaker.MakeToil("MakeNewToils");
			checkArrestResistance.initAction = delegate()
			{
				if (this.job.def.makeTargetPrisoner)
				{
					Pawn pawn = (Pawn)this.job.targetA.Thing;
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnAttemptArrested(pawn);
					}
					GenClamor.DoClamor(pawn, 10f, ClamorDefOf.Harm);
					if (!pawn.IsPrisoner && !pawn.IsSlave)
					{
						QuestUtility.SendQuestTargetSignals(pawn.questTags, "Arrested", pawn.Named("SUBJECT"));
						if (pawn.Faction != null)
						{
							QuestUtility.SendQuestTargetSignals(pawn.Faction.questTags, "FactionMemberArrested", pawn.Faction.Named("FACTION"));
						}
					}
					if (this.job.def == JobDefOf.Arrest && !pawn.CheckAcceptArrest(this.pawn))
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
				}
			};
			yield return Toils_Jump.JumpIf(checkArrestResistance, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return goToTakee;
			yield return checkArrestResistance;
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			startCarrying.FailOnBedNoLongerUsable(TargetIndex.B, TargetIndex.A);
			startCarrying.AddPreInitAction(new Action(this.CheckMakeTakeeGuest));
			startCarrying.AddFinishAction(delegate
			{
				if (this.pawn.Faction == this.Takee.Faction)
				{
					this.CheckMakeTakeePrisoner();
				}
			});
			Toil goToBed = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOn(() => !this.pawn.IsCarryingPawn(this.Takee));
			goToBed.FailOnBedNoLongerUsable(TargetIndex.B, TargetIndex.A);
			yield return Toils_Jump.JumpIf(goToBed, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return startCarrying;
			yield return goToBed;
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.CheckMakeTakeePrisoner();
				if (this.Takee.playerSettings == null)
				{
					this.Takee.playerSettings = new Pawn_PlayerSettings(this.Takee);
				}
			};
			yield return toil;
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return Toils_Bed.TuckIntoBed(this.DropBed, this.pawn, this.Takee, this.TakeeRescued);
			yield return Toils_General.Do(delegate
			{
				if (!this.job.ritualTag.NullOrEmpty())
				{
					Lord lord = this.Takee.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.Takee, this.job.ritualTag);
					}
					Lord lord2 = this.pawn.GetLord();
					lordJob_Ritual = (((lord2 != null) ? lord2.LordJob : null) as LordJob_Ritual);
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.pawn, this.job.ritualTag);
					}
				}
			});
			yield break;
		}

		// Token: 0x060078A7 RID: 30887 RVA: 0x00248378 File Offset: 0x00246578
		private void CheckMakeTakeePrisoner()
		{
			if (this.job.def.makeTargetPrisoner)
			{
				if (this.Takee.guest.Released)
				{
					this.Takee.guest.Released = false;
					this.Takee.guest.SetExclusiveInteraction(PrisonerInteractionModeDefOf.MaintainOnly);
					GenGuest.RemoveHealthyPrisonerReleasedThoughts(this.Takee);
				}
				if (!this.Takee.IsPrisonerOfColony)
				{
					this.Takee.guest.CapturedBy(Faction.OfPlayer, this.pawn);
				}
			}
		}

		// Token: 0x060078A8 RID: 30888 RVA: 0x00248404 File Offset: 0x00246604
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan() && this.Takee.DevelopmentalStage != DevelopmentalStage.Baby)
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
				QuestUtility.SendQuestTargetSignals(this.Takee.questTags, "Rescued", this.Takee.Named("SUBJECT"));
			}
		}

		// Token: 0x040051FD RID: 20989
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x040051FE RID: 20990
		private const TargetIndex BedIndex = TargetIndex.B;
	}
}
