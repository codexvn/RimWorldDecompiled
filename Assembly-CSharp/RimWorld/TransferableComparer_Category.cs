using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034BB RID: 13499
	public class TransferableComparer_Category : TransferableComparer
	{
		// Token: 0x06012F99 RID: 77721 RVA: 0x005A16D7 File Offset: 0x0059F8D7
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Category.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x06012F9A RID: 77722 RVA: 0x005A16EC File Offset: 0x0059F8EC
		public static int Compare(ThingDef lhsTh, ThingDef rhsTh)
		{
			if (lhsTh.category != rhsTh.category)
			{
				return lhsTh.category.CompareTo(rhsTh.category);
			}
			float num = TransferableUIUtility.DefaultListOrderPriority(lhsTh);
			float num2 = TransferableUIUtility.DefaultListOrderPriority(rhsTh);
			if (num != num2)
			{
				return num.CompareTo(num2);
			}
			int num3 = 0;
			if (!lhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				num3 = (int)lhsTh.thingCategories[0].index;
			}
			int value = 0;
			if (!rhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				value = (int)rhsTh.thingCategories[0].index;
			}
			return num3.CompareTo(value);
		}
	}
}
