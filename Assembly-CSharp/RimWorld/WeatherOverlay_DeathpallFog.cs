using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021AB RID: 8619
	[StaticConstructorOnStartup]
	public class WeatherOverlay_DeathpallFog : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC5E RID: 48222 RVA: 0x003696BC File Offset: 0x003678BC
		public WeatherOverlay_DeathpallFog()
		{
			this.worldOverlayMat = WeatherOverlay_DeathpallFog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.001f;
			this.worldOverlayPanSpeed2 = 0.0005f;
			this.worldPanDir1 = new Vector2(1f, 0.2f);
			this.worldPanDir1.Normalize();
			this.worldPanDir2 = new Vector2(-1f, -0.2f);
			this.worldPanDir2.Normalize();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(new Color(1f, 1f, 1f, 0.7f));
			});
		}

		// Token: 0x040081AE RID: 33198
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/DeathpallFogOverlayWorld", -1);
	}
}
