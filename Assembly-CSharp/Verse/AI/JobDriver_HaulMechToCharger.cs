using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E58 RID: 3672
	public class JobDriver_HaulMechToCharger : JobDriver
	{
		// Token: 0x06005C7F RID: 23679 RVA: 0x001D84BC File Offset: 0x001D66BC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x001D8511 File Offset: 0x001D6711
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return toil;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil toil2 = Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.OnCell);
			yield return toil2;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return Toils_General.Do(delegate
			{
				this.pawn.Map.reservationManager.Release(this.job.targetB, this.pawn, this.job);
				Pawn pawn = (Pawn)this.job.targetA.Thing;
				Building_MechCharger t = (Building_MechCharger)this.job.targetB.Thing;
				Job newJob = JobMaker.MakeJob(JobDefOf.MechCharge, t);
				pawn.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			});
			yield break;
		}

		// Token: 0x040042E8 RID: 17128
		private const TargetIndex MechInd = TargetIndex.A;

		// Token: 0x040042E9 RID: 17129
		private const TargetIndex ChargerInd = TargetIndex.B;

		// Token: 0x040042EA RID: 17130
		private const TargetIndex ChargerCellInd = TargetIndex.C;
	}
}
