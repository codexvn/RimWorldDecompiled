using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200381F RID: 14367
	public class PlaceWorker_LinkToCampfire : PlaceWorker
	{
		// Token: 0x06013DED RID: 81389 RVA: 0x005E6218 File Offset: 0x005E4418
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawRadiusRing(center, this.range);
			List<Thing> forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, this.range, ThingDefOf.Campfire, null);
			for (int i = 0; i < forCell.Count; i++)
			{
				GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), forCell[i].TrueCenter(), SimpleColor.Green, 0.2f);
			}
		}

		// Token: 0x0400DBD0 RID: 56272
		public float range = 11.9f;
	}
}
