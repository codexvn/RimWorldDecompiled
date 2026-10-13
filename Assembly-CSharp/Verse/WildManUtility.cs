using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001C5 RID: 453
	public static class WildManUtility
	{
		// Token: 0x06000C77 RID: 3191 RVA: 0x0003AB63 File Offset: 0x00038D63
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan && !p.IsSubhuman;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0003AB7D File Offset: 0x00038D7D
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.IsAnimal || p.IsWildMan();
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0003AB8F File Offset: 0x00038D8F
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0003ABA6 File Offset: 0x00038DA6
		public static bool WildManShouldReachOutsideNow(Pawn p)
		{
			return p.IsWildMan() && !p.mindState.WildManEverReachedOutside && (!p.IsPrisoner || p.guest.Released);
		}
	}
}
