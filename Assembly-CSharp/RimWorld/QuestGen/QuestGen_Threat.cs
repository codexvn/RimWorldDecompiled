using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020039DB RID: 14811
	public static class QuestGen_Threat
	{
		// Token: 0x06014532 RID: 83250 RVA: 0x00613A24 File Offset: 0x00611C24
		public static void Raid(this Quest quest, MapParent parent, float points, Faction faction, string inSignalLeave = null, string customLetterLabel = null, string customLetterText = null, RulePack customLetterLabelRules = null, RulePack customLetterTextRules = null, IntVec3? walkInSpot = null, string tag = null, string inSignal = null, string rootSymbol = "root", PawnsArrivalModeDef raidArrivalMode = null, RaidStrategyDef raidStrategy = null, PawnGroupKindDef pawnGroupKind = null, bool silent = false, bool canTimeoutOrFlee = true, bool canSteal = true, bool canKidnap = true)
		{
			if (pawnGroupKind == null)
			{
				pawnGroupKind = PawnGroupKindDefOf.Combat;
			}
			Map target = parent.HasMap ? parent.Map : Find.AnyPlayerHomeMap;
			QuestPart_Incident questPart_Incident = new QuestPart_Incident
			{
				debugLabel = "raid",
				incident = IncidentDefOf.RaidEnemy
			};
			IncidentParms incidentParms = new IncidentParms
			{
				forced = true,
				target = target,
				points = Mathf.Max(points, faction.def.MinPointsToGeneratePawnGroup(pawnGroupKind, null)),
				faction = faction,
				pawnGroupKind = pawnGroupKind,
				pawnGroupMakerSeed = new int?(Rand.Int),
				inSignalEnd = inSignalLeave,
				questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(tag),
				raidArrivalMode = raidArrivalMode,
				raidStrategy = raidStrategy,
				canTimeoutOrFlee = canTimeoutOrFlee,
				canKidnap = canKidnap,
				canSteal = canSteal,
				sendLetter = !silent,
				silent = silent
			};
			if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
			{
				QuestGen.AddTextRequest(rootSymbol, delegate(string x)
				{
					incidentParms.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, rootSymbol));
			}
			if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
			{
				QuestGen.AddTextRequest(rootSymbol, delegate(string x)
				{
					incidentParms.customLetterText = x;
				}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, rootSymbol));
			}
			IncidentWorker_Raid incidentWorker_Raid = (IncidentWorker_Raid)questPart_Incident.incident.Worker;
			incidentWorker_Raid.ResolveRaidStrategy(incidentParms, pawnGroupKind);
			incidentWorker_Raid.ResolveRaidArriveMode(incidentParms);
			incidentWorker_Raid.ResolveRaidAgeRestriction(incidentParms);
			if (incidentParms.raidArrivalMode.walkIn)
			{
				incidentParms.spawnCenter = (walkInSpot ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(pawnGroupKind, incidentParms, true);
			float a = IncidentWorker_Raid.AdjustedRaidPoints(defaultPawnGroupMakerParms.points, incidentParms.raidArrivalMode, incidentParms.raidStrategy, defaultPawnGroupMakerParms.faction, pawnGroupKind, incidentParms.target, null);
			defaultPawnGroupMakerParms.points = Mathf.Max(a, defaultPawnGroupMakerParms.faction.def.MinPointsToGeneratePawnGroup(pawnGroupKind, null));
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(defaultPawnGroupMakerParms);
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.MapParent = parent;
			questPart_Incident.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("raidPawnKinds", PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor)),
				new Rule_String("raidArrivalModeInfo", incidentParms.raidArrivalMode.textWillArrive.Formatted(faction))
			});
		}

		// Token: 0x06014533 RID: 83251 RVA: 0x00613D0C File Offset: 0x00611F0C
		public static QuestPart_RandomRaid RandomRaid(this Quest quest, MapParent mapParent, FloatRange pointsRange, Faction faction = null, string inSignal = null, PawnsArrivalModeDef arrivalMode = null, RaidStrategyDef raidStrategy = null, string customLetterLabel = null, string customLetterText = null, bool useCurrentThreatPoints = false)
		{
			QuestPart_RandomRaid questPart_RandomRaid = new QuestPart_RandomRaid
			{
				mapParent = mapParent,
				faction = faction,
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pointsRange = pointsRange,
				arrivalMode = arrivalMode,
				raidStrategy = raidStrategy,
				customLetterLabel = customLetterLabel,
				customLetterText = customLetterText,
				useCurrentThreatPoints = useCurrentThreatPoints
			};
			quest.AddPart(questPart_RandomRaid);
			return questPart_RandomRaid;
		}
	}
}
