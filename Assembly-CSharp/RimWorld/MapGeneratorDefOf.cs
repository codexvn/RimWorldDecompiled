using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035C3 RID: 13763
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x060133B1 RID: 78769 RVA: 0x005B5D4C File Offset: 0x005B3F4C
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}

		// Token: 0x0400D2A3 RID: 53923
		public static MapGeneratorDef Encounter;

		// Token: 0x0400D2A4 RID: 53924
		public static MapGeneratorDef Base_Player;

		// Token: 0x0400D2A5 RID: 53925
		public static MapGeneratorDef Base_Faction;

		// Token: 0x0400D2A6 RID: 53926
		[MayRequireAnomaly]
		public static MapGeneratorDef Undercave;

		// Token: 0x0400D2A7 RID: 53927
		[MayRequireAnomaly]
		public static MapGeneratorDef MetalHell;

		// Token: 0x0400D2A8 RID: 53928
		[MayRequireAnomaly]
		public static MapGeneratorDef Labyrinth;

		// Token: 0x0400D2A9 RID: 53929
		[MayRequireOdyssey]
		public static MapGeneratorDef SpacePocket;

		// Token: 0x0400D2AA RID: 53930
		[MayRequireOdyssey]
		public static MapGeneratorDef Space;

		// Token: 0x0400D2AB RID: 53931
		[MayRequireOdyssey]
		public static MapGeneratorDef OrbitalRelay;

		// Token: 0x0400D2AC RID: 53932
		[MayRequireOdyssey]
		public static MapGeneratorDef Mechhive;

		// Token: 0x0400D2AD RID: 53933
		[MayRequireOdyssey]
		public static MapGeneratorDef InsectLair;
	}
}
