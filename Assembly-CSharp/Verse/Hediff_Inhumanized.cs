using System;

namespace Verse
{
	// Token: 0x02000748 RID: 1864
	public class Hediff_Inhumanized : Hediff
	{
		// Token: 0x06002D7A RID: 11642 RVA: 0x000EA5B6 File Offset: 0x000E87B6
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModsConfig.AnomalyActive)
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			Find.StudyManager.UpdateStudiableCache(this.pawn, this.pawn.MapHeld);
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000EA5F3 File Offset: 0x000E87F3
		public override void PostRemoved()
		{
			base.PostRemoved();
			Find.StudyManager.UpdateStudiableCache(this.pawn, this.pawn.MapHeld);
		}
	}
}
