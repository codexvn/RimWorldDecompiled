using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A78 RID: 14968
	public class QuestNode_GetRandomInRangeForChallengeRating : QuestNode
	{
		// Token: 0x0601473E RID: 83774 RVA: 0x0061BE28 File Offset: 0x0061A028
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float randomInRange = this.GetRangeFromRating().RandomInRange;
			slate.Set<float>(this.storeAs.GetValue(slate), this.roundRandom.GetValue(slate) ? ((float)GenMath.RoundRandom(randomInRange)) : randomInRange, false);
		}

		// Token: 0x0601473F RID: 83775 RVA: 0x0061BE78 File Offset: 0x0061A078
		public FloatRange GetRangeFromRating()
		{
			int challengeRating = QuestGen.quest.challengeRating;
			Slate slate = QuestGen.slate;
			if (challengeRating == 3)
			{
				return this.threeStarRange.GetValue(slate);
			}
			if (challengeRating == 2)
			{
				return this.twoStarRange.GetValue(slate);
			}
			return this.oneStarRange.GetValue(slate);
		}

		// Token: 0x06014740 RID: 83776 RVA: 0x0061BEC4 File Offset: 0x0061A0C4
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), 0, false);
			return true;
		}

		// Token: 0x0400E254 RID: 57940
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E255 RID: 57941
		public SlateRef<FloatRange> oneStarRange;

		// Token: 0x0400E256 RID: 57942
		public SlateRef<FloatRange> twoStarRange;

		// Token: 0x0400E257 RID: 57943
		public SlateRef<FloatRange> threeStarRange;

		// Token: 0x0400E258 RID: 57944
		public SlateRef<bool> roundRandom;
	}
}
