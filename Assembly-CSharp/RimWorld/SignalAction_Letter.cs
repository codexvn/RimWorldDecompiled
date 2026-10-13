using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ABF RID: 10943
	public class SignalAction_Letter : SignalAction
	{
		// Token: 0x0600F3D3 RID: 62419 RVA: 0x0046D610 File Offset: 0x0046B810
		protected override void DoAction(SignalArgs args)
		{
			if (this.requireLookTargetsNotDestroyed && this.lookTargets.PrimaryTarget.ThingDestroyed)
			{
				return;
			}
			Pawn pawn;
			Pawn pawn2;
			if (args.TryGetArg<Pawn>("SUBJECT", out pawn))
			{
				pawn2 = (this.fixedPawnReference ?? pawn);
				if (!this.lookTargets.IsValid())
				{
					this.lookTargets = pawn;
				}
			}
			else
			{
				pawn2 = this.fixedPawnReference;
			}
			ValueTuple<string, string> labelMessage = this.GetLabelMessage(pawn2);
			Find.LetterStack.ReceiveLetter(labelMessage.Item1, labelMessage.Item2, this.letterDef, this.lookTargets, null, null, null, null, 0, true);
		}

		// Token: 0x0600F3D4 RID: 62420 RVA: 0x0046D6B4 File Offset: 0x0046B8B4
		[return: TupleElementNames(new string[]
		{
			"label",
			"message"
		})]
		private ValueTuple<string, string> GetLabelMessage(Pawn pawn = null)
		{
			string item;
			if (!string.IsNullOrEmpty(this.letterLabel))
			{
				item = this.letterLabel;
			}
			else if (pawn != null)
			{
				item = this.letterLabelKey.Translate(pawn.Named("PAWN"));
			}
			else
			{
				item = this.letterLabelKey.Translate();
			}
			string item2;
			if (!string.IsNullOrEmpty(this.letterMessage))
			{
				item2 = this.letterMessage;
			}
			else if (pawn != null)
			{
				item2 = this.letterMessageKey.Translate(pawn.Named("PAWN"));
			}
			else
			{
				item2 = this.letterMessageKey.Translate();
			}
			return new ValueTuple<string, string>(item, item2);
		}

		// Token: 0x0600F3D5 RID: 62421 RVA: 0x0046D758 File Offset: 0x0046B958
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.letterLabelKey, "letterLabelKey", null, false);
			Scribe_Values.Look<string>(ref this.letterLabel, "letterLabel", null, false);
			Scribe_Values.Look<bool>(ref this.requireLookTargetsNotDestroyed, "requireLookTargetsSpawned", false, false);
			Scribe_Values.Look<string>(ref this.letterMessageKey, "letterMessageKey", null, false);
			Scribe_Values.Look<string>(ref this.letterMessage, "letterMessage", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Defs.Look<LetterDef>(ref this.letterDef, "letterDef");
			Scribe_References.Look<Pawn>(ref this.fixedPawnReference, "fixedPawnReference", true);
			Scribe_Deep.Look<Letter>(ref this.tmpLetter, "letter", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.tmpLetter != null)
			{
				this.letterLabel = this.tmpLetter.Label;
				this.lookTargets = this.tmpLetter.lookTargets;
				this.letterDef = this.tmpLetter.def;
				ChoiceLetter choiceLetter = this.tmpLetter as ChoiceLetter;
				if (choiceLetter != null)
				{
					this.letterMessage = choiceLetter.Text;
				}
			}
		}

		// Token: 0x0400A0A1 RID: 41121
		public string letterLabelKey;

		// Token: 0x0400A0A2 RID: 41122
		public string letterMessageKey;

		// Token: 0x0400A0A3 RID: 41123
		public LetterDef letterDef;

		// Token: 0x0400A0A4 RID: 41124
		public LookTargets lookTargets;

		// Token: 0x0400A0A5 RID: 41125
		public Pawn fixedPawnReference;

		// Token: 0x0400A0A6 RID: 41126
		private string letterLabel;

		// Token: 0x0400A0A7 RID: 41127
		private string letterMessage;

		// Token: 0x0400A0A8 RID: 41128
		public bool requireLookTargetsNotDestroyed;

		// Token: 0x0400A0A9 RID: 41129
		private Letter tmpLetter;
	}
}
