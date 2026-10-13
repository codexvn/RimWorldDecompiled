using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F70 RID: 12144
	public interface ITrader
	{
		// Token: 0x17002C3C RID: 11324
		// (get) Token: 0x060111AE RID: 70062
		TraderKindDef TraderKind { get; }

		// Token: 0x17002C3D RID: 11325
		// (get) Token: 0x060111AF RID: 70063
		IEnumerable<Thing> Goods { get; }

		// Token: 0x17002C3E RID: 11326
		// (get) Token: 0x060111B0 RID: 70064
		int RandomPriceFactorSeed { get; }

		// Token: 0x17002C3F RID: 11327
		// (get) Token: 0x060111B1 RID: 70065
		string TraderName { get; }

		// Token: 0x17002C40 RID: 11328
		// (get) Token: 0x060111B2 RID: 70066
		bool CanTradeNow { get; }

		// Token: 0x17002C41 RID: 11329
		// (get) Token: 0x060111B3 RID: 70067
		float TradePriceImprovementOffsetForPlayer { get; }

		// Token: 0x17002C42 RID: 11330
		// (get) Token: 0x060111B4 RID: 70068
		Faction Faction { get; }

		// Token: 0x17002C43 RID: 11331
		// (get) Token: 0x060111B5 RID: 70069
		TradeCurrency TradeCurrency { get; }

		// Token: 0x060111B6 RID: 70070
		IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator);

		// Token: 0x060111B7 RID: 70071
		void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator);

		// Token: 0x060111B8 RID: 70072
		void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator);
	}
}
