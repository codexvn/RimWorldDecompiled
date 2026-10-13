using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000E7E RID: 3710
	public class JobDriver_Follow : JobDriver
	{
		// Token: 0x06005D7F RID: 23935 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x001DD627 File Offset: 0x001DB827
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
				if (this.pawn.Position.InHorDistOf(pawn.Position, 4f) && this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable))
				{
					return;
				}
				if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				if (!this.pawn.pather.Moving || this.pawn.pather.Destination != pawn)
				{
					this.pawn.pather.StartPath(pawn, PathEndMode.Touch);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x001D7A1B File Offset: 0x001D5C1B
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x040043C3 RID: 17347
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x040043C4 RID: 17348
		private const int Distance = 4;
	}
}
