using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200380C RID: 14348
	public class PlaceWorker_ShipLandingBeacon : PlaceWorker
	{
		// Token: 0x06013DB1 RID: 81329 RVA: 0x005E50DC File Offset: 0x005E32DC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompShipLandingBeacon)))
			{
				ShipLandingBeaconUtility.DrawLinesToNearbyBeacons(def, center, rot, currentMap, thing);
			}
		}
	}
}
