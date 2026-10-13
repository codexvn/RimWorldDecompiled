using System;

namespace RimWorld
{
	// Token: 0x020035CB RID: 13771
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x060133B9 RID: 78777 RVA: 0x005B5DD4 File Offset: 0x005B3FD4
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}

		// Token: 0x0400D2FF RID: 54015
		public static FleshTypeDef Normal;

		// Token: 0x0400D300 RID: 54016
		public static FleshTypeDef Mechanoid;

		// Token: 0x0400D301 RID: 54017
		public static FleshTypeDef Insectoid;

		// Token: 0x0400D302 RID: 54018
		[MayRequireAnomaly]
		public static FleshTypeDef EntityMechanical;

		// Token: 0x0400D303 RID: 54019
		[MayRequireAnomaly]
		public static FleshTypeDef EntityFlesh;

		// Token: 0x0400D304 RID: 54020
		[MayRequireAnomaly]
		public static FleshTypeDef Fleshbeast;

		// Token: 0x0400D305 RID: 54021
		[MayRequireOdyssey]
		public static FleshTypeDef Drone;
	}
}
