using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F80 RID: 8064
	public static class OverlayDrawHandler
	{
		// Token: 0x0600B235 RID: 45621 RVA: 0x0032F39B File Offset: 0x0032D59B
		public static void DrawPowerGridOverlayThisFrame()
		{
			OverlayDrawHandler.lastPowerGridDrawFrame = Time.frameCount;
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x0600B236 RID: 45622 RVA: 0x0032F3A7 File Offset: 0x0032D5A7
		public static bool ShouldDrawPowerGrid
		{
			get
			{
				return OverlayDrawHandler.lastPowerGridDrawFrame + 1 >= Time.frameCount;
			}
		}

		// Token: 0x0600B237 RID: 45623 RVA: 0x0032F3BA File Offset: 0x0032D5BA
		public static void DrawZonesThisFrame()
		{
			OverlayDrawHandler.lastZoneDrawFrame = Time.frameCount;
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x0600B238 RID: 45624 RVA: 0x0032F3C6 File Offset: 0x0032D5C6
		public static bool ShouldDrawZones
		{
			get
			{
				return Find.PlaySettings.showZones || Time.frameCount <= OverlayDrawHandler.lastZoneDrawFrame + 1;
			}
		}

		// Token: 0x040079E0 RID: 31200
		private static int lastPowerGridDrawFrame;

		// Token: 0x040079E1 RID: 31201
		private static int lastZoneDrawFrame;
	}
}
