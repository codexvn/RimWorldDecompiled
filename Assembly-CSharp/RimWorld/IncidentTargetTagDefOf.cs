using System;

namespace RimWorld
{
	// Token: 0x020035D8 RID: 13784
	[DefOf]
	public static class IncidentTargetTagDefOf
	{
		// Token: 0x060133C6 RID: 78790 RVA: 0x005B5EB1 File Offset: 0x005B40B1
		static IncidentTargetTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTagDefOf));
		}

		// Token: 0x0400D350 RID: 54096
		public static IncidentTargetTagDef World;

		// Token: 0x0400D351 RID: 54097
		public static IncidentTargetTagDef Map_RaidBeacon;

		// Token: 0x0400D352 RID: 54098
		public static IncidentTargetTagDef Map_PlayerHome;

		// Token: 0x0400D353 RID: 54099
		public static IncidentTargetTagDef Map_Misc;
	}
}
