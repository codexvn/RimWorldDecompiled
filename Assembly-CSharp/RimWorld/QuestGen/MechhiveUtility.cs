using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B19 RID: 15129
	public static class MechhiveUtility
	{
		// Token: 0x0601495B RID: 84315 RVA: 0x00624340 File Offset: 0x00622540
		public static void FireRaid(Map map, float points, int fireTick = -1, string customLetterLabel = null, string customLetterText = null)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = Mathf.Max(points, Faction.OfMechanoids.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
			incidentParms.faction = Faction.OfMechanoids;
			incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
			incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			incidentParms.customLetterLabel = (customLetterLabel ?? "MechhiveRaidLetterLabel".Translate());
			incidentParms.customLetterText = (customLetterText ?? "MechhiveRaidLetterText".Translate());
			incidentParms.canRoofPunch = new bool?(false);
			if (fireTick < 0)
			{
				fireTick = Find.TickManager.TicksGame;
			}
			Find.Storyteller.incidentQueue.Add(IncidentDefOf.RaidEnemy, fireTick, incidentParms, 0);
		}
	}
}
