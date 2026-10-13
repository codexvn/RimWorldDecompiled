using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B92 RID: 11154
	public class ThingSetMaker_UniqueWeapon : ThingSetMaker
	{
		// Token: 0x0600F963 RID: 63843 RVA: 0x00487DBC File Offset: 0x00485FBC
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return ModsConfig.OdysseyActive && (parms.countRange == null || parms.countRange.Value.max > 0) && (parms.totalMarketValueRange == null || parms.totalMarketValueRange.Value.max > 0f) && this.AllGeneratableThingsDebugSub(parms).Any<ThingDef>();
		}

		// Token: 0x0600F964 RID: 63844 RVA: 0x00487E2C File Offset: 0x0048602C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = (parms.countRange != null) ? parms.countRange.GetValueOrDefault().RandomInRange : 1;
			if (parms.countRange != null)
			{
				num = Mathf.Max(parms.countRange.Value.RandomInRange, num);
			}
			FloatRange floatRange = parms.totalMarketValueRange ?? new FloatRange(0f, float.MaxValue);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				bool flag = i == num - 1;
				int num3 = 999;
				Thing thing;
				do
				{
					thing = ThingMaker.MakeThing((from x in DefDatabase<ThingDef>.AllDefs
					where x.HasComp<CompUniqueWeapon>()
					select x).RandomElement<ThingDef>(), null);
				}
				while (num3-- > 0 && (num2 + thing.MarketValue > floatRange.max || (flag && num2 + thing.MarketValue < floatRange.min)));
				if (num3 <= 0)
				{
					break;
				}
				num2 += thing.MarketValue;
				outThings.Add(thing);
			}
		}

		// Token: 0x0600F965 RID: 63845 RVA: 0x00487F56 File Offset: 0x00486156
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return from x in DefDatabase<ThingDef>.AllDefs
			where x.HasComp<CompUniqueWeapon>()
			select x;
		}
	}
}
