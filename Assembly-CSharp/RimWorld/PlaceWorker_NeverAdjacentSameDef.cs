using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037FC RID: 14332
	public class PlaceWorker_NeverAdjacentSameDef : PlaceWorker
	{
		// Token: 0x06013D81 RID: 81281 RVA: 0x005E460C File Offset: 0x005E280C
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				if (c.InBounds(map))
				{
					List<Thing> list = map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						if (thing2 != thingToIgnore)
						{
							if (thing2.def.category != ThingCategory.Building || thing2.def != def)
							{
								if (!thing2.def.IsBlueprint && !thing2.def.IsFrame)
								{
									goto IL_C0;
								}
								ThingDef thingDef = thing2.def.entityDefToBuild as ThingDef;
								if (thingDef == null || thingDef != def)
								{
									goto IL_C0;
								}
							}
							return "CannotPlaceAdjacentSameDef".Translate();
						}
						IL_C0:;
					}
				}
			}
			return true;
		}
	}
}
