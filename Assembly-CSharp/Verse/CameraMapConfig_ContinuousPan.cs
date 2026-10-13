using System;

namespace Verse
{
	// Token: 0x020001D7 RID: 471
	public class CameraMapConfig_ContinuousPan : CameraMapConfig
	{
		// Token: 0x06000CEA RID: 3306 RVA: 0x0003D299 File Offset: 0x0003B499
		public CameraMapConfig_ContinuousPan()
		{
			this.dollyRateKeys = 10f;
			this.dollyRateScreenEdge = 5f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}
	}
}
