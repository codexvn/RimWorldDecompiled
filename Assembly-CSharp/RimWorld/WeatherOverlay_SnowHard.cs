using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B5 RID: 8629
	[StaticConstructorOnStartup]
	public class WeatherOverlay_SnowHard : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC7B RID: 48251 RVA: 0x00369FAC File Offset: 0x003681AC
		public WeatherOverlay_SnowHard()
		{
			this.worldOverlayMat = WeatherOverlay_SnowHard.SnowOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.008f;
			this.worldPanDir1 = new Vector2(-0.5f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.009f;
			this.worldPanDir2 = new Vector2(-0.48f, -1f);
			this.worldPanDir2.Normalize();
		}

		// Token: 0x040081C6 RID: 33222
		private static readonly Material SnowOverlayWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
