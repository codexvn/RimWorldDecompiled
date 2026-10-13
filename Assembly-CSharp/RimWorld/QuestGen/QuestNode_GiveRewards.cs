using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ACB RID: 15051
	public class QuestNode_GiveRewards : QuestNode
	{
		// Token: 0x06014850 RID: 84048 RVA: 0x0061FA3F File Offset: 0x0061DC3F
		protected override bool TestRunInt(Slate slate)
		{
			return this.nodeIfChosenPawnSignalUsed == null || this.nodeIfChosenPawnSignalUsed.TestRun(slate);
		}

		// Token: 0x06014851 RID: 84049 RVA: 0x0061FA58 File Offset: 0x0061DC58
		protected override void RunInt()
		{
			QuestGen.quest.GiveRewards(this.parms.GetValue(QuestGen.slate), this.inSignal.GetValue(QuestGen.slate), this.customLetterLabel.GetValue(QuestGen.slate), this.customLetterText.GetValue(QuestGen.slate), this.customLetterLabelRules.GetValue(QuestGen.slate), this.customLetterTextRules.GetValue(QuestGen.slate), this.useDifficultyFactor.GetValue(QuestGen.slate), delegate
			{
				QuestNode questNode = this.nodeIfChosenPawnSignalUsed;
				if (questNode == null)
				{
					return;
				}
				questNode.Run();
			}, this.variants.GetValue(QuestGen.slate), this.addCampLootReward.GetValue(QuestGen.slate), QuestGen.slate.Get<Pawn>("asker", null, false), false, false, null);
		}

		// Token: 0x06014852 RID: 84050 RVA: 0x00002C42 File Offset: 0x00000E42
		[Obsolete("Will be removed in the future")]
		private List<Reward> GenerateRewards(RewardsGeneratorParams parmsResolved, Slate slate, bool addDescriptionRules, ref bool chosenPawnSignalUsed, QuestPart_Choice.Choice choice, int variants)
		{
			return null;
		}

		// Token: 0x06014853 RID: 84051 RVA: 0x00002C42 File Offset: 0x00000E42
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_SocialOnly(RewardsGeneratorParams parms, bool disallowRoyalFavor)
		{
			return null;
		}

		// Token: 0x06014854 RID: 84052 RVA: 0x00002C42 File Offset: 0x00000E42
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_RoyalFavorOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x06014855 RID: 84053 RVA: 0x00002C42 File Offset: 0x00000E42
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_ThingsOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x06014856 RID: 84054 RVA: 0x00002C42 File Offset: 0x00000E42
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateNonRepeatingRewards(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x06014857 RID: 84055 RVA: 0x00002501 File Offset: 0x00000701
		[Obsolete("Will be removed in the future")]
		private int GetDisplayPriority(QuestPart_Choice.Choice choice)
		{
			return 0;
		}

		// Token: 0x0400E376 RID: 58230
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E377 RID: 58231
		public SlateRef<RewardsGeneratorParams> parms;

		// Token: 0x0400E378 RID: 58232
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400E379 RID: 58233
		public SlateRef<string> customLetterText;

		// Token: 0x0400E37A RID: 58234
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400E37B RID: 58235
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400E37C RID: 58236
		public SlateRef<bool?> useDifficultyFactor;

		// Token: 0x0400E37D RID: 58237
		public QuestNode nodeIfChosenPawnSignalUsed;

		// Token: 0x0400E37E RID: 58238
		public SlateRef<int?> variants;

		// Token: 0x0400E37F RID: 58239
		public SlateRef<bool> addCampLootReward;

		// Token: 0x0400E380 RID: 58240
		private List<List<Reward>> generatedRewards = new List<List<Reward>>();

		// Token: 0x0400E381 RID: 58241
		private const float MinRewardValue = 250f;

		// Token: 0x0400E382 RID: 58242
		private const int DefaultVariants = 3;
	}
}
