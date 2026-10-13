using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035DD RID: 13789
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x060133CB RID: 78795 RVA: 0x005B5F06 File Offset: 0x005B4106
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x0400D378 RID: 54136
		public static TerrainAffordanceDef Light;

		// Token: 0x0400D379 RID: 54137
		public static TerrainAffordanceDef Medium;

		// Token: 0x0400D37A RID: 54138
		public static TerrainAffordanceDef Heavy;

		// Token: 0x0400D37B RID: 54139
		public static TerrainAffordanceDef SmoothableStone;

		// Token: 0x0400D37C RID: 54140
		public static TerrainAffordanceDef MovingFluid;

		// Token: 0x0400D37D RID: 54141
		public static TerrainAffordanceDef Bridgeable;

		// Token: 0x0400D37E RID: 54142
		public static TerrainAffordanceDef Walkable;
	}
}
