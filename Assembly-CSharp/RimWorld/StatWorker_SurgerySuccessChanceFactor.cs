using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003757 RID: 14167
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x06013A32 RID: 80434 RVA: 0x005D54AC File Offset: 0x005D36AC
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Def def = req.Def;
			if (!(def is ThingDef))
			{
				return false;
			}
			ThingDef thingDef = def as ThingDef;
			return typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass);
		}
	}
}
