using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE8 RID: 12264
	public class Alert_QuestExpiresSoon : Alert
	{
		// Token: 0x17002CCC RID: 11468
		// (get) Token: 0x06011465 RID: 70757 RVA: 0x004F2E44 File Offset: 0x004F1044
		private Quest QuestExpiring
		{
			get
			{
				int num = int.MaxValue;
				this.questExpiring = null;
				foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
				{
					if (!quest.dismissed && !quest.Historical && !quest.initiallyAccepted && quest.State == QuestState.NotYetAccepted && quest.TicksUntilExpiry > 0 && quest.TicksUntilExpiry < 60000 && quest.TicksUntilExpiry < num)
					{
						this.questExpiring = quest;
						num = quest.TicksUntilExpiry;
					}
				}
				return this.questExpiring;
			}
		}

		// Token: 0x06011466 RID: 70758 RVA: 0x004F2A9B File Offset: 0x004F0C9B
		public Alert_QuestExpiresSoon()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06011467 RID: 70759 RVA: 0x004F2EF4 File Offset: 0x004F10F4
		protected override void OnClick()
		{
			if (this.questExpiring != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.questExpiring);
			}
		}

		// Token: 0x06011468 RID: 70760 RVA: 0x004F2F28 File Offset: 0x004F1128
		public override string GetLabel()
		{
			if (this.questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoon".Translate(this.questExpiring.TicksUntilExpiry.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x06011469 RID: 70761 RVA: 0x004F2F60 File Offset: 0x004F1160
		public override TaggedString GetExplanation()
		{
			if (this.questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoonDesc".Translate(this.questExpiring.name, this.questExpiring.TicksUntilExpiry.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor));
		}

		// Token: 0x0601146A RID: 70762 RVA: 0x004F2FBB File Offset: 0x004F11BB
		public override AlertReport GetReport()
		{
			return this.QuestExpiring != null;
		}

		// Token: 0x0400B31F RID: 45855
		private Quest questExpiring;

		// Token: 0x0400B320 RID: 45856
		private const int TicksToAlert = 60000;
	}
}
