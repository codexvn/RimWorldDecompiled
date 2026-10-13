using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A64 RID: 14948
	public class QuestNode_GetPawnCountByPointsWeighted : QuestNode
	{
		// Token: 0x060146F9 RID: 83705 RVA: 0x0061B13A File Offset: 0x0061933A
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060146FA RID: 83706 RVA: 0x0061B144 File Offset: 0x00619344
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060146FB RID: 83707 RVA: 0x0061B154 File Offset: 0x00619354
		private void SetVars(Slate slate)
		{
			float x = slate.Get<float>("points", 0f, false);
			float num = this.pointsCurve.GetValue(slate).Evaluate(x);
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			int num2;
			if (this.challengeRating.TryGetValue(slate, out num2))
			{
				if (num2 == 1)
				{
					num = Mathf.Min(num, (float)this.maxCountOneStar.GetValue(slate));
				}
				else if (num2 == 2)
				{
					num = Mathf.Min(num, (float)this.maxCountTwoStar.GetValue(slate));
				}
				else
				{
					num = Mathf.Min(num, (float)this.maxCountThreeStar.GetValue(slate));
				}
			}
			SimpleCurve value = this.chancesCurve.GetValue(slate);
			for (int i = value.Points.Count - 1; i >= 0; i--)
			{
				if (value.Points[i].x <= num)
				{
					value.Points.Insert(i + 1, new CurvePoint(num + 1f, 0f));
					break;
				}
				value.Points[i] = new CurvePoint(0f, 0f);
			}
			float num3 = Rand.ByCurve(value);
			if (this.roundRandom.GetValue(slate))
			{
				num3 = (float)GenMath.RoundRandom(num3);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), Mathf.Clamp(num3, 1f, num), false);
		}

		// Token: 0x0400E211 RID: 57873
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E212 RID: 57874
		public SlateRef<int> challengeRating;

		// Token: 0x0400E213 RID: 57875
		public SlateRef<int> maxCountOneStar;

		// Token: 0x0400E214 RID: 57876
		public SlateRef<int> maxCountTwoStar;

		// Token: 0x0400E215 RID: 57877
		public SlateRef<int> maxCountThreeStar;

		// Token: 0x0400E216 RID: 57878
		public SlateRef<SimpleCurve> pointsCurve;

		// Token: 0x0400E217 RID: 57879
		public SlateRef<SimpleCurve> chancesCurve;

		// Token: 0x0400E218 RID: 57880
		public SlateRef<bool> roundRandom;
	}
}
