using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021A8 RID: 8616
	[StaticConstructorOnStartup]
	public class WeatherOverlay_BloodRain : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC58 RID: 48216 RVA: 0x003694C4 File Offset: 0x003676C4
		public WeatherOverlay_BloodRain()
		{
			this.worldOverlayMat = WeatherOverlay_BloodRain.BloodRainMat;
			this.worldOverlayPanSpeed1 = 0.015f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.022f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
			base.ForcedOverlayColor = new Color?(new Color(0.6f, 0f, 0f));
		}

		// Token: 0x040081AB RID: 33195
		private static readonly Material BloodRainMat = MatLoader.LoadMat("Weather/BloodRainOverlayWorld", -1);
	}
}
