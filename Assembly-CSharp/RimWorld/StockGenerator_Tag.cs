using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F9C RID: 12188
	public class StockGenerator_Tag : StockGenerator
	{
		// Token: 0x060112A6 RID: 70310 RVA: 0x004EBADB File Offset: 0x004E9CDB
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			Func<ThingDef, bool> <>9__0;
			int num;
			for (int i = 0; i < numThingDefsToUse; i = num + 1)
			{
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef d) => this.HandlesThingDef(d) && d.tradeability.TraderCanSell() && d.PlayerAcquirable && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(d)) && !generatedDefs.Contains(d)));
				}
				ThingDef chosenThingDef;
				if (!allDefs.Where(predicate).TryRandomElementByWeight(new Func<ThingDef, float>(this.SelectionWeight), out chosenThingDef))
				{
					yield break;
				}
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef), faction))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				generatedDefs.Add(chosenThingDef);
				chosenThingDef = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060112A7 RID: 70311 RVA: 0x004EBAF2 File Offset: 0x004E9CF2
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains(this.tradeTag);
		}

		// Token: 0x060112A8 RID: 70312 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}

		// Token: 0x0400B229 RID: 45609
		[NoTranslate]
		public string tradeTag;

		// Token: 0x0400B22A RID: 45610
		private IntRange thingDefCountRange = IntRange.One;

		// Token: 0x0400B22B RID: 45611
		private List<ThingDef> excludedThingDefs;
	}
}
