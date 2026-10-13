using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002A1 RID: 673
	public class PocketMapProperties
	{
		// Token: 0x04000DF8 RID: 3576
		public BiomeDef biome;

		// Token: 0x04000DF9 RID: 3577
		public List<TileMutatorDef> tileMutators = new List<TileMutatorDef>();

		// Token: 0x04000DFA RID: 3578
		public float temperature;

		// Token: 0x04000DFB RID: 3579
		public bool destroyOnParentMapAbandoned = true;

		// Token: 0x04000DFC RID: 3580
		public bool preventPrisonerEscape = true;

		// Token: 0x04000DFD RID: 3581
		public bool canLaunchGravship;

		// Token: 0x04000DFE RID: 3582
		public bool canBeCleaned;
	}
}
