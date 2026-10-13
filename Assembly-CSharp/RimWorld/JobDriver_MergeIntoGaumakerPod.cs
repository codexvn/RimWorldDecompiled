using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200157C RID: 5500
	public class JobDriver_MergeIntoGaumakerPod : JobDriver
	{
		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06008268 RID: 33384 RVA: 0x00272D86 File Offset: 0x00270F86
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06008269 RID: 33385 RVA: 0x00273104 File Offset: 0x00271304
		private CompGaumakerPod GaumakerPod
		{
			get
			{
				return this.job.targetB.Thing.TryGetComp<CompGaumakerPod>();
			}
		}

		// Token: 0x0600826A RID: 33386 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600826B RID: 33387 RVA: 0x0027311B File Offset: 0x0027131B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnDespawnedOrNull(TargetIndex.B);
			this.FailOn(() => this.GaumakerPod.Full);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false);
			yield return Toils_General.WaitWith(TargetIndex.B, 120, true, false, false, TargetIndex.None, PathEndMode.Touch);
			yield return Toils_General.Do(delegate
			{
				this.GaumakerPod.TryAcceptPawn(this.pawn);
			});
			yield break;
		}

		// Token: 0x04005692 RID: 22162
		private const int WaitTicks = 120;
	}
}
