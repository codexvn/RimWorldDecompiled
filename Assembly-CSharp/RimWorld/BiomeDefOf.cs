using System;

namespace RimWorld
{
	// Token: 0x020035A3 RID: 13731
	[DefOf]
	public static class BiomeDefOf
	{
		// Token: 0x06013391 RID: 78737 RVA: 0x005B5B2C File Offset: 0x005B3D2C
		static BiomeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BiomeDefOf));
		}

		// Token: 0x0400CEE9 RID: 52969
		public static BiomeDef IceSheet;

		// Token: 0x0400CEEA RID: 52970
		public static BiomeDef Tundra;

		// Token: 0x0400CEEB RID: 52971
		public static BiomeDef BorealForest;

		// Token: 0x0400CEEC RID: 52972
		public static BiomeDef TemperateForest;

		// Token: 0x0400CEED RID: 52973
		public static BiomeDef Desert;

		// Token: 0x0400CEEE RID: 52974
		public static BiomeDef SeaIce;

		// Token: 0x0400CEEF RID: 52975
		public static BiomeDef Ocean;

		// Token: 0x0400CEF0 RID: 52976
		public static BiomeDef Lake;

		// Token: 0x0400CEF1 RID: 52977
		public static BiomeDef ColdBog;

		// Token: 0x0400CEF2 RID: 52978
		public static BiomeDef TropicalRainforest;

		// Token: 0x0400CEF3 RID: 52979
		public static BiomeDef TropicalSwamp;

		// Token: 0x0400CEF4 RID: 52980
		[MayRequireAnomaly]
		public static BiomeDef Undercave;

		// Token: 0x0400CEF5 RID: 52981
		[MayRequireOdyssey]
		public static BiomeDef Scarlands;

		// Token: 0x0400CEF6 RID: 52982
		[MayRequireOdyssey]
		public static BiomeDef Space;

		// Token: 0x0400CEF7 RID: 52983
		[MayRequireOdyssey]
		public static BiomeDef Orbit;

		// Token: 0x0400CEF8 RID: 52984
		[MayRequireOdyssey]
		public static BiomeDef Glowforest;
	}
}
