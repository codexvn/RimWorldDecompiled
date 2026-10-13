using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DF RID: 991
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x06001956 RID: 6486 RVA: 0x0007FF1C File Offset: 0x0007E11C
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0007FF68 File Offset: 0x0007E168
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04001663 RID: 5731
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04001664 RID: 5732
		public const int MaterialCount = 100;

		// Token: 0x04001665 RID: 5733
		private const float Opacity = 0.25f;
	}
}
