using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B0 RID: 8624
	[StaticConstructorOnStartup]
	public class WeatherOverlay_GrayPallAsh : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC6A RID: 48234 RVA: 0x003699B4 File Offset: 0x00367BB4
		public WeatherOverlay_GrayPallAsh()
		{
			this.worldOverlayMat = WeatherOverlay_GrayPallAsh.DarkParticlesOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.003f;
			this.worldPanDir1 = new Vector2(-0.12f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.001f;
			this.worldPanDir2 = new Vector2(0.12f, -1f);
			this.worldPanDir2.Normalize();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.SetOverlayColor(new Color(1f, 1f, 1f, 0.5f));
			});
		}

		// Token: 0x040081B3 RID: 33203
		private static readonly Material DarkParticlesOverlayWorld = MatLoader.LoadMat("Weather/GraypallAshesOverlayWorld", -1);
	}
}
