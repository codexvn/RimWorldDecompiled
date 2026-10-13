using System;

namespace RimWorld.Planet
{
	// Token: 0x02003C65 RID: 15461
	public class WorldCameraConfig_CarWithContinuousZoom : WorldCameraConfig_Car
	{
		// Token: 0x06014FEE RID: 85998 RVA: 0x0064914E File Offset: 0x0064734E
		public WorldCameraConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
