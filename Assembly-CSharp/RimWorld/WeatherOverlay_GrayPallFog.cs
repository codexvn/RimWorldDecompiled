using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021AF RID: 8623
	[StaticConstructorOnStartup]
	public class WeatherOverlay_GrayPallFog : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC67 RID: 48231 RVA: 0x003698FC File Offset: 0x00367AFC
		public WeatherOverlay_GrayPallFog()
		{
			this.worldOverlayMat = WeatherOverlay_GrayPallFog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.001f;
			this.worldOverlayPanSpeed2 = 0.0005f;
			this.worldPanDir1 = new Vector2(1f, 0.2f);
			this.worldPanDir1.Normalize();
			this.worldPanDir2 = new Vector2(-1f, -0.2f);
			this.worldPanDir2.Normalize();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(new Color(1f, 1f, 1f, 1f));
			});
		}

		// Token: 0x040081B2 RID: 33202
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/UnnaturalFogOverlayWorld", -1);
	}
}
