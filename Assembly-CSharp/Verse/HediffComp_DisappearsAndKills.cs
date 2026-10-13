using System;

namespace Verse
{
	// Token: 0x020006A5 RID: 1701
	public class HediffComp_DisappearsAndKills : HediffComp_DisappearsDisableable
	{
		// Token: 0x06002AC1 RID: 10945 RVA: 0x000E0614 File Offset: 0x000DE814
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (this.disabled)
			{
				return;
			}
			if (this.ticksToDisappear > 0)
			{
				return;
			}
			if (base.Pawn.Dead)
			{
				return;
			}
			base.Pawn.Kill(null, null);
			this.parent.Notify_PawnDied(null, null);
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000E0672 File Offset: 0x000DE872
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.disabled, "disabled", false, false);
		}
	}
}
