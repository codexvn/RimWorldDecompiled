using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003825 RID: 14373
	public class PlaceWorker_CocoonTriggerRadius : PlaceWorker
	{
		// Token: 0x06013DF9 RID: 81401 RVA: 0x005E64C4 File Offset: 0x005E46C4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_WakeUpDormant compProperties = def.GetCompProperties<CompProperties_WakeUpDormant>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.wakeUpCheckRadius, Color.white, null);
		}
	}
}
