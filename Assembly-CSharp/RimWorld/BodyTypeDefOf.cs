using System;

namespace RimWorld
{
	// Token: 0x020035E4 RID: 13796
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x060133D2 RID: 78802 RVA: 0x005B5F7D File Offset: 0x005B417D
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDefOf));
		}

		// Token: 0x0400D3A1 RID: 54177
		public static BodyTypeDef Male;

		// Token: 0x0400D3A2 RID: 54178
		public static BodyTypeDef Female;

		// Token: 0x0400D3A3 RID: 54179
		public static BodyTypeDef Thin;

		// Token: 0x0400D3A4 RID: 54180
		public static BodyTypeDef Hulk;

		// Token: 0x0400D3A5 RID: 54181
		public static BodyTypeDef Fat;

		// Token: 0x0400D3A6 RID: 54182
		[MayRequireBiotech]
		public static BodyTypeDef Baby;

		// Token: 0x0400D3A7 RID: 54183
		[MayRequireBiotech]
		public static BodyTypeDef Child;
	}
}
