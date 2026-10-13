using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DE RID: 990
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x06001953 RID: 6483 RVA: 0x0007FD8C File Offset: 0x0007DF8C
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0007FE8C File Offset: 0x0007E08C
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0007FEF6 File Offset: 0x0007E0F6
		public static Material Mat(int ind, bool transparent)
		{
			if (ind >= 100)
			{
				ind = 99;
			}
			if (ind < 0)
			{
				ind = 0;
			}
			if (!transparent)
			{
				return DebugMatsSpectrum.spectrumMatsOpaque[ind];
			}
			return DebugMatsSpectrum.spectrumMatsTranparent[ind];
		}

		// Token: 0x0400165F RID: 5727
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x04001660 RID: 5728
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x04001661 RID: 5729
		public const int MaterialCount = 100;

		// Token: 0x04001662 RID: 5730
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};
	}
}
