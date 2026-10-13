using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021BB RID: 8635
	[StaticConstructorOnStartup]
	public class WeatherOverlay_ToxRain : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC8A RID: 48266 RVA: 0x0036A3B0 File Offset: 0x003685B0
		public WeatherOverlay_ToxRain()
		{
			this.worldOverlayMat = WeatherOverlay_ToxRain.RainOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040081D2 RID: 33234
		private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/ToxRainOverlayWorld", -1);
	}
}
