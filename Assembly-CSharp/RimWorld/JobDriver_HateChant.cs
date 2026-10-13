using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011AD RID: 4525
	public class JobDriver_HateChant : JobDriver
	{
		// Token: 0x06006EAC RID: 28332 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006EAD RID: 28333 RVA: 0x0022840B File Offset: 0x0022660B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil stand = Toils_General.Wait(int.MaxValue, TargetIndex.None);
			stand.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceCell(this.pawn.Map.Center);
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				Pawn actor = stand.actor;
				if (actor.IsHashIntervalTick(100, delta))
				{
					actor.jobs.CheckForJobOverride(0f, true);
				}
			};
			stand.socialMode = RandomSocialMode.Off;
			stand.defaultCompleteMode = ToilCompleteMode.Never;
			stand.handlingFacing = true;
			yield return stand;
			yield break;
		}
	}
}
