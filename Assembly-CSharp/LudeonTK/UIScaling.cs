using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000B1 RID: 177
	public static class UIScaling
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x000165E0 File Offset: 0x000147E0
		public static Rect AdjustRectToUIScaling(Rect rect)
		{
			Rect result = rect;
			result.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect.xMin);
			result.yMin = UIScaling.AdjustCoordToUIScalingFloor(rect.yMin);
			result.xMax = UIScaling.AdjustCoordToUIScalingCeil(rect.xMax);
			result.yMax = UIScaling.AdjustCoordToUIScalingCeil(rect.yMax);
			return result;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001663C File Offset: 0x0001483C
		public static float AdjustCoordToUIScalingFloor(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Floor(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00016664 File Offset: 0x00014864
		public static float AdjustCoordToUIScalingCeil(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Ceiling(num)) / Prefs.UIScale;
			return coord - num2;
		}
	}
}
