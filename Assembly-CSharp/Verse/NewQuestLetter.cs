using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BAA RID: 2986
	public class NewQuestLetter : ChoiceLetter
	{
		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x00183FE0 File Offset: 0x001821E0
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewQuest", false);
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocation;
				}
				yield return base.Option_Close;
				yield break;
			}
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x00183FF0 File Offset: 0x001821F0
		public override void OpenLetter()
		{
			if (this.quest != null && !base.ArchivedOnly)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
				Find.LetterStack.RemoveLetter(this);
				return;
			}
			base.OpenLetter();
		}
	}
}
