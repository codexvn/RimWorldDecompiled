using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012AB RID: 4779
	public class JobDriver_LayDownResting : JobDriver_LayDownAwake
	{
		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06007521 RID: 29985 RVA: 0x001E5B21 File Offset: 0x001E3D21
		public override Rot4 ForcedLayingRotation
		{
			get
			{
				return Rot4.Invalid;
			}
		}

		// Token: 0x06007522 RID: 29986 RVA: 0x0023CE67 File Offset: 0x0023B067
		public override Toil LayDownToil(bool hasBed)
		{
			return Toils_LayDown.LayDown(TargetIndex.A, hasBed, this.LookForOtherJobs, this.CanSleep, this.CanRest, PawnPosture.LayingOnGroundFaceUp, false);
		}
	}
}
