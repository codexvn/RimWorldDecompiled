using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001409 RID: 5129
	public class JobGiver_AIFollowOverseer : JobGiver_AIFollowPawn
	{
		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06007CFC RID: 31996 RVA: 0x00222D4F File Offset: 0x00220F4F
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06007CFD RID: 31997 RVA: 0x0025A085 File Offset: 0x00258285
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return pawn.GetOverseer();
		}

		// Token: 0x06007CFE RID: 31998 RVA: 0x00259689 File Offset: 0x00257889
		protected override float GetRadius(Pawn pawn)
		{
			return 5f;
		}

		// Token: 0x06007CFF RID: 31999 RVA: 0x0025A08D File Offset: 0x0025828D
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.GetOverseer() == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}
