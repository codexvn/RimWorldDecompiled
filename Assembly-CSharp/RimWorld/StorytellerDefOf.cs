using System;

namespace RimWorld
{
	// Token: 0x020035A6 RID: 13734
	[DefOf]
	public static class StorytellerDefOf
	{
		// Token: 0x06013394 RID: 78740 RVA: 0x005B5B5F File Offset: 0x005B3D5F
		static StorytellerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StorytellerDefOf));
		}

		// Token: 0x0400CF04 RID: 52996
		public static StorytellerDef Cassandra;

		// Token: 0x0400CF05 RID: 52997
		public static StorytellerDef Tutor;
	}
}
