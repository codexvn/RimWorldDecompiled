using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D31 RID: 11569
	[StaticConstructorOnStartup]
	public class MechCarrierGizmo : Gizmo
	{
		// Token: 0x0601041B RID: 66587 RVA: 0x004B3158 File Offset: 0x004B1358
		public MechCarrierGizmo(CompMechCarrier carrier)
		{
			this.carrier = carrier;
			this.targetValue = (float)carrier.maxToFill / (float)carrier.Props.maxIngredientCount;
			if (MechCarrierGizmo.bandPercentages == null)
			{
				MechCarrierGizmo.bandPercentages = new List<float>();
				int num = 12;
				for (int i = 0; i <= num; i++)
				{
					float item = 1f / (float)num * (float)i;
					MechCarrierGizmo.bandPercentages.Add(item);
				}
			}
		}

		// Token: 0x0601041C RID: 66588 RVA: 0x0018263D File Offset: 0x0018083D
		public override float GetWidth(float maxWidth)
		{
			return 160f;
		}

		// Token: 0x0601041D RID: 66589 RVA: 0x004B31C4 File Offset: 0x004B13C4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(10f);
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Small;
			TaggedString labelCap = this.carrier.Props.fixedIngredient.LabelCap;
			float height = Text.CalcHeight(labelCap, rect2.width);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, labelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			this.lastTargetValue = this.targetValue;
			float num = rect2.height - rect3.height;
			float num2 = num - 4f;
			float num3 = (num - num2) / 2f;
			Rect rect4 = new Rect(rect2.x, rect3.yMax + num3, rect2.width, num2);
			Widgets.DraggableBar(rect4, MechCarrierGizmo.BarTex, MechCarrierGizmo.BarHighlightTex, MechCarrierGizmo.EmptyBarTex, MechCarrierGizmo.DragBarTex, ref MechCarrierGizmo.draggingBar, this.carrier.PercentageFull, ref this.targetValue, MechCarrierGizmo.bandPercentages, 24, 0f, 1f);
			Text.Anchor = TextAnchor.MiddleCenter;
			rect4.y -= 2f;
			Widgets.Label(rect4, this.carrier.IngredientCount.ToString() + " / " + this.carrier.Props.maxIngredientCount.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect4, () => this.GetResourceBarTip(), Gen.HashCombineInt(this.carrier.GetHashCode(), 34242369));
			if (this.lastTargetValue != this.targetValue)
			{
				this.carrier.maxToFill = Mathf.RoundToInt(this.targetValue * (float)this.carrier.Props.maxIngredientCount);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0601041E RID: 66590 RVA: 0x004B33B0 File Offset: 0x004B15B0
		private string GetResourceBarTip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MechCarrierAutofillResources".Translate() + " " + this.carrier.Props.fixedIngredient.label + ": " + this.carrier.maxToFill.ToString());
			stringBuilder.AppendInNewLine("MechCarrierClickToSetAutofillAmount".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendInNewLine("MechCarrierAutofillDesc".Translate(this.carrier.parent.def.label, this.carrier.Props.spawnPawnKind.labelPlural));
			return stringBuilder.ToString();
		}

		// Token: 0x0400A9C5 RID: 43461
		private CompMechCarrier carrier;

		// Token: 0x0400A9C6 RID: 43462
		private const float Width = 160f;

		// Token: 0x0400A9C7 RID: 43463
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x0400A9C8 RID: 43464
		private static readonly Texture2D BarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x0400A9C9 RID: 43465
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x0400A9CA RID: 43466
		private static readonly Texture2D DragBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));

		// Token: 0x0400A9CB RID: 43467
		private const int Increments = 24;

		// Token: 0x0400A9CC RID: 43468
		private static bool draggingBar;

		// Token: 0x0400A9CD RID: 43469
		private float lastTargetValue;

		// Token: 0x0400A9CE RID: 43470
		private float targetValue;

		// Token: 0x0400A9CF RID: 43471
		private static List<float> bandPercentages;
	}
}
