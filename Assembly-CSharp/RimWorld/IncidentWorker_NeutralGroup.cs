using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E13 RID: 7699
	public abstract class IncidentWorker_NeutralGroup : IncidentWorker_PawnsArrive
	{
		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x0600AB5B RID: 43867 RVA: 0x0030FD69 File Offset: 0x0030DF69
		protected virtual PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Peaceful;
			}
		}

		// Token: 0x0600AB5C RID: 43868 RVA: 0x0030FD70 File Offset: 0x0030DF70
		public override bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			if (!base.FactionCanBeGroupSource(f, parms, desperate))
			{
				return false;
			}
			if (f.Hidden || f.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (f.def.pawnGroupMakers == null || !f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == this.PawnGroupKindDef))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, f) && (f.def.neutralArrivalLayerWhitelist.NullOrEmpty<PlanetLayerDef>() || f.def.neutralArrivalLayerWhitelist.Contains(map.Tile.LayerDef)) && (f.def.neutralArrivalLayerBlacklist.NullOrEmpty<PlanetLayerDef>() || !f.def.neutralArrivalLayerBlacklist.Contains(map.Tile.LayerDef));
		}

		// Token: 0x0600AB5D RID: 43869 RVA: 0x0030FE50 File Offset: 0x0030E050
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			using (List<GameCondition>.Enumerator enumerator = ((Map)parms.target).GameConditionManager.ActiveConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.preventNeutralVisitors)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600AB5E RID: 43870 RVA: 0x0030FEC8 File Offset: 0x0030E0C8
		protected bool TryResolveParms(IncidentParms parms)
		{
			if (!this.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			this.ResolveParmsPoints(parms);
			return true;
		}

		// Token: 0x0600AB5F RID: 43871 RVA: 0x0030FEE0 File Offset: 0x0030E0E0
		protected virtual bool TryResolveParmsGeneral(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return (parms.spawnCenter.IsValid || RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, false, null)) && (parms.faction != null || base.CandidateFactions(parms, false).TryRandomElement(out parms.faction) || base.CandidateFactions(parms, true).TryRandomElement(out parms.faction));
		}

		// Token: 0x0600AB60 RID: 43872
		protected abstract void ResolveParmsPoints(IncidentParms parms);

		// Token: 0x0600AB61 RID: 43873 RVA: 0x0030FF54 File Offset: 0x0030E154
		protected List<Pawn> SpawnPawns(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(this.PawnGroupKindDef, parms, true), false).ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				List<Pawn> storeGeneratedNeutralPawns = parms.storeGeneratedNeutralPawns;
				if (storeGeneratedNeutralPawns != null)
				{
					storeGeneratedNeutralPawns.Add(pawn);
				}
			}
			return list;
		}
	}
}
