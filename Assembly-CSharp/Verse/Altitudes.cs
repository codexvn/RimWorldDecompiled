using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003A5 RID: 933
	public static class Altitudes
	{
		// Token: 0x060017DF RID: 6111 RVA: 0x000797D8 File Offset: 0x000779D8
		static Altitudes()
		{
			for (int i = 0; i < 40; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.36585367f;
			}
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00079826 File Offset: 0x00077A26
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0007982F File Offset: 0x00077A2F
		public static float AltitudeFor(this AltitudeLayer alt, float incOffset)
		{
			return alt.AltitudeFor() + incOffset * 0.03658537f;
		}

		// Token: 0x0400156C RID: 5484
		private const int NumAltitudeLayers = 40;

		// Token: 0x0400156D RID: 5485
		private static readonly float[] Alts = new float[40];

		// Token: 0x0400156E RID: 5486
		private const float LayerSpacing = 0.36585367f;

		// Token: 0x0400156F RID: 5487
		public const float AltInc = 0.03658537f;

		// Token: 0x04001570 RID: 5488
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.03658537f, 0f);
	}
}
