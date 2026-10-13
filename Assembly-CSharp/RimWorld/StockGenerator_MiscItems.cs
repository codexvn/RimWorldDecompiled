using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F7F RID: 12159
	public abstract class StockGenerator_MiscItems : StockGenerator
	{
		// Token: 0x06011202 RID: 70146 RVA: 0x004E9AB4 File Offset: 0x004E7CB4
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			int count = this.countRange.RandomInRange;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				ThingDef def;
				if (!(from t in DefDatabase<ThingDef>.AllDefs
				where this.HandlesThingDef(t) && t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate
				select t).TryRandomElementByWeight(new Func<ThingDef, float>(this.SelectionWeight), out def))
				{
					yield break;
				}
				yield return this.MakeThing(def, faction);
				num = i;
			}
			yield break;
		}

		// Token: 0x06011203 RID: 70147 RVA: 0x004E9ACB File Offset: 0x004E7CCB
		protected virtual Thing MakeThing(ThingDef def, Faction faction)
		{
			return StockGeneratorUtility.TryMakeForStockSingle(def, 1, faction);
		}

		// Token: 0x06011204 RID: 70148 RVA: 0x004E9AD5 File Offset: 0x004E7CD5
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy;
		}

		// Token: 0x06011205 RID: 70149 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected virtual float SelectionWeight(ThingDef thingDef)
		{
			return 1f;
		}
	}
}
