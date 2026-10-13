using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F82 RID: 12162
	public class StockGenerator_ReinforcedBarrels : StockGenerator
	{
		// Token: 0x06011214 RID: 70164 RVA: 0x004E9D53 File Offset: 0x004E7F53
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			if (Find.Storyteller.difficulty.classicMortars)
			{
				yield break;
			}
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(ThingDefOf.ReinforcedBarrel, base.RandomCountOf(ThingDefOf.ReinforcedBarrel), faction))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06011215 RID: 70165 RVA: 0x004E9D6A File Offset: 0x004E7F6A
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == ThingDefOf.ReinforcedBarrel;
		}

		// Token: 0x06011216 RID: 70166 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			return Enumerable.Empty<string>();
		}
	}
}
