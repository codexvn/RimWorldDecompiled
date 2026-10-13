using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001408 RID: 5128
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x06007CF8 RID: 31992 RVA: 0x00222D4F File Offset: 0x00220F4F
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06007CF9 RID: 31993 RVA: 0x0025A038 File Offset: 0x00258238
		protected override Pawn GetFollowee(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return null;
			}
			return pawn.playerSettings.Master;
		}

		// Token: 0x06007CFA RID: 31994 RVA: 0x0025A04F File Offset: 0x0025824F
		protected override float GetRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 3f;
		}

		// Token: 0x0400541E RID: 21534
		public const float RadiusUnreleased = 3f;

		// Token: 0x0400541F RID: 21535
		public const float RadiusReleased = 50f;
	}
}
