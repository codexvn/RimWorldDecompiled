using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021AA RID: 8618
	[StaticConstructorOnStartup]
	public class WeatherOverlay_DeepBloodFog : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC5C RID: 48220 RVA: 0x00369614 File Offset: 0x00367814
		public WeatherOverlay_DeepBloodFog()
		{
			this.worldOverlayMat = WeatherOverlay_DeepBloodFog.FogOverlayWorld;
			this.worldOverlayPanSpeed1 = 0.001f;
			this.worldOverlayPanSpeed2 = 0.002f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir1.Normalize();
			this.worldPanDir2 = new Vector2(0.5f, -0.1f);
			this.worldPanDir2.Normalize();
			base.ForcedOverlayColor = new Color?(new Color(0.6f, 0f, 0f));
		}

		// Token: 0x040081AD RID: 33197
		private static readonly Material FogOverlayWorld = MatLoader.LoadMat("Weather/DeepBloodFogOverlayWorld", -1);
	}
}
