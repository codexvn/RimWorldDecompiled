using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003822 RID: 14370
	public class PlaceWorker_BandNode : PlaceWorker
	{
		// Token: 0x06013DF3 RID: 81395 RVA: 0x005E6304 File Offset: 0x005E4504
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(center, rot, def.Size).ExpandedBy(1))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def == def || thingList[i].def.entityDefToBuild == def)
						{
							return "MustNotBePlacedCloseToAnother".Translate(def);
						}
					}
				}
			}
			return true;
		}
	}
}
