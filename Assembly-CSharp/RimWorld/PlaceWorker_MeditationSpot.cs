using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200380D RID: 14349
	public class PlaceWorker_MeditationSpot : PlaceWorker
	{
		// Token: 0x06013DB3 RID: 81331 RVA: 0x005E510C File Offset: 0x005E330C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			MeditationUtility.DrawMeditationSpotOverlay(center, Find.CurrentMap);
		}
	}
}
