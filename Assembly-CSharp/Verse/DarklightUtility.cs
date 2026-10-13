using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D8 RID: 984
	public static class DarklightUtility
	{
		// Token: 0x0600193E RID: 6462 RVA: 0x0007F924 File Offset: 0x0007DB24
		public static bool IsDarklight(Color color)
		{
			if (color.r > color.g || color.r > color.b)
			{
				return false;
			}
			float num;
			float num2;
			if (color.g > color.b)
			{
				num = color.g;
				num2 = color.b;
			}
			else
			{
				num = color.b;
				num2 = color.g;
			}
			return num != 0f && color.r <= num / 2f && num2 / num > 0.85f;
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0007F9A3 File Offset: 0x0007DBA3
		public static bool IsDarklightAt(IntVec3 position, Map map)
		{
			return position.InBounds(map) && position.Roofed(map) && map.glowGrid.PsychGlowAt(position) >= PsychGlow.Lit && DarklightUtility.IsDarklight(map.glowGrid.VisualGlowAt(position));
		}

		// Token: 0x0400164E RID: 5710
		private static FloatRange DarklightHueRange = new FloatRange(0.49f, 0.51f);

		// Token: 0x0400164F RID: 5711
		public static readonly Color DefaultDarklight = new Color32(78, 226, 229, byte.MaxValue);
	}
}
