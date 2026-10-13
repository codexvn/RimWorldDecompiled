using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037FD RID: 14333
	public class PlaceWorker_ShowFacilitiesConnections : PlaceWorker
	{
		// Token: 0x06013D83 RID: 81283 RVA: 0x005E4724 File Offset: 0x005E2924
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			ThingDef thingDef = bdef as ThingDef;
			if (thingDef != null)
			{
				Map currentMap = Find.CurrentMap;
				if (thingDef.HasComp(typeof(CompAffectedByFacilities)))
				{
					CompAffectedByFacilities.DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(curX, ref curY, thingDef, center, rot, currentMap);
					return;
				}
				CompFacility.DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(curX, ref curY, thingDef, center, rot, currentMap);
			}
		}

		// Token: 0x06013D84 RID: 81284 RVA: 0x005E4770 File Offset: 0x005E2970
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.HasComp(typeof(CompAffectedByFacilities)))
			{
				CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
				return;
			}
			CompFacility.DrawLinesToPotentialThingsToLinkTo(def, center, rot, currentMap);
		}
	}
}
