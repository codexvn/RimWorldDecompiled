using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034BA RID: 13498
	public class TransferableComparer_Archonexus : TransferableComparer
	{
		// Token: 0x06012F96 RID: 77718 RVA: 0x005A168D File Offset: 0x0059F88D
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Archonexus.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x06012F97 RID: 77719 RVA: 0x005A16A0 File Offset: 0x0059F8A0
		public static int Compare(ThingDef lhsTh, ThingDef rhsTh)
		{
			float num = TransferableUIUtility.DefaultArchonexusItemListOrderPriority(lhsTh);
			float num2 = TransferableUIUtility.DefaultArchonexusItemListOrderPriority(rhsTh);
			if (num != num2)
			{
				return num.CompareTo(num2);
			}
			return TransferableComparer_Category.Compare(lhsTh, rhsTh);
		}
	}
}
