using System;
using System.Linq;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002089 RID: 8329
	public class GenStep_WorkSitePawns : GenStep
	{
		// Token: 0x17001BDD RID: 7133
		// (get) Token: 0x0600B67A RID: 46714 RVA: 0x00346F6D File Offset: 0x0034516D
		public override int SeedPart
		{
			get
			{
				return 237483478;
			}
		}

		// Token: 0x0600B67B RID: 46715 RVA: 0x00346F74 File Offset: 0x00345174
		public static int GetEnemiesCount(Site site, SitePartParams parms, PawnGroupKindDef workerGroupKind)
		{
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms);
			return PawnGroupMakerUtility.GeneratePawnKindsExample(GenStep_WorkSitePawns.GroupMakerParmsWorkers(site.Tile, site.Faction, parms.threatPoints, pawnGroupMakerSeed, workerGroupKind)).Count<PawnKindDef>() + PawnGroupMakerUtility.GeneratePawnKindsExample(GenStep_WorkSitePawns.GroupMakerParmsFighters(site.Tile, site.Faction, parms.threatPoints, pawnGroupMakerSeed)).Count<PawnKindDef>();
		}

		// Token: 0x0600B67C RID: 46716 RVA: 0x00346FD0 File Offset: 0x003451D0
		private static PawnGroupMakerParms GroupMakerParmsWorkers(PlanetTile tile, Faction faction, float points, int seed, PawnGroupKindDef workerGroupKind)
		{
			float a = points / 2f;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = workerGroupKind,
				tile = tile,
				faction = faction,
				inhabitants = true,
				seed = new int?(seed)
			};
			pawnGroupMakerParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x0600B67D RID: 46717 RVA: 0x00347034 File Offset: 0x00345234
		private static PawnGroupMakerParms GroupMakerParmsFighters(PlanetTile tile, Faction faction, float points, int seed)
		{
			float a = points / 2f;
			PawnGroupKindDef groupKindDef = PawnGroupKindDefOf.Combat;
			if (!faction.def.pawnGroupMakers.Any((PawnGroupMaker maker) => maker.kindDef == groupKindDef))
			{
				groupKindDef = PawnGroupKindDefOf.Settlement;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = groupKindDef,
				tile = tile,
				faction = faction,
				inhabitants = true,
				generateFightersOnly = true,
				seed = new int?(seed)
			};
			pawnGroupMakerParms.points = Mathf.Max(a, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x0600B67E RID: 46718 RVA: 0x003470DC File Offset: 0x003452DC
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rect;
			IntVec3 baseCenter;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rect))
			{
				baseCenter = rect.CenterCell;
				Log.Error("No rect of interest set when running GenStep_WorkSitePawns!");
			}
			else
			{
				baseCenter = map.Center;
			}
			Faction faction = parms.sitePart.site.Faction;
			Lord singlePawnLord = LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, baseCenter, 25000, false), map, null);
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.singlePawnLord = singlePawnLord;
			resolveParams.singlePawnSpawnCellExtraPredicate = ((IntVec3 x) => map.reachability.CanReachMapEdge(x, traverseParms));
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms);
			resolveParams.pawnGroupMakerParams = GenStep_WorkSitePawns.GroupMakerParmsWorkers(map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed, ((SitePartWorker_WorkSite)parms.sitePart.def.Worker).WorkerGroupKind);
			resolveParams.pawnGroupKindDef = resolveParams.pawnGroupMakerParams.groupKind;
			BaseGen.symbolStack.Push("pawnGroup", resolveParams, null);
			ResolveParams resolveParams2 = resolveParams;
			resolveParams2.pawnGroupMakerParams = GenStep_WorkSitePawns.GroupMakerParmsFighters(map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed);
			resolveParams2.pawnGroupKindDef = resolveParams2.pawnGroupMakerParams.groupKind;
			BaseGen.symbolStack.Push("pawnGroup", resolveParams2, null);
			BaseGen.globalSettings.map = map;
			BaseGen.Generate();
		}
	}
}
