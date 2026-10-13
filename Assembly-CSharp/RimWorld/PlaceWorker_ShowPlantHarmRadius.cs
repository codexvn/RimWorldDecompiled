using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003801 RID: 14337
	public class PlaceWorker_ShowPlantHarmRadius : PlaceWorker
	{
		// Token: 0x06013D8C RID: 81292 RVA: 0x005E481C File Offset: 0x005E2A1C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing == null)
			{
				return;
			}
			CompPlantHarmRadius compPlantHarmRadius = thing.TryGetComp<CompPlantHarmRadius>();
			if (compPlantHarmRadius == null)
			{
				return;
			}
			float currentRadius = compPlantHarmRadius.CurrentRadius;
			if (currentRadius < 50f)
			{
				GenDraw.DrawRadiusRing(center, currentRadius);
			}
		}
	}
}
