using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037F6 RID: 14326
	public class PlaceWorker_MultiCellDoor : PlaceWorker
	{
		// Token: 0x06013D70 RID: 81264 RVA: 0x005E3E98 File Offset: 0x005E2098
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			foreach (IntVec3 center2 in DoorUtility.WallRequirementCells(def, center, rot))
			{
				GhostDrawer.DrawGhostThing(center2, Rot4.South, ThingDefOf.Wall, null, Color.grey, AltitudeLayer.Blueprint, null, false, null);
			}
		}

		// Token: 0x06013D71 RID: 81265 RVA: 0x005E3EFC File Offset: 0x005E20FC
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef == null)
			{
				return;
			}
			using (IEnumerator<IntVec3> enumerator = DoorUtility.WallRequirementCells(thingDef, loc, rot).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!DoorUtility.EncapsulatingWallAt(enumerator.Current, map, true))
					{
						Messages.Message("MessageBuildingRequiresAdjacentWalls".Translate(def).CapitalizeFirst(), MessageTypeDefOf.CautionInput, false);
						break;
					}
				}
			}
		}
	}
}
