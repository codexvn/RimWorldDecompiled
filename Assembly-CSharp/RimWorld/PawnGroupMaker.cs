using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200237F RID: 9087
	public class PawnGroupMaker
	{
		// Token: 0x0600C4F8 RID: 50424 RVA: 0x00394CCF File Offset: 0x00392ECF
		public float MinPointsToGenerateAnything(FactionDef faction, PawnGroupMakerParms parms = null)
		{
			return this.kindDef.Worker.MinPointsToGenerateAnything(this, faction, parms);
		}

		// Token: 0x0600C4F9 RID: 50425 RVA: 0x00394CE4 File Offset: 0x00392EE4
		public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
		{
			return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
		}

		// Token: 0x0600C4FA RID: 50426 RVA: 0x00394CF9 File Offset: 0x00392EF9
		public IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			return this.kindDef.Worker.GeneratePawnKindsExample(parms, this);
		}

		// Token: 0x0600C4FB RID: 50427 RVA: 0x00394D10 File Offset: 0x00392F10
		public bool CanGenerateFrom(PawnGroupMakerParms parms)
		{
			if (parms.points > this.maxTotalPoints)
			{
				return false;
			}
			if (this.disallowedStrategies != null && this.disallowedStrategies.Contains(parms.raidStrategy))
			{
				return false;
			}
			if (parms.points < this.MinPointsToGenerateAnything(parms.faction.def, parms))
			{
				return false;
			}
			if (parms.raidStrategy != null)
			{
				RaidStrategyWorker_WithRequiredPawnKinds raidStrategyWorker_WithRequiredPawnKinds = parms.raidStrategy.Worker as RaidStrategyWorker_WithRequiredPawnKinds;
				if (raidStrategyWorker_WithRequiredPawnKinds != null && !raidStrategyWorker_WithRequiredPawnKinds.CanUseWithGroupMaker(this))
				{
					return false;
				}
			}
			return this.kindDef.Worker.CanGenerateFrom(parms, this);
		}

		// Token: 0x04008658 RID: 34392
		public PawnGroupKindDef kindDef;

		// Token: 0x04008659 RID: 34393
		public float commonality = 100f;

		// Token: 0x0400865A RID: 34394
		public List<RaidStrategyDef> disallowedStrategies;

		// Token: 0x0400865B RID: 34395
		public float maxTotalPoints = 9999999f;

		// Token: 0x0400865C RID: 34396
		public List<PawnGenOption> options = new List<PawnGenOption>();

		// Token: 0x0400865D RID: 34397
		public List<PawnGenOption> traders = new List<PawnGenOption>();

		// Token: 0x0400865E RID: 34398
		public List<PawnGenOption> carriers = new List<PawnGenOption>();

		// Token: 0x0400865F RID: 34399
		public List<PawnGenOption> guards = new List<PawnGenOption>();
	}
}
