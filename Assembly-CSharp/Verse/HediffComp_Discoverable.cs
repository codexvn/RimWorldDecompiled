using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006AA RID: 1706
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x000E0756 File Offset: 0x000DE956
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000E0763 File Offset: 0x000DE963
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000E0777 File Offset: 0x000DE977
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.pawn.IsHashIntervalTick(103, delta))
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000E0794 File Offset: 0x000DE994
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000E079C File Offset: 0x000DE99C
		private void CheckDiscovered()
		{
			if (this.discovered)
			{
				return;
			}
			if (!this.parent.Visible)
			{
				return;
			}
			this.discovered = true;
			if (this.Props.sendLetterWhenDiscovered && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					string str;
					if (!this.Props.discoverLetterLabel.NullOrEmpty())
					{
						str = string.Format(this.Props.discoverLetterLabel, base.Pawn.LabelShortCap).CapitalizeFirst();
					}
					else
					{
						str = "LetterLabelNewDisease".Translate() + ": " + base.Def.LabelCap;
					}
					string str2;
					if (!this.Props.discoverLetterText.NullOrEmpty())
					{
						str2 = this.Props.discoverLetterText.Formatted(base.Pawn.LabelIndefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else if (this.parent.Part == null)
					{
						str2 = "NewDisease".Translate(base.Pawn.Named("PAWN"), base.Def.label, base.Pawn.LabelDefinite()).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else
					{
						str2 = "NewPartDisease".Translate(base.Pawn.Named("PAWN"), this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.label).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					Find.LetterStack.ReceiveLetter(str, str2, (this.Props.letterType != null) ? this.Props.letterType : LetterDefOf.NegativeEvent, base.Pawn, null, null, null, null, 0, true);
					return;
				}
				string text;
				if (!this.Props.discoverLetterText.NullOrEmpty())
				{
					string value = base.Pawn.KindLabelIndefinite();
					if (base.Pawn.Name.IsValid && !base.Pawn.Name.Numerical)
					{
						Name name = base.Pawn.Name;
						value = ((name != null) ? name.ToString() : null) + " (" + base.Pawn.KindLabel + ")";
					}
					text = this.Props.discoverLetterText.Formatted(value, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else if (this.parent.Part == null)
				{
					text = "NewDiseaseAnimal".Translate(base.Pawn.LabelShort, base.Def.LabelCap, base.Pawn.LabelDefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else
				{
					text = "NewPartDiseaseAnimal".Translate(base.Pawn.LabelShort, this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.LabelCap, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				Messages.Message(text, base.Pawn, (this.Props.messageType != null) ? this.Props.messageType : MessageTypeDefOf.NegativeHealthEvent, true);
			}
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000E0794 File Offset: 0x000DE994
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000E0BDE File Offset: 0x000DEDDE
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered.ToString();
		}

		// Token: 0x04002298 RID: 8856
		private bool discovered;

		// Token: 0x04002299 RID: 8857
		private const int CheckInterval = 103;
	}
}
