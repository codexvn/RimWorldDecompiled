using System;

namespace RimWorld
{
	// Token: 0x02002FB9 RID: 12217
	public static class TradeabilityUtility
	{
		// Token: 0x06011366 RID: 70502 RVA: 0x004EE8BB File Offset: 0x004ECABB
		public static bool PlayerCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Sellable;
		}

		// Token: 0x06011367 RID: 70503 RVA: 0x004EE8C7 File Offset: 0x004ECAC7
		public static bool TraderCanSell(this Tradeability tradeability)
		{
			return tradeability == Tradeability.All || tradeability == Tradeability.Buyable;
		}
	}
}
