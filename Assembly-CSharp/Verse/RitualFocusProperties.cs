using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200025E RID: 606
	public class RitualFocusProperties
	{
		// Token: 0x04000B39 RID: 2873
		public IntRange spectateDistance = new IntRange(2, 2);

		// Token: 0x04000B3A RID: 2874
		public SpectateRectSide allowedSpectateSides = SpectateRectSide.Horizontal;

		// Token: 0x04000B3B RID: 2875
		public bool consumable;
	}
}
