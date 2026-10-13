using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002858 RID: 10328
	public class SitePartWorker_WorkSite_Logging : SitePartWorker_WorkSite
	{
		// Token: 0x1700226D RID: 8813
		// (get) Token: 0x0600E1D6 RID: 57814 RVA: 0x00422C95 File Offset: 0x00420E95
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowLoggingCamps
				select p;
			}
		}

		// Token: 0x1700226E RID: 8814
		// (get) Token: 0x0600E1D7 RID: 57815 RVA: 0x00422CC0 File Offset: 0x00420EC0
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Loggers;
			}
		}

		// Token: 0x0600E1D8 RID: 57816 RVA: 0x00422CC7 File Offset: 0x00420EC7
		public override bool CanSpawnOn(PlanetTile tile)
		{
			return Find.WorldGrid[tile].PrimaryBiome.TreeDensity >= BiomeDefOf.Tundra.TreeDensity;
		}
	}
}
