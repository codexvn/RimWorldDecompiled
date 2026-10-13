using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B96 RID: 2966
	[StaticConstructorOnStartup]
	public abstract class Gizmo_Slider : Gizmo
	{
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06004BCD RID: 19405 RVA: 0x0018263D File Offset: 0x0018083D
		protected virtual float Width
		{
			get
			{
				return 160f;
			}
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00182644 File Offset: 0x00180844
		public sealed override float GetWidth(float maxWidth)
		{
			return this.Width;
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06004BCF RID: 19407
		// (set) Token: 0x06004BD0 RID: 19408
		protected abstract float Target { get; set; }

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004BD1 RID: 19409
		protected abstract float ValuePercent { get; }

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x0018264C File Offset: 0x0018084C
		protected virtual Color BarColor { get; }

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06004BD3 RID: 19411 RVA: 0x00182654 File Offset: 0x00180854
		protected virtual Color BarHighlightColor { get; }

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x0018265C File Offset: 0x0018085C
		protected virtual Color BarDragColor { get; }

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06004BD5 RID: 19413 RVA: 0x00182664 File Offset: 0x00180864
		protected virtual FloatRange DragRange { get; } = FloatRange.ZeroToOne;

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06004BD6 RID: 19414 RVA: 0x0018266C File Offset: 0x0018086C
		protected virtual bool IsDraggable { get; }

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06004BD7 RID: 19415 RVA: 0x00182674 File Offset: 0x00180874
		protected virtual string BarLabel
		{
			get
			{
				return this.ValuePercent.ToStringPercent("0");
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06004BD8 RID: 19416
		protected abstract string Title { get; }

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06004BD9 RID: 19417 RVA: 0x00182686 File Offset: 0x00180886
		protected virtual int Increments { get; } = 20;

		// Token: 0x06004BDA RID: 19418 RVA: 0x0018268E File Offset: 0x0018088E
		protected virtual IEnumerable<float> GetBarThresholds()
		{
			yield break;
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06004BDB RID: 19419 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual string HighlightTag
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06004BDC RID: 19420
		// (set) Token: 0x06004BDD RID: 19421
		protected abstract bool DraggingBar { get; set; }

		// Token: 0x06004BDE RID: 19422
		protected abstract string GetTooltip();

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06004BDF RID: 19423 RVA: 0x00182697 File Offset: 0x00180897
		// (set) Token: 0x06004BE5 RID: 19429 RVA: 0x00182ACD File Offset: 0x00180CCD
		public sealed override float Order
		{
			get
			{
				return -100f;
			}
			set
			{
				base.Order = value;
			}
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x001826A0 File Offset: 0x001808A0
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.targetValuePct = Mathf.Clamp(this.Target, this.DragRange.min, this.DragRange.max);
			this.barTex = ((this.BarColor == default(Color)) ? Gizmo_Slider.BarTex : SolidColorMaterials.NewSolidColorTexture(this.BarColor));
			this.barHighlightTex = ((this.BarHighlightColor == default(Color)) ? Gizmo_Slider.BarHighlightTex : SolidColorMaterials.NewSolidColorTexture(this.BarHighlightColor));
			this.barDragTex = ((this.BarDragColor == default(Color)) ? Gizmo_Slider.DragBarTex : SolidColorMaterials.NewSolidColorTexture(this.BarDragColor));
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x00182770 File Offset: 0x00180970
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(8f);
			Widgets.DrawWindowBackground(rect);
			bool flag = false;
			Text.Font = GameFont.Small;
			Rect headerRect = rect2;
			headerRect.height = Text.LineHeight;
			this.DrawHeader(headerRect, ref flag);
			this.barRect = rect2;
			this.barRect.yMin = headerRect.yMax + 8f;
			if (!this.IsDraggable)
			{
				Widgets.FillableBar(this.barRect, this.ValuePercent, this.barTex, Gizmo_Slider.EmptyBarTex, true);
				using (IEnumerator<float> enumerator = this.GetBarThresholds().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						float num = enumerator.Current;
						GUI.DrawTexture(new Rect
						{
							x = this.barRect.x + 3f + (this.barRect.width - 8f) * num,
							y = this.barRect.y + this.barRect.height - 9f,
							width = 2f,
							height = 6f
						}, (this.ValuePercent < num) ? BaseContent.GreyTex : BaseContent.BlackTex);
					}
					goto IL_201;
				}
			}
			bool draggingBar = this.DraggingBar;
			Widgets.DraggableBar(this.barRect, this.barTex, this.barHighlightTex, Gizmo_Slider.EmptyBarTex, this.barDragTex, ref draggingBar, this.ValuePercent, ref this.targetValuePct, this.GetBarThresholds(), this.Increments, this.DragRange.min, this.DragRange.max);
			this.DraggingBar = draggingBar;
			this.targetValuePct = Mathf.Clamp(this.targetValuePct, this.DragRange.min, this.DragRange.max);
			this.Target = this.targetValuePct;
			IL_201:
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(this.barRect, this.BarLabel);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect) && !flag)
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, new Func<string>(this.GetTooltip), Gen.HashCombineInt(this.GetHashCode(), 8573612));
			}
			if (!this.HighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.HighlightTag);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x00182A00 File Offset: 0x00180C00
		protected virtual void DrawHeader(Rect headerRect, ref bool mouseOverElement)
		{
			string text = this.Title;
			text = text.Truncate(headerRect.width, null);
			Widgets.Label(headerRect, text);
		}

		// Token: 0x04003624 RID: 13860
		private Texture2D barTex;

		// Token: 0x04003625 RID: 13861
		private Texture2D barHighlightTex;

		// Token: 0x04003626 RID: 13862
		private Texture2D barDragTex;

		// Token: 0x04003627 RID: 13863
		private float targetValuePct;

		// Token: 0x04003628 RID: 13864
		private bool initialized;

		// Token: 0x04003629 RID: 13865
		protected Rect barRect;

		// Token: 0x0400362A RID: 13866
		private const float Spacing = 8f;

		// Token: 0x0400362B RID: 13867
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x0400362C RID: 13868
		private static readonly Texture2D BarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x0400362D RID: 13869
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x0400362E RID: 13870
		private static readonly Texture2D DragBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));
	}
}
