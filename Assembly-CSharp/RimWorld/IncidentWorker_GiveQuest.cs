using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E20 RID: 7712
	public class IncidentWorker_GiveQuest : IncidentWorker
	{
		// Token: 0x0600AB8D RID: 43917 RVA: 0x00310ED8 File Offset: 0x0030F0D8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			QuestScriptDef questScriptDef = this.def.questScriptDef ?? parms.questScriptDef;
			return (questScriptDef == null || questScriptDef.CanRun(parms.points, parms.target)) && IncidentWorker_GiveQuest.CanQuestOccurOnTile(parms.target.Tile, questScriptDef) && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.Any<Pawn>();
		}

		// Token: 0x0600AB8E RID: 43918 RVA: 0x00310F40 File Offset: 0x0030F140
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			QuestScriptDef questScriptDef;
			if ((questScriptDef = this.def.questScriptDef) == null)
			{
				questScriptDef = (parms.questScriptDef ?? NaturalRandomQuestChooser.ChooseNaturalRandomQuest(parms.points, parms.target));
			}
			QuestScriptDef questScriptDef2 = questScriptDef;
			if (questScriptDef2 == null)
			{
				return false;
			}
			if (!IncidentWorker_GiveQuest.CanQuestOccurOnTile(parms.target.Tile, questScriptDef2))
			{
				return false;
			}
			parms.questScriptDef = questScriptDef2;
			this.GiveQuest(parms, questScriptDef2);
			return true;
		}

		// Token: 0x0600AB8F RID: 43919 RVA: 0x00310FA4 File Offset: 0x0030F1A4
		protected virtual void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questDef, parms.points);
			if (!quest.hidden && questDef.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}

		// Token: 0x0600AB90 RID: 43920 RVA: 0x00310FD8 File Offset: 0x0030F1D8
		private static bool CanQuestOccurOnTile(PlanetTile tile, QuestScriptDef quest)
		{
			if (!tile.Valid)
			{
				return true;
			}
			if (quest != null)
			{
				PlanetLayerDef layerDef = tile.LayerDef;
				if (!quest.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !quest.layerWhitelist.Contains(layerDef))
				{
					return false;
				}
				if (!quest.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && quest.layerBlacklist.Contains(layerDef))
				{
					return false;
				}
				if (!quest.canOccurOnAllPlanetLayers && layerDef.onlyAllowWhitelistedIncidents && (quest.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || !quest.layerWhitelist.Contains(layerDef)))
				{
					return false;
				}
			}
			return !tile.LayerDef.onlyAllowWhitelistedQuests;
		}
	}
}
