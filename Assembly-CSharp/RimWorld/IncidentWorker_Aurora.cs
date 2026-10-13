using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E1F RID: 7711
	public class IncidentWorker_Aurora : IncidentWorker_MakeGameCondition
	{
		// Token: 0x0600AB8A RID: 43914 RVA: 0x00310E28 File Offset: 0x0030F028
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome && !map.GameConditionManager.IsAlwaysDarkOutside && !this.AuroraWillEndSoon(map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600AB8B RID: 43915 RVA: 0x00310EA4 File Offset: 0x0030F0A4
		private bool AuroraWillEndSoon(Map map)
		{
			return GenCelestial.CurCelestialSunGlow(map) > 0.5f || GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs + 5000) > 0.5f;
		}

		// Token: 0x040075F3 RID: 30195
		private const int EnsureMinDurationTicks = 5000;
	}
}
