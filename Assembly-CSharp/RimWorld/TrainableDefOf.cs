using System;

namespace RimWorld
{
	// Token: 0x020035A4 RID: 13732
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x06013392 RID: 78738 RVA: 0x005B5B3D File Offset: 0x005B3D3D
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		// Token: 0x0400CEF9 RID: 52985
		public static TrainableDef Tameness;

		// Token: 0x0400CEFA RID: 52986
		public static TrainableDef Obedience;

		// Token: 0x0400CEFB RID: 52987
		public static TrainableDef Release;

		// Token: 0x0400CEFC RID: 52988
		[MayRequireOdyssey]
		public static TrainableDef AttackTarget;

		// Token: 0x0400CEFD RID: 52989
		[MayRequireOdyssey]
		public static TrainableDef Comfort;

		// Token: 0x0400CEFE RID: 52990
		[MayRequireOdyssey]
		public static TrainableDef Forage;

		// Token: 0x0400CEFF RID: 52991
		[MayRequireOdyssey]
		public static TrainableDef Dig;

		// Token: 0x0400CF00 RID: 52992
		[MayRequireOdyssey]
		public static TrainableDef EggSpew;

		// Token: 0x0400CF01 RID: 52993
		[MayRequireOdyssey]
		public static TrainableDef SludgeSpew;
	}
}
