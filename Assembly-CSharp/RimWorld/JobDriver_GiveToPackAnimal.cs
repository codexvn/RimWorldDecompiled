using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200113D RID: 4413
	public class JobDriver_GiveToPackAnimal : JobDriver
	{
		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06006BAF RID: 27567 RVA: 0x0021DBF0 File Offset: 0x0021BDF0
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06006BB0 RID: 27568 RVA: 0x0021DC14 File Offset: 0x0021BE14
		private Pawn Animal
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06006BB1 RID: 27569 RVA: 0x0021DC3A File Offset: 0x0021BE3A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x0021DC5D File Offset: 0x0021BE5D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil findNearestCarrier = this.FindCarrierToil();
			yield return findNearestCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).JumpIf(() => !this.CanCarryAtLeastOne(this.Animal), findNearestCarrier);
			yield return this.GiveToCarrierAsMuchAsPossibleToil();
			yield return Toils_Jump.JumpIf(findNearestCarrier, () => this.pawn.carryTracker.CarriedThing != null);
			yield break;
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x0021DC6D File Offset: 0x0021BE6D
		private Toil FindCarrierToil()
		{
			Toil toil = ToilMaker.MakeToil("FindCarrierToil");
			toil.initAction = delegate()
			{
				Pawn pawn = this.FindCarrier();
				if (pawn == null)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				this.job.SetTarget(TargetIndex.B, pawn);
			};
			return toil;
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x0021DC8C File Offset: 0x0021BE8C
		private Pawn FindCarrier()
		{
			IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(this.pawn);
			Pawn animal = this.Animal;
			if (animal != null && enumerable.Contains(animal) && animal.RaceProps.packAnimal && this.CanCarryAtLeastOne(animal))
			{
				return animal;
			}
			Pawn pawn = null;
			float num = -1f;
			foreach (Pawn pawn2 in enumerable)
			{
				if (pawn2.RaceProps.packAnimal && this.CanCarryAtLeastOne(pawn2))
				{
					float num2 = (float)pawn2.Position.DistanceToSquared(this.pawn.Position);
					if (pawn == null || num2 < num)
					{
						pawn = pawn2;
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x0021DD54 File Offset: 0x0021BF54
		private bool CanCarryAtLeastOne(Pawn carrier)
		{
			return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, this.Item, 1);
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x0021DD66 File Offset: 0x0021BF66
		private Toil GiveToCarrierAsMuchAsPossibleToil()
		{
			Toil toil = ToilMaker.MakeToil("GiveToCarrierAsMuchAsPossibleToil");
			toil.initAction = delegate()
			{
				if (this.Item == null)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				int count = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(this.Animal, this.Item), this.Item.stackCount);
				this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.Item, this.Animal.inventory.innerContainer, count, true);
			};
			return toil;
		}

		// Token: 0x04004BB4 RID: 19380
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04004BB5 RID: 19381
		private const TargetIndex AnimalInd = TargetIndex.B;
	}
}
