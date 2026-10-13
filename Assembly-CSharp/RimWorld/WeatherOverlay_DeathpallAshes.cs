using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021AC RID: 8620
	[StaticConstructorOnStartup]
	public class WeatherOverlay_DeathpallAshes : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC61 RID: 48225 RVA: 0x00369774 File Offset: 0x00367974
		public WeatherOverlay_DeathpallAshes()
		{
			this.worldOverlayMat = WeatherOverlay_DeathpallAshes.AshesMat;
			this.worldOverlayPanSpeed1 = 0.003f;
			this.worldPanDir1 = new Vector2(-0.12f, -1f);
			this.worldPanDir1.Normalize();
			this.worldOverlayPanSpeed2 = 0.001f;
			this.worldPanDir2 = new Vector2(0.12f, -1f);
			this.worldPanDir2.Normalize();
			base.ForcedOverlayColor = new Color?(new Color(0.5f, 0.5f, 0.5f));
		}

		// Token: 0x040081AF RID: 33199
		private static readonly Material AshesMat = MatLoader.LoadMat("Weather/DeathpallAshesOverlayWorld", -1);
	}
}
