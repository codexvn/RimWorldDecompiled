using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035DA RID: 13786
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x060133C8 RID: 78792 RVA: 0x005B5ED3 File Offset: 0x005B40D3
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		// Token: 0x0400D358 RID: 54104
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x0400D359 RID: 54105
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x0400D35A RID: 54106
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x0400D35B RID: 54107
		public static ImplementOwnerTypeDef Terrain;

		// Token: 0x0400D35C RID: 54108
		public static ImplementOwnerTypeDef NativeVerb;
	}
}
