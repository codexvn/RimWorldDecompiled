using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003832 RID: 14386
	public class PlaceWorker_InvalidOverSubstructure : PlaceWorker
	{
		// Token: 0x06013E22 RID: 81442 RVA: 0x005E6FF8 File Offset: 0x005E51F8
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return AcceptanceReport.WasAccepted;
			}
			foreach (IntVec3 c in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
			{
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						return "MessageCannotPlaceOverSubstructure".Translate();
					}
					foreach (Blueprint blueprint in map.thingGrid.ThingsListAt(c).OfType<Blueprint>())
					{
						TerrainDef terrainDef2 = blueprint.def.entityDefToBuild as TerrainDef;
						if (terrainDef2 != null && terrainDef2.IsSubstructure)
						{
							return "MessageCannotPlaceOverSubstructure".Translate();
						}
					}
				}
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
