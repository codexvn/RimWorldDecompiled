using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200335A RID: 13146
	public class ChoiceLetter_AcceptCreepJoiner : ChoiceLetter
	{
		// Token: 0x17003026 RID: 12326
		// (get) Token: 0x0601279F RID: 75679 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003027 RID: 12327
		// (get) Token: 0x060127A0 RID: 75680 RVA: 0x00573C4B File Offset: 0x00571E4B
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && this.quest != null && (this.quest.State == QuestState.Ongoing || this.quest.State == QuestState.NotYetAccepted);
			}
		}

		// Token: 0x17003028 RID: 12328
		// (get) Token: 0x060127A1 RID: 75681 RVA: 0x00573C7D File Offset: 0x00571E7D
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly || !this.pawn.Spawned)
				{
					yield return base.Option_Close;
				}
				else
				{
					DiaOption diaOption = new DiaOption("AcceptCreeper".Translate(this.pawn.Named("PAWN"), this.speaker.Named("SPEAKER")));
					DiaOption optionCapture = new DiaOption("CaptureCreeper".Translate(this.pawn.Named("PAWN"), this.speaker.Named("SPEAKER")));
					DiaOption optionReject = new DiaOption("RejectCreeper".Translate(this.pawn.Named("PAWN"), this.speaker.Named("SPEAKER")));
					diaOption.action = delegate()
					{
						if (!this.pawn.Spawned)
						{
							return;
						}
						Find.SignalManager.SendSignal(new Signal(this.signalAccept, false));
						Find.LetterStack.RemoveLetter(this);
					};
					diaOption.resolveTree = true;
					optionCapture.action = delegate()
					{
						if (!this.pawn.Spawned)
						{
							return;
						}
						Find.SignalManager.SendSignal(new Signal(this.signalCapture, false));
					};
					optionCapture.resolveTree = true;
					string newDisabledReason;
					if (this.pawn.Spawned && !CaptureUtility.CanArrest(this.speaker, this.pawn, out newDisabledReason))
					{
						optionCapture.Disable(newDisabledReason);
					}
					optionReject.action = delegate()
					{
						if (!this.pawn.Spawned)
						{
							return;
						}
						Find.SignalManager.SendSignal(new Signal(this.signalReject, false));
						Find.LetterStack.RemoveLetter(this);
					};
					optionReject.resolveTree = true;
					yield return diaOption;
					yield return optionCapture;
					yield return optionReject;
					if (this.lookTargets.IsValid())
					{
						yield return base.Option_JumpToLocationAndPostpone;
					}
					yield return base.Option_Postpone;
					optionCapture = null;
					optionReject = null;
				}
				yield break;
			}
		}

		// Token: 0x060127A2 RID: 75682 RVA: 0x00573C90 File Offset: 0x00571E90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalAccept, "signalAccept", null, false);
			Scribe_Values.Look<string>(ref this.signalCapture, "signalCapture", null, false);
			Scribe_Values.Look<string>(ref this.signalReject, "signalReject", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.speaker, "speaker", false);
		}

		// Token: 0x0400BFFD RID: 49149
		public string signalAccept;

		// Token: 0x0400BFFE RID: 49150
		public string signalCapture;

		// Token: 0x0400BFFF RID: 49151
		public string signalReject;

		// Token: 0x0400C000 RID: 49152
		public Pawn pawn;

		// Token: 0x0400C001 RID: 49153
		public Pawn speaker;
	}
}
