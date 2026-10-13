using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B57 RID: 11095
	public static class SkyfallerUtility
	{
		// Token: 0x0600F824 RID: 63524 RVA: 0x00482EC4 File Offset: 0x004810C4
		public static bool CanPossiblyFallOnColonist(ThingDef skyfaller, IntVec3 c, Map map)
		{
			CellRect cellRect = GenAdj.OccupiedRect(c, Rot4.North, skyfaller.size);
			int dist = Mathf.Max(Mathf.CeilToInt(skyfaller.skyfaller.explosionRadius) + 7, 14);
			foreach (IntVec3 c2 in cellRect.ExpandedBy(dist))
			{
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null && pawn.IsColonist)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
