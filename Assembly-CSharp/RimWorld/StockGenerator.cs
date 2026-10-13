using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F76 RID: 12150
	public abstract class StockGenerator
	{
		// Token: 0x060111DA RID: 70106 RVA: 0x004E95F8 File Offset: 0x004E77F8
		public virtual void ResolveReferences(TraderKindDef trader)
		{
			this.trader = trader;
		}

		// Token: 0x060111DB RID: 70107 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x060111DC RID: 70108
		public abstract IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null);

		// Token: 0x060111DD RID: 70109
		public abstract bool HandlesThingDef(ThingDef thingDef);

		// Token: 0x060111DE RID: 70110 RVA: 0x004E9601 File Offset: 0x004E7801
		public virtual Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return thingDef.tradeability;
		}

		// Token: 0x060111DF RID: 70111 RVA: 0x004E9614 File Offset: 0x004E7814
		public bool TryGetPriceType(ThingDef thingDef, TradeAction action, out PriceType priceType)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				priceType = PriceType.Undefined;
				return false;
			}
			priceType = this.price;
			return true;
		}

		// Token: 0x060111E0 RID: 70112 RVA: 0x004E9630 File Offset: 0x004E7830
		protected int RandomCountOf(ThingDef def)
		{
			IntRange intRange = this.countRange;
			if (this.customCountRanges != null)
			{
				for (int i = 0; i < this.customCountRanges.Count; i++)
				{
					if (this.customCountRanges[i].thingDef == def)
					{
						intRange = this.customCountRanges[i].countRange;
						break;
					}
				}
			}
			if (intRange.max <= 0 && this.totalPriceRange.max <= 0f)
			{
				return 0;
			}
			if (intRange.max > 0 && this.totalPriceRange.max <= 0f)
			{
				return intRange.RandomInRange;
			}
			if (intRange.max <= 0 && this.totalPriceRange.max > 0f)
			{
				return Mathf.RoundToInt(this.totalPriceRange.RandomInRange / def.BaseMarketValue);
			}
			int num = 0;
			int randomInRange;
			do
			{
				randomInRange = intRange.RandomInRange;
				num++;
			}
			while (num <= 100 && !this.totalPriceRange.Includes((float)randomInRange * def.BaseMarketValue));
			return randomInRange;
		}

		// Token: 0x0400B195 RID: 45461
		[Unsaved(false)]
		public TraderKindDef trader;

		// Token: 0x0400B196 RID: 45462
		public IntRange countRange = IntRange.Zero;

		// Token: 0x0400B197 RID: 45463
		public List<ThingDefCountRangeClass> customCountRanges;

		// Token: 0x0400B198 RID: 45464
		public FloatRange totalPriceRange = FloatRange.Zero;

		// Token: 0x0400B199 RID: 45465
		public TechLevel maxTechLevelGenerate = TechLevel.Archotech;

		// Token: 0x0400B19A RID: 45466
		public TechLevel maxTechLevelBuy = TechLevel.Archotech;

		// Token: 0x0400B19B RID: 45467
		public PriceType price = PriceType.Normal;
	}
}
