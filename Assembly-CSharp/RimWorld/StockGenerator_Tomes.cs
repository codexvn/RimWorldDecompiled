using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F84 RID: 12164
	public class StockGenerator_Tomes : StockGenerator_SingleDef
	{
		// Token: 0x06011221 RID: 70177 RVA: 0x004E9F17 File Offset: 0x004E8117
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			if (!ModsConfig.AnomalyActive)
			{
				yield break;
			}
			if (!Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent)
			{
				yield break;
			}
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(ThingDefOf.Tome, base.RandomCountOf(ThingDefOf.Tome), faction))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06011222 RID: 70178 RVA: 0x004E9F2E File Offset: 0x004E812E
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return ModsConfig.AnomalyActive && Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent && thingDef == ThingDefOf.Tome;
		}

		// Token: 0x06011223 RID: 70179 RVA: 0x004E9F59 File Offset: 0x004E8159
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			yield break;
		}
	}
}
