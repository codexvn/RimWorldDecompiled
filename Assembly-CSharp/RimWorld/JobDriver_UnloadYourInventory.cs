using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001339 RID: 4921
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		// Token: 0x060078E8 RID: 30952 RVA: 0x0024923F File Offset: 0x0024743F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		// Token: 0x060078E9 RID: 30953 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060078EA RID: 30954 RVA: 0x00249259 File Offset: 0x00247459
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Wait(10, TargetIndex.None);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (!this.pawn.inventory.UnloadEverything)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				ThingCount firstUnloadableThing = this.pawn.inventory.FirstUnloadableThing;
				IntVec3 c;
				if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
				{
					Thing thing;
					this.pawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				this.job.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
				this.job.SetTarget(TargetIndex.B, c);
				this.countToDrop = firstUnloadableThing.Count;
			};
			yield return toil;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null, false);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing == null || !this.pawn.inventory.innerContainer.Contains(thing))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !thing.def.EverStorable(false))
				{
					this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, this.countToDrop, out thing, null, null);
					base.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					this.pawn.inventory.innerContainer.TryTransferToContainer(thing, this.pawn.carryTracker.innerContainer, this.countToDrop, out thing, true);
					this.job.count = this.countToDrop;
					this.job.SetTarget(TargetIndex.A, thing);
				}
				thing.SetForbidden(false, false);
			};
			yield return toil2;
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield break;
		}

		// Token: 0x0400521C RID: 21020
		private int countToDrop = -1;

		// Token: 0x0400521D RID: 21021
		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		// Token: 0x0400521E RID: 21022
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x0400521F RID: 21023
		private const int UnloadDuration = 10;
	}
}
