using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003816 RID: 14358
	public class PlaceWorker_RequireNaturePsycaster : PlaceWorker
	{
		// Token: 0x06013DC9 RID: 81353 RVA: 0x005E5880 File Offset: 0x005E3A80
		public override bool IsBuildDesignatorVisible(BuildableDef def)
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (MeditationFocusDefOf.Natural.CanPawnUse(p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
