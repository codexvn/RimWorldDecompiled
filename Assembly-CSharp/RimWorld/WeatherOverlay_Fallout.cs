using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B7 RID: 8631
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fallout : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC7F RID: 48255 RVA: 0x0036A0BC File Offset: 0x003682BC
		public WeatherOverlay_Fallout()
		{
			this.worldOverlayMat = WeatherOverlay_Fallout.FalloutOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0008f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.0012f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040081C8 RID: 33224
		private static readonly Material FalloutOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
