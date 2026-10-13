using System;

namespace RimWorld
{
	// Token: 0x02003620 RID: 13856
	[DefOf]
	public static class GravshipComponentTypeDefOf
	{
		// Token: 0x0601340B RID: 78859 RVA: 0x005B6346 File Offset: 0x005B4546
		static GravshipComponentTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GravshipComponentTypeDefOf));
		}

		// Token: 0x0400D58D RID: 54669
		[MayRequireOdyssey]
		public static GravshipComponentTypeDef SignalJammer;
	}
}
