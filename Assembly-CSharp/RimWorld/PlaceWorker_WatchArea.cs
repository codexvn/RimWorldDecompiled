using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003803 RID: 14339
	public class PlaceWorker_WatchArea : PlaceWorker
	{
		// Token: 0x06013D90 RID: 81296 RVA: 0x005E4874 File Offset: 0x005E2A74
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(WatchBuildingUtility.CalculateWatchCells(def, center, rot, currentMap).ToList<IntVec3>(), 2900);
		}
	}
}
