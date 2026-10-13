using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003059 RID: 12377
	public class Alert_NeedHoldingPlatform : Alert
	{
		// Token: 0x06011624 RID: 71204 RVA: 0x004FCA4C File Offset: 0x004FAC4C
		public Alert_NeedHoldingPlatform()
		{
			this.defaultLabel = "AlertHoldingPlatform".Translate();
			this.defaultExplanation = "AlertHoldingPlatformDesc".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x06011625 RID: 71205 RVA: 0x004FCA88 File Offset: 0x004FAC88
		public override AlertReport GetReport()
		{
			if ((float)Find.TickManager.TicksGame < 240000f)
			{
				return false;
			}
			if (Find.Anomaly.hasBuiltHoldingPlatform)
			{
				return false;
			}
			if (Find.CurrentMap == null)
			{
				return false;
			}
			if (!Find.Anomaly.AnomalyStudyEnabled)
			{
				return false;
			}
			if (Find.Anomaly.Level == 1 && Find.Anomaly.TicksSinceLastLevelChange < 7200)
			{
				return false;
			}
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint);
			for (int i = 0; i < list.Count; i++)
			{
				Blueprint blueprint = list[i] as Blueprint;
				if (blueprint != null)
				{
					ThingDef thingDef = blueprint.def.entityDefToBuild as ThingDef;
					if (thingDef != null && ThingRequestGroup.EntityHolder.Includes(thingDef))
					{
						return false;
					}
				}
			}
			List<Thing> list2 = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
			for (int j = 0; j < list2.Count; j++)
			{
				Frame frame = list2[j] as Frame;
				if (frame != null)
				{
					ThingDef thingDef2 = frame.def.entityDefToBuild as ThingDef;
					if (thingDef2 != null && ThingRequestGroup.EntityHolder.Includes(thingDef2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
