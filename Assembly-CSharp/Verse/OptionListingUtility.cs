using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFE RID: 3070
	public static class OptionListingUtility
	{
		// Token: 0x06004E75 RID: 20085 RVA: 0x0018F208 File Offset: 0x0018D408
		public static float DrawOptionListing(Rect rect, List<ListableOption> optList)
		{
			float num = 0f;
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			foreach (ListableOption listableOption in optList)
			{
				num += listableOption.DrawOption(new Vector2(0f, num), rect.width) + 7f;
			}
			Widgets.EndGroup();
			return num;
		}

		// Token: 0x040037BE RID: 14270
		private const float OptionSpacing = 7f;
	}
}
