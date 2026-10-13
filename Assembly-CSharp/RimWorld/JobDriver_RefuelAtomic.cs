using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012EA RID: 4842
	public class JobDriver_RefuelAtomic : JobDriver
	{
		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x060076BD RID: 30397 RVA: 0x002427B0 File Offset: 0x002409B0
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x060076BE RID: 30398 RVA: 0x002427D1 File Offset: 0x002409D1
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x060076BF RID: 30399 RVA: 0x002427E0 File Offset: 0x002409E0
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060076C0 RID: 30400 RVA: 0x00242804 File Offset: 0x00240A04
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060076C1 RID: 30401 RVA: 0x00242852 File Offset: 0x00240A52
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			base.AddEndCondition(delegate
			{
				if (!this.RefuelableComp.IsFull)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			base.AddFailCondition(() => (!this.job.playerForced && !this.RefuelableComp.ShouldAutoRefuelNowIgnoringFuelPct) || !this.RefuelableComp.allowAutoRefuel);
			base.AddFailCondition(() => !this.RefuelableComp.allowAutoRefuel && !this.job.playerForced);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			Toil getNextIngredient = Toils_General.Label();
			yield return getNextIngredient;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false, false);
			yield return Toils_Jump.JumpIf(getNextIngredient, () => !this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			findPlaceTarget = null;
			yield return Toils_General.Wait(240, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.None);
			yield break;
		}

		// Token: 0x04005115 RID: 20757
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04005116 RID: 20758
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x04005117 RID: 20759
		private const TargetIndex FuelPlaceCellInd = TargetIndex.C;

		// Token: 0x04005118 RID: 20760
		private const int RefuelingDuration = 240;
	}
}
