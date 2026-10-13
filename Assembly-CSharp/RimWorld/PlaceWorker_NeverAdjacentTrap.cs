using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037FB RID: 14331
	public class PlaceWorker_NeverAdjacentTrap : PlaceWorker
	{
		// Token: 0x06013D7E RID: 81278 RVA: 0x000026EA File Offset: 0x000008EA
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x06013D7F RID: 81279 RVA: 0x005E44E0 File Offset: 0x005E26E0
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					if (thing2 != thingToIgnore && ((thing2.def.category == ThingCategory.Building && thing2.def.building.isTrap) || ((thing2.def.IsBlueprint || thing2.def.IsFrame) && thing2.def.entityDefToBuild is ThingDef && ((ThingDef)thing2.def.entityDefToBuild).building.isTrap)))
					{
						return "CannotPlaceAdjacentTrap".Translate();
					}
				}
			}
			return true;
		}
	}
}
