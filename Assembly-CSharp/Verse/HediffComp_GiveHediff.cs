using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006BD RID: 1725
	public class HediffComp_GiveHediff : HediffComp
	{
		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000E18B4 File Offset: 0x000DFAB4
		private HediffCompProperties_GiveHediff Props
		{
			get
			{
				return (HediffCompProperties_GiveHediff)this.props;
			}
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000E18C4 File Offset: 0x000DFAC4
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.Severity < this.Props.atSeverity)
			{
				return;
			}
			if (this.Props.skipIfAlreadyExists && base.Pawn.health.hediffSet.HasHediff(this.Props.hediffDef, false))
			{
				return;
			}
			base.Pawn.health.AddHediff(this.Props.hediffDef, null, null, null);
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				this.SendLetter();
			}
			if (this.Props.disappearsAfterGiving)
			{
				base.Pawn.health.RemoveHediff(this.parent);
			}
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000E1978 File Offset: 0x000DFB78
		private void SendLetter()
		{
			if (this.Props.letterLabel == null)
			{
				return;
			}
			Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(base.Pawn.Named("PAWN")), this.Props.letterText.Formatted(base.Pawn.Named("PAWN")), this.Props.letterDef ?? LetterDefOf.NegativeEvent, base.Pawn, null, null, null, null, 0, true);
		}
	}
}
