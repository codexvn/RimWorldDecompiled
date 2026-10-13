using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003821 RID: 14369
	public class PlaceWorker_ToxifierGenerator : PlaceWorker
	{
		// Token: 0x06013DF1 RID: 81393 RVA: 0x005E62D8 File Offset: 0x005E44D8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Toxifier compProperties = def.GetCompProperties<CompProperties_Toxifier>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white, null);
			}
		}
	}
}
