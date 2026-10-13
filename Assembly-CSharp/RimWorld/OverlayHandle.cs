using System;

namespace RimWorld
{
	// Token: 0x0200296F RID: 10607
	public struct OverlayHandle
	{
		// Token: 0x0600E8E1 RID: 59617 RVA: 0x0043CBC5 File Offset: 0x0043ADC5
		public OverlayHandle(ThingOverlaysHandle thingOverlayHandle, OverlayTypes overlayType, int handleId)
		{
			this.thingOverlayHandle = thingOverlayHandle;
			this.overlayType = overlayType;
			this.handleId = handleId;
		}

		// Token: 0x04009B1D RID: 39709
		public readonly ThingOverlaysHandle thingOverlayHandle;

		// Token: 0x04009B1E RID: 39710
		public readonly OverlayTypes overlayType;

		// Token: 0x04009B1F RID: 39711
		public int handleId;
	}
}
