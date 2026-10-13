using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001275 RID: 4725
	public class JobDriver_FillFermentingBarrel : JobDriver
	{
		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06007394 RID: 29588 RVA: 0x00237F2C File Offset: 0x0023612C
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06007395 RID: 29589 RVA: 0x00237F54 File Offset: 0x00236154
		protected Thing Wort
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06007396 RID: 29590 RVA: 0x00237F78 File Offset: 0x00236178
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Wort, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007397 RID: 29591 RVA: 0x00237FCB File Offset: 0x002361CB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			base.AddEndCondition(delegate
			{
				if (this.Barrel.SpaceLeftForWort > 0)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.Barrel.SpaceLeftForWort;
			});
			Toil reserveWort = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null, false);
			yield return reserveWort;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveWort, TargetIndex.B, TargetIndex.None, true, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Wait(200, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.Barrel.AddWort(this.Wort);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		// Token: 0x04004FA3 RID: 20387
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x04004FA4 RID: 20388
		private const TargetIndex WortInd = TargetIndex.B;

		// Token: 0x04004FA5 RID: 20389
		private const int Duration = 200;
	}
}
