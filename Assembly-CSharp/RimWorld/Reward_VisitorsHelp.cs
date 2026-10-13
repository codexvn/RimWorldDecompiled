using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020036A4 RID: 13988
	[StaticConstructorOnStartup]
	public class Reward_VisitorsHelp : Reward
	{
		// Token: 0x17003212 RID: 12818
		// (get) Token: 0x060136D4 RID: 79572 RVA: 0x005C5687 File Offset: 0x005C3887
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_VisitorsHelp_Label".Translate(), Reward_VisitorsHelp.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x060136D5 RID: 79573 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060136D6 RID: 79574 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060136D7 RID: 79575 RVA: 0x005C5698 File Offset: 0x005C3898
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_VisitorsHelp".Translate().Resolve();
		}

		// Token: 0x0400D74F RID: 55119
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/VisitorsHelp", true);
	}
}
