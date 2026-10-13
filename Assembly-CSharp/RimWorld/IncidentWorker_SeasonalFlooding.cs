using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC9 RID: 7625
	public class IncidentWorker_SeasonalFlooding : IncidentWorker
	{
		// Token: 0x0600AA29 RID: 43561 RVA: 0x0030ADA4 File Offset: 0x00308FA4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			GenSpawn.Spawn(ThingDefOf.SeasonalFlood, map.Center, map, WipeMode.Vanish);
			return true;
		}

		// Token: 0x0600AA2A RID: 43562 RVA: 0x0030ADD4 File Offset: 0x00308FD4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			SurfaceTile surfaceTile = map.TileInfo as SurfaceTile;
			if (surfaceTile == null)
			{
				return false;
			}
			if (surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>())
			{
				return false;
			}
			if (map.mapTemperature.OutdoorTemp < -7f)
			{
				return false;
			}
			Vector2 longLat = Find.WorldGrid.LongLatOf(Find.WorldSelector.SelectedTile);
			Season season = GenDate.Season((long)Find.TickManager.TicksGame, longLat);
			return season == Season.Spring || season == Season.PermanentSummer;
		}
	}
}
