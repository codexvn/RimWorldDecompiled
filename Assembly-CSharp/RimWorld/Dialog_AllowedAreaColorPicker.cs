using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003110 RID: 12560
	public class Dialog_AllowedAreaColorPicker : Dialog_ColorPickerBase
	{
		// Token: 0x17002DE0 RID: 11744
		// (get) Token: 0x06011A5E RID: 72286 RVA: 0x00510AFF File Offset: 0x0050ECFF
		protected override Color DefaultColor
		{
			get
			{
				return this.oldColor;
			}
		}

		// Token: 0x17002DE1 RID: 11745
		// (get) Token: 0x06011A5F RID: 72287 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool ShowDarklight
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002DE2 RID: 11746
		// (get) Token: 0x06011A60 RID: 72288 RVA: 0x00510B07 File Offset: 0x0050ED07
		protected override List<Color> PickableColors
		{
			get
			{
				return Dialog_AllowedAreaColorPicker.colors;
			}
		}

		// Token: 0x17002DE3 RID: 11747
		// (get) Token: 0x06011A61 RID: 72289 RVA: 0x0031CA56 File Offset: 0x0031AC56
		protected override float ForcedColorValue
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17002DE4 RID: 11748
		// (get) Token: 0x06011A62 RID: 72290 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool ShowColorTemperatureBar
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06011A63 RID: 72291 RVA: 0x00510B10 File Offset: 0x0050ED10
		public Dialog_AllowedAreaColorPicker(Area_Allowed area) : base(Widgets.ColorComponents.None, Widgets.ColorComponents.None)
		{
			this.area = area;
			float h;
			float s;
			float num;
			Color.RGBToHSV(area.Color, out h, out s, out num);
			this.color = Color.HSVToRGB(h, s, 0.5f);
			this.oldColor = this.color;
		}

		// Token: 0x06011A64 RID: 72292 RVA: 0x00510B5C File Offset: 0x0050ED5C
		protected override void SaveColor(Color color)
		{
			float hue;
			float sat;
			float num;
			Color.RGBToHSV(color, out hue, out sat, out num);
			ColorInt colorInt = new ColorInt(this.area.Color);
			colorInt.SetHueSaturation(hue, sat);
			this.area.SetColor(colorInt.ToColor);
		}

		// Token: 0x0400B5B6 RID: 46518
		protected const float AreaColorValue = 0.5f;

		// Token: 0x0400B5B7 RID: 46519
		private static readonly List<Color> colors = new List<Color>
		{
			Color.HSVToRGB(0f, 0f, 0.5f),
			Color.HSVToRGB(0f, 0.5f, 0.5f),
			Color.HSVToRGB(0f, 0.33f, 0.5f),
			Color.HSVToRGB(0.055555556f, 1f, 0.5f),
			Color.HSVToRGB(0.055555556f, 0.5f, 0.5f),
			Color.HSVToRGB(0.055555556f, 0.33f, 0.5f),
			Color.HSVToRGB(0.11111111f, 1f, 0.5f),
			Color.HSVToRGB(0.11111111f, 0.5f, 0.5f),
			Color.HSVToRGB(0.11111111f, 0.33f, 0.5f),
			Color.HSVToRGB(0.16666667f, 1f, 0.5f),
			Color.HSVToRGB(0.16666667f, 0.5f, 0.5f),
			Color.HSVToRGB(0.16666667f, 0.33f, 0.5f),
			Color.HSVToRGB(0.22222222f, 1f, 0.5f),
			Color.HSVToRGB(0.22222222f, 0.5f, 0.5f),
			Color.HSVToRGB(0.22222222f, 0.33f, 0.5f),
			Color.HSVToRGB(0.2777778f, 1f, 0.5f),
			Color.HSVToRGB(0.2777778f, 0.5f, 0.5f),
			Color.HSVToRGB(0.2777778f, 0.33f, 0.5f),
			Color.HSVToRGB(0.33333334f, 1f, 0.5f),
			Color.HSVToRGB(0.33333334f, 0.5f, 0.5f),
			Color.HSVToRGB(0.33333334f, 0.33f, 0.5f),
			Color.HSVToRGB(0.3888889f, 1f, 0.5f),
			Color.HSVToRGB(0.3888889f, 0.5f, 0.5f),
			Color.HSVToRGB(0.3888889f, 0.33f, 0.5f),
			Color.HSVToRGB(0.44444445f, 1f, 0.5f),
			Color.HSVToRGB(0.44444445f, 0.5f, 0.5f),
			Color.HSVToRGB(0.44444445f, 0.33f, 0.5f),
			Color.HSVToRGB(0.5f, 1f, 0.5f),
			Color.HSVToRGB(0.5f, 0.5f, 0.5f),
			Color.HSVToRGB(0.5f, 0.33f, 0.5f),
			Color.HSVToRGB(0.5555556f, 1f, 0.5f),
			Color.HSVToRGB(0.5555556f, 0.5f, 0.5f),
			Color.HSVToRGB(0.5555556f, 0.33f, 0.5f),
			Color.HSVToRGB(0.6111111f, 1f, 0.5f),
			Color.HSVToRGB(0.6111111f, 0.5f, 0.5f),
			Color.HSVToRGB(0.6111111f, 0.33f, 0.5f),
			Color.HSVToRGB(0.6666667f, 1f, 0.5f),
			Color.HSVToRGB(0.6666667f, 0.5f, 0.5f),
			Color.HSVToRGB(0.6666667f, 0.33f, 0.5f),
			Color.HSVToRGB(0.7222222f, 1f, 0.5f),
			Color.HSVToRGB(0.7222222f, 0.5f, 0.5f),
			Color.HSVToRGB(0.7222222f, 0.33f, 0.5f),
			Color.HSVToRGB(0.7777778f, 1f, 0.5f),
			Color.HSVToRGB(0.7777778f, 0.5f, 0.5f),
			Color.HSVToRGB(0.7777778f, 0.33f, 0.5f),
			Color.HSVToRGB(0.8333333f, 1f, 0.5f),
			Color.HSVToRGB(0.8333333f, 0.5f, 0.5f),
			Color.HSVToRGB(0.8333333f, 0.33f, 0.5f),
			Color.HSVToRGB(0.8888889f, 1f, 0.5f),
			Color.HSVToRGB(0.8888889f, 0.5f, 0.5f),
			Color.HSVToRGB(0.8888889f, 0.33f, 0.5f),
			Color.HSVToRGB(0.9444444f, 1f, 0.5f),
			Color.HSVToRGB(0.9444444f, 0.5f, 0.5f),
			Color.HSVToRGB(0.9444444f, 0.33f, 0.5f)
		};

		// Token: 0x0400B5B8 RID: 46520
		private Area_Allowed area;
	}
}
