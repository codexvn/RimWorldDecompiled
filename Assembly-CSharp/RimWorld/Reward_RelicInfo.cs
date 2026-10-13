using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200369B RID: 13979
	public class Reward_RelicInfo : Reward
	{
		// Token: 0x17003203 RID: 12803
		// (get) Token: 0x0601368B RID: 79499 RVA: 0x005C4D97 File Offset: 0x005C2F97
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_RelicInfoLabel".Translate(this.relic.Label), delegate(Rect rect)
				{
					this.relic.DrawIcon(rect);
				}, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate()
				{
					Quest quest = this.quest;
					if (((quest != null) ? quest.parent : null) != null)
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest.parent);
					}
				});
				yield break;
			}
		}

		// Token: 0x0601368C RID: 79500 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0601368D RID: 79501 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0601368E RID: 79502 RVA: 0x005C4DA7 File Offset: 0x005C2FA7
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_RelicInfo".Translate(this.relic.Label);
		}

		// Token: 0x0601368F RID: 79503 RVA: 0x005C4DC8 File Offset: 0x005C2FC8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x0400D732 RID: 55090
		public Precept_Relic relic;

		// Token: 0x0400D733 RID: 55091
		public Quest quest;
	}
}
