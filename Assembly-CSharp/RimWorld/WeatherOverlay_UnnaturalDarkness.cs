using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B8 RID: 8632
	[StaticConstructorOnStartup]
	public class WeatherOverlay_UnnaturalDarkness : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC81 RID: 48257 RVA: 0x0036A144 File Offset: 0x00368344
		public WeatherOverlay_UnnaturalDarkness()
		{
			this.worldOverlayMat = WeatherOverlay_UnnaturalDarkness.DarkParticlesOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.004f;
			this.worldPanDir1 = new Vector2(-0.4f, -0.6f).normalized;
			this.worldOverlayPanSpeed2 = 0.005f;
			this.worldPanDir2 = new Vector2(0.2f, -0.8f).normalized;
		}

		// Token: 0x040081C9 RID: 33225
		private static readonly Material DarkParticlesOverlayWorld = MatLoader.LoadMat("Weather/DarknessOverlayWorld", -1);
	}
}
