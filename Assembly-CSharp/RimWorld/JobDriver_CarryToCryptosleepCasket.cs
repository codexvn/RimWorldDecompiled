using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001237 RID: 4663
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x0600720C RID: 29196 RVA: 0x00233664 File Offset: 0x00231864
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x0600720D RID: 29197 RVA: 0x0023368C File Offset: 0x0023188C
		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600720E RID: 29198 RVA: 0x002336B4 File Offset: 0x002318B4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DropPod, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x00233707 File Offset: 0x00231907
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !this.DropPod.Accepts(this.Takee));
			Toil goToTakee = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.DropPod.GetDirectlyHeldThings().Count > 0).FailOn(() => !this.pawn.CanReach(this.Takee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil startCarryingTakee = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil goToThing = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell, false);
			yield return Toils_Jump.JumpIf(goToThing, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return goToTakee;
			yield return startCarryingTakee;
			yield return goToThing;
			Toil toil = Toils_General.Wait(500, TargetIndex.B);
			toil.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				this.DropPod.TryAcceptThing(this.Takee, true);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
		}

		// Token: 0x06007210 RID: 29200 RVA: 0x00233717 File Offset: 0x00231917
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				this.Takee
			};
		}

		// Token: 0x04004EEC RID: 20204
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x04004EED RID: 20205
		private const TargetIndex DropPodInd = TargetIndex.B;
	}
}
