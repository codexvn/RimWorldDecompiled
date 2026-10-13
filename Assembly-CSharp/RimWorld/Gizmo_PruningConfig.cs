using System;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002EB2 RID: 11954
	[StaticConstructorOnStartup]
	public class Gizmo_PruningConfig : Gizmo
	{
		// Token: 0x17002B6B RID: 11115
		// (get) Token: 0x06010DC0 RID: 69056 RVA: 0x004DAED7 File Offset: 0x004D90D7
		private float DesiredConnectionStrength
		{
			get
			{
				if (!this.draggingBar)
				{
					return this.connection.DesiredConnectionStrength;
				}
				return this.selectedStrengthTarget;
			}
		}

		// Token: 0x17002B6C RID: 11116
		// (get) Token: 0x06010DC1 RID: 69057 RVA: 0x004DAEF3 File Offset: 0x004D90F3
		private float OverrideHeight
		{
			get
			{
				return 75f + this.ExtraHeight;
			}
		}

		// Token: 0x06010DC2 RID: 69058 RVA: 0x004DAF01 File Offset: 0x004D9101
		public Gizmo_PruningConfig(CompTreeConnection connection)
		{
			this.connection = connection;
			this.Order = -100f;
		}

		// Token: 0x06010DC3 RID: 69059 RVA: 0x003C421E File Offset: 0x003C241E
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		// Token: 0x06010DC4 RID: 69060 RVA: 0x004DAF38 File Offset: 0x004D9138
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y - this.ExtraHeight, this.GetWidth(maxWidth), this.OverrideHeight);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			float yMin = rect3.yMin;
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3.x, ref yMin, rect3.width, "ConnectionStrength".Translate(), default(TipSignal));
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect3.x, ref yMin, rect3.width, "DesiredConnectionStrength".Translate() + ": " + this.DesiredConnectionStrength.ToStringPercent(), default(TipSignal));
			Widgets.Label(rect3.x, ref yMin, rect3.width, "PruningHoursToMaintain".Translate() + ": " + this.connection.PruningHoursToMaintain(this.DesiredConnectionStrength).ToString("F1"), default(TipSignal));
			Text.Font = GameFont.Small;
			if (Mouse.IsOver(rect2) && !this.draggingBar)
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect2, () => this.GetTip(), 9493937);
			}
			this.DrawBar(rect2, yMin);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06010DC5 RID: 69061 RVA: 0x004DB0BC File Offset: 0x004D92BC
		private string GetTip()
		{
			string text = "DesiredConnectionStrengthDesc".Translate(this.connection.parent.Named("TREE"), this.connection.ConnectedPawn.Named("CONNECTEDPAWN"), this.connection.ConnectionStrengthLossPerDay.ToStringPercent().Named("FALL")).Resolve();
			string text2 = this.connection.AffectingBuildingsDescription("CurrentlyAffectedBy");
			if (!text2.NullOrEmpty())
			{
				text = text + "\n\n" + text2;
			}
			return text;
		}

		// Token: 0x06010DC6 RID: 69062 RVA: 0x004DB148 File Offset: 0x004D9348
		private void DrawThreshold(Rect rect, float percent, float strValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (strValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x06010DC7 RID: 69063 RVA: 0x004DB1D4 File Offset: 0x004D93D4
		private void DrawStrengthTarget(Rect rect, float percent)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect(rect.x + 3f + num, rect.y, 2f, rect.height), Gizmo_PruningConfig.StrengthTargetTex);
			float num2 = UIScaling.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = UIScaling.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, Gizmo_PruningConfig.StrengthTargetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, Gizmo_PruningConfig.StrengthTargetTex);
		}

		// Token: 0x06010DC8 RID: 69064 RVA: 0x004DB2B0 File Offset: 0x004D94B0
		private void DrawBar(Rect inRect, float curY)
		{
			Rect rect = inRect;
			rect.xMin += 10f;
			rect.xMax -= 10f;
			rect.yMax = inRect.yMax - 4f;
			rect.yMin = curY + 10f;
			bool flag = Mouse.IsOver(rect);
			float connectionStrength = this.connection.ConnectionStrength;
			Widgets.FillableBar(rect, connectionStrength, flag ? Gizmo_PruningConfig.StrengthHighlightTex : Gizmo_PruningConfig.StrengthTex, Gizmo_PruningConfig.EmptyBarTex, true);
			foreach (CurvePoint curvePoint in this.connection.Props.maxDryadsPerConnectionStrengthCurve.Points)
			{
				if (curvePoint.x > 0f)
				{
					this.DrawThreshold(rect, curvePoint.x, connectionStrength);
				}
			}
			float num = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - (rect.x + 3f)) / (rect.width - 8f) * 20f) / 20f, 0f, 1f);
			Event current = Event.current;
			if (current.type == EventType.MouseDown && current.button == 0 && flag)
			{
				this.selectedStrengthTarget = num;
				this.draggingBar = true;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				current.Use();
			}
			if (current.type == EventType.MouseDrag && current.button == 0 && this.draggingBar && flag)
			{
				if (Mathf.Abs(num - this.selectedStrengthTarget) > 1E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				this.selectedStrengthTarget = num;
				current.Use();
			}
			if (current.type == EventType.MouseUp && current.button == 0 && this.draggingBar)
			{
				if (this.selectedStrengthTarget >= 0f)
				{
					this.connection.DesiredConnectionStrength = this.selectedStrengthTarget;
				}
				this.selectedStrengthTarget = -1f;
				this.draggingBar = false;
				current.Use();
			}
			this.DrawStrengthTarget(rect, this.DesiredConnectionStrength);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.connection.ConnectionStrength.ToStringPercent());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x0400AF82 RID: 44930
		private CompTreeConnection connection;

		// Token: 0x0400AF83 RID: 44931
		private float selectedStrengthTarget = -1f;

		// Token: 0x0400AF84 RID: 44932
		private bool draggingBar;

		// Token: 0x0400AF85 RID: 44933
		private const float Width = 212f;

		// Token: 0x0400AF86 RID: 44934
		private static readonly Texture2D StrengthTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.Orange);

		// Token: 0x0400AF87 RID: 44935
		private static readonly Texture2D StrengthHighlightTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.LightOrange);

		// Token: 0x0400AF88 RID: 44936
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x0400AF89 RID: 44937
		private static readonly Texture2D StrengthTargetTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.DarkOrange);

		// Token: 0x0400AF8A RID: 44938
		private float ExtraHeight = Text.LineHeight * 1.5f;
	}
}
