using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020036A0 RID: 13984
	[StaticConstructorOnStartup]
	public class Reward_ShuttleLoot : Reward
	{
		// Token: 0x1700320C RID: 12812
		// (get) Token: 0x060136B6 RID: 79542 RVA: 0x005C53F7 File Offset: 0x005C35F7
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_ShuttleLoot_Label".Translate(), Reward_ShuttleLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x060136B7 RID: 79543 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060136B8 RID: 79544 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060136B9 RID: 79545 RVA: 0x005C5408 File Offset: 0x005C3608
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ShuttleLoot".Translate().Resolve();
		}

		// Token: 0x0400D745 RID: 55109
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/CrashedShuttle", true);
	}
}
