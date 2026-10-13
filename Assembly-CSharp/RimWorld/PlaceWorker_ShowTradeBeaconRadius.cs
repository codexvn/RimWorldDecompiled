using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037FF RID: 14335
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x06013D88 RID: 81288 RVA: 0x005E47D4 File Offset: 0x005E29D4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap), 2900);
		}
	}
}
