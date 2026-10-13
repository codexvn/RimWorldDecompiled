using System;

namespace Verse
{
	// Token: 0x020001DA RID: 474
	public class CameraMapConfig_CarWithContinuousZoom : CameraMapConfig_Car
	{
		// Token: 0x06000CEE RID: 3310 RVA: 0x0003D3F7 File Offset: 0x0003B5F7
		public CameraMapConfig_CarWithContinuousZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
