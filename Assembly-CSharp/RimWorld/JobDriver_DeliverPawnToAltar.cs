using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011A1 RID: 4513
	public class JobDriver_DeliverPawnToAltar : JobDriver
	{
		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x06006E5A RID: 28250 RVA: 0x00227230 File Offset: 0x00225430
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06006E5B RID: 28251 RVA: 0x00227258 File Offset: 0x00225458
		protected Building DropAltar
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x06006E5C RID: 28252 RVA: 0x00227280 File Offset: 0x00225480
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			base.Map.reservationManager.ReleaseAllForTarget(this.Takee);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DropAltar, this.job, 1, 0, null, errorOnFailed, false);
		}

		// Token: 0x06006E5D RID: 28253 RVA: 0x002272E9 File Offset: 0x002254E9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Deliver to altar job"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.C);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOn(() => !this.pawn.CanReach(this.DropAltar, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return toil;
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil goToAltar = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.ClosestTouch, false);
			yield return Toils_Jump.JumpIf(goToAltar, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return startCarrying;
			yield return goToAltar;
			yield return Toils_General.Do(delegate
			{
				IntVec3 c = this.DropAltar.Position;
				if (this.DropAltar.def.hasInteractionCell)
				{
					IntVec3 interactionCell = this.DropAltar.InteractionCell;
					IntVec3 b = (this.DropAltar.Position - interactionCell).ClampInsideRect(new CellRect(-1, -1, 3, 3));
					c = interactionCell + b;
				}
				else if (this.DropAltar.def.Size.z % 2 != 0)
				{
					c = this.DropAltar.Position + new IntVec3(0, 0, -this.DropAltar.def.Size.z / 2).RotatedBy(this.DropAltar.Rotation);
				}
				this.job.SetTarget(TargetIndex.B, c);
			});
			yield return Toils_Reserve.Release(TargetIndex.C);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, false, false);
			yield break;
		}

		// Token: 0x04004D11 RID: 19729
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x04004D12 RID: 19730
		private const TargetIndex TargetCellIndex = TargetIndex.B;

		// Token: 0x04004D13 RID: 19731
		private const TargetIndex AltarIndex = TargetIndex.C;
	}
}
