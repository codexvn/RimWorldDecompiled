using System;

namespace RimWorld.Planet
{
	// Token: 0x02003C63 RID: 15459
	public class WorldCameraConfig_ContinuousPanAndZoom : WorldCameraConfig_ContinuousPan
	{
		// Token: 0x06014FEB RID: 85995 RVA: 0x00648FF4 File Offset: 0x006471F4
		public WorldCameraConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.03f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
