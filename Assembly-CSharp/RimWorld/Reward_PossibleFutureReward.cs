using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003699 RID: 13977
	[StaticConstructorOnStartup]
	public class Reward_PossibleFutureReward : Reward
	{
		// Token: 0x17003200 RID: 12800
		// (get) Token: 0x0601367C RID: 79484 RVA: 0x005C4C4F File Offset: 0x005C2E4F
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PossibleFutureReward_Label".Translate(), Reward_PossibleFutureReward.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x0601367D RID: 79485 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0601367E RID: 79486 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0601367F RID: 79487 RVA: 0x005C4C60 File Offset: 0x005C2E60
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PossibleFutureReward".Translate().Resolve();
		}

		// Token: 0x0400D72D RID: 55085
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/QuestionMark", true);
	}
}
