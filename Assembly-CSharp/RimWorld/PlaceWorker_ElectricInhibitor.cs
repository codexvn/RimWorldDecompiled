using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200382B RID: 14379
	public class PlaceWorker_ElectricInhibitor : PlaceWorker
	{
		// Token: 0x06013E0A RID: 81418 RVA: 0x005E6AEC File Offset: 0x005E4CEC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			PlaceWorker_ElectricInhibitor.tmpCells.Clear();
			PlaceWorker_ElectricInhibitor.tmpCells.AddRange(ContainmentUtility.GetInhibitorAffectedCells(def, center, rot, Find.CurrentMap));
			GenDraw.DrawFieldEdges(PlaceWorker_ElectricInhibitor.tmpCells, Color.white, null, null, 2900);
			PlaceWorker_ElectricInhibitor.tmpCells.Clear();
		}

		// Token: 0x06013E0B RID: 81419 RVA: 0x005E6B44 File Offset: 0x005E4D44
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = checkingDef as ThingDef;
			if (thingDef != null && ContainmentUtility.IsLinearBuildingBlocked(thingDef, loc, rot, map, thingToIgnore))
			{
				return new AcceptanceReport("InhibitorSpaceOccupied".Translate());
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0400DBD3 RID: 56275
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
