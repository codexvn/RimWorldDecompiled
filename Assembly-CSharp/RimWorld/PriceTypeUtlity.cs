using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020034B7 RID: 13495
	public static class PriceTypeUtlity
	{
		// Token: 0x06012F8E RID: 77710 RVA: 0x005A0F5C File Offset: 0x0059F15C
		public static float PriceMultiplier(this PriceType pType)
		{
			switch (pType)
			{
			case PriceType.VeryCheap:
				return 0.4f;
			case PriceType.Cheap:
				return 0.7f;
			case PriceType.Normal:
				return 1f;
			case PriceType.Expensive:
				return 2f;
			case PriceType.Exorbitant:
				return 5f;
			default:
				return -1f;
			}
		}

		// Token: 0x06012F8F RID: 77711 RVA: 0x005A0FAC File Offset: 0x0059F1AC
		public static PriceType ClosestPriceType(float priceFactor)
		{
			float num = 99999f;
			PriceType priceType = PriceType.Undefined;
			foreach (object obj in Enum.GetValues(typeof(PriceType)))
			{
				PriceType priceType2 = (PriceType)obj;
				float num2 = Mathf.Abs(priceFactor - priceType2.PriceMultiplier());
				if (num2 < num)
				{
					num = num2;
					priceType = priceType2;
				}
			}
			if (priceType == PriceType.Undefined)
			{
				priceType = PriceType.VeryCheap;
			}
			return priceType;
		}
	}
}
