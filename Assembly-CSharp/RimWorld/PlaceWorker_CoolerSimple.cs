using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037F7 RID: 14327
	public class PlaceWorker_CoolerSimple : PlaceWorker
	{
		// Token: 0x06013D73 RID: 81267 RVA: 0x005E3F84 File Offset: 0x005E2184
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			Room room = center.GetRoom(currentMap);
			if (room != null && !room.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomCold, null, null, 2900);
			}
		}
	}
}
