using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DD6 RID: 7638
	public class IncidentWorker_SightstealerSwarm : IncidentWorker
	{
		// Token: 0x0600AA5A RID: 43610 RVA: 0x0030B98C File Offset: 0x00309B8C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			List<Pawn> list = this.GenerateSightstealers(parms, parms.points * 0.5f);
			PawnsArrivalModeDefOf.EdgeWalkInDistributed.Worker.TryResolveRaidSpawnCenter(parms);
			PawnsArrivalModeDefOf.EdgeWalkInDistributed.Worker.Arrive(list, parms);
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(list.RandomElement<Pawn>());
			}
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_SightstealerAssault(), map, list);
			SoundDefOf.Sightstealer_DistantHowl.PlayOneShotOnCamera(null);
			base.SendStandardLetter(parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600AA5B RID: 43611 RVA: 0x0030BA2C File Offset: 0x00309C2C
		private List<Pawn> GenerateSightstealers(IncidentParms parms, float points)
		{
			Map map = (Map)parms.target;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Sightstealers,
				tile = map.Tile,
				faction = Faction.OfEntities,
				points = points
			};
			pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, null) * 1.05f);
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true).ToList<Pawn>();
		}

		// Token: 0x04007582 RID: 30082
		private const float PointsFactor = 0.5f;
	}
}
