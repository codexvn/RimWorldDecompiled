using System;

namespace Verse.AI
{
	// Token: 0x02000EAE RID: 3758
	public class JobDriver_CastVerbOnceStaticReserve : JobDriver_CastVerbOnceStatic
	{
		// Token: 0x06005EA1 RID: 24225 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}
	}
}
