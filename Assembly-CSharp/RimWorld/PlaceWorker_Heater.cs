using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037EC RID: 14316
	public class PlaceWorker_Heater : PlaceWorker
	{
		// Token: 0x06013D58 RID: 81240 RVA: 0x005E3B5C File Offset: 0x005E1D5C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			Room room = center.GetRoom(currentMap);
			if (room != null && !room.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(room.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomHot, null, null, 2900);
			}
		}
	}
}
