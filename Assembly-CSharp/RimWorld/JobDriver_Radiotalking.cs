using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200120A RID: 4618
	public class JobDriver_Radiotalking : JobDriver
	{
		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x060070CB RID: 28875 RVA: 0x0022F6A0 File Offset: 0x0022D8A0
		public Building_CommsConsole CommsConsole
		{
			get
			{
				return (Building_CommsConsole)base.TargetThingA;
			}
		}

		// Token: 0x060070CC RID: 28876 RVA: 0x0022F6AD File Offset: 0x0022D8AD
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060070CD RID: 28877 RVA: 0x0022F6CB File Offset: 0x0022D8CB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			this.FailOnChildLearningConditions<JobDriver_Radiotalking>();
			this.FailOn(() => !this.CommsConsole.CanUseCommsNow);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn(() => !this.CommsConsole.CanUseCommsNow);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
				LearningUtility.LearningTickCheckEnd(this.pawn, delta, false);
			};
			toil.WithEffect(EffecterDefOf.Radiotalking, TargetIndex.A, null);
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}
	}
}
