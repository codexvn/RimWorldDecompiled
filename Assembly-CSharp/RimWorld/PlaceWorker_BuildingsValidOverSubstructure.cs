using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003833 RID: 14387
	public class PlaceWorker_BuildingsValidOverSubstructure : PlaceWorker
	{
		// Token: 0x06013E24 RID: 81444 RVA: 0x005E711C File Offset: 0x005E531C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return AcceptanceReport.WasAccepted;
			}
			foreach (Thing thing2 in loc.GetThingList(map))
			{
				if (thing2.def == ThingDefOf.GravEngine)
				{
					return AcceptanceReport.WasAccepted;
				}
				if (thing2.def.IsEdifice() && thing2.Faction != Faction.OfPlayer)
				{
					return "MessageSubstructureBlocked".Translate();
				}
				Blueprint blueprint = thing2 as Blueprint;
				List<PlaceWorker> placeWorkers;
				if (blueprint != null)
				{
					placeWorkers = blueprint.def.entityDefToBuild.PlaceWorkers;
				}
				else
				{
					placeWorkers = thing2.def.PlaceWorkers;
				}
				if (placeWorkers != null)
				{
					using (List<PlaceWorker>.Enumerator enumerator2 = placeWorkers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current is PlaceWorker_InvalidOverSubstructure)
							{
								return "MessageSubstructureBlocked".Translate();
							}
						}
					}
				}
			}
			return AcceptanceReport.WasAccepted;
		}
	}
}
