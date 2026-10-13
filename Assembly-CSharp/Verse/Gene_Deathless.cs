using System;

namespace Verse
{
	// Token: 0x02000661 RID: 1633
	public class Gene_Deathless : Gene
	{
		// Token: 0x0600292D RID: 10541 RVA: 0x000D8B7C File Offset: 0x000D6D7C
		public override void PostRemove()
		{
			base.PostRemove();
			this.pawn.health.CheckForStateChange(null, null);
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000D8BA9 File Offset: 0x000D6DA9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSkillReductionTick, "lastSkillReductionTick", -99999, false);
		}

		// Token: 0x040021A8 RID: 8616
		public int lastSkillReductionTick = -99999;
	}
}
