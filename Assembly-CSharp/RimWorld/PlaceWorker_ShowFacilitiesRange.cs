using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037FE RID: 14334
	public class PlaceWorker_ShowFacilitiesRange : PlaceWorker
	{
		// Token: 0x06013D86 RID: 81286 RVA: 0x005E47A8 File Offset: 0x005E29A8
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Facility compProperties = def.GetCompProperties<CompProperties_Facility>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.maxDistance - 0.1f);
			}
		}
	}
}
