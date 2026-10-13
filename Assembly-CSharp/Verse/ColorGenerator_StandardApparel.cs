using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200022E RID: 558
	public class ColorGenerator_StandardApparel : ColorGenerator
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x00044114 File Offset: 0x00042314
		public override Color ExemplaryColor
		{
			get
			{
				return new Color(0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0004412C File Offset: 0x0004232C
		public override Color NewRandomizedColor()
		{
			if (Rand.Value < 0.1f)
			{
				return Color.white;
			}
			if (Rand.Value < 0.1f)
			{
				return new Color(0.4f, 0.4f, 0.4f);
			}
			Color white = Color.white;
			float num = Rand.Range(0f, 0.6f);
			white.r -= num * Rand.Value;
			white.g -= num * Rand.Value;
			white.b -= num * Rand.Value;
			if (Rand.Value < 0.2f)
			{
				white.r *= 0.4f;
				white.g *= 0.4f;
				white.b *= 0.4f;
			}
			return white;
		}

		// Token: 0x0400096B RID: 2411
		private const float DarkAmp = 0.4f;
	}
}
