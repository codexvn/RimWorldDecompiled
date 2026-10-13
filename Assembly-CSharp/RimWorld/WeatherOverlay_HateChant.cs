using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021B9 RID: 8633
	[StaticConstructorOnStartup]
	public class WeatherOverlay_HateChant : WeatherOverlayDualPanner
	{
		// Token: 0x0600BC83 RID: 48259 RVA: 0x0036A1C4 File Offset: 0x003683C4
		public WeatherOverlay_HateChant()
		{
			this.worldOverlayMat = WeatherOverlay_HateChant.HateChantOverlayWorld;
		}

		// Token: 0x040081CA RID: 33226
		private static readonly Material HateChantOverlayWorld = MatLoader.LoadMat("Weather/HateChantOverlayWorld", -1);
	}
}
