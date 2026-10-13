using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200139A RID: 5018
	public class JobDriver_MechCharge : JobDriver
	{
		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06007B04 RID: 31492 RVA: 0x002509CF File Offset: 0x0024EBCF
		public Building_MechCharger Charger
		{
			get
			{
				return (Building_MechCharger)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007B05 RID: 31493 RVA: 0x002509E6 File Offset: 0x0024EBE6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Charger, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007B06 RID: 31494 RVA: 0x00250A0E File Offset: 0x0024EC0E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.Charger.CanPawnChargeCurrently(this.pawn));
			yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.initAction = delegate()
			{
				this.Charger.StartCharging(this.pawn);
			};
			toil.AddFinishAction(delegate
			{
				if (this.Charger.CurrentlyChargingMech == this.pawn)
				{
					this.Charger.StopCharging();
				}
			});
			toil.handlingFacing = true;
			Toil toil2 = toil;
			toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.Charger.Position);
				if (this.pawn.needs.energy.CurLevel >= JobGiver_GetEnergy.GetMaxRechargeLimit(this.pawn))
				{
					base.ReadyForNextToil();
				}
			}));
			yield return toil;
			yield break;
		}

		// Token: 0x04005331 RID: 21297
		private const TargetIndex ChargerInd = TargetIndex.A;
	}
}
