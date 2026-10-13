using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001754 RID: 5972
	public class RaidStrategyWorker_SiegeMechanoid : RaidStrategyWorker_Siege
	{
		// Token: 0x06008C25 RID: 35877 RVA: 0x0029B5D9 File Offset: 0x002997D9
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind) && Faction.OfMechanoids != null && parms.faction == Faction.OfMechanoids && ModsConfig.RoyaltyActive;
		}

		// Token: 0x06008C26 RID: 35878 RVA: 0x0029B60F File Offset: 0x0029980F
		public override void TryGenerateThreats(IncidentParms parms)
		{
			parms.mechClusterSketch = MechClusterGenerator.GenerateClusterSketch(parms.points, parms.target as Map, true, false);
		}

		// Token: 0x06008C27 RID: 35879 RVA: 0x0029B62F File Offset: 0x0029982F
		public override List<Pawn> SpawnThreats(IncidentParms parms)
		{
			return MechClusterUtility.SpawnCluster(parms.spawnCenter, (Map)parms.target, parms.mechClusterSketch, true, true, parms.questTag).OfType<Pawn>().ToList<Pawn>();
		}

		// Token: 0x06008C28 RID: 35880 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return null;
		}

		// Token: 0x06008C29 RID: 35881 RVA: 0x000026EA File Offset: 0x000008EA
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
		}
	}
}
