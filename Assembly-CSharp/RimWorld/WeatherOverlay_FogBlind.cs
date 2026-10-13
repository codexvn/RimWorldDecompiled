using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021AE RID: 8622
	[StaticConstructorOnStartup]
	public class WeatherOverlay_FogBlind : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC65 RID: 48229 RVA: 0x0036988C File Offset: 0x00367A8C
		public WeatherOverlay_FogBlind()
		{
			this.worldOverlayMat = WeatherOverlay_FogBlind.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 5E-05f;
			this.worldOverlayPanSpeed2 = 4E-05f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x040081B1 RID: 33201
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/BlindFogOverlayWorld", -1);
	}
}
