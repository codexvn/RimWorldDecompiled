using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002400 RID: 9216
	public static class InterrogateUtility
	{
		// Token: 0x0600C7B2 RID: 51122 RVA: 0x003A3B92 File Offset: 0x003A1D92
		public static float GetChance(Pawn pawn)
		{
			return pawn.GetStatValue(StatDefOf.SocialImpact, true, -1) * 0.15f;
		}

		// Token: 0x0400883D RID: 34877
		private const float SocialFactor = 0.15f;
	}
}
