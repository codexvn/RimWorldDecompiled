using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200382F RID: 14383
	[Obsolete("Use Substructure affordance instead.")]
	public class PlaceWorker_OnSubstructure : PlaceWorker
	{
		// Token: 0x06013E19 RID: 81433 RVA: 0x005E6D2C File Offset: 0x005E4F2C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
			{
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
					if (terrainDef == null || !terrainDef.IsSubstructure)
					{
						return "MessageMustBePlacedOnSubstructure".Translate();
					}
				}
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
