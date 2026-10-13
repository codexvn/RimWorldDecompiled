using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001337 RID: 4919
	public class JobDriver_UnloadInventory : JobDriver
	{
		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x060078DB RID: 30939 RVA: 0x00248EF4 File Offset: 0x002470F4
		private Pawn OtherPawn
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Pawn;
			}
		}

		// Token: 0x060078DC RID: 30940 RVA: 0x00248F15 File Offset: 0x00247115
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.OtherPawn, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060078DD RID: 30941 RVA: 0x00248F38 File Offset: 0x00247138
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Wait(10, TargetIndex.None);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				Pawn otherPawn = this.OtherPawn;
				if (!otherPawn.inventory.UnloadEverything)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
				IntVec3 c;
				if (!firstUnloadableThing.Thing.def.EverStorable(false) || !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
				{
					Thing thing;
					otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
					base.EndJobWith(JobCondition.Succeeded);
					if (thing != null)
					{
						thing.SetForbidden(false, false);
					}
				}
				else
				{
					Thing thing2;
					otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
					this.job.count = thing2.stackCount;
					this.job.SetTarget(TargetIndex.B, thing2);
					this.job.SetTarget(TargetIndex.C, c);
					firstUnloadableThing.Thing.SetForbidden(false, false);
				}
				if (otherPawn.RaceProps.packAnimal && otherPawn.inventory.innerContainer.Count == 0)
				{
					otherPawn.Drawer.renderer.SetAllGraphicsDirty();
				}
			};
			yield return toil;
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		// Token: 0x04005213 RID: 21011
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		// Token: 0x04005214 RID: 21012
		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		// Token: 0x04005215 RID: 21013
		private const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x04005216 RID: 21014
		private const int UnloadDuration = 10;
	}
}
