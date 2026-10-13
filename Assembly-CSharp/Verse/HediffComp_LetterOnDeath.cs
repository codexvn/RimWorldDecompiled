using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006DB RID: 1755
	public class HediffComp_LetterOnDeath : HediffComp
	{
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002B7D RID: 11133 RVA: 0x000E3464 File Offset: 0x000E1664
		public HediffCompProperties_LetterOnDeath Props
		{
			get
			{
				return (HediffCompProperties_LetterOnDeath)this.props;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000E3471 File Offset: 0x000E1671
		private bool ShouldSendLetter
		{
			get
			{
				return (!this.Props.onlyIfNoMechanitorDied || !Find.History.mechanitorEverDied) && (this.parent.pawn == null || !PawnGenerator.IsBeingGenerated(this.parent.pawn));
			}
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000E34B0 File Offset: 0x000E16B0
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			if (this.ShouldSendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(this.parent.Named("HEDIFF")), this.Props.letterText.Formatted(this.parent.pawn.Named("PAWN"), this.parent.Named("HEDIFF")), this.Props.letterDef ?? LetterDefOf.NeutralEvent, this.parent.pawn, null, null, null, null, 0, true);
			}
		}
	}
}
