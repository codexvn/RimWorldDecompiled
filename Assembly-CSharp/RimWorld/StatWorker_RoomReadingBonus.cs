using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003755 RID: 14165
	public class StatWorker_RoomReadingBonus : StatWorker
	{
		// Token: 0x06013A2E RID: 80430 RVA: 0x005D5360 File Offset: 0x005D3560
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && typeof(Building_Bookcase).IsAssignableFrom(thingDef.thingClass);
		}
	}
}
