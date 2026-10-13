using System;

namespace RimWorld
{
	// Token: 0x0200358E RID: 13710
	[DefOf]
	public static class ScenPartDefOf
	{
		// Token: 0x0601337C RID: 78716 RVA: 0x005B59C7 File Offset: 0x005B3BC7
		static ScenPartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenPartDefOf));
		}

		// Token: 0x0400CBE3 RID: 52195
		public static ScenPartDef PlayerFaction;

		// Token: 0x0400CBE4 RID: 52196
		public static ScenPartDef ConfigPage_ConfigureStartingPawns;

		// Token: 0x0400CBE5 RID: 52197
		public static ScenPartDef PlayerPawnsArriveMethod;

		// Token: 0x0400CBE6 RID: 52198
		public static ScenPartDef PlanetLayerFixed;
	}
}
