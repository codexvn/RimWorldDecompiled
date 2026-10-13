using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026B4 RID: 9908
	public class StageFailTrigger_PawnAsleep : StageFailTrigger
	{
		// Token: 0x0600D947 RID: 55623 RVA: 0x00405655 File Offset: 0x00403855
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			return ritual.PawnWithRole(this.pawnId).jobs.curDriver.asleep;
		}

		// Token: 0x0600D948 RID: 55624 RVA: 0x00405672 File Offset: 0x00403872
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.pawnId, "pawnId", null, false);
		}

		// Token: 0x04009338 RID: 37688
		[NoTranslate]
		public string pawnId;
	}
}
