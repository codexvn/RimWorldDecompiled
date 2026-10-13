using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011F4 RID: 4596
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		// Token: 0x06007049 RID: 28745 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600704A RID: 28746 RVA: 0x0022DB87 File Offset: 0x0022BD87
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil toil = Toils_General.Wait(this.job.def.joyDuration, TargetIndex.None);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.tickIntervalAction = new Action<int>(this.WaitTickAction);
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return toil;
			yield break;
		}

		// Token: 0x0600704B RID: 28747
		protected abstract void WaitTickAction(int delta);

		// Token: 0x04004E16 RID: 19990
		protected const TargetIndex TargetThingIndex = TargetIndex.A;
	}
}
