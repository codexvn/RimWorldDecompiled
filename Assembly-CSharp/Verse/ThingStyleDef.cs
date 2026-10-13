using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030B RID: 779
	public class ThingStyleDef : Def
	{
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x0005CBB7 File Offset: 0x0005ADB7
		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x0005CBBF File Offset: 0x0005ADBF
		public Texture2D UIIcon
		{
			get
			{
				return this.uiIcon;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x0005CBC7 File Offset: 0x0005ADC7
		public bool UseWornGraphicMask
		{
			get
			{
				return this.useWornGraphicMask;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x0005CBD0 File Offset: 0x0005ADD0
		public StyleCategoryDef Category
		{
			get
			{
				if (this.hasCategory == null)
				{
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						if (styleCategoryDef.thingDefStyles.Any((ThingDefStyle x) => x.StyleDef == this))
						{
							this.cachedCategory = styleCategoryDef;
							this.hasCategory = new bool?(true);
							break;
						}
					}
					if (this.cachedCategory == null)
					{
						this.hasCategory = new bool?(false);
					}
				}
				return this.cachedCategory;
			}
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0005CC6C File Offset: 0x0005AE6C
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.graphicData.shaderType == null)
					{
						this.graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					this.graphic = this.graphicData.Graphic;
					this.blueprintGraphicData = new GraphicData();
					this.blueprintGraphicData.CopyFrom(this.graphicData);
					this.blueprintGraphicData.shaderType = ShaderTypeDefOf.EdgeDetect;
					this.blueprintGraphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
					this.blueprintGraphicData.colorTwo = Color.white;
					this.blueprintGraphicData.shadowData = null;
					this.blueprintGraphicData.renderQueue = 2950;
					if (this.graphic == BaseContent.BadGraphic)
					{
						this.graphic = null;
					}
					this.ResolveUIIcon();
				});
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005CC88 File Offset: 0x0005AE88
		public Texture2D IconForIndex(int index, Rot4? rot = null)
		{
			return (Texture2D)this.graphic.ExtractInnerGraphicFor(null, new int?(index)).MatAt(rot ?? Rot4.North, null).mainTexture;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0005CCD0 File Offset: 0x0005AED0
		private void ResolveUIIcon()
		{
			if (!this.uiIconPath.NullOrEmpty())
			{
				this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				return;
			}
			if (this.graphic != null)
			{
				Material material = this.graphic.ExtractInnerGraphicFor(null, null).MatAt(Rot4.North, null);
				this.uiIcon = (Texture2D)material.mainTexture;
			}
		}

		// Token: 0x04001268 RID: 4712
		[MustTranslate]
		public string overrideLabel;

		// Token: 0x04001269 RID: 4713
		public GraphicData graphicData;

		// Token: 0x0400126A RID: 4714
		public GraphicData blueprintGraphicData;

		// Token: 0x0400126B RID: 4715
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x0400126C RID: 4716
		public float uiIconScale = 1f;

		// Token: 0x0400126D RID: 4717
		[NoTranslate]
		public string wornGraphicPath;

		// Token: 0x0400126E RID: 4718
		public Color color;

		// Token: 0x0400126F RID: 4719
		private Graphic graphic;

		// Token: 0x04001270 RID: 4720
		private Texture2D uiIcon;

		// Token: 0x04001271 RID: 4721
		private bool useWornGraphicMask;

		// Token: 0x04001272 RID: 4722
		private StyleCategoryDef cachedCategory;

		// Token: 0x04001273 RID: 4723
		private bool? hasCategory;
	}
}
