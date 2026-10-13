using System;

namespace RimWorld
{
	// Token: 0x020035DE RID: 13790
	[DefOf]
	public static class WorkGiverDefOf
	{
		// Token: 0x060133CC RID: 78796 RVA: 0x005B5F17 File Offset: 0x005B4117
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}

		// Token: 0x0400D37F RID: 54143
		public static WorkGiverDef Repair;

		// Token: 0x0400D380 RID: 54144
		public static WorkGiverDef ConstructRemoveFloors;

		// Token: 0x0400D381 RID: 54145
		public static WorkGiverDef FightFires;
	}
}
