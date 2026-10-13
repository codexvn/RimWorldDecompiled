using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003831 RID: 14385
	public class PlaceWorker_InRangeOfGravEngine : PlaceWorker
	{
		// Token: 0x06013E1F RID: 81439 RVA: 0x005E6EC4 File Offset: 0x005E50C4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			CompProperties_GravshipFacility compProperties = def.GetCompProperties<CompProperties_GravshipFacility>();
			foreach (Thing thing2 in currentMap.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				SimpleColor color;
				if (center.InHorDistOf(thing2.Position, compProperties.maxDistance))
				{
					color = SimpleColor.Green;
				}
				else
				{
					color = SimpleColor.Red;
				}
				GenDraw.DrawLineBetween(center.ToVector3Shifted(), thing2.TrueCenter(), color, 0.2f);
			}
		}

		// Token: 0x06013E20 RID: 81440 RVA: 0x005E6F58 File Offset: 0x005E5158
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			ThingDef thingDef = checkingDef as ThingDef;
			if (thingDef == null)
			{
				return AcceptanceReport.WasRejected;
			}
			CompProperties_GravshipFacility compProperties = thingDef.GetCompProperties<CompProperties_GravshipFacility>();
			foreach (Thing thing2 in map.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				if (loc.InHorDistOf(thing2.Position, compProperties.maxDistance))
				{
					return AcceptanceReport.WasAccepted;
				}
			}
			return "MessageMustBePlacedInRangeOfGravEngine".Translate();
		}
	}
}
