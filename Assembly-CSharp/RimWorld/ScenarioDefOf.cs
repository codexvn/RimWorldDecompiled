using System;

namespace RimWorld
{
	// Token: 0x020035A7 RID: 13735
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x06013395 RID: 78741 RVA: 0x005B5B70 File Offset: 0x005B3D70
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}

		// Token: 0x0400CF06 RID: 52998
		public static ScenarioDef Crashlanded;

		// Token: 0x0400CF07 RID: 52999
		public static ScenarioDef Tutorial;
	}
}
