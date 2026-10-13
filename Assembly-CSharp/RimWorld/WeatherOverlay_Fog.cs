using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021AD RID: 8621
	[StaticConstructorOnStartup]
	public class WeatherOverlay_Fog : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC63 RID: 48227 RVA: 0x0036981C File Offset: 0x00367A1C
		public WeatherOverlay_Fog()
		{
			this.worldOverlayMat = WeatherOverlay_Fog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0005f;
			this.worldOverlayPanSpeed2 = 0.0004f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, -1f);
		}

		// Token: 0x040081B0 RID: 33200
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/FogOverlayWorld", -1);
	}
}
