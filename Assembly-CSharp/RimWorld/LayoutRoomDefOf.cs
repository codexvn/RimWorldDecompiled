using System;

namespace RimWorld
{
	// Token: 0x020035FF RID: 13823
	[DefOf]
	public static class LayoutRoomDefOf
	{
		// Token: 0x060133EC RID: 78828 RVA: 0x005B6137 File Offset: 0x005B4337
		static LayoutRoomDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LayoutRoomDefOf));
		}

		// Token: 0x0400D4F8 RID: 54520
		[MayRequireAnomaly]
		public static LayoutRoomDef LabyrinthObelisk;

		// Token: 0x0400D4F9 RID: 54521
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsCorridor;

		// Token: 0x0400D4FA RID: 54522
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsReactorCorridor;

		// Token: 0x0400D4FB RID: 54523
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientOrbitalCorridor;

		// Token: 0x0400D4FC RID: 54524
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsReactor;

		// Token: 0x0400D4FD RID: 54525
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsTerraformer;

		// Token: 0x0400D4FE RID: 54526
		[MayRequireOdyssey]
		public static LayoutRoomDef MechanoidEngineRoom;

		// Token: 0x0400D4FF RID: 54527
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientEngineRoom;

		// Token: 0x0400D500 RID: 54528
		[MayRequireOdyssey]
		public static LayoutRoomDef Mechhive_Stabilizer;
	}
}
