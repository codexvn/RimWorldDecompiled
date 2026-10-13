using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D32 RID: 15666
	public static class BestCaravanPawnUtility
	{
		// Token: 0x0601552B RID: 87339 RVA: 0x0065FFCD File Offset: 0x0065E1CD
		public static Pawn FindBestDiplomat(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.NegotiationAbility, null);
		}

		// Token: 0x0601552C RID: 87340 RVA: 0x0065FFDC File Offset: 0x0065E1DC
		public static Pawn FindBestNegotiator(Caravan caravan, Faction negotiatingWith = null, TraderKindDef trader = null)
		{
			Predicate<Pawn> pawnValidator = null;
			if (negotiatingWith != null)
			{
				pawnValidator = ((Pawn p) => p.CanTradeWith(negotiatingWith, trader).Accepted);
			}
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.TradePriceImprovement, pawnValidator);
		}

		// Token: 0x0601552D RID: 87341 RVA: 0x00660020 File Offset: 0x0065E220
		public static Pawn FindBestEntertainingPawnFor(Caravan caravan, Pawn forPawn)
		{
			Pawn pawn = null;
			float num = -1f;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn2 = caravan.pawns[i];
				if (pawn2 != forPawn && pawn2.RaceProps.Humanlike && !pawn2.Dead && !pawn2.Downed && !pawn2.InMentalState && pawn2.IsPrisoner == forPawn.IsPrisoner && !StatDefOf.SocialImpact.Worker.IsDisabledFor(pawn2))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.SocialImpact, true, -1);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0601552E RID: 87342 RVA: 0x006600C4 File Offset: 0x0065E2C4
		public static Pawn FindPawnWithBestStat(Caravan caravan, StatDef stat, Predicate<Pawn> pawnValidator = null)
		{
			Pawn pawn = null;
			float num = -1f;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (BestCaravanPawnUtility.IsConsciousOwner(pawn2, caravan) && !stat.Worker.IsDisabledFor(pawn2) && (pawnValidator == null || pawnValidator(pawn2)))
				{
					float statValue = pawn2.GetStatValue(stat, true, -1);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0601552F RID: 87343 RVA: 0x0066013B File Offset: 0x0065E33B
		private static bool IsConsciousOwner(Pawn pawn, Caravan caravan)
		{
			return !pawn.Dead && !pawn.Downed && !pawn.InMentalState && caravan.IsOwner(pawn);
		}
	}
}
