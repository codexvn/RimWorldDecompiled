using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA5 RID: 7589
	public class IncidentWorker_ColdSnap : IncidentWorker_MakeGameCondition
	{
		// Token: 0x0600A9AE RID: 43438 RVA: 0x00308F82 File Offset: 0x00307182
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_ColdSnap.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x0600A9AF RID: 43439 RVA: 0x00308F9F File Offset: 0x0030719F
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp > 0f && map.mapTemperature.SeasonalTemp < 15f;
		}
	}
}
