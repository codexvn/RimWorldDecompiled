using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B40 RID: 15168
	public class QuestPart_CreepJoinerOutcomes : QuestPart
	{
		// Token: 0x060149F5 RID: 84469 RVA: 0x00628600 File Offset: 0x00626800
		public void ShowOfferLetter(Pawn_CreepJoinerTracker creepjoiner)
		{
			if (this.letter != null)
			{
				this.letter.OpenLetter();
				return;
			}
			TaggedString label = "LetterCreeperInviteJoins".Translate(this.pawn.Named("PAWN"));
			TaggedString taggedString = creepjoiner.form.letterPrompt.Formatted(this.pawn.Named("PAWN")).CapitalizeFirst();
			taggedString += "\n\n" + creepjoiner.benefit.letterExtra.Formatted(this.pawn.Named("PAWN")).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCreeperInviteAppend".Translate(this.pawn.Named("PAWN")).CapitalizeFirst();
			this.letter = (ChoiceLetter_AcceptCreepJoiner)LetterMaker.MakeLetter(label, taggedString, LetterDefOf.AcceptCreepJoiner, null, this.quest);
			this.letter.signalAccept = this.signalAccept;
			this.letter.signalCapture = this.signalCapture;
			this.letter.signalReject = this.signalReject;
			this.letter.pawn = this.pawn;
			this.letter.speaker = creepjoiner.speaker;
			Find.LetterStack.ReceiveLetter(this.letter, null, 0, true);
			this.letter.OpenLetter();
		}

		// Token: 0x060149F6 RID: 84470 RVA: 0x00628764 File Offset: 0x00626964
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Pawn pawn = this.pawn;
			Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = (pawn != null) ? pawn.creepjoiner : null;
			if (pawn_CreepJoinerTracker == null)
			{
				return;
			}
			if (signal.tag == this.signalShow)
			{
				this.ShowOfferLetter(pawn_CreepJoinerTracker);
				return;
			}
			if (signal.tag == this.signalAttacked)
			{
				Pawn instigatorPawn;
				signal.args.TryGetArg<Pawn>("INSTIGATOR", out instigatorPawn);
				pawn_CreepJoinerTracker.Notify_CreepJoinerAttacked(instigatorPawn);
				return;
			}
			if (signal.tag == this.signalReject)
			{
				pawn_CreepJoinerTracker.Notify_CreepJoinerRejected();
				return;
			}
			if (signal.tag == this.signalTimeout)
			{
				pawn_CreepJoinerTracker.Notify_CreepJoinerRejected();
				return;
			}
			if (signal.tag == this.signalCapture)
			{
				CaptureUtility.OrderArrest(pawn_CreepJoinerTracker.speaker, this.pawn);
			}
		}

		// Token: 0x060149F7 RID: 84471 RVA: 0x0062882A File Offset: 0x00626A2A
		public override void Cleanup()
		{
			this.CloseLetter();
		}

		// Token: 0x060149F8 RID: 84472 RVA: 0x00628832 File Offset: 0x00626A32
		private void CloseLetter()
		{
			if (this.letter != null && Find.LetterStack.LettersListForReading.Contains(this.letter))
			{
				Find.LetterStack.RemoveLetter(this.letter);
			}
		}

		// Token: 0x060149F9 RID: 84473 RVA: 0x00628864 File Offset: 0x00626A64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<ChoiceLetter_AcceptCreepJoiner>(ref this.letter, "letter", false);
			Scribe_Values.Look<string>(ref this.signalAccept, "signalAccept", null, false);
			Scribe_Values.Look<string>(ref this.signalCapture, "signalCapture", null, false);
			Scribe_Values.Look<string>(ref this.signalReject, "signalReject", null, false);
			Scribe_Values.Look<string>(ref this.signalAttacked, "signalAttacked", null, false);
			Scribe_Values.Look<string>(ref this.signalTimeout, "signalTimeout", null, false);
			Scribe_Values.Look<string>(ref this.signalShow, "signalShow", null, false);
			Scribe_Values.Look<int>(ref this.timeout, "timeout", 0, false);
		}

		// Token: 0x0400E4F6 RID: 58614
		public Pawn pawn;

		// Token: 0x0400E4F7 RID: 58615
		public string signalAccept;

		// Token: 0x0400E4F8 RID: 58616
		public string signalCapture;

		// Token: 0x0400E4F9 RID: 58617
		public string signalReject;

		// Token: 0x0400E4FA RID: 58618
		public string signalAttacked;

		// Token: 0x0400E4FB RID: 58619
		public string signalShow;

		// Token: 0x0400E4FC RID: 58620
		public string signalTimeout;

		// Token: 0x0400E4FD RID: 58621
		public int timeout;

		// Token: 0x0400E4FE RID: 58622
		private ChoiceLetter_AcceptCreepJoiner letter;
	}
}
