using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003578 RID: 13688
	public static class DeathRefusalUtility
	{
		// Token: 0x06013363 RID: 78691 RVA: 0x005B556C File Offset: 0x005B376C
		public static bool PlayerHasCorpseWithDeathRefusal()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
				for (int j = 0; j < list.Count; j++)
				{
					Corpse corpse = list[j] as Corpse;
					if (corpse != null && DeathRefusalUtility.HasPlayerControlledDeathRefusal(corpse.InnerPawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06013364 RID: 78692 RVA: 0x005B55D8 File Offset: 0x005B37D8
		public static bool HasPlayerControlledDeathRefusal(Pawn pawn)
		{
			Hediff_DeathRefusal firstHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			return firstHediff != null && firstHediff.PlayerControlled;
		}
	}
}
