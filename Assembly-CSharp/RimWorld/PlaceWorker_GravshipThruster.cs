using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003830 RID: 14384
	public class PlaceWorker_GravshipThruster : PlaceWorker
	{
		// Token: 0x06013E1B RID: 81435 RVA: 0x005E6DC0 File Offset: 0x005E4FC0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_GravshipThruster compProperties = def.GetCompProperties<CompProperties_GravshipThruster>();
			if (compProperties != null)
			{
				compProperties.GetExclusionZone(center, rot, ref PlaceWorker_GravshipThruster.exclusionCells);
				GenDraw.DrawFieldEdges(PlaceWorker_GravshipThruster.exclusionCells, 2900);
			}
		}

		// Token: 0x06013E1C RID: 81436 RVA: 0x005E6DF4 File Offset: 0x005E4FF4
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			Thing thing;
			bool flag;
			if (CompGravshipThruster.IsBlocked(def as ThingDef, map, loc, rot, out thing, out flag))
			{
				if (thing != null)
				{
					Messages.Message("WarningThrusterBlocked".Translate(def.LabelCap, thing.Label), MessageTypeDefOf.CautionInput, false);
					return;
				}
				if (flag)
				{
					Messages.Message("WarningThrusterBlocked".Translate(def.LabelCap, TerrainDefOf.Substructure.label), MessageTypeDefOf.CautionInput, false);
					return;
				}
			}
			else if (!CompGravshipThruster.IsOutdoors(def as ThingDef, map, loc, rot))
			{
				Messages.Message("WarningThrusterInside".Translate(def.LabelCap), MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0400DBD6 RID: 56278
		private static List<IntVec3> exclusionCells = new List<IntVec3>();
	}
}
