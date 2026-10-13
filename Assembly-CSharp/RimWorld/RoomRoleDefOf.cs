using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035B6 RID: 13750
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x060133A4 RID: 78756 RVA: 0x005B5C6F File Offset: 0x005B3E6F
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}

		// Token: 0x0400D12A RID: 53546
		public static RoomRoleDef None;

		// Token: 0x0400D12B RID: 53547
		public static RoomRoleDef Bedroom;

		// Token: 0x0400D12C RID: 53548
		public static RoomRoleDef Barracks;

		// Token: 0x0400D12D RID: 53549
		public static RoomRoleDef PrisonCell;

		// Token: 0x0400D12E RID: 53550
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x0400D12F RID: 53551
		public static RoomRoleDef Hospital;

		// Token: 0x0400D130 RID: 53552
		public static RoomRoleDef Workshop;

		// Token: 0x0400D131 RID: 53553
		public static RoomRoleDef Laboratory;

		// Token: 0x0400D132 RID: 53554
		[MayRequireRoyalty]
		public static RoomRoleDef ThroneRoom;

		// Token: 0x0400D133 RID: 53555
		[MayRequireIdeology]
		public static RoomRoleDef WorshipRoom;
	}
}
