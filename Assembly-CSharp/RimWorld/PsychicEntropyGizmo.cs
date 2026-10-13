using System;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020024CE RID: 9422
	[StaticConstructorOnStartup]
	public class PsychicEntropyGizmo : Gizmo
	{
		// Token: 0x0600CD63 RID: 52579 RVA: 0x003C3810 File Offset: 0x003C1A10
		public PsychicEntropyGizmo(Pawn_PsychicEntropyTracker tracker)
		{
			this.tracker = tracker;
			this.Order = -100f;
			this.targetValue = tracker.TargetPsyfocus;
			this.LimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Limited", true);
			this.UnlimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Unlimited", true);
		}

		// Token: 0x0600CD64 RID: 52580 RVA: 0x003C3870 File Offset: 0x003C1A70
		private void DrawThreshold(Rect rect, float percent, float entropyValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (entropyValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x0600CD65 RID: 52581 RVA: 0x003C38FC File Offset: 0x003C1AFC
		private void DrawPsyfocusTarget(Rect rect, float percent)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect
			{
				x = rect.x + 3f + num,
				y = rect.y,
				width = 2f,
				height = rect.height
			}, PsychicEntropyGizmo.PsyfocusTargetTex);
			float num2 = UIScaling.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = UIScaling.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, PsychicEntropyGizmo.PsyfocusTargetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, PsychicEntropyGizmo.PsyfocusTargetTex);
		}

		// Token: 0x0600CD66 RID: 52582 RVA: 0x003C39F8 File Offset: 0x003C1BF8
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			Command_Psycast command_Psycast = MapGizmoUtility.LastMouseOverGizmo as Command_Psycast;
			float num = Mathf.Repeat(Time.time, 0.85f);
			float num2 = 1f;
			if (num < 0.1f)
			{
				num2 = num / 0.1f;
			}
			else if (num >= 0.25f)
			{
				num2 = 1f - (num - 0.25f) / 0.6f;
			}
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.y += 6f;
			rect3.height = Text.LineHeight;
			Widgets.Label(rect3, "PsychicEntropyShort".Translate());
			Rect rect4 = rect2;
			rect4.y += 38f;
			rect4.height = Text.LineHeight;
			Widgets.Label(rect4, "PsyfocusLabelGizmo".Translate());
			Rect rect5 = rect2;
			rect5.x += 63f;
			rect5.y += 6f;
			rect5.width = 100f;
			rect5.height = 22f;
			float entropyRelativeValue = this.tracker.EntropyRelativeValue;
			Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue, 1f), PsychicEntropyGizmo.EntropyBarTex, PsychicEntropyGizmo.EmptyBarTex, true);
			if (this.tracker.EntropyValue > this.tracker.MaxEntropy)
			{
				Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue - 1f, 1f), PsychicEntropyGizmo.OverLimitBarTex, PsychicEntropyGizmo.EntropyBarTex, true);
			}
			if (command_Psycast != null)
			{
				Ability ability = command_Psycast.Ability;
				if (ability.def.EntropyGain > 1E-45f)
				{
					Rect rect6 = rect5.ContractedBy(3f);
					float width = rect6.width;
					float num3 = this.tracker.EntropyToRelativeValue(this.tracker.EntropyValue + ability.def.EntropyGain);
					float num4 = entropyRelativeValue;
					if (num4 > 1f)
					{
						num4 -= 1f;
						num3 -= 1f;
					}
					rect6.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect6.xMin + num4 * width);
					rect6.width = UIScaling.AdjustCoordToUIScalingFloor(Mathf.Max(Mathf.Min(num3, 1f) - num4, 0f) * width);
					GUI.color = new Color(1f, 1f, 1f, num2 * 0.7f);
					GenUI.DrawTextureWithMaterial(rect6, PsychicEntropyGizmo.EntropyBarTexAdd, null, default(Rect));
					GUI.color = Color.white;
				}
			}
			if (this.tracker.EntropyValue > this.tracker.MaxEntropy)
			{
				foreach (KeyValuePair<PsychicEntropySeverity, float> keyValuePair in Pawn_PsychicEntropyTracker.EntropyThresholds)
				{
					if (keyValuePair.Value > 1f && keyValuePair.Value < 2f)
					{
						this.DrawThreshold(rect5, keyValuePair.Value - 1f, entropyRelativeValue);
					}
				}
			}
			string label = this.tracker.EntropyValue.ToString("F0") + " / " + this.tracker.MaxEntropy.ToString("F0");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect5, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.white;
			Rect rect7 = rect2;
			rect7.width = 175f;
			rect7.height = 38f;
			TooltipHandler.TipRegion(rect7, delegate()
			{
				float f = this.tracker.EntropyValue / this.tracker.RecoveryRate;
				TaggedString t = ("PsychicEntropy".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor) + Mathf.Round(this.tracker.EntropyValue).ToString() + " / " + Mathf.Round(this.tracker.MaxEntropy).ToString();
				t += "\n" + "PawnTooltipPsychicEntropyStats".Translate(this.tracker.RecoveryRate.ToString("0.#"), Mathf.Round(f));
				return (t + ("\n\n" + "PawnTooltipPsychicEntropyDesc".Translate())).Resolve();
			}, Gen.HashCombineInt(this.tracker.GetHashCode(), 133858));
			Rect rect8 = rect2;
			rect8.x += 63f;
			rect8.y += 38f;
			rect8.width = 100f;
			rect8.height = 22f;
			this.lastTargetValue = this.targetValue;
			if (this.tracker.Pawn.IsColonistPlayerControlled)
			{
				Widgets.DraggableBar(rect8, PsychicEntropyGizmo.PsyfocusBarTex, PsychicEntropyGizmo.PsyfocusBarHighlightTex, PsychicEntropyGizmo.EmptyBarTex, PsychicEntropyGizmo.PsyfocusTargetTex, ref PsychicEntropyGizmo.draggingBar, this.tracker.CurrentPsyfocus, ref this.targetValue, Pawn_PsychicEntropyTracker.PsyfocusBandPercentages, 16, 0f, 1f);
				if (this.lastTargetValue != this.targetValue)
				{
					this.tracker.SetPsyfocusTarget(this.targetValue);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MeditationDesiredPsyfocus, KnowledgeAmount.Total);
				}
			}
			else
			{
				Widgets.FillableBar(rect8, this.tracker.CurrentPsyfocus, PsychicEntropyGizmo.PsyfocusBarTex, PsychicEntropyGizmo.EmptyBarTex, true);
			}
			UIHighlighter.HighlightOpportunity(rect8, "PsyfocusBar");
			if (command_Psycast != null)
			{
				float min = command_Psycast.Ability.def.PsyfocusCostRange.min;
				if (min > 1E-45f)
				{
					Rect rect9 = rect8.ContractedBy(3f);
					float num5 = Mathf.Max(this.tracker.CurrentPsyfocus - min, 0f);
					float width2 = rect9.width;
					rect9.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect9.xMin + num5 * width2);
					rect9.width = UIScaling.AdjustCoordToUIScalingCeil((this.tracker.CurrentPsyfocus - num5) * width2);
					GUI.color = new Color(1f, 1f, 1f, num2);
					GenUI.DrawTextureWithMaterial(rect9, PsychicEntropyGizmo.PsyfocusBarTexReduce, null, default(Rect));
					GUI.color = Color.white;
				}
			}
			Rect rect10 = rect2;
			rect10.y += 38f;
			rect10.width = 175f;
			rect10.height = 38f;
			TooltipHandler.TipRegion(rect10, () => this.tracker.PsyfocusTipString(this.selectedPsyfocusTarget), Gen.HashCombineInt(this.tracker.GetHashCode(), 133873));
			if (this.tracker.Pawn.IsColonistPlayerControlled)
			{
				float num6 = 32f;
				float num7 = 4f;
				float num8 = rect2.height / 2f - num6 + num7;
				float num9 = rect2.width - num6;
				Rect rect11 = new Rect(rect2.x + num9, rect2.y + num8, num6, num6);
				if (Widgets.ButtonImage(rect11, this.tracker.limitEntropyAmount ? this.LimitedTex : this.UnlimitedTex, true, null))
				{
					this.tracker.limitEntropyAmount = !this.tracker.limitEntropyAmount;
					if (this.tracker.limitEntropyAmount)
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
				}
				TooltipHandler.TipRegionByKey(rect11, "PawnTooltipPsychicEntropyLimit");
			}
			float num10;
			if (this.TryGetPainMultiplier(this.tracker.Pawn, out num10))
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				string recoveryBonus = (num10 - 1f).ToStringPercent("F0");
				string recoveryBonus2 = recoveryBonus;
				float widthCached = recoveryBonus2.GetWidthCached();
				Rect rect12 = rect2;
				rect12.x += rect2.width - widthCached / 2f - 16f;
				rect12.y += 38f;
				rect12.width = widthCached;
				rect12.height = Text.LineHeight;
				GUI.color = PsychicEntropyGizmo.PainBoostColor;
				Widgets.Label(rect12, recoveryBonus2);
				GUI.color = Color.white;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.UpperLeft;
				TooltipHandler.TipRegion(rect12.ContractedBy(-1f), () => "PawnTooltipPsychicEntropyPainFocus".Translate(this.tracker.Pawn.health.hediffSet.PainTotal.ToStringPercent("F0"), recoveryBonus), Gen.HashCombineInt(this.tracker.GetHashCode(), 133878));
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0600CD67 RID: 52583 RVA: 0x003C41C8 File Offset: 0x003C23C8
		private bool TryGetPainMultiplier(Pawn pawn, out float painMultiplier)
		{
			List<StatPart> parts = StatDefOf.PsychicEntropyRecoveryRate.parts;
			for (int i = 0; i < parts.Count; i++)
			{
				StatPart_Pain statPart_Pain = parts[i] as StatPart_Pain;
				if (statPart_Pain != null)
				{
					painMultiplier = statPart_Pain.PainFactor(this.tracker.Pawn);
					return true;
				}
			}
			painMultiplier = 0f;
			return false;
		}

		// Token: 0x0600CD68 RID: 52584 RVA: 0x003C421E File Offset: 0x003C241E
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		// Token: 0x04008BD0 RID: 35792
		private Pawn_PsychicEntropyTracker tracker;

		// Token: 0x04008BD1 RID: 35793
		private float selectedPsyfocusTarget = -1f;

		// Token: 0x04008BD2 RID: 35794
		private static bool draggingBar;

		// Token: 0x04008BD3 RID: 35795
		private float lastTargetValue;

		// Token: 0x04008BD4 RID: 35796
		private float targetValue;

		// Token: 0x04008BD5 RID: 35797
		private Texture2D LimitedTex;

		// Token: 0x04008BD6 RID: 35798
		private Texture2D UnlimitedTex;

		// Token: 0x04008BD7 RID: 35799
		private const string LimitedIconPath = "UI/Icons/EntropyLimit/Limited";

		// Token: 0x04008BD8 RID: 35800
		private const string UnlimitedIconPath = "UI/Icons/EntropyLimit/Unlimited";

		// Token: 0x04008BD9 RID: 35801
		public const float CostPreviewFadeIn = 0.1f;

		// Token: 0x04008BDA RID: 35802
		public const float CostPreviewSolid = 0.15f;

		// Token: 0x04008BDB RID: 35803
		public const float CostPreviewFadeInSolid = 0.25f;

		// Token: 0x04008BDC RID: 35804
		public const float CostPreviewFadeOut = 0.6f;

		// Token: 0x04008BDD RID: 35805
		private static readonly Color PainBoostColor = new Color(0.2f, 0.65f, 0.35f);

		// Token: 0x04008BDE RID: 35806
		private static readonly Texture2D EntropyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.46f, 0.34f, 0.35f));

		// Token: 0x04008BDF RID: 35807
		private static readonly Texture2D EntropyBarTexAdd = SolidColorMaterials.NewSolidColorTexture(new Color(0.78f, 0.72f, 0.66f));

		// Token: 0x04008BE0 RID: 35808
		private static readonly Texture2D OverLimitBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.75f, 0.2f, 0.15f));

		// Token: 0x04008BE1 RID: 35809
		private static readonly Texture2D PsyfocusBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x04008BE2 RID: 35810
		private static readonly Texture2D PsyfocusBarTexReduce = SolidColorMaterials.NewSolidColorTexture(new Color(0.65f, 0.83f, 0.83f));

		// Token: 0x04008BE3 RID: 35811
		private static readonly Texture2D PsyfocusBarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x04008BE4 RID: 35812
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x04008BE5 RID: 35813
		private static readonly Texture2D PsyfocusTargetTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));
	}
}
