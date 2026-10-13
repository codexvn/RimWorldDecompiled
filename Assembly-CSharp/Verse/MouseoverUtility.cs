using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFB RID: 3067
	public static class MouseoverUtility
	{
		// Token: 0x06004E6A RID: 20074 RVA: 0x0018EF7E File Offset: 0x0018D17E
		static MouseoverUtility()
		{
			MouseoverUtility.MakePermaCache();
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x0018EF7E File Offset: 0x0018D17E
		public static void Reset()
		{
			MouseoverUtility.MakePermaCache();
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x0018EF88 File Offset: 0x0018D188
		private static void MakePermaCache()
		{
			MouseoverUtility.glowStrings = new string[101];
			for (int i = 0; i <= 100; i++)
			{
				MouseoverUtility.glowStrings[i] = GlowGrid.PsychGlowAtGlow((float)i / 100f).GetLabel() + " (" + ((float)i / 100f).ToStringPercent() + ")";
			}
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x0018EFE3 File Offset: 0x0018D1E3
		public static string GetGlowLabelByValue(float value)
		{
			return MouseoverUtility.glowStrings[Mathf.RoundToInt(value * 100f)];
		}

		// Token: 0x040037B6 RID: 14262
		private static string[] glowStrings;
	}
}
