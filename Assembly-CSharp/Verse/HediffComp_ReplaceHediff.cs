using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020006FD RID: 1789
	public class HediffComp_ReplaceHediff : HediffComp
	{
		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002BDF RID: 11231 RVA: 0x000E478C File Offset: 0x000E298C
		public HediffCompProperties_ReplaceHediff Props
		{
			get
			{
				return (HediffCompProperties_ReplaceHediff)this.props;
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000E4799 File Offset: 0x000E2999
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.Props.manuallyTriggered || this.parent.Severity < this.Props.severity)
			{
				return;
			}
			this.Trigger();
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000E47C8 File Offset: 0x000E29C8
		public void Trigger()
		{
			if (!this.Props.message.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
			{
				Messages.Message(this.Props.message.Formatted(this.parent.pawn.Named("PAWN")), this.parent.pawn, this.Props.messageDef ?? MessageTypeDefOf.NegativeEvent, true);
			}
			HediffComp_ReplaceHediff.added.Clear();
			foreach (HediffCompProperties_ReplaceHediff.TriggeredHediff triggeredHediff in this.Props.hediffs)
			{
				triggeredHediff.ApplyTo(this.parent.pawn, HediffComp_ReplaceHediff.added);
			}
			if (HediffComp_ReplaceHediff.added.Empty<Hediff>())
			{
				this.parent.pawn.health.RemoveHediff(this.parent);
				return;
			}
			if (!this.Props.letterLabel.NullOrEmpty() && !this.Props.letterDesc.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
			{
				string arg = (from x in HediffComp_ReplaceHediff.added
				where x.Part != null
				select x.Part.LabelCap).ToLineList("  - ", false);
				TaggedString label = this.Props.letterLabel.Formatted(this.parent.pawn.Named("PAWN"), arg.Named("ORGANS"));
				TaggedString text = this.Props.letterDesc.Formatted(this.parent.pawn.Named("PAWN"), arg.Named("ORGANS"));
				Find.LetterStack.ReceiveLetter(label, text, this.Props.letterDef ?? LetterDefOf.NeutralEvent, this.parent.pawn, null, null, null, null, 0, true);
			}
			this.parent.pawn.health.RemoveHediff(this.parent);
			HediffComp_ReplaceHediff.added.Clear();
		}

		// Token: 0x0400234F RID: 9039
		private static readonly List<Hediff> added = new List<Hediff>();
	}
}
