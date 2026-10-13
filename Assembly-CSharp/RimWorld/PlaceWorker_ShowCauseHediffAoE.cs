using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037F3 RID: 14323
	public class PlaceWorker_ShowCauseHediffAoE : PlaceWorker
	{
		// Token: 0x06013D69 RID: 81257 RVA: 0x005E3DE4 File Offset: 0x005E1FE4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_CauseHediff_AoE compProperties = def.GetCompProperties<CompProperties_CauseHediff_AoE>();
			if (compProperties != null)
			{
				GenDraw.DrawRadiusRing(center, compProperties.range, Color.white, null);
			}
		}
	}
}
