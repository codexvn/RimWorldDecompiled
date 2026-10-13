using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200359B RID: 13723
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x06013389 RID: 78729 RVA: 0x005B5AA4 File Offset: 0x005B3CA4
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}

		// Token: 0x0400CE6E RID: 52846
		public static BodyDef Human;

		// Token: 0x0400CE6F RID: 52847
		public static BodyDef MechanicalCentipede;
	}
}
