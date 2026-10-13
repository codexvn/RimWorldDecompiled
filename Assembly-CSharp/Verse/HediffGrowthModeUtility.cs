using System;

namespace Verse
{
	// Token: 0x020006CD RID: 1741
	public static class HediffGrowthModeUtility
	{
		// Token: 0x06002B3A RID: 11066 RVA: 0x000E223C File Offset: 0x000E043C
		public static string GetLabel(this HediffGrowthMode m)
		{
			switch (m)
			{
			case HediffGrowthMode.Growing:
				return "HediffGrowthMode_Growing".Translate();
			case HediffGrowthMode.Stable:
				return "HediffGrowthMode_Stable".Translate();
			case HediffGrowthMode.Remission:
				return "HediffGrowthMode_Remission".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
