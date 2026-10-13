using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200022F RID: 559
	public class ColorGenerator_Options : ColorGenerator
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x000441F8 File Offset: 0x000423F8
		public override Color ExemplaryColor
		{
			get
			{
				ColorOption colorOption = null;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (colorOption == null || this.options[i].weight > colorOption.weight)
					{
						colorOption = this.options[i];
					}
				}
				if (colorOption == null)
				{
					return Color.white;
				}
				if (colorOption.only.a >= 0f)
				{
					return colorOption.only;
				}
				return new Color((colorOption.min.r + colorOption.max.r) / 2f, (colorOption.min.g + colorOption.max.g) / 2f, (colorOption.min.b + colorOption.max.b) / 2f, (colorOption.min.a + colorOption.max.a) / 2f);
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x000442E1 File Offset: 0x000424E1
		public override Color NewRandomizedColor()
		{
			return this.options.RandomElementByWeight((ColorOption pi) => pi.weight).RandomizedColor();
		}

		// Token: 0x0400096C RID: 2412
		public List<ColorOption> options = new List<ColorOption>();
	}
}
