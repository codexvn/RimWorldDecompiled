using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C14 RID: 7188
	public class QuestPart_Bestowing_TargetChangedTitle : QuestPart
	{
		// Token: 0x0600A017 RID: 40983 RVA: 0x002E2690 File Offset: 0x002E0890
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				RoyalTitleDef titleAwardedWhenUpdating = this.pawn.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.pawn.royalty.GetFavor(this.bestower.Faction));
				string key;
				string key2;
				LetterDef textLetterDef;
				if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.seniority > this.currentTitle.seniority)
				{
					key = "LetterLabelBestowingCeremonyTitleUpdated";
					key2 = "LetterTextBestowingCeremonyTitleUpdated";
					textLetterDef = LetterDefOf.NeutralEvent;
					SoundDefOf.Quest_Concluded.PlayOneShotOnCamera(null);
				}
				else
				{
					key = "LetterQuestFailedLabel";
					key2 = "LetterQuestCompletedFail";
					textLetterDef = LetterDefOf.NegativeEvent;
					SoundDefOf.Quest_Failed.PlayOneShotOnCamera(null);
				}
				Find.LetterStack.ReceiveLetter(key.Translate(this.pawn.Named("TARGET")), key2.Translate(this.quest.name.CapitalizeFirst(), this.pawn.Named("TARGET")), textLetterDef, this.pawn, null, this.quest, null, null, 0, true);
				this.quest.End(QuestEndOutcome.Unknown, false, true);
			}
		}

		// Token: 0x0600A018 RID: 40984 RVA: 0x002E27BC File Offset: 0x002E09BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.currentTitle, "currentTitle");
		}

		// Token: 0x04006EB6 RID: 28342
		public string inSignal;

		// Token: 0x04006EB7 RID: 28343
		public Pawn pawn;

		// Token: 0x04006EB8 RID: 28344
		public Pawn bestower;

		// Token: 0x04006EB9 RID: 28345
		public RoyalTitleDef currentTitle;
	}
}
