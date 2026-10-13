using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E64 RID: 3684
	public class JobDriver_PlayStatic : JobDriver_BabyPlay
	{
		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06005CCD RID: 23757 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override JobDriver_BabyPlay.StartingConditions StartingCondition
		{
			get
			{
				return JobDriver_BabyPlay.StartingConditions.GotoBaby;
			}
		}

		// Token: 0x06005CCE RID: 23758 RVA: 0x001D9447 File Offset: 0x001D7647
		protected override IEnumerable<Toil> Play()
		{
			Toil toil = ToilMaker.MakeToil("Play");
			toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
			toil.handlingFacing = true;
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(base.Baby);
				if (this.pawn.IsHashIntervalTick(1250, delta))
				{
					this.pawn.interactions.TryInteractWith(base.Baby, InteractionDefOf.BabyPlay);
				}
				if (this.roomPlayGainFactor < 0f)
				{
					this.roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(base.Baby);
				}
				if (BabyPlayUtility.PlayTickCheckEnd(base.Baby, this.pawn, this.roomPlayGainFactor, delta, null))
				{
					this.pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
			yield return toil;
			yield break;
		}

		// Token: 0x04004315 RID: 17173
		private const int InteractionIntervalTicks = 1250;
	}
}
