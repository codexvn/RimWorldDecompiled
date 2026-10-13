using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E9 RID: 14313
	public class PlaceWorker_HeadOnShipBeam : PlaceWorker
	{
		// Token: 0x06013D52 RID: 81234 RVA: 0x005E3A50 File Offset: 0x005E1C50
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			IntVec3 c = loc + rot.FacingCell * -1;
			if (!c.InBounds(map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice == null || edifice.def != ThingDefOf.Ship_Beam)
			{
				return "MustPlaceHeadOnShipBeam".Translate();
			}
			return true;
		}
	}
}
