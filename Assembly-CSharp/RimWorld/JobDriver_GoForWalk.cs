using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011D9 RID: 4569
	public class JobDriver_GoForWalk : JobDriver
	{
		// Token: 0x06006FA7 RID: 28583 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006FA8 RID: 28584 RVA: 0x0022BAEF File Offset: 0x00229CEF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickIntervalAction = delegate(int delta)
			{
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, null);
			};
			yield return goToil;
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (this.job.targetQueueA.Count <= 0)
				{
					return;
				}
				LocalTargetInfo targetA = this.job.targetQueueA[0];
				this.job.targetQueueA.RemoveAt(0);
				this.job.targetA = targetA;
				this.JumpToToil(goToil);
			};
			yield return toil;
			yield break;
		}
	}
}
