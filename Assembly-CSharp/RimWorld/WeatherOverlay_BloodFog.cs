using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021A9 RID: 8617
	[StaticConstructorOnStartup]
	public class WeatherOverlay_BloodFog : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC5A RID: 48218 RVA: 0x0036956C File Offset: 0x0036776C
		public WeatherOverlay_BloodFog()
		{
			this.worldOverlayMat = WeatherOverlay_BloodFog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.0005f;
			this.worldOverlayPanSpeed2 = 0.0004f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir1.Normalize();
			this.worldPanDir2 = new Vector2(0.5f, -0.1f);
			this.worldPanDir2.Normalize();
			base.ForcedOverlayColor = new Color?(new Color(0.6f, 0f, 0f));
		}

		// Token: 0x040081AC RID: 33196
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/BloodFogOverlayWorld", -1);
	}
}
