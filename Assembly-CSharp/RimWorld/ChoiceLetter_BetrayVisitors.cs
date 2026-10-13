using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003365 RID: 13157
	[Obsolete("Betray visitors is now a subquest of refugee quest.")]
	public class ChoiceLetter_BetrayVisitors : ChoiceLetter
	{
		// Token: 0x17003040 RID: 12352
		// (get) Token: 0x060127F5 RID: 75765 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003041 RID: 12353
		// (get) Token: 0x060127F6 RID: 75766 RVA: 0x005752A4 File Offset: 0x005734A4
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.quest == null || this.quest.State != QuestState.Ongoing)
				{
					return false;
				}
				if (this.requiresAliveAsker && (this.asker == null || this.asker.Dead))
				{
					return false;
				}
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Spawned && !this.pawns[i].Destroyed)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17003042 RID: 12354
		// (get) Token: 0x060127F7 RID: 75767 RVA: 0x00575331 File Offset: 0x00573531
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocationAndPostpone;
				}
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", true);
				}
				yield return base.Option_Close;
				yield break;
			}
		}

		// Token: 0x060127F8 RID: 75768 RVA: 0x00575344 File Offset: 0x00573544
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_Values.Look<bool>(ref this.requiresAliveAsker, "requiresAliveAsker", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400C029 RID: 49193
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400C02A RID: 49194
		public Pawn asker;

		// Token: 0x0400C02B RID: 49195
		public bool requiresAliveAsker;
	}
}
