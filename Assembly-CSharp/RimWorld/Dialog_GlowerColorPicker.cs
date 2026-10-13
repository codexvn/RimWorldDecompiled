using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003182 RID: 12674
	public class Dialog_GlowerColorPicker : Dialog_ColorPickerBase
	{
		// Token: 0x17002E4E RID: 11854
		// (get) Token: 0x06011D41 RID: 73025 RVA: 0x0052B9C4 File Offset: 0x00529BC4
		protected override Color DefaultColor
		{
			get
			{
				return this.glower.Props.glowColor.ToColor;
			}
		}

		// Token: 0x17002E4F RID: 11855
		// (get) Token: 0x06011D42 RID: 73026 RVA: 0x0052B9DB File Offset: 0x00529BDB
		protected override bool ShowDarklight { get; } = 1;

		// Token: 0x17002E50 RID: 11856
		// (get) Token: 0x06011D43 RID: 73027 RVA: 0x0052B9E3 File Offset: 0x00529BE3
		protected override List<Color> PickableColors
		{
			get
			{
				return Dialog_GlowerColorPicker.colors;
			}
		}

		// Token: 0x17002E51 RID: 11857
		// (get) Token: 0x06011D44 RID: 73028 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected override float ForcedColorValue
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17002E52 RID: 11858
		// (get) Token: 0x06011D45 RID: 73029 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ShowColorTemperatureBar
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011D46 RID: 73030 RVA: 0x0052B9EC File Offset: 0x00529BEC
		public Dialog_GlowerColorPicker(CompGlower glower, IList<CompGlower> extraGlowers, Widgets.ColorComponents visibleTextfields, Widgets.ColorComponents editableTextfields) : base(visibleTextfields, editableTextfields)
		{
			this.glower = glower;
			this.extraGlowers = new CompGlower[extraGlowers.Count];
			extraGlowers.CopyTo(this.extraGlowers, 0);
			float h;
			float s;
			float num;
			Color.RGBToHSV(glower.GlowColor.ToColor, out h, out s, out num);
			this.color = Color.HSVToRGB(h, s, 1f);
			this.oldColor = this.color;
		}

		// Token: 0x06011D47 RID: 73031 RVA: 0x0052BA68 File Offset: 0x00529C68
		protected override void SaveColor(Color color)
		{
			float hue;
			float sat;
			float num;
			Color.RGBToHSV(color, out hue, out sat, out num);
			ColorInt glowColor = this.glower.GlowColor;
			glowColor.SetHueSaturation(hue, sat);
			this.glower.GlowColor = glowColor;
			foreach (CompGlower compGlower in this.extraGlowers)
			{
				glowColor = compGlower.GlowColor;
				glowColor.SetHueSaturation(hue, sat);
				compGlower.GlowColor = glowColor;
			}
		}

		// Token: 0x0400B85F RID: 47199
		protected const float GlowValue = 1f;

		// Token: 0x0400B860 RID: 47200
		private static readonly List<Color> colors = new List<Color>
		{
			Color.HSVToRGB(0f, 0f, 1f),
			Color.HSVToRGB(0f, 0.5f, 1f),
			Color.HSVToRGB(0f, 0.33f, 1f),
			Color.HSVToRGB(0.055555556f, 1f, 1f),
			Color.HSVToRGB(0.055555556f, 0.5f, 1f),
			Color.HSVToRGB(0.055555556f, 0.33f, 1f),
			Color.HSVToRGB(0.11111111f, 1f, 1f),
			Color.HSVToRGB(0.11111111f, 0.5f, 1f),
			Color.HSVToRGB(0.11111111f, 0.33f, 1f),
			Color.HSVToRGB(0.16666667f, 1f, 1f),
			Color.HSVToRGB(0.16666667f, 0.5f, 1f),
			Color.HSVToRGB(0.16666667f, 0.33f, 1f),
			Color.HSVToRGB(0.22222222f, 1f, 1f),
			Color.HSVToRGB(0.22222222f, 0.5f, 1f),
			Color.HSVToRGB(0.22222222f, 0.33f, 1f),
			Color.HSVToRGB(0.2777778f, 1f, 1f),
			Color.HSVToRGB(0.2777778f, 0.5f, 1f),
			Color.HSVToRGB(0.2777778f, 0.33f, 1f),
			Color.HSVToRGB(0.33333334f, 1f, 1f),
			Color.HSVToRGB(0.33333334f, 0.5f, 1f),
			Color.HSVToRGB(0.33333334f, 0.33f, 1f),
			Color.HSVToRGB(0.3888889f, 1f, 1f),
			Color.HSVToRGB(0.3888889f, 0.5f, 1f),
			Color.HSVToRGB(0.3888889f, 0.33f, 1f),
			Color.HSVToRGB(0.44444445f, 1f, 1f),
			Color.HSVToRGB(0.44444445f, 0.5f, 1f),
			Color.HSVToRGB(0.44444445f, 0.33f, 1f),
			Color.HSVToRGB(0.5f, 1f, 1f),
			Color.HSVToRGB(0.5f, 0.5f, 1f),
			Color.HSVToRGB(0.5f, 0.33f, 1f),
			Color.HSVToRGB(0.5555556f, 1f, 1f),
			Color.HSVToRGB(0.5555556f, 0.5f, 1f),
			Color.HSVToRGB(0.5555556f, 0.33f, 1f),
			Color.HSVToRGB(0.6111111f, 1f, 1f),
			Color.HSVToRGB(0.6111111f, 0.5f, 1f),
			Color.HSVToRGB(0.6111111f, 0.33f, 1f),
			Color.HSVToRGB(0.6666667f, 1f, 1f),
			Color.HSVToRGB(0.6666667f, 0.5f, 1f),
			Color.HSVToRGB(0.6666667f, 0.33f, 1f),
			Color.HSVToRGB(0.7222222f, 1f, 1f),
			Color.HSVToRGB(0.7222222f, 0.5f, 1f),
			Color.HSVToRGB(0.7222222f, 0.33f, 1f),
			Color.HSVToRGB(0.7777778f, 1f, 1f),
			Color.HSVToRGB(0.7777778f, 0.5f, 1f),
			Color.HSVToRGB(0.7777778f, 0.33f, 1f),
			Color.HSVToRGB(0.8333333f, 1f, 1f),
			Color.HSVToRGB(0.8333333f, 0.5f, 1f),
			Color.HSVToRGB(0.8333333f, 0.33f, 1f),
			Color.HSVToRGB(0.8888889f, 1f, 1f),
			Color.HSVToRGB(0.8888889f, 0.5f, 1f),
			Color.HSVToRGB(0.8888889f, 0.33f, 1f),
			Color.HSVToRGB(0.9444444f, 1f, 1f),
			Color.HSVToRGB(0.9444444f, 0.5f, 1f),
			Color.HSVToRGB(0.9444444f, 0.33f, 1f)
		};

		// Token: 0x0400B861 RID: 47201
		private CompGlower glower;

		// Token: 0x0400B862 RID: 47202
		private CompGlower[] extraGlowers;
	}
}
