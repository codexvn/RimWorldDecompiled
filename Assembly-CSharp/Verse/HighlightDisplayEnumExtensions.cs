using System;

namespace Verse
{
	// Token: 0x02000D4B RID: 3403
	public static class HighlightDisplayEnumExtensions
	{
		// Token: 0x060056DC RID: 22236 RVA: 0x001BED28 File Offset: 0x001BCF28
		public static string ToStringHuman(this DotHighlightDisplayMode mode)
		{
			switch (mode)
			{
			case DotHighlightDisplayMode.None:
				return "None".Translate();
			case DotHighlightDisplayMode.HighlightHostiles:
				return "DotHighlightDisplayMode_HighlightHostiles".Translate();
			case DotHighlightDisplayMode.HighlightAll:
				return "DotHighlightDisplayMode_HighlightAll".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060056DD RID: 22237 RVA: 0x001BED7E File Offset: 0x001BCF7E
		public static string ToStringHuman(this HighlightStyleMode mode)
		{
			if (mode == HighlightStyleMode.Dots)
			{
				return "HighlightStyleMode_Dots".Translate();
			}
			if (mode != HighlightStyleMode.Silhouettes)
			{
				throw new NotImplementedException();
			}
			return "HighlightStyleMode_Silhouettes".Translate();
		}
	}
}
