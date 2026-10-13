using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020026B3 RID: 9907
	public class StageFailTrigger_TargetPawnUnreachable : StageFailTrigger
	{
		// Token: 0x0600D944 RID: 55620 RVA: 0x004055F0 File Offset: 0x004037F0
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			Pawn pawn = ritual.PawnWithRole(this.takerId);
			Pawn t = ritual.PawnWithRole(this.takeeId);
			return !pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x0600D945 RID: 55621 RVA: 0x00405629 File Offset: 0x00403829
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.takerId, "takerId", null, false);
			Scribe_Values.Look<string>(ref this.takeeId, "takeeId", null, false);
		}

		// Token: 0x04009336 RID: 37686
		[NoTranslate]
		public string takerId;

		// Token: 0x04009337 RID: 37687
		[NoTranslate]
		public string takeeId;
	}
}
