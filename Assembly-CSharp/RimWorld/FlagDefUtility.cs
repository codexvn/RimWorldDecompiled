using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A33 RID: 6707
	public static class FlagDefUtility
	{
		// Token: 0x0600972A RID: 38698 RVA: 0x002C2B8A File Offset: 0x002C0D8A
		public static bool HasFlag(this ulong flags, RenderSkipFlagDef flagDef)
		{
			return (flags & flagDef) > 0UL;
		}

		// Token: 0x0600972B RID: 38699 RVA: 0x002C2B98 File Offset: 0x002C0D98
		public static void SetMaskFromIndex(Def def, ref ulong mask)
		{
			if (def.defName == "None")
			{
				mask = 0UL;
				return;
			}
			mask = 1UL << (int)def.index;
		}
	}
}
