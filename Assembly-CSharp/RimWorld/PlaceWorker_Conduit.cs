using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037EA RID: 14314
	public class PlaceWorker_Conduit : PlaceWorker
	{
		// Token: 0x06013D54 RID: 81236 RVA: 0x005E3AB4 File Offset: 0x005E1CB4
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			List<Thing> thingList = loc.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing2 = thingList[i];
				if (!GenConstruct.CanReplace(checkingDef, thing2.def, null, null))
				{
					if (thing2.def.EverTransmitsPower)
					{
						return false;
					}
					if (thing2.def.entityDefToBuild != null)
					{
						ThingDef thingDef = thing2.def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.EverTransmitsPower)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}
}
