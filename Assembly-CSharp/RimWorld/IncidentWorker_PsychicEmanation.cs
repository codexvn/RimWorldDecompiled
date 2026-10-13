using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DC0 RID: 7616
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		// Token: 0x0600AA0B RID: 43531 RVA: 0x0030A56C File Offset: 0x0030876C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.PsychicDronerShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		// Token: 0x0600AA0C RID: 43532 RVA: 0x0030A5D8 File Offset: 0x003087D8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(parms, map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender, parms.points);
			return true;
		}

		// Token: 0x0600AA0D RID: 43533
		protected abstract void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points);
	}
}
