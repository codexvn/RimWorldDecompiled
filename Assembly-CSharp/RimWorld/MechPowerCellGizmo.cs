using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D3F RID: 11583
	[StaticConstructorOnStartup]
	public class MechPowerCellGizmo : Gizmo
	{
		// Token: 0x06010465 RID: 66661 RVA: 0x004B42BB File Offset: 0x004B24BB
		public MechPowerCellGizmo(CompMechPowerCell carrier)
		{
			this.powerCell = carrier;
		}

		// Token: 0x06010466 RID: 66662 RVA: 0x0018263D File Offset: 0x0018083D
		public override float GetWidth(float maxWidth)
		{
			return 160f;
		}

		// Token: 0x06010467 RID: 66663 RVA: 0x004B42CC File Offset: 0x004B24CC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(10f);
			Widgets.DrawWindowBackground(rect);
			string text;
			if (!this.powerCell.Props.labelOverride.NullOrEmpty())
			{
				text = this.powerCell.Props.labelOverride;
			}
			else
			{
				text = "MechPowerCell".Translate();
			}
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, Text.CalcHeight(text, rect2.width) + 8f);
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, text);
			MechPowerCellGizmo.<>c__DisplayClass7_1 CS$<>8__locals2;
			CS$<>8__locals2.barRect = new Rect(rect2.x, rect3.yMax, rect2.width, rect2.height - rect3.height);
			Widgets.FillableBar(CS$<>8__locals2.barRect, this.powerCell.PercentFull, MechPowerCellGizmo.BarTex, MechPowerCellGizmo.EmptyBarTex, true);
			for (int i = 2500; i < this.powerCell.Props.totalPowerTicks; i += 2500)
			{
				MechPowerCellGizmo.<GizmoOnGUI>g__DoBarThreshold|7_1((float)i / (float)this.powerCell.Props.totalPowerTicks, ref CS$<>8__locals2);
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(CS$<>8__locals2.barRect, Mathf.CeilToInt((float)this.powerCell.PowerTicksLeft / 2500f).ToString() + "LetterHour".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			string tooltip;
			if (!this.powerCell.Props.tooltipOverride.NullOrEmpty())
			{
				tooltip = this.powerCell.Props.tooltipOverride;
			}
			else
			{
				tooltip = "MechPowerCellTip".Translate();
			}
			TooltipHandler.TipRegion(rect2, () => tooltip, Gen.HashCombineInt(this.powerCell.GetHashCode(), 34242369));
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06010469 RID: 66665 RVA: 0x004B4508 File Offset: 0x004B2708
		[CompilerGenerated]
		internal static void <GizmoOnGUI>g__DoBarThreshold|7_1(float percent, ref MechPowerCellGizmo.<>c__DisplayClass7_1 A_1)
		{
			GUI.DrawTexture(new Rect
			{
				x = A_1.barRect.x + 3f + (A_1.barRect.width - 8f) * percent,
				y = A_1.barRect.y + A_1.barRect.height - 9f,
				width = 2f,
				height = 6f
			}, BaseContent.BlackTex);
		}

		// Token: 0x0400A9FC RID: 43516
		private CompMechPowerCell powerCell;

		// Token: 0x0400A9FD RID: 43517
		private const float Width = 160f;

		// Token: 0x0400A9FE RID: 43518
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(12, 45, 45, byte.MaxValue));

		// Token: 0x0400A9FF RID: 43519
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(GenUI.FillableBar_Empty);

		// Token: 0x0400AA00 RID: 43520
		private const int BarThresholdTickIntervals = 2500;
	}
}
