using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003820 RID: 14368
	public class PlaceWorker_MustBeIndoors : PlaceWorker
	{
		// Token: 0x06013DEF RID: 81391 RVA: 0x005E62A0 File Offset: 0x005E44A0
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Room room = loc.GetRoom(map);
			if (room == null || room.TouchesMapEdge)
			{
				return "MustBePlacedIndoors".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
