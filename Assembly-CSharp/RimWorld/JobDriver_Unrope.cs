using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200113B RID: 4411
	public class JobDriver_Unrope : JobDriver
	{
		// Token: 0x06006BA4 RID: 27556 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x0021DAD3 File Offset: 0x0021BCD3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false);
			yield return Toils_Rope.GotoRopeAttachmentInteractionCell(TargetIndex.A);
			yield return Toils_Rope.UnropeFromSpot(TargetIndex.A);
			yield break;
		}

		// Token: 0x04004BAF RID: 19375
		private const TargetIndex AnimalInd = TargetIndex.A;
	}
}
