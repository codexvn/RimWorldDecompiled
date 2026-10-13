using System;

namespace Verse
{
	// Token: 0x02000751 RID: 1873
	public class Hediff_LightExposure : Hediff
	{
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002DCC RID: 11724 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000EBAF2 File Offset: 0x000E9CF2
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Light exposure"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000EBB1C File Offset: 0x000E9D1C
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.SpawnedOrAnyParentSpawned && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.Dead)
			{
				bool flag = this.pawn.MapHeld.glowGrid.PsychGlowAt(this.pawn.PositionHeld) > PsychGlow.Dark;
				this.Severity += (flag ? 0.4f : -0.25f);
			}
		}

		// Token: 0x0400241F RID: 9247
		private const float ExposurePerSecond_Lit = 0.4f;

		// Token: 0x04002420 RID: 9248
		private const float ExposurePerSecond_Unlit = -0.25f;
	}
}
