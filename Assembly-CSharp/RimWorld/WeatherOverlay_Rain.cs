using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B2 RID: 8626
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Rain : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC6F RID: 48239 RVA: 0x00369ADC File Offset: 0x00367CDC
		public WeatherOverlay_Rain()
		{
			this.worldOverlayMat = WeatherOverlay_Rain.RainOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040081B5 RID: 33205
		private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld", -1);
	}
}
