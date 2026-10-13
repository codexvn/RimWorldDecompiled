using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003827 RID: 14375
	public class PlaceWorker_NeverAdjacentUnstandableRadial : PlaceWorker
	{
		// Token: 0x06013DFD RID: 81405 RVA: 0x005E65B4 File Offset: 0x005E47B4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(GenRadial.RadialCellsAround(center, (float)def.size.x + 0.9f, true).ToList<IntVec3>(), Color.white, null, null, 2900);
		}

		// Token: 0x06013DFE RID: 81406 RVA: 0x005E65F8 File Offset: 0x005E47F8
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenRadial.RadialCellsAround(center, (float)def.Size.x + 0.9f, true))
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != thingToIgnore && (!c.Walkable(map) || list[i].def.passability != Traversability.Standable))
					{
						return "MustPlaceAdjacentStandable".Translate();
					}
				}
			}
			return true;
		}
	}
}
