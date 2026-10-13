using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A8F RID: 14991
	public class QuestNode_SplitRandomly : QuestNode
	{
		// Token: 0x0601478E RID: 83854 RVA: 0x0061D485 File Offset: 0x0061B685
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0601478F RID: 83855 RVA: 0x0061D48F File Offset: 0x0061B68F
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06014790 RID: 83856 RVA: 0x0061D49C File Offset: 0x0061B69C
		private void DoWork(Slate slate)
		{
			float num = this.valueToSplit.GetValue(slate) ?? 1f;
			QuestNode_SplitRandomly.tmpMinFractions.Clear();
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction1.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction2.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction3.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction4.GetValue(slate));
			QuestNode_SplitRandomly.tmpMinFractions.Add(this.minFraction5.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Clear();
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow1.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow2.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow3.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow4.GetValue(slate));
			QuestNode_SplitRandomly.tmpZeroIfFractionBelow.Add(this.zeroIfFractionBelow5.GetValue(slate));
			Rand.SplitRandomly(num, this.countToSplit.GetValue(slate), QuestNode_SplitRandomly.tmpValues, QuestNode_SplitRandomly.tmpZeroIfFractionBelow, QuestNode_SplitRandomly.tmpMinFractions);
			for (int i = 0; i < QuestNode_SplitRandomly.tmpValues.Count; i++)
			{
				if (this.storeAsFormat.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAsFormat.GetValue(slate).Formatted(i.Named("INDEX")), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				if (i == 0 && this.storeAs1.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs1.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 1 && this.storeAs2.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs2.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 2 && this.storeAs3.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs3.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 3 && this.storeAs4.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs4.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
				else if (i == 4 && this.storeAs5.GetValue(slate) != null)
				{
					slate.Set<float>(this.storeAs5.GetValue(slate), QuestNode_SplitRandomly.tmpValues[i], false);
				}
			}
		}

		// Token: 0x0400E2A2 RID: 58018
		[NoTranslate]
		public SlateRef<string> storeAsFormat;

		// Token: 0x0400E2A3 RID: 58019
		[NoTranslate]
		public SlateRef<string> storeAs1;

		// Token: 0x0400E2A4 RID: 58020
		[NoTranslate]
		public SlateRef<string> storeAs2;

		// Token: 0x0400E2A5 RID: 58021
		[NoTranslate]
		public SlateRef<string> storeAs3;

		// Token: 0x0400E2A6 RID: 58022
		[NoTranslate]
		public SlateRef<string> storeAs4;

		// Token: 0x0400E2A7 RID: 58023
		[NoTranslate]
		public SlateRef<string> storeAs5;

		// Token: 0x0400E2A8 RID: 58024
		public SlateRef<float?> valueToSplit;

		// Token: 0x0400E2A9 RID: 58025
		public SlateRef<int> countToSplit;

		// Token: 0x0400E2AA RID: 58026
		public SlateRef<float> zeroIfFractionBelow1;

		// Token: 0x0400E2AB RID: 58027
		public SlateRef<float> zeroIfFractionBelow2;

		// Token: 0x0400E2AC RID: 58028
		public SlateRef<float> zeroIfFractionBelow3;

		// Token: 0x0400E2AD RID: 58029
		public SlateRef<float> zeroIfFractionBelow4;

		// Token: 0x0400E2AE RID: 58030
		public SlateRef<float> zeroIfFractionBelow5;

		// Token: 0x0400E2AF RID: 58031
		public SlateRef<float> minFraction1;

		// Token: 0x0400E2B0 RID: 58032
		public SlateRef<float> minFraction2;

		// Token: 0x0400E2B1 RID: 58033
		public SlateRef<float> minFraction3;

		// Token: 0x0400E2B2 RID: 58034
		public SlateRef<float> minFraction4;

		// Token: 0x0400E2B3 RID: 58035
		public SlateRef<float> minFraction5;

		// Token: 0x0400E2B4 RID: 58036
		private static List<float> tmpValues = new List<float>();

		// Token: 0x0400E2B5 RID: 58037
		private static List<float> tmpZeroIfFractionBelow = new List<float>();

		// Token: 0x0400E2B6 RID: 58038
		private static List<float> tmpMinFractions = new List<float>();
	}
}
