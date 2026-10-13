using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B6 RID: 8630
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowGentle : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC7D RID: 48253 RVA: 0x0036A034 File Offset: 0x00368234
		public WeatherOverlay_SnowGentle()
		{
			this.worldOverlayMat = WeatherOverlay_SnowGentle.SnowGentleOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.002f;
			this.worldPanDir1 = new Vector2(-0.25f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.003f;
			this.worldPanDir2 = new Vector2(-0.24f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040081C7 RID: 33223
		private static readonly Material SnowGentleOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
