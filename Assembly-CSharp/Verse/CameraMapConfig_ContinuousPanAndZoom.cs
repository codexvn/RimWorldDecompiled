using System;

namespace Verse
{
	// Token: 0x020001D8 RID: 472
	public class CameraMapConfig_ContinuousPanAndZoom : CameraMapConfig_ContinuousPan
	{
		// Token: 0x06000CEB RID: 3307 RVA: 0x0003D2CD File Offset: 0x0003B4CD
		public CameraMapConfig_ContinuousPanAndZoom()
		{
			this.zoomSpeed = 0.043f;
			this.zoomPreserveFactor = 1f;
			this.smoothZoom = true;
		}
	}
}
