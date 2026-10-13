using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006E2 RID: 1762
	public class HediffComp_MessageAfterTicks : HediffComp
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002B99 RID: 11161 RVA: 0x000E3993 File Offset: 0x000E1B93
		protected HediffCompProperties_MessageAfterTicks Props
		{
			get
			{
				return (HediffCompProperties_MessageAfterTicks)this.props;
			}
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000E39A0 File Offset: 0x000E1BA0
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksUntilMessage = this.Props.ticks;
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000E39BC File Offset: 0x000E1BBC
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.ticksUntilMessage == 0)
			{
				if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
				{
					if (this.Props.messageType != null)
					{
						Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
					}
					if (this.Props.letterType != null)
					{
						Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(base.Pawn), this.GetLetterText(), this.Props.letterType, base.Pawn, null, null, null, null, 0, true);
					}
				}
				this.ticksUntilMessage--;
				return;
			}
			if (this.ticksUntilMessage > 0)
			{
				this.ticksUntilMessage--;
			}
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000E3AA7 File Offset: 0x000E1CA7
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilMessage, "ticksUntilMessage", 0, false);
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000E3AC4 File Offset: 0x000E1CC4
		private TaggedString GetLetterText()
		{
			Hediff_Pregnant hediff_Pregnant = this.parent as Hediff_Pregnant;
			if (hediff_Pregnant != null && hediff_Pregnant.Mother != null && hediff_Pregnant.Mother != hediff_Pregnant.pawn)
			{
				TaggedString taggedString = "IvfPregnancyLetterText".Translate(hediff_Pregnant.pawn.NameFullColored);
				if (hediff_Pregnant.Mother != null && hediff_Pregnant.Father != null)
				{
					taggedString += "\n\n" + "IvfPregnancyLetterParents".Translate(hediff_Pregnant.Mother.NameFullColored, hediff_Pregnant.Father.NameFullColored);
				}
				return taggedString;
			}
			return this.Props.letterText.Formatted(base.Pawn);
		}

		// Token: 0x04002319 RID: 8985
		private int ticksUntilMessage;
	}
}
