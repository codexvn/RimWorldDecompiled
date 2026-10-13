using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001407 RID: 5127
	public class JobGiver_AIFollowEscortee : JobGiver_AIFollowPawn
	{
		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x06007CF4 RID: 31988 RVA: 0x0025A010 File Offset: 0x00258210
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x06007CF5 RID: 31989 RVA: 0x0025A014 File Offset: 0x00258214
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (Pawn)pawn.mindState.duty.focus.Thing;
		}

		// Token: 0x06007CF6 RID: 31990 RVA: 0x00255E10 File Offset: 0x00254010
		protected override float GetRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
