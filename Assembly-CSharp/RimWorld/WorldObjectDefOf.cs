using System;

namespace RimWorld
{
	// Token: 0x020035C2 RID: 13762
	[DefOf]
	public static class WorldObjectDefOf
	{
		// Token: 0x060133B0 RID: 78768 RVA: 0x005B5D3B File Offset: 0x005B3F3B
		static WorldObjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorldObjectDefOf));
		}

		// Token: 0x0400D28B RID: 53899
		public static WorldObjectDef Caravan;

		// Token: 0x0400D28C RID: 53900
		public static WorldObjectDef Settlement;

		// Token: 0x0400D28D RID: 53901
		public static WorldObjectDef AbandonedSettlement;

		// Token: 0x0400D28E RID: 53902
		public static WorldObjectDef AbandonedCamp;

		// Token: 0x0400D28F RID: 53903
		public static WorldObjectDef EscapeShip;

		// Token: 0x0400D290 RID: 53904
		public static WorldObjectDef Ambush;

		// Token: 0x0400D291 RID: 53905
		public static WorldObjectDef DestroyedSettlement;

		// Token: 0x0400D292 RID: 53906
		public static WorldObjectDef AttackedNonPlayerCaravan;

		// Token: 0x0400D293 RID: 53907
		public static WorldObjectDef TravellingTransporters;

		// Token: 0x0400D294 RID: 53908
		public static WorldObjectDef RoutePlannerWaypoint;

		// Token: 0x0400D295 RID: 53909
		public static WorldObjectDef Site;

		// Token: 0x0400D296 RID: 53910
		public static WorldObjectDef PocketMap;

		// Token: 0x0400D297 RID: 53911
		public static WorldObjectDef Camp;

		// Token: 0x0400D298 RID: 53912
		[MayRequireRoyalty]
		public static WorldObjectDef TravelingShuttle;

		// Token: 0x0400D299 RID: 53913
		public static WorldObjectDef Debug_Arena;

		// Token: 0x0400D29A RID: 53914
		[MayRequireIdeology]
		public static WorldObjectDef Settlement_SecondArchonexusCycle;

		// Token: 0x0400D29B RID: 53915
		[MayRequireIdeology]
		public static WorldObjectDef Settlement_ThirdArchonexusCycle;

		// Token: 0x0400D29C RID: 53916
		[MayRequireIdeology]
		public static WorldObjectDef AbandonedArchotechStructures;

		// Token: 0x0400D29D RID: 53917
		[MayRequireOdyssey]
		public static WorldObjectDef Gravship;

		// Token: 0x0400D29E RID: 53918
		[MayRequireOdyssey]
		public static WorldObjectDef ClaimableSite;

		// Token: 0x0400D29F RID: 53919
		[MayRequireOdyssey]
		public static WorldObjectDef ClaimableSpaceSite;

		// Token: 0x0400D2A0 RID: 53920
		[MayRequireOdyssey]
		public static WorldObjectDef Mechhive;

		// Token: 0x0400D2A1 RID: 53921
		[MayRequireOdyssey]
		public static WorldObjectDef GravshipLaunch;

		// Token: 0x0400D2A2 RID: 53922
		[MayRequireOdyssey]
		public static WorldObjectDef AbandonedLandmark;
	}
}
