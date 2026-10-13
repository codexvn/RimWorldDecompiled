using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006A6 RID: 1702
	public class HediffComp_DisappearsAndKills_Shambler : HediffComp_DisappearsAndKills
	{
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002AC4 RID: 10948 RVA: 0x000E0694 File Offset: 0x000DE894
		private CompHoldingPlatformTarget compHoldingPlatformTarget
		{
			get
			{
				return base.Pawn.TryGetComp<CompHoldingPlatformTarget>();
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x000E06A1 File Offset: 0x000DE8A1
		protected override bool Paused
		{
			get
			{
				if (!this.disabled)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = this.compHoldingPlatformTarget;
					return compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform;
				}
				return true;
			}
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000E06C0 File Offset: 0x000DE8C0
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (this.disabled)
			{
				return;
			}
			if (!base.Pawn.Dead)
			{
				base.Pawn.Kill(null, null);
			}
		}
	}
}
