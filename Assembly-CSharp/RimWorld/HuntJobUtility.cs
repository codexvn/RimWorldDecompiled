using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001294 RID: 4756
	public static class HuntJobUtility
	{
		// Token: 0x0600747B RID: 29819 RVA: 0x0023B100 File Offset: 0x00239300
		public static bool WasKilledByHunter(Pawn pawn, DamageInfo? dinfo)
		{
			if (dinfo == null)
			{
				return false;
			}
			Pawn pawn2 = dinfo.Value.Instigator as Pawn;
			if (pawn2 == null || pawn2.CurJob == null)
			{
				return false;
			}
			JobDriver_Hunt jobDriver_Hunt = pawn2.jobs.curDriver as JobDriver_Hunt;
			return jobDriver_Hunt != null && jobDriver_Hunt.Victim == pawn;
		}
	}
}
