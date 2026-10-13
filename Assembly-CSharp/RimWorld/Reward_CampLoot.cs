using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003680 RID: 13952
	[StaticConstructorOnStartup]
	public class Reward_CampLoot : Reward
	{
		// Token: 0x170031E1 RID: 12769
		// (get) Token: 0x060135D6 RID: 79318 RVA: 0x005C2FFF File Offset: 0x005C11FF
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_CampLoot_Label".Translate(), Reward_CampLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x060135D7 RID: 79319 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060135D8 RID: 79320 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060135D9 RID: 79321 RVA: 0x005C3010 File Offset: 0x005C1210
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_CampLoot".Translate().Resolve();
		}

		// Token: 0x0400D6D0 RID: 54992
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
