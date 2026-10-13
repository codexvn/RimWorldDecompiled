using System;

namespace RimWorld
{
	// Token: 0x020034BD RID: 13501
	public class TransferableComparer_MarketValue : TransferableComparer
	{
		// Token: 0x06012F9F RID: 77727 RVA: 0x005A1810 File Offset: 0x0059FA10
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.AnyThing.MarketValue.CompareTo(rhs.AnyThing.MarketValue);
		}
	}
}
