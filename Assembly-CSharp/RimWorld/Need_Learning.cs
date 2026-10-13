using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002484 RID: 9348
	public class Need_Learning : Need
	{
		// Token: 0x17001ED2 RID: 7890
		// (get) Token: 0x0600CAC7 RID: 51911 RVA: 0x003B645B File Offset: 0x003B465B
		protected override bool IsFrozen
		{
			get
			{
				return base.IsFrozen || this.pawn.Deathresting;
			}
		}

		// Token: 0x17001ED3 RID: 7891
		// (get) Token: 0x0600CAC8 RID: 51912 RVA: 0x003B6472 File Offset: 0x003B4672
		public bool Suspended
		{
			get
			{
				return this.IsFrozen;
			}
		}

		// Token: 0x17001ED4 RID: 7892
		// (get) Token: 0x0600CAC9 RID: 51913 RVA: 0x003B647C File Offset: 0x003B467C
		public LearningCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return LearningCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return LearningCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return LearningCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return LearningCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return LearningCategory.High;
				}
				return LearningCategory.Extreme;
			}
		}

		// Token: 0x0600CACA RID: 51914 RVA: 0x003B64D5 File Offset: 0x003B46D5
		public Need_Learning(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>
			{
				0.15f,
				0.3f,
				0.7f,
				0.85f
			};
		}

		// Token: 0x0600CACB RID: 51915 RVA: 0x003B6515 File Offset: 0x003B4715
		public void Learn(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			amount = Mathf.Min(amount, 1f - this.CurLevel);
			this.curLevelInt += amount;
		}

		// Token: 0x0600CACC RID: 51916 RVA: 0x003B6544 File Offset: 0x003B4744
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (this.pawn.learning == null)
			{
				return;
			}
			List<LearningDesireDef> activeLearningDesires = this.pawn.learning.ActiveLearningDesires;
			float num = (float)activeLearningDesires.Count * 30f;
			float num2 = rect.xMax - num;
			for (int i = 0; i < activeLearningDesires.Count; i++)
			{
				Rect rect2 = new Rect(num2 + (float)i * 30f, rect.y, 30f, 30f);
				LearningDesireDef learningDesire = activeLearningDesires[i];
				GUI.DrawTexture(rect2.ContractedBy(5f), activeLearningDesires[i].Icon);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, () => string.Concat(new string[]
					{
						learningDesire.LabelCap.Colorize(ColoredText.TipSectionTitleColor),
						"\n\n",
						learningDesire.description,
						"\n\n",
						"LearningDesireTooltip".Translate().Colorize(ColoredText.SubtleGrayColor)
					}).ResolveTags(), learningDesire.GetHashCode());
					doTooltip = false;
				}
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip, drawLabel);
		}

		// Token: 0x0600CACD RID: 51917 RVA: 0x003B6638 File Offset: 0x003B4838
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 0.00045000002f;
			}
			if (this.pawn.ageTracker.canGainGrowthPoints)
			{
				this.pawn.ageTracker.growthPoints += this.pawn.ageTracker.GrowthPointsPerDay * 0.0025f;
			}
		}

		// Token: 0x0600CACE RID: 51918 RVA: 0x003B66A0 File Offset: 0x003B48A0
		public override string GetTipString()
		{
			if (Need_Learning.learningActivitiesLineList == null)
			{
				Need_Learning.learningActivitiesLineList = (from d in DefDatabase<LearningDesireDef>.AllDefsListForReading
				select d.label).ToList<string>().ToLineList("  - ", true);
			}
			return (base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor) + "\n" + this.def.description.Formatted(Need_Learning.learningActivitiesLineList.Named("ACTIVITIES"), this.pawn.Named("PAWN")).Resolve();
		}

		// Token: 0x04008A33 RID: 35379
		private static string learningActivitiesLineList;

		// Token: 0x04008A34 RID: 35380
		public const float BaseFallPerTick = 3E-06f;

		// Token: 0x04008A35 RID: 35381
		public const float BaseFallPerInterval = 0.00045000002f;

		// Token: 0x04008A36 RID: 35382
		public const float ThresholdEmpty = 0.01f;

		// Token: 0x04008A37 RID: 35383
		public const float ThresholdLow = 0.15f;

		// Token: 0x04008A38 RID: 35384
		public const float ThresholdSatisfied = 0.3f;

		// Token: 0x04008A39 RID: 35385
		public const float ThresholdHigh = 0.7f;

		// Token: 0x04008A3A RID: 35386
		public const float ThresholdVeryHigh = 0.85f;

		// Token: 0x04008A3B RID: 35387
		public const float IconSize = 30f;

		// Token: 0x04008A3C RID: 35388
		public const float IconPad = 5f;
	}
}
