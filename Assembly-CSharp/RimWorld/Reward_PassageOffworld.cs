using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003692 RID: 13970
	[StaticConstructorOnStartup]
	public class Reward_PassageOffworld : Reward
	{
		// Token: 0x170031F8 RID: 12792
		// (get) Token: 0x06013652 RID: 79442 RVA: 0x005C4477 File Offset: 0x005C2677
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PassageOffworld_Label".Translate(), Reward_PassageOffworld.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06013653 RID: 79443 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06013654 RID: 79444 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06013655 RID: 79445 RVA: 0x005C4488 File Offset: 0x005C2688
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PassageOffworld".Translate().Resolve();
		}

		// Token: 0x0400D70D RID: 55053
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Stars", true);
	}
}
