using System;

namespace RimWorld
{
	// Token: 0x02003617 RID: 13847
	[DefOf]
	public static class MapMeshFlagDefOf
	{
		// Token: 0x06013402 RID: 78850 RVA: 0x005B62AD File Offset: 0x005B44AD
		static MapMeshFlagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapMeshFlagDefOf));
		}

		// Token: 0x0400D549 RID: 54601
		public static MapMeshFlagDef None;

		// Token: 0x0400D54A RID: 54602
		public static MapMeshFlagDef Things;

		// Token: 0x0400D54B RID: 54603
		public static MapMeshFlagDef FogOfWar;

		// Token: 0x0400D54C RID: 54604
		public static MapMeshFlagDef Buildings;

		// Token: 0x0400D54D RID: 54605
		public static MapMeshFlagDef GroundGlow;

		// Token: 0x0400D54E RID: 54606
		public static MapMeshFlagDef Terrain;

		// Token: 0x0400D54F RID: 54607
		public static MapMeshFlagDef Roofs;

		// Token: 0x0400D550 RID: 54608
		public static MapMeshFlagDef Snow;

		// Token: 0x0400D551 RID: 54609
		public static MapMeshFlagDef Zone;

		// Token: 0x0400D552 RID: 54610
		public static MapMeshFlagDef Plan;

		// Token: 0x0400D553 RID: 54611
		public static MapMeshFlagDef PowerGrid;

		// Token: 0x0400D554 RID: 54612
		public static MapMeshFlagDef BuildingsDamage;

		// Token: 0x0400D555 RID: 54613
		public static MapMeshFlagDef Gas;

		// Token: 0x0400D556 RID: 54614
		[MayRequireBiotech]
		public static MapMeshFlagDef Pollution;

		// Token: 0x0400D557 RID: 54615
		[MayRequireOdyssey]
		public static MapMeshFlagDef Sand;
	}
}
