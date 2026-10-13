using System;

namespace RimWorld
{
	// Token: 0x020034BF RID: 13503
	public class TransferableComparer_Name : TransferableComparer
	{
		// Token: 0x06012FA3 RID: 77731 RVA: 0x005A1875 File Offset: 0x0059FA75
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return lhs.Label.CompareTo(rhs.Label);
		}
	}
}
