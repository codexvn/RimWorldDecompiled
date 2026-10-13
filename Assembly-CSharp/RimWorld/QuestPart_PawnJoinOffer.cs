using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C72 RID: 7282
	public class QuestPart_PawnJoinOffer : QuestPartActivable
	{
		// Token: 0x0600A245 RID: 41541 RVA: 0x002EA5F1 File Offset: 0x002E87F1
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			if (this.sendLetterOnEnable)
			{
				this.SendLetter();
			}
		}

		// Token: 0x0600A246 RID: 41542 RVA: 0x002EA608 File Offset: 0x002E8808
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.sendLetterOnEnable || this.letterSent || this.pawn == null || this.pawn.needs == null || this.pawn.needs.mood == null || !this.pawn.IsHashIntervalTick(2500))
			{
				return;
			}
			float curLevelPercentage = this.pawn.needs.mood.CurLevelPercentage;
			if (curLevelPercentage < 0.5f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(QuestPart_PawnJoinOffer.JoinMTBbyMoodCurve.Evaluate(curLevelPercentage), 60000f, 2500f))
			{
				this.SendLetter();
			}
		}

		// Token: 0x0600A247 RID: 41543 RVA: 0x002EA6A8 File Offset: 0x002E88A8
		private void SendLetter()
		{
			if (this.letterSent)
			{
				return;
			}
			this.letter = (ChoiceLetter_AcceptVisitors)LetterMaker.MakeLetter(this.letterLabel, this.letterText, LetterDefOf.AcceptVisitors, null, this.quest);
			this.letter.title = this.letterTitle;
			this.letter.pawns.Add(this.pawn);
			this.letter.quest = this.quest;
			this.letter.acceptedSignal = this.outSignalPawnAccepted;
			this.letter.rejectedSignal = this.outSignalPawnRejected;
			this.letter.lookTargets = new LookTargets(this.pawn);
			this.letter.charity = this.charity;
			Find.LetterStack.ReceiveLetter(this.letter, null, 0, true);
			this.letterSent = true;
			if (this.pawn.guest != null)
			{
				this.pawn.guest.Recruitable = true;
			}
		}

		// Token: 0x0600A248 RID: 41544 RVA: 0x002EA7A9 File Offset: 0x002E89A9
		public override void Cleanup()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x0600A249 RID: 41545 RVA: 0x002EA7A9 File Offset: 0x002E89A9
		protected override void Disable()
		{
			this.RemoveLetterIfActive();
		}

		// Token: 0x0600A24A RID: 41546 RVA: 0x002EA7B1 File Offset: 0x002E89B1
		private void RemoveLetterIfActive()
		{
			if (this.letter != null && Find.LetterStack.LettersListForReading.Contains(this.letter))
			{
				Find.LetterStack.RemoveLetter(this.letter);
			}
		}

		// Token: 0x0600A24B RID: 41547 RVA: 0x002EA7E4 File Offset: 0x002E89E4
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled || this.letterSent || this.pawn == null)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, string.Format("Add Join Letter for {0} ", this.pawn.NameShortColored) + base.GetType().Name, true, true, true, null))
			{
				this.SendLetter();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x0600A24C RID: 41548 RVA: 0x002EA87C File Offset: 0x002E8A7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<bool>(ref this.letterSent, "letterSent", false, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnAccepted, "outSignalPawnAccepted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnRejected, "outSignalPawnRejected", null, false);
			Scribe_Values.Look<string>(ref this.letterLabel, "letterLabel", null, false);
			Scribe_Values.Look<string>(ref this.letterText, "letterText", null, false);
			Scribe_Values.Look<string>(ref this.letterTitle, "letterTitle", null, false);
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			Scribe_Values.Look<bool>(ref this.sendLetterOnEnable, "sendLetterOnEnable", false, false);
		}

		// Token: 0x0400701D RID: 28701
		public Pawn pawn;

		// Token: 0x0400701E RID: 28702
		public bool letterSent;

		// Token: 0x0400701F RID: 28703
		public string outSignalPawnAccepted;

		// Token: 0x04007020 RID: 28704
		public string outSignalPawnRejected;

		// Token: 0x04007021 RID: 28705
		public string letterLabel;

		// Token: 0x04007022 RID: 28706
		public string letterText;

		// Token: 0x04007023 RID: 28707
		public string letterTitle;

		// Token: 0x04007024 RID: 28708
		public bool charity;

		// Token: 0x04007025 RID: 28709
		public bool sendLetterOnEnable;

		// Token: 0x04007026 RID: 28710
		private ChoiceLetter_AcceptVisitors letter;

		// Token: 0x04007027 RID: 28711
		private const float MinMoodPercentage = 0.5f;

		// Token: 0x04007028 RID: 28712
		private static readonly SimpleCurve JoinMTBbyMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.5f, 60f),
				true
			},
			{
				new CurvePoint(1f, 15f),
				true
			}
		};

		// Token: 0x04007029 RID: 28713
		private const int CheckInterval = 2500;
	}
}
