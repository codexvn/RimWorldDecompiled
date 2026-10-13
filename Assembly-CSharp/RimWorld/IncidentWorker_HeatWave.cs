using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA4 RID: 7588
	public class IncidentWorker_HeatWave : IncidentWorker_MakeGameCondition
	{
		// Token: 0x0600A9AB RID: 43435 RVA: 0x00308F4E File Offset: 0x0030714E
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && IncidentWorker_HeatWave.IsTemperatureAppropriate((Map)parms.target);
		}

		// Token: 0x0600A9AC RID: 43436 RVA: 0x00308F6B File Offset: 0x0030716B
		public static bool IsTemperatureAppropriate(Map map)
		{
			return map.mapTemperature.SeasonalTemp >= 20f;
		}
	}
}
