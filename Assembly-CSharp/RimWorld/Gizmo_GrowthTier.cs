using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003869 RID: 14441
	[StaticConstructorOnStartup]
	public class Gizmo_GrowthTier : Gizmo
	{
		// Token: 0x170032FD RID: 13053
		// (get) Token: 0x06013F63 RID: 81763 RVA: 0x005EC39D File Offset: 0x005EA59D
		private float Width
		{
			get
			{
				return 190f;
			}
		}

		// Token: 0x170032FE RID: 13054
		// (get) Token: 0x06013F64 RID: 81764 RVA: 0x005EC3A4 File Offset: 0x005EA5A4
		private int GrowthTier
		{
			get
			{
				return this.child.ageTracker.GrowthTier;
			}
		}

		// Token: 0x170032FF RID: 13055
		// (get) Token: 0x06013F65 RID: 81765 RVA: 0x005EC3B6 File Offset: 0x005EA5B6
		public override bool Visible
		{
			get
			{
				return this.child.IsColonistPlayerControlled || this.child.IsPrisonerOfColony || this.child.IsSlaveOfColony;
			}
		}

		// Token: 0x06013F66 RID: 81766 RVA: 0x005EC3DF File Offset: 0x005EA5DF
		public override float GetWidth(float maxWidth)
		{
			return this.Width;
		}

		// Token: 0x06013F67 RID: 81767 RVA: 0x005EC3E7 File Offset: 0x005EA5E7
		public Gizmo_GrowthTier(Pawn child)
		{
			this.child = child;
			this.Order = -100f;
		}

		// Token: 0x06013F68 RID: 81768 RVA: 0x005EC404 File Offset: 0x005EA604
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(8f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, rect2.height / 2f);
			Rect rect4 = new Rect(rect2.x, rect3.yMax, rect2.width, rect3.height);
			rect3.yMax -= 2f;
			rect4.yMin += 2f;
			this.DrawGrowthTier(rect3);
			this.DrawLearning(rect4);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06013F69 RID: 81769 RVA: 0x005EC4C8 File Offset: 0x005EA6C8
		private string GrowthTierTooltip(int tier)
		{
			TaggedString t = string.Concat(new string[]
			{
				("StatsReport_GrowthTier".Translate() + ": ").AsTipTitle(),
				tier.ToString(),
				"\n",
				"StatsReport_GrowthTierDesc".Translate().Colorize(ColoredText.SubtleGrayColor),
				"\n\n"
			});
			if (this.child.ageTracker.AtMaxGrowthTier)
			{
				t += ("MaxTier".Translate() + ": ").AsTipTitle() + "MaxTierDesc".Translate(this.child.Named("PAWN"));
			}
			else
			{
				t += ("ProgressToNextGrowthTier".Translate() + ": ").AsTipTitle() + Mathf.FloorToInt(this.child.ageTracker.growthPoints).ToString() + " / " + GrowthUtility.GrowthTiers[tier + 1].pointsRequirement.ToString();
				if (this.child.ageTracker.canGainGrowthPoints)
				{
					t += string.Format(" (+{0})", "PerDay".Translate(this.child.ageTracker.GrowthPointsPerDay.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
				}
			}
			if (this.child.ageTracker.AgeBiologicalYears < 13)
			{
				int num = 0;
				for (int i = this.child.ageTracker.AgeBiologicalYears + 1; i <= 13; i++)
				{
					if (GrowthUtility.IsGrowthBirthday(i))
					{
						num = i;
						break;
					}
				}
				t += "\n\n" + ("NextGrowthMomentAt".Translate() + ": ").AsTipTitle() + num.ToString();
			}
			GrowthUtility.GrowthTier growthTier = GrowthUtility.GrowthTiers[tier];
			t += "\n\n" + ("ThisGrowthTier".Translate(tier) + ":").AsTipTitle();
			if (growthTier.passionGainsRange.TrueMax > 0)
			{
				t += "\n  - " + "NumPassionsFromOptions".Translate(growthTier.passionGainsRange.ToString(), growthTier.passionChoices);
			}
			t += "\n  - " + "NumTraitsFromOptions".Translate(growthTier.traitGains, growthTier.traitChoices);
			if (!this.child.ageTracker.AtMaxGrowthTier)
			{
				GrowthUtility.GrowthTier growthTier2 = GrowthUtility.GrowthTiers[tier + 1];
				t += "\n\n" + ("NextGrowthTier".Translate(tier + 1) + ":").AsTipTitle();
				if (growthTier2.passionGainsRange.TrueMax > 0)
				{
					t += "\n  - " + "NumPassionsFromOptions".Translate(growthTier2.passionGainsRange.ToString(), growthTier2.passionChoices);
				}
				t += "\n  - " + "NumTraitsFromOptions".Translate(growthTier2.traitGains, growthTier2.traitChoices);
			}
			return t.Resolve();
		}

		// Token: 0x06013F6A RID: 81770 RVA: 0x005EC858 File Offset: 0x005EAA58
		private void DrawGrowthTier(Rect rect)
		{
			int growthTier = this.GrowthTier;
			Rect rect2 = rect;
			rect2.xMax = rect.x + rect.width * 0.55f;
			string label = "StatsReport_GrowthTier".Translate() + ": " + growthTier.ToString();
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, label);
			Text.Anchor = TextAnchor.UpperLeft;
			float percentToNextGrowthTier = this.child.ageTracker.PercentToNextGrowthTier;
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			rect3.yMin += 2f;
			rect3.yMax -= 2f;
			Widgets.FillableBar(rect3, percentToNextGrowthTier, Gizmo_GrowthTier.BarTex, Gizmo_GrowthTier.EmptyBarTex, true);
			Text.Anchor = TextAnchor.MiddleCenter;
			float pointsRequirement = GrowthUtility.GrowthTiers[GrowthUtility.GrowthTiers.Length - 1].pointsRequirement;
			string label2 = this.child.ageTracker.AtMaxGrowthTier ? (pointsRequirement.ToString() + " / " + pointsRequirement.ToString()) : (Mathf.FloorToInt(this.child.ageTracker.growthPoints).ToString() + " / " + GrowthUtility.GrowthTiers[growthTier + 1].pointsRequirement.ToString());
			Widgets.Label(rect3, label2);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, new TipSignal(this.GrowthTierTooltip(growthTier), this.child.thingIDNumber ^ 837825001));
			}
		}

		// Token: 0x06013F6B RID: 81771 RVA: 0x005EC9F8 File Offset: 0x005EABF8
		private void DrawLearning(Rect rect)
		{
			if (this.child.needs.learning == null)
			{
				return;
			}
			Rect rect2 = rect;
			rect2.xMax = rect.x + rect.width * 0.55f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, NeedDefOf.Learning.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			rect3.yMin += 2f;
			rect3.yMax -= 2f;
			Widgets.FillableBar(rect3, this.child.needs.learning.CurLevelPercentage, Widgets.BarFullTexHor, Gizmo_GrowthTier.EmptyBarTex, true);
			Text.Anchor = TextAnchor.MiddleCenter;
			string label = this.child.needs.learning.CurLevelPercentage.ToStringPercent();
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, this.child.needs.learning.GetTipString());
			}
		}

		// Token: 0x0400DC6B RID: 56427
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(GenUI.FillableBar_Empty);

		// Token: 0x0400DC6C RID: 56428
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1254902f, 0.5411765f, 0f));

		// Token: 0x0400DC6D RID: 56429
		private const float Spacing = 8f;

		// Token: 0x0400DC6E RID: 56430
		private const float LabelWidthPercent = 0.55f;

		// Token: 0x0400DC6F RID: 56431
		private const float BarMarginY = 2f;

		// Token: 0x0400DC70 RID: 56432
		private const int GrowthTierTooltipId = 837825001;

		// Token: 0x0400DC71 RID: 56433
		private Pawn child;
	}
}
