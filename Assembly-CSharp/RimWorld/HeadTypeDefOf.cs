using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200360B RID: 13835
	[DefOf]
	public static class HeadTypeDefOf
	{
		// Token: 0x060133F6 RID: 78838 RVA: 0x005B61E1 File Offset: 0x005B43E1
		static HeadTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HeadTypeDefOf));
		}

		// Token: 0x0400D520 RID: 54560
		public static HeadTypeDef Skull;

		// Token: 0x0400D521 RID: 54561
		public static HeadTypeDef Stump;
	}
}
