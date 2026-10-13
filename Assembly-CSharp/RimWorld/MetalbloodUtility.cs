using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002320 RID: 8992
	public static class MetalbloodUtility
	{
		// Token: 0x0600C333 RID: 49971 RVA: 0x003896B8 File Offset: 0x003878B8
		public static bool HasMetalblood(this Pawn pawn)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == HediffDefOf.Metalblood)
				{
					return true;
				}
			}
			return false;
		}
	}
}
