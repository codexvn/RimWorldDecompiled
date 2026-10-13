using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002FB2 RID: 12210
	public static class TraderCaravanUtility
	{
		// Token: 0x0601132A RID: 70442 RVA: 0x004EDE94 File Offset: 0x004EC094
		public static TraderCaravanRole GetTraderCaravanRole(this Pawn p)
		{
			if (p.kindDef == PawnKindDefOf.Slave)
			{
				return TraderCaravanRole.Chattel;
			}
			if (p.kindDef.trader)
			{
				return TraderCaravanRole.Trader;
			}
			if (p.kindDef.RaceProps.packAnimal && p.inventory.innerContainer.Any)
			{
				return TraderCaravanRole.Carrier;
			}
			if (p.RaceProps.Animal)
			{
				return TraderCaravanRole.Chattel;
			}
			return TraderCaravanRole.Guard;
		}

		// Token: 0x0601132B RID: 70443 RVA: 0x004EDEF8 File Offset: 0x004EC0F8
		public static Pawn FindTrader(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].GetTraderCaravanRole() == TraderCaravanRole.Trader)
				{
					return lord.ownedPawns[i];
				}
			}
			return null;
		}

		// Token: 0x0601132C RID: 70444 RVA: 0x004EDF3D File Offset: 0x004EC13D
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
