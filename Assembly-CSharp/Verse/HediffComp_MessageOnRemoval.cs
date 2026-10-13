using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006E6 RID: 1766
	public class HediffComp_MessageOnRemoval : HediffComp_MessageBase
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002BA4 RID: 11172 RVA: 0x000E3C3F File Offset: 0x000E1E3F
		protected HediffCompProperties_MessageOnRemoval Props
		{
			get
			{
				return (HediffCompProperties_MessageOnRemoval)this.props;
			}
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000E3C4C File Offset: 0x000E1E4C
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (((this.Props.messageOnZeroSeverity && this.parent.Severity <= 0f) || (this.Props.messageOnNonZeroSeverity && this.parent.Severity > 0f)) && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
			{
				this.Message();
			}
		}
	}
}
