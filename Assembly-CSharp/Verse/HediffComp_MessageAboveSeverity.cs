using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006DF RID: 1759
	public class HediffComp_MessageAboveSeverity : HediffComp
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000E37F8 File Offset: 0x000E19F8
		protected HediffCompProperties_MessageAboveSeverity Props
		{
			get
			{
				return (HediffCompProperties_MessageAboveSeverity)this.props;
			}
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000E3808 File Offset: 0x000E1A08
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (!this.messageSent && this.parent.Severity >= this.Props.severity)
			{
				if (PawnUtility.ShouldSendNotificationAbout(base.Pawn) && this.Props.messageType != null)
				{
					Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
				}
				this.messageSent = true;
			}
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000E3892 File Offset: 0x000E1A92
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.messageSent, "messageSent", false, false);
		}

		// Token: 0x0400230E RID: 8974
		private bool messageSent;
	}
}
