using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003802 RID: 14338
	public class PlaceWorker_ShowExplosionRadius : PlaceWorker
	{
		// Token: 0x06013D8E RID: 81294 RVA: 0x005E4850 File Offset: 0x005E2A50
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Explosive compProperties = def.GetCompProperties<CompProperties_Explosive>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.explosiveRadius);
		}
	}
}
