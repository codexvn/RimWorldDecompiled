using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B7B RID: 11131
	public class ThingSetMaker_Books : ThingSetMaker
	{
		// Token: 0x0600F8F4 RID: 63732 RVA: 0x004862E8 File Offset: 0x004844E8
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			ThingDef thingDef;
			return (parms.countRange == null || parms.countRange.Value.max > 0) && TechprintUtility.TryGetTechprintDefToGenerate(parms.makingFaction, out thingDef, null, (((parms.totalMarketValueRange != null) ? new float?(parms.totalMarketValueRange.GetValueOrDefault().max) : null) * this.marketValueFactor) ?? float.MaxValue);
		}

		// Token: 0x0600F8F5 RID: 63733 RVA: 0x00486398 File Offset: 0x00484598
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = 1;
			if (parms.countRange != null)
			{
				num = Mathf.Max(parms.countRange.Value.RandomInRange, num);
			}
			for (int i = 0; i < num; i++)
			{
				outThings.Add(BookUtility.MakeBook(ArtGenerationContext.Outsider, parms.qualityGenerator));
			}
		}

		// Token: 0x0600F8F6 RID: 63734 RVA: 0x004863EE File Offset: 0x004845EE
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return from x in DefDatabase<ThingDef>.AllDefs
			where x.HasComp<CompBook>()
			select x;
		}

		// Token: 0x0400A3B4 RID: 41908
		private float marketValueFactor = 1f;
	}
}
