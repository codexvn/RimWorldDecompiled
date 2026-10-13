using System;

namespace Verse
{
	// Token: 0x020006A8 RID: 1704
	public class HediffComp_DisappearsOnDeath : HediffComp
	{
		// Token: 0x06002AC9 RID: 10953 RVA: 0x000E071E File Offset: 0x000DE91E
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			base.Pawn.health.RemoveHediff(this.parent);
		}
	}
}
