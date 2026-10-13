using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C00 RID: 3072
	public static class Pulser
	{
		// Token: 0x06004E79 RID: 20089 RVA: 0x0018F2D7 File Offset: 0x0018D4D7
		public static float PulseBrightness(float frequency, float amplitude)
		{
			return Pulser.PulseBrightness(frequency, amplitude, Time.realtimeSinceStartup);
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x0018F2E8 File Offset: 0x0018D4E8
		public static float PulseBrightness(float frequency, float amplitude, float time)
		{
			float num = time * 6.2831855f;
			num *= frequency;
			float t = (1f - Mathf.Cos(num)) * 0.5f;
			return Mathf.Lerp(1f - amplitude, 1f, t);
		}
	}
}
