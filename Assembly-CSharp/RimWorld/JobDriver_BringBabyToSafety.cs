using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001152 RID: 4434
	public class JobDriver_BringBabyToSafety : JobDriver
	{
		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06006C76 RID: 27766 RVA: 0x000024E7 File Offset: 0x000006E7
		private Pawn Baby
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06006C77 RID: 27767 RVA: 0x0022165B File Offset: 0x0021F85B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Baby, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006C78 RID: 27768 RVA: 0x0022167E File Offset: 0x0021F87E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFailCondition(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			base.AddFailCondition(delegate
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return !ChildcareUtility.CanSuckle(this.Baby, out breastfeedFailReason);
			});
			if (!this.job.playerForced && this.Baby.Spawned && !this.Baby.ComfortableTemperatureAtCell(this.Baby.Position, this.Baby.Map) && (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.Baby)))
			{
				yield return Toils_General.Do(delegate
				{
					Messages.Message("MessageTakingBabyToSafeTemperature".Translate(this.pawn.Named("ADULT"), this.Baby.Named("BABY")), new LookTargets(new TargetInfo[]
					{
						this.pawn,
						this.Baby
					}), MessageTypeDefOf.NeutralEvent, true);
				});
			}
			Toil findBedForBaby = this.FindBedForBaby();
			yield return Toils_Jump.JumpIf(findBedForBaby, () => this.pawn.IsCarryingPawn(this.Baby)).FailOn(() => !this.pawn.IsCarryingPawn(this.Baby) && (this.pawn.Downed || this.pawn.Drafted));
			foreach (Toil toil in JobDriver_PickupToHold.Toils(this, TargetIndex.A, true))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return findBedForBaby;
			yield return Toils_Reserve.ReserveDestinationOrThing(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell).FailOnInvalidOrDestroyed(TargetIndex.B).FailOnForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOn(() => !this.pawn.IsCarryingPawn(this.Baby) || this.pawn.Downed || this.pawn.Drafted);
			yield return Toils_Reserve.ReleaseDestinationOrThing(TargetIndex.B);
			yield return Toils_Bed.TuckIntoBed(TargetIndex.B, TargetIndex.A, false);
			yield break;
			yield break;
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x00221690 File Offset: 0x0021F890
		private Toil FindBedForBaby()
		{
			Toil toil = ToilMaker.MakeToil("FindBedForBaby");
			toil.initAction = delegate()
			{
				LocalTargetInfo pack = ChildcareUtility.SafePlaceForBaby(this.Baby, this.pawn, false);
				LocalTargetInfo pack2 = LocalTargetInfo.Invalid;
				if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(this.Baby))
				{
					pack2 = JobGiver_PrepareCaravan_GatherDownedPawns.FindRandomDropCell(this.pawn, this.Baby);
				}
				if (pack.IsValid)
				{
					if (pack2.IsValid && pack2.Cell.DistanceTo(this.pawn.Position) < pack.Cell.DistanceTo(this.pawn.Position))
					{
						toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack2);
						return;
					}
					toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack);
					return;
				}
				else
				{
					if (pack2.IsValid)
					{
						toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack2);
						return;
					}
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					return;
				}
			};
			toil.FailOn(() => !this.pawn.IsCarryingPawn(this.Baby) || this.pawn.Downed || this.pawn.Drafted);
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04004C36 RID: 19510
		private const TargetIndex BabyInd = TargetIndex.A;

		// Token: 0x04004C37 RID: 19511
		private const TargetIndex BabyBedInd = TargetIndex.B;
	}
}
