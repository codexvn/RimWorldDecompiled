using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E87 RID: 3719
	public class JobDriver_PickupToHold : JobDriver
	{
		// Token: 0x06005DC1 RID: 24001 RVA: 0x001DE348 File Offset: 0x001DC548
		public static bool TryMakePreToilReservations(JobDriver driver, bool errorOnFailed)
		{
			driver.pawn.Map.pawnDestinationReservationManager.Reserve(driver.pawn, driver.job, driver.job.GetTarget(TargetIndex.A).Cell);
			return driver.pawn.Reserve(driver.job.GetTarget(TargetIndex.A), driver.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005DC2 RID: 24002 RVA: 0x001DE3AC File Offset: 0x001DC5AC
		public static IEnumerable<Toil> Toils(JobDriver driver, TargetIndex HeldItem = TargetIndex.A, bool subtractNumTakenFromJobCount = true)
		{
			driver.FailOn(() => !driver.GetActor().health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
			driver.FailOnDestroyedOrNull(HeldItem);
			driver.FailOnForbidden(HeldItem);
			Toil end = Toils_General.Label();
			yield return Toils_Jump.JumpIf(end, () => driver.GetActor().IsCarryingThing(driver.GetActor().CurJob.GetTarget(HeldItem).Thing));
			yield return Toils_Goto.GotoThing(HeldItem, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(HeldItem);
			yield return Toils_Haul.StartCarryThing(HeldItem, false, subtractNumTakenFromJobCount, false, true, false);
			yield return end;
			yield break;
		}

		// Token: 0x06005DC3 RID: 24003 RVA: 0x001DE3CA File Offset: 0x001DC5CA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return JobDriver_PickupToHold.TryMakePreToilReservations(this, errorOnFailed);
		}

		// Token: 0x06005DC4 RID: 24004 RVA: 0x001DE3D3 File Offset: 0x001DC5D3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return JobDriver_PickupToHold.Toils(this, TargetIndex.A, true);
		}

		// Token: 0x040043DA RID: 17370
		private const TargetIndex HeldItemInd = TargetIndex.A;
	}
}
