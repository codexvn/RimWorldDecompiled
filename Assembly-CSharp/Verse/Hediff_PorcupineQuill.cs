using System;

namespace Verse
{
	// Token: 0x0200075D RID: 1885
	public class Hediff_PorcupineQuill : HediffWithComps
	{
		// Token: 0x06002E3A RID: 11834 RVA: 0x000EDC9C File Offset: 0x000EBE9C
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (this.pawn != null && this.pawn.RaceProps != null && !this.pawn.RaceProps.IsFlesh)
			{
				this.pawn.health.RemoveHediff(this);
			}
		}
	}
}
