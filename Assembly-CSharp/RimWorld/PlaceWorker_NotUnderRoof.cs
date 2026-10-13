using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E4 RID: 14308
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		// Token: 0x06013D46 RID: 81222 RVA: 0x005E36CC File Offset: 0x005E18CC
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (checkingDef.Size.x == 1 && checkingDef.Size.z == 1)
			{
				if (map.roofGrid.Roofed(loc))
				{
					return new AcceptanceReport("MustPlaceUnroofed".Translate());
				}
			}
			else
			{
				foreach (IntVec3 c in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
				{
					if (map.roofGrid.Roofed(c))
					{
						return new AcceptanceReport("MustPlaceUnroofed".Translate());
					}
				}
			}
			return true;
		}
	}
}
