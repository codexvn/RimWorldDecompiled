using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037F1 RID: 14321
	public class PlaceWorker_ShowInstrumentAoE : PlaceWorker
	{
		// Token: 0x06013D63 RID: 81251 RVA: 0x005E3CBC File Offset: 0x005E1EBC
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = (ThingDef)checkingDef;
			PlaceWorker_ShowInstrumentAoE.tmpCells.Clear();
			int num = GenRadial.NumCellsInRadius(thingDef.building.instrumentRange);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = loc + GenRadial.RadialPattern[i];
				if (Building_MusicalInstrument.IsAffectedByInstrument(thingDef, loc, intVec, map))
				{
					PlaceWorker_ShowInstrumentAoE.tmpCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(PlaceWorker_ShowInstrumentAoE.tmpCells, 2900);
			return true;
		}

		// Token: 0x0400DBA8 RID: 56232
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
