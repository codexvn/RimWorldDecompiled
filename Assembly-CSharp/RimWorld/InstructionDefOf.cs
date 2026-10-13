using System;

namespace RimWorld
{
	// Token: 0x020035E9 RID: 13801
	[DefOf]
	public static class InstructionDefOf
	{
		// Token: 0x060133D7 RID: 78807 RVA: 0x005B5FD2 File Offset: 0x005B41D2
		static InstructionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InstructionDefOf));
		}

		// Token: 0x0400D3BE RID: 54206
		public static InstructionDef RandomizeCharacter;

		// Token: 0x0400D3BF RID: 54207
		public static InstructionDef ChooseLandingSite;
	}
}
