using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025F RID: 607
	public struct SkyColorSet
	{
		// Token: 0x06000F76 RID: 3958 RVA: 0x000491CD File Offset: 0x000473CD
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x000491EC File Offset: 0x000473EC
		public static SkyColorSet Lerp(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, B.sky, t),
				shadow = Color.Lerp(A.shadow, B.shadow, t),
				overlay = Color.Lerp(A.overlay, B.overlay, t),
				saturation = Mathf.Lerp(A.saturation, B.saturation, t)
			};
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00049268 File Offset: 0x00047468
		public static SkyColorSet LerpDarken(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, A.sky.Min(B.sky), t),
				shadow = Color.Lerp(A.shadow, A.shadow.Min(B.shadow), t),
				overlay = Color.Lerp(A.overlay, A.overlay.Min(B.overlay), t),
				saturation = Mathf.Lerp(A.saturation, Mathf.Min(A.saturation, B.saturation), t)
			};
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00049310 File Offset: 0x00047510
		public override string ToString()
		{
			string[] array = new string[9];
			array[0] = "(sky=";
			int num = 1;
			Color color = this.sky;
			array[num] = color.ToString();
			array[2] = ", shadow=";
			int num2 = 3;
			color = this.shadow;
			array[num2] = color.ToString();
			array[4] = ", overlay=";
			int num3 = 5;
			color = this.overlay;
			array[num3] = color.ToString();
			array[6] = ", sat=";
			array[7] = this.saturation.ToString();
			array[8] = ")";
			return string.Concat(array);
		}

		// Token: 0x04000B3C RID: 2876
		public Color sky;

		// Token: 0x04000B3D RID: 2877
		public Color shadow;

		// Token: 0x04000B3E RID: 2878
		public Color overlay;

		// Token: 0x04000B3F RID: 2879
		public float saturation;
	}
}
