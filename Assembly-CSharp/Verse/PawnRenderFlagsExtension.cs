using System;

namespace Verse
{
	// Token: 0x020005FC RID: 1532
	public static class PawnRenderFlagsExtension
	{
		// Token: 0x06002741 RID: 10049 RVA: 0x000C57B3 File Offset: 0x000C39B3
		public static bool FlagSet(this PawnRenderFlags flags, PawnRenderFlags flag)
		{
			return (flags & flag) > PawnRenderFlags.None;
		}
	}
}
