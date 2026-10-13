using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E6 RID: 14310
	public class Placeworker_AttachedToWall : PlaceWorker
	{
		// Token: 0x06013D4A RID: 81226 RVA: 0x005E37D4 File Offset: 0x005E19D4
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			List<Thing> thingList = loc.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing2 = thingList[i];
				ThingDef thingDef = GenConstruct.BuiltDefOf(thing2.def) as ThingDef;
				if (((thingDef != null) ? thingDef.building : null) != null)
				{
					if (thingDef.Fillage == FillCategory.Full)
					{
						return false;
					}
					if (thingDef.building.isAttachment && thing2.Rotation == rot)
					{
						return "SomethingPlacedOnThisWall".Translate();
					}
				}
			}
			IntVec3 c = loc + GenAdj.CardinalDirections[rot.AsInt];
			if (!c.InBounds(map))
			{
				return false;
			}
			thingList = c.GetThingList(map);
			bool flag = false;
			for (int j = 0; j < thingList.Count; j++)
			{
				ThingDef thingDef2 = GenConstruct.BuiltDefOf(thingList[j].def) as ThingDef;
				if (thingDef2 != null && thingDef2.building != null)
				{
					if (!thingDef2.building.supportsWallAttachments)
					{
						flag = true;
					}
					else if (thingDef2.Fillage == FillCategory.Full)
					{
						return true;
					}
				}
			}
			if (flag)
			{
				return "CannotSupportAttachment".Translate();
			}
			return "MustPlaceOnWall".Translate();
		}
	}
}
