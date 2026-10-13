using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200382C RID: 14380
	public class PlaceWorker_ProximityDetector : PlaceWorker
	{
		// Token: 0x06013E0E RID: 81422 RVA: 0x005E6B8F File Offset: 0x005E4D8F
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawRadiusRing(center, 19.9f, Color.white, null);
		}
	}
}
