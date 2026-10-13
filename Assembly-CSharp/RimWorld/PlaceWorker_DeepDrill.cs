using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037EE RID: 14318
	public class PlaceWorker_DeepDrill : PlaceWorker_ShowDeepResources
	{
		// Token: 0x06013D5C RID: 81244 RVA: 0x005E3BD3 File Offset: 0x005E1DD3
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (DeepDrillUtility.GetNextResource(loc, map) == null)
			{
				return "MustPlaceOnDrillable".Translate();
			}
			return true;
		}
	}
}
