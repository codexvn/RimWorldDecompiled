using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E5 RID: 14309
	public class Placeworker_OnlyUnderThickRoof : PlaceWorker
	{
		// Token: 0x06013D48 RID: 81224 RVA: 0x005E3790 File Offset: 0x005E1990
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(loc);
			if (roofDef == null || !roofDef.isThickRoof)
			{
				return new AcceptanceReport("MustPlaceUnderThickRoof".Translate());
			}
			return true;
		}
	}
}
