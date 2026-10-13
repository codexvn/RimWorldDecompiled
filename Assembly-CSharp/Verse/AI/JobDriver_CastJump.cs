using System;

namespace Verse.AI
{
	// Token: 0x02000EA1 RID: 3745
	public class JobDriver_CastJump : JobDriver_CastVerbOnceStatic
	{
		// Token: 0x06005E5B RID: 24155 RVA: 0x001DDCB7 File Offset: 0x001DBEB7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}
	}
}
