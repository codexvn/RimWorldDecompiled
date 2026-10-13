using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E7 RID: 14311
	public class PlaceWorker_OnSteamGeyser : PlaceWorker
	{
		// Token: 0x06013D4C RID: 81228 RVA: 0x005E391C File Offset: 0x005E1B1C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			Thing thing2 = map.thingGrid.ThingAt(loc, ThingDefOf.SteamGeyser);
			if (thing2 == null || thing2.Position != loc)
			{
				return "MustPlaceOnSteamGeyser".Translate();
			}
			return true;
		}

		// Token: 0x06013D4D RID: 81229 RVA: 0x005E3963 File Offset: 0x005E1B63
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			return otherDef == ThingDefOf.SteamGeyser;
		}

		// Token: 0x06013D4E RID: 81230 RVA: 0x005E3970 File Offset: 0x005E1B70
		public override void DrawMouseAttachments(BuildableDef def)
		{
			List<Thing> list = Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.SteamGeyser);
			for (int i = 0; i < list.Count; i++)
			{
				TargetHighlighter.Highlight(list[i], true, true, false);
			}
		}
	}
}
