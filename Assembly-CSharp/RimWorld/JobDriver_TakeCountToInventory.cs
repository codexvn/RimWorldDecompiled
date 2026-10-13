using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200132C RID: 4908
	public class JobDriver_TakeCountToInventory : JobDriver
	{
		// Token: 0x06007888 RID: 30856 RVA: 0x00247F4B File Offset: 0x0024614B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 10, this.job.count, null, true, false);
		}

		// Token: 0x06007889 RID: 30857 RVA: 0x00247F7A File Offset: 0x0024617A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Haul.TakeToInventory(TargetIndex.A, this.job.count);
			yield break;
		}
	}
}
