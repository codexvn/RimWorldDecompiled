using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000768 RID: 1896
	public class Hediff_RotStinkExposure : HediffWithComps
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002E7C RID: 11900 RVA: 0x000EF08D File Offset: 0x000ED28D
		public override bool Visible
		{
			get
			{
				return base.Visible && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.LungRot, false);
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x000EF0B7 File Offset: 0x000ED2B7
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity <= 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent("F0");
			}
		}
	}
}
