using System;

namespace Verse
{
	// Token: 0x020006D4 RID: 1748
	public class HediffCompProperties_Invisibility : HediffCompProperties
	{
		// Token: 0x06002B5B RID: 11099 RVA: 0x000E2BF5 File Offset: 0x000E0DF5
		public HediffCompProperties_Invisibility()
		{
			this.compClass = typeof(HediffComp_Invisibility);
		}

		// Token: 0x040022F5 RID: 8949
		public bool visibleToPlayer;

		// Token: 0x040022F6 RID: 8950
		public int fadeDurationTicks;

		// Token: 0x040022F7 RID: 8951
		public int recoverFromDisruptedTicks;

		// Token: 0x040022F8 RID: 8952
		public bool affectedByDisruptor = true;
	}
}
