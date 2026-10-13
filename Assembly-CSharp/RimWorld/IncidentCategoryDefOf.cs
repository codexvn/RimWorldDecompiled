using System;

namespace RimWorld
{
	// Token: 0x020035E1 RID: 13793
	[DefOf]
	public static class IncidentCategoryDefOf
	{
		// Token: 0x060133CF RID: 78799 RVA: 0x005B5F4A File Offset: 0x005B414A
		static IncidentCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentCategoryDefOf));
		}

		// Token: 0x0400D393 RID: 54163
		public static IncidentCategoryDef Misc;

		// Token: 0x0400D394 RID: 54164
		public static IncidentCategoryDef ThreatSmall;

		// Token: 0x0400D395 RID: 54165
		public static IncidentCategoryDef ThreatBig;

		// Token: 0x0400D396 RID: 54166
		public static IncidentCategoryDef DiseaseHuman;

		// Token: 0x0400D397 RID: 54167
		public static IncidentCategoryDef GiveQuest;

		// Token: 0x0400D398 RID: 54168
		public static IncidentCategoryDef DeepDrillInfestation;

		// Token: 0x0400D399 RID: 54169
		public static IncidentCategoryDef Special;
	}
}
