using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B1 RID: 8625
	[StaticConstructorOnStartup]
	public class WeatherOverlay_NoxiousHaze : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC6D RID: 48237 RVA: 0x00369A6C File Offset: 0x00367C6C
		public WeatherOverlay_NoxiousHaze()
		{
			this.worldOverlayMat = WeatherOverlay_NoxiousHaze.NoxiusHazeOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0004f;
			this.worldOverlayPanSpeed2 = 0.0003f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x040081B4 RID: 33204
		private static readonly Material NoxiusHazeOverlayWorld = MatLoader.LoadMat("Weather/NoxiousHazeOverlayWorld", -1);
	}
}
