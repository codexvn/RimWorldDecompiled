using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F2 RID: 1010
	public static class CurvedScalerExt
	{
		// Token: 0x060019B8 RID: 6584 RVA: 0x000820BC File Offset: 0x000802BC
		public static Vector3 ScaleAtTime(this List<CurvedScaler> scalers, float ageSecs)
		{
			Vector3 vector = Vector3.one;
			foreach (CurvedScaler curvedScaler in scalers)
			{
				Rand.PushState(curvedScaler.GetHashCode());
				float randomInRange = curvedScaler.scaleTime.RandomInRange;
				float randomInRange2 = curvedScaler.scaleAmt.RandomInRange;
				float x = Mathf.Clamp01(ageSecs / randomInRange);
				vector += randomInRange2 * curvedScaler.curve.Evaluate(x) * curvedScaler.axisMask;
				Rand.PopState();
			}
			return vector;
		}
	}
}
