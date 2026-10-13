using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002856 RID: 10326
	public class SitePartWorker_WorkSite_Mining : SitePartWorker_WorkSite
	{
		// Token: 0x1700226B RID: 8811
		// (get) Token: 0x0600E1CE RID: 57806 RVA: 0x00422BF7 File Offset: 0x00420DF7
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowMiningCamps
				select p;
			}
		}

		// Token: 0x1700226C RID: 8812
		// (get) Token: 0x0600E1CF RID: 57807 RVA: 0x00422C22 File Offset: 0x00420E22
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Miners;
			}
		}

		// Token: 0x0600E1D0 RID: 57808 RVA: 0x00422C2C File Offset: 0x00420E2C
		public override bool CanSpawnOn(PlanetTile tile)
		{
			Hilliness hilliness = Find.WorldGrid[tile].hilliness;
			return hilliness >= Hilliness.LargeHills && hilliness < Hilliness.Impassable;
		}

		// Token: 0x0600E1D1 RID: 57809 RVA: 0x00422C54 File Offset: 0x00420E54
		protected override void OnLootChosen(Site site, SitePart sitePart, SitePartWorker_WorkSite.CampLootThingStruct loot)
		{
			site.customLabel = sitePart.def.label.Formatted(loot.thing.Named("THING"));
		}
	}
}
