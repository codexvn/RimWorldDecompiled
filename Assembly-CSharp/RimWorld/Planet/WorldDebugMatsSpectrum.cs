using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C72 RID: 15474
	[StaticConstructorOnStartup]
	public static class WorldDebugMatsSpectrum
	{
		// Token: 0x06015040 RID: 86080 RVA: 0x0064AEE4 File Offset: 0x006490E4
		static WorldDebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				WorldDebugMatsSpectrum.spectrumMats[i] = MatsFromSpectrum.Get(WorldDebugMatsSpectrum.DebugSpectrum, (float)i / 100f, ShaderDatabase.WorldOverlayTransparent);
				WorldDebugMatsSpectrum.spectrumMats[i].renderQueue = 3510;
			}
		}

		// Token: 0x06015041 RID: 86081 RVA: 0x0064AF43 File Offset: 0x00649143
		public static Material Mat(int ind)
		{
			ind = Mathf.Clamp(ind, 0, 99);
			return WorldDebugMatsSpectrum.spectrumMats[ind];
		}

		// Token: 0x0400EA86 RID: 60038
		private static readonly Material[] spectrumMats = new Material[100];

		// Token: 0x0400EA87 RID: 60039
		public const int MaterialCount = 100;

		// Token: 0x0400EA88 RID: 60040
		private const float Opacity = 0.25f;

		// Token: 0x0400EA89 RID: 60041
		private static readonly Color[] DebugSpectrum = DebugMatsSpectrum.DebugSpectrum;
	}
}
