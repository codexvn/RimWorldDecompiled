using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003368 RID: 13160
	public class ChoiceLetter_ChoosePawn : ChoiceLetter
	{
		// Token: 0x17003045 RID: 12357
		// (get) Token: 0x06012805 RID: 75781 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003046 RID: 12358
		// (get) Token: 0x06012806 RID: 75782 RVA: 0x00575508 File Offset: 0x00573708
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.chosenPawnSignal.NullOrEmpty())
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x17003047 RID: 12359
		// (get) Token: 0x06012807 RID: 75783 RVA: 0x0057555E File Offset: 0x0057375E
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					int num;
					for (int i = 0; i < this.pawns.Count; i = num + 1)
					{
						if (!this.pawns[i].DestroyedOrNull())
						{
							yield return this.Option_ChoosePawn(this.pawns[i]);
						}
						num = i;
					}
					yield return base.Option_Postpone;
				}
				else
				{
					yield return base.Option_Close;
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocationAndPostpone;
				}
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", true);
				}
				yield break;
			}
		}

		// Token: 0x06012808 RID: 75784 RVA: 0x00575570 File Offset: 0x00573770
		private DiaOption Option_ChoosePawn(Pawn p)
		{
			return new DiaOption(p.LabelCap)
			{
				action = delegate()
				{
					if (!this.chosenPawnSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.chosenPawnSignal, p.Named("CHOSEN")));
					}
					Find.LetterStack.RemoveLetter(this);
				},
				resolveTree = true
			};
		}

		// Token: 0x06012809 RID: 75785 RVA: 0x005755BC File Offset: 0x005737BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400C032 RID: 49202
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400C033 RID: 49203
		public string chosenPawnSignal;
	}
}
