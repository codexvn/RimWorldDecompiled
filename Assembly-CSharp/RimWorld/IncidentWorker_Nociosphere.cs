using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB9 RID: 7609
	public class IncidentWorker_Nociosphere : IncidentWorker
	{
		// Token: 0x0600A9F5 RID: 43509 RVA: 0x0030A000 File Offset: 0x00308200
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 spawnCenter = parms.spawnCenter;
			if (!spawnCenter.IsValid && !RCellFinder.TryFindRandomSpotJustOutsideColony(parms.spawnCenter, map, out spawnCenter))
			{
				return false;
			}
			for (int i = 0; i < map.mapPawns.AllPawnsCount; i++)
			{
				if (map.mapPawns.AllPawns[i].kindDef == PawnKindDefOf.Nociosphere)
				{
					return false;
				}
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Nociosphere, Faction.OfEntities, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			NociosphereUtility.SkipTo((Pawn)GenSpawn.Spawn(pawn, spawnCenter, map, WipeMode.Vanish), spawnCenter);
			base.SendStandardLetter(parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
