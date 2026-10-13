using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020036A2 RID: 13986
	[StaticConstructorOnStartup]
	public class Reward_Unknown : Reward
	{
		// Token: 0x1700320F RID: 12815
		// (get) Token: 0x060136C5 RID: 79557 RVA: 0x005C553F File Offset: 0x005C373F
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_Unknown_Label".Translate(), Reward_Unknown.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x060136C6 RID: 79558 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060136C7 RID: 79559 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060136C8 RID: 79560 RVA: 0x005C5550 File Offset: 0x005C3750
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Unknown".Translate().Resolve();
		}

		// Token: 0x0400D74A RID: 55114
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
