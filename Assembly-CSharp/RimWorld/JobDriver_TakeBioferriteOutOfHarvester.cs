using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200132A RID: 4906
	public class JobDriver_TakeBioferriteOutOfHarvester : JobDriver
	{
		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06007879 RID: 30841 RVA: 0x00247BD4 File Offset: 0x00245DD4
		protected Building_BioferriteHarvester Harvester
		{
			get
			{
				return (Building_BioferriteHarvester)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x0600787A RID: 30842 RVA: 0x00247BFC File Offset: 0x00245DFC
		protected Thing Bioferrite
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600787B RID: 30843 RVA: 0x00247C1D File Offset: 0x00245E1D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Harvester, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600787C RID: 30844 RVA: 0x00247C40 File Offset: 0x00245E40
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Wait(200, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn(() => this.Harvester.containedBioferrite < 1f).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				Thing thing = this.Harvester.TakeOutBioferrite();
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

		// Token: 0x040051EA RID: 20970
		private const TargetIndex HarvesterInd = TargetIndex.A;

		// Token: 0x040051EB RID: 20971
		private const TargetIndex BioferriteToHaulInd = TargetIndex.B;

		// Token: 0x040051EC RID: 20972
		private const TargetIndex StorageCellInd = TargetIndex.C;

		// Token: 0x040051ED RID: 20973
		private const int Duration = 200;
	}
}
