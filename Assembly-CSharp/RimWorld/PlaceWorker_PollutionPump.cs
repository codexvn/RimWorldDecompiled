using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003823 RID: 14371
	public class PlaceWorker_PollutionPump : PlaceWorker
	{
		// Token: 0x06013DF5 RID: 81397 RVA: 0x005E63D8 File Offset: 0x005E45D8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_PollutionPump compProperties = def.GetCompProperties<CompProperties_PollutionPump>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white, null);
		}
	}
}
