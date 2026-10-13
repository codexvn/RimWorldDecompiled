using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E8A RID: 3722
	public class JobDriver_TakeInventory : JobDriver
	{
		// Token: 0x06005DD1 RID: 24017 RVA: 0x001DE64B File Offset: 0x001DC84B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 10, this.job.count, null, errorOnFailed, false);
		}

		// Token: 0x06005DD2 RID: 24018 RVA: 0x001DE679 File Offset: 0x001DC879
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOn(() => this.job.checkEncumbrance && MassUtility.CountToPickUpUntilOverEncumbered(this.pawn, this.job.targetA.Thing) == 0);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return toil;
			if (this.job.takeInventoryDelay > 0)
			{
				Toil toil2 = Toils_General.Wait(this.job.takeInventoryDelay, TargetIndex.None);
				toil2.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
				toil2.tickIntervalAction = delegate(int delta)
				{
					this.pawn.rotationTracker.FaceTarget(base.TargetThingA);
				};
				toil2.handlingFacing = true;
				yield return toil2;
			}
			yield return Toils_Haul.TakeToInventory(TargetIndex.A, this.job.count);
			yield break;
		}
	}
}
