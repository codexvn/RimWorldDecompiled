using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003073 RID: 12403
	public static class PlayerPawnsDisplayOrderUtility
	{
		// Token: 0x0601169F RID: 71327 RVA: 0x00500F74 File Offset: 0x004FF174
		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter);
		}

		// Token: 0x060116A0 RID: 71328 RVA: 0x00500F81 File Offset: 0x004FF181
		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter);
		}

		// Token: 0x0400B46B RID: 46187
		private static Func<Pawn, int> displayOrderGetter = delegate(Pawn x)
		{
			if (x.playerSettings == null)
			{
				return 999999;
			}
			return x.playerSettings.displayOrder;
		};
	}
}
