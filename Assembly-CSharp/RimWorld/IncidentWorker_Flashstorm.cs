using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9A RID: 7578
	public class IncidentWorker_Flashstorm : IncidentWorker
	{
		// Token: 0x0600A988 RID: 43400 RVA: 0x00308567 File Offset: 0x00306767
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return !((Map)parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.Flashstorm);
		}

		// Token: 0x0600A989 RID: 43401 RVA: 0x00308588 File Offset: 0x00306788
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, duration);
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			base.SendStandardLetter(this.def.letterLabel, GameConditionDefOf.Flashstorm.letterText, this.def.letterDef, parms, new TargetInfo(gameCondition_Flashstorm.centerLocation.ToIntVec3, map, false), Array.Empty<NamedArgument>());
			if (map.weatherManager.curWeather.rainRate > 0.1f)
			{
				map.weatherDecider.StartNextWeather();
			}
			return true;
		}
	}
}
