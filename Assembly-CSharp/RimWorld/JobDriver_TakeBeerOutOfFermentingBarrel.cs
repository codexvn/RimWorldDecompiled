using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001328 RID: 4904
	public class JobDriver_TakeBeerOutOfFermentingBarrel : JobDriver
	{
		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x0600786A RID: 30826 RVA: 0x00247860 File Offset: 0x00245A60
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x0600786B RID: 30827 RVA: 0x00247888 File Offset: 0x00245A88
		protected Thing Beer
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600786C RID: 30828 RVA: 0x002478A9 File Offset: 0x00245AA9
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600786D RID: 30829 RVA: 0x002478CC File Offset: 0x00245ACC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Wait(200, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.Barrel.Fermented).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				Thing thing = this.Barrel.TakeOutBeer();
				GenPlace.TryPlaceThing(thing, this.pawn.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
				StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing, false);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
				{
					this.job.SetTarget(TargetIndex.C, c);
					this.job.SetTarget(TargetIndex.B, thing);
					this.job.count = thing.stackCount;
					return;
				}
				base.EndJobWith(JobCondition.Incompletable);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null, false);
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		// Token: 0x040051E1 RID: 20961
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x040051E2 RID: 20962
		private const TargetIndex BeerToHaulInd = TargetIndex.B;

		// Token: 0x040051E3 RID: 20963
		private const TargetIndex StorageCellInd = TargetIndex.C;

		// Token: 0x040051E4 RID: 20964
		private const int Duration = 200;
	}
}
