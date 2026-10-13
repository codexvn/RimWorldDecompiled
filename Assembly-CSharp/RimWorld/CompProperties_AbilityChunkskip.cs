using System;

namespace RimWorld
{
	// Token: 0x020021FC RID: 8700
	public class CompProperties_AbilityChunkskip : CompProperties_AbilityEffect
	{
		// Token: 0x0600BE7F RID: 48767 RVA: 0x00373BEA File Offset: 0x00371DEA
		public CompProperties_AbilityChunkskip()
		{
			this.compClass = typeof(CompAbilityEffect_Chunkskip);
		}

		// Token: 0x040082DB RID: 33499
		public int chunkCount;

		// Token: 0x040082DC RID: 33500
		public float scatterRadius;
	}
}
