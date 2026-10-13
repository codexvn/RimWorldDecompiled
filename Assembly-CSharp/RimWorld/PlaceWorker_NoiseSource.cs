using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003828 RID: 14376
	public class PlaceWorker_NoiseSource : PlaceWorker
	{
		// Token: 0x06013E00 RID: 81408 RVA: 0x005E66D0 File Offset: 0x005E48D0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_NoiseSource compProperties = def.GetCompProperties<CompProperties_NoiseSource>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white, null);
			}
		}
	}
}
