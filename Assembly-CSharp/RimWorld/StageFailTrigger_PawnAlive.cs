using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026B5 RID: 9909
	public class StageFailTrigger_PawnAlive : StageFailTrigger
	{
		// Token: 0x0600D94A RID: 55626 RVA: 0x0040568C File Offset: 0x0040388C
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			Pawn pawn = ritual.PawnWithRole(this.pawnId);
			return pawn != null && !pawn.Dead;
		}

		// Token: 0x0600D94B RID: 55627 RVA: 0x004056B4 File Offset: 0x004038B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.pawnId, "pawnId", null, false);
		}

		// Token: 0x04009339 RID: 37689
		[NoTranslate]
		public string pawnId;
	}
}
