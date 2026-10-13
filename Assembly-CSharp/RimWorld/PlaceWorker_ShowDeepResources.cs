using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037ED RID: 14317
	public class PlaceWorker_ShowDeepResources : PlaceWorker
	{
		// Token: 0x06013D5A RID: 81242 RVA: 0x005E3BA8 File Offset: 0x005E1DA8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap.deepResourceGrid.AnyActiveDeepScannersOnMap())
			{
				currentMap.deepResourceGrid.MarkForDraw();
			}
		}
	}
}
