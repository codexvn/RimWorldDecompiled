using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200367B RID: 13947
	[StaticConstructorOnStartup]
	public class Reward_ArchonexusMap : Reward
	{
		// Token: 0x170031DA RID: 12762
		// (get) Token: 0x060135B1 RID: 79281 RVA: 0x005C2B6A File Offset: 0x005C0D6A
		private static Texture2D Icon
		{
			get
			{
				if (Reward_ArchonexusMap.icon == null)
				{
					Reward_ArchonexusMap.icon = ContentFinder<Texture2D>.Get("UI/Icons/ArchonexusMapPart", true);
				}
				return Reward_ArchonexusMap.icon;
			}
		}

		// Token: 0x170031DB RID: 12763
		// (get) Token: 0x060135B2 RID: 79282 RVA: 0x005C2B8E File Offset: 0x005C0D8E
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_ArchonexusMapPartLabel".Translate(this.currentPart, 3), Reward_ArchonexusMap.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x060135B3 RID: 79283 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060135B4 RID: 79284 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060135B5 RID: 79285 RVA: 0x005C2B9E File Offset: 0x005C0D9E
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ArchonexusMapPart".Translate(this.currentPart, 3);
		}

		// Token: 0x060135B6 RID: 79286 RVA: 0x005C2BC0 File Offset: 0x005C0DC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.currentPart, "currentPart", 0, false);
		}

		// Token: 0x0400D6BD RID: 54973
		public int currentPart = 1;

		// Token: 0x0400D6BE RID: 54974
		private const int totalParts = 3;

		// Token: 0x0400D6BF RID: 54975
		private static Texture2D icon;
	}
}
