using System;

namespace RimWorld
{
	// Token: 0x020035CD RID: 13773
	[DefOf]
	public static class SlaveInteractionModeDefOf
	{
		// Token: 0x060133BB RID: 78779 RVA: 0x005B5DF6 File Offset: 0x005B3FF6
		static SlaveInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SlaveInteractionModeDefOf));
		}

		// Token: 0x0400D312 RID: 54034
		[MayRequireIdeology]
		public static SlaveInteractionModeDef NoInteraction;

		// Token: 0x0400D313 RID: 54035
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Imprison;

		// Token: 0x0400D314 RID: 54036
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Suppress;

		// Token: 0x0400D315 RID: 54037
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Emancipate;

		// Token: 0x0400D316 RID: 54038
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Execute;
	}
}
