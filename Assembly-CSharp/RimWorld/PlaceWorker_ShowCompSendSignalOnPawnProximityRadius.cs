using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003800 RID: 14336
	public class PlaceWorker_ShowCompSendSignalOnPawnProximityRadius : PlaceWorker
	{
		// Token: 0x06013D8A RID: 81290 RVA: 0x005E47F8 File Offset: 0x005E29F8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_SendSignalOnMotion compProperties = def.GetCompProperties<CompProperties_SendSignalOnMotion>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.radius);
		}
	}
}
