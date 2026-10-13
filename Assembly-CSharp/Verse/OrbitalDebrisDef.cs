using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002AD RID: 685
	public class OrbitalDebrisDef : Def
	{
		// Token: 0x04000E56 RID: 3670
		public List<OrbitalDebrisDef.DebrisGraphic> graphics = new List<OrbitalDebrisDef.DebrisGraphic>();

		// Token: 0x020002AE RID: 686
		public class DebrisGraphic
		{
			// Token: 0x04000E57 RID: 3671
			public GraphicData graphicData;

			// Token: 0x04000E58 RID: 3672
			public float weighting = 1f;

			// Token: 0x04000E59 RID: 3673
			public int order;

			// Token: 0x04000E5A RID: 3674
			public float parallaxPer10Cells = 1f;
		}
	}
}
