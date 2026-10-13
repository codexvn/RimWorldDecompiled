using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F77 RID: 12151
	public class StockGeneratorUtility
	{
		// Token: 0x060111E2 RID: 70114 RVA: 0x004E975A File Offset: 0x004E795A
		public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count, Faction faction)
		{
			if (thingDef.MadeFromStuff || thingDef.tradeNeverStack || thingDef.tradeNeverGenerateStacked)
			{
				int num;
				for (int i = 0; i < count; i = num + 1)
				{
					Thing thing = StockGeneratorUtility.TryMakeForStockSingle(thingDef, 1, faction);
					if (thing != null)
					{
						yield return thing;
					}
					num = i;
				}
			}
			else
			{
				Thing thing2 = StockGeneratorUtility.TryMakeForStockSingle(thingDef, count, faction);
				if (thing2 != null)
				{
					yield return thing2;
				}
			}
			yield break;
		}

		// Token: 0x060111E3 RID: 70115 RVA: 0x004E9778 File Offset: 0x004E7978
		public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount, Faction faction)
		{
			if (stackCount <= 0)
			{
				return null;
			}
			if (!thingDef.tradeability.TraderCanSell())
			{
				string str = "Tried to make non-trader-sellable thing for trader stock: ";
				ThingDef thingDef2 = thingDef;
				Log.Error(str + ((thingDef2 != null) ? thingDef2.ToString() : null));
				return null;
			}
			ThingDef stuff = null;
			if (thingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined, true)
				where !PawnWeaponGenerator.IsDerpWeapon(thingDef, x)
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, stuff);
			thing.stackCount = stackCount;
			return thing;
		}
	}
}
