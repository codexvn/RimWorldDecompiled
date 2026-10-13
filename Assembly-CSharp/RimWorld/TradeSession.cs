using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FB3 RID: 12211
	public static class TradeSession
	{
		// Token: 0x17002C8B RID: 11403
		// (get) Token: 0x0601132D RID: 70445 RVA: 0x004EDF4F File Offset: 0x004EC14F
		public static bool Active
		{
			get
			{
				return TradeSession.trader != null;
			}
		}

		// Token: 0x17002C8C RID: 11404
		// (get) Token: 0x0601132E RID: 70446 RVA: 0x004EDF59 File Offset: 0x004EC159
		public static TradeCurrency TradeCurrency
		{
			get
			{
				return TradeSession.trader.TradeCurrency;
			}
		}

		// Token: 0x0601132F RID: 70447 RVA: 0x004EDF68 File Offset: 0x004EC168
		public static void SetupWith(ITrader newTrader, Pawn newPlayerNegotiator, bool giftMode)
		{
			if (!newTrader.CanTradeNow)
			{
				Log.Warning("Called SetupWith with a trader not willing to trade now.");
			}
			TradeSession.trader = newTrader;
			TradeSession.playerNegotiator = newPlayerNegotiator;
			TradeSession.giftMode = giftMode;
			TradeSession.deal = new TradeDeal();
			if (!giftMode && TradeSession.deal.cannotSellReasons.Count > 0)
			{
				Messages.Message("MessageCannotSellItemsReason".Translate() + TradeSession.deal.cannotSellReasons.ToCommaList(true, false).CapitalizeFirst(), MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x06011330 RID: 70448 RVA: 0x004EDFED File Offset: 0x004EC1ED
		public static void Close()
		{
			TradeSession.trader = null;
		}

		// Token: 0x0400B27F RID: 45695
		public static ITrader trader;

		// Token: 0x0400B280 RID: 45696
		public static Pawn playerNegotiator;

		// Token: 0x0400B281 RID: 45697
		public static TradeDeal deal;

		// Token: 0x0400B282 RID: 45698
		public static bool giftMode;
	}
}
