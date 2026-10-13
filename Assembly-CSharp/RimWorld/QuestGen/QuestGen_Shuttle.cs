using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039D8 RID: 14808
	public static class QuestGen_Shuttle
	{
		// Token: 0x0601451A RID: 83226 RVA: 0x00612A34 File Offset: 0x00610C34
		public static Thing GenerateShuttle(Faction owningFaction = null, IEnumerable<Pawn> requiredPawns = null, IEnumerable<ThingDefCount> requiredItems = null, bool acceptColonists = false, bool onlyAcceptColonists = false, bool onlyAcceptHealthy = false, int requireColonistCount = -1, bool dropEverythingIfUnsatisfied = false, bool leaveImmediatelyWhenSatisfied = false, bool dropEverythingOnArrival = false, bool stayAfterDroppedEverythingOnArrival = false, WorldObject missionShuttleTarget = null, WorldObject missionShuttleHome = null, int maxColonistCount = -1, ThingDef shuttleDef = null, bool permitShuttle = false, bool hideControls = true, bool allowSlaves = false, bool requireAllColonistsOnMap = false, bool acceptColonyPrisoners = false)
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Shuttle"))
			{
				return null;
			}
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(shuttleDef ?? ThingDefOf.Shuttle, null);
			if (owningFaction != null)
			{
				thing.SetFaction(owningFaction, null);
			}
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			if (requiredPawns != null)
			{
				compShuttle.requiredPawns.AddRange(requiredPawns);
			}
			if (requiredItems != null)
			{
				compShuttle.requiredItems.AddRange(requiredItems);
			}
			compShuttle.acceptColonists = acceptColonists;
			compShuttle.onlyAcceptColonists = onlyAcceptColonists;
			compShuttle.onlyAcceptHealthy = onlyAcceptHealthy;
			compShuttle.requiredColonistCount = requireColonistCount;
			compShuttle.maxColonistCount = maxColonistCount;
			compShuttle.permitShuttle = permitShuttle;
			compShuttle.allowSlaves = allowSlaves;
			compShuttle.requireAllColonistsOnMap = requireAllColonistsOnMap;
			compShuttle.acceptColonyPrisoners = acceptColonyPrisoners;
			return thing;
		}

		// Token: 0x0601451B RID: 83227 RVA: 0x00612AE0 File Offset: 0x00610CE0
		public static QuestPart_SendShuttleAway SendShuttleAway(this Quest quest, Thing shuttle, bool dropEverything = false, string inSignal = null)
		{
			if (shuttle == null)
			{
				return null;
			}
			QuestPart_SendShuttleAway questPart_SendShuttleAway = new QuestPart_SendShuttleAway();
			questPart_SendShuttleAway.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SendShuttleAway.shuttle = shuttle;
			questPart_SendShuttleAway.dropEverything = dropEverything;
			quest.AddPart(questPart_SendShuttleAway);
			return questPart_SendShuttleAway;
		}

		// Token: 0x0601451C RID: 83228 RVA: 0x00612B30 File Offset: 0x00610D30
		public static QuestPart_SendShuttleAwayOnCleanup SendShuttleAwayOnCleanup(this Quest quest, Thing shuttle, bool dropEverything = false)
		{
			QuestPart_SendShuttleAwayOnCleanup questPart_SendShuttleAwayOnCleanup = new QuestPart_SendShuttleAwayOnCleanup();
			questPart_SendShuttleAwayOnCleanup.shuttle = shuttle;
			questPart_SendShuttleAwayOnCleanup.dropEverything = dropEverything;
			quest.AddPart(questPart_SendShuttleAwayOnCleanup);
			return questPart_SendShuttleAwayOnCleanup;
		}

		// Token: 0x0601451D RID: 83229 RVA: 0x00612B5C File Offset: 0x00610D5C
		public static QuestPart_AddContentsToShuttle AddContentsToShuttle(this Quest quest, Thing shuttle, IEnumerable<Thing> contents, string inSignal = null)
		{
			if (contents == null)
			{
				return null;
			}
			QuestPart_AddContentsToShuttle questPart_AddContentsToShuttle = new QuestPart_AddContentsToShuttle();
			questPart_AddContentsToShuttle.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AddContentsToShuttle.shuttle = shuttle;
			questPart_AddContentsToShuttle.Things = contents;
			quest.AddPart(questPart_AddContentsToShuttle);
			return questPart_AddContentsToShuttle;
		}

		// Token: 0x0601451E RID: 83230 RVA: 0x00612BAC File Offset: 0x00610DAC
		public static QuestPart_ShuttleLeaveDelay ShuttleLeaveDelay(this Quest quest, Thing shuttle, int delayTicks, string inSignalEnable = null, IEnumerable<string> inSignalsDisable = null, string outSignalComplete = null, Action complete = null)
		{
			QuestPart_ShuttleLeaveDelay questPart_ShuttleLeaveDelay = new QuestPart_ShuttleLeaveDelay();
			questPart_ShuttleLeaveDelay.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleLeaveDelay.delayTicks = delayTicks;
			questPart_ShuttleLeaveDelay.shuttle = shuttle;
			questPart_ShuttleLeaveDelay.expiryInfoPart = "ShuttleDepartsIn".Translate();
			questPart_ShuttleLeaveDelay.expiryInfoPartTip = "ShuttleDepartsOn".Translate();
			if (inSignalsDisable != null)
			{
				foreach (string item in inSignalsDisable)
				{
					questPart_ShuttleLeaveDelay.inSignalsDisable.Add(item);
				}
			}
			if (!outSignalComplete.NullOrEmpty())
			{
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(outSignalComplete);
			}
			if (complete != null)
			{
				string text = QuestGen.GenerateNewSignal("ShuttleLeaveDelay", true);
				QuestGenUtility.RunInner(complete, text);
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(text);
			}
			quest.AddPart(questPart_ShuttleLeaveDelay);
			return questPart_ShuttleLeaveDelay;
		}

		// Token: 0x0601451F RID: 83231 RVA: 0x00612C9C File Offset: 0x00610E9C
		public static QuestPart_ShuttleDelay ShuttleDelay(this Quest quest, int delayTicks, IEnumerable<Pawn> lodgers, Action complete = null, string inSignalEnable = null, IEnumerable<string> inSignalsDisable = null, bool alert = false)
		{
			QuestPart_ShuttleDelay questPart_ShuttleDelay = new QuestPart_ShuttleDelay();
			questPart_ShuttleDelay.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleDelay.delayTicks = delayTicks;
			questPart_ShuttleDelay.alert = alert;
			if (lodgers != null)
			{
				questPart_ShuttleDelay.lodgers.AddRange(lodgers);
			}
			questPart_ShuttleDelay.expiryInfoPart = "ShuttleArrivesIn".Translate();
			questPart_ShuttleDelay.expiryInfoPartTip = "ShuttleArrivesOn".Translate();
			if (complete != null)
			{
				string text = QuestGen.GenerateNewSignal("ShuttleDelay", true);
				QuestGenUtility.RunInner(complete, text);
				questPart_ShuttleDelay.outSignalsCompleted.Add(text);
			}
			quest.AddPart(questPart_ShuttleDelay);
			return questPart_ShuttleDelay;
		}

		// Token: 0x06014520 RID: 83232 RVA: 0x00612D40 File Offset: 0x00610F40
		public static QuestPart_RequirePawnsCurrentlyOnShuttle RequirePawnsCurrentlyOnShuttle(this Quest quest, Thing shuttle, int requiredColonistCount = 0, string inSignal = null)
		{
			QuestPart_RequirePawnsCurrentlyOnShuttle questPart_RequirePawnsCurrentlyOnShuttle = new QuestPart_RequirePawnsCurrentlyOnShuttle
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shuttle = shuttle,
				requiredColonistCount = requiredColonistCount
			};
			quest.AddPart(questPart_RequirePawnsCurrentlyOnShuttle);
			return questPart_RequirePawnsCurrentlyOnShuttle;
		}

		// Token: 0x06014521 RID: 83233 RVA: 0x00612D88 File Offset: 0x00610F88
		public static QuestPart_RequiredShuttleThings RequiredShuttleThings(this Quest quest, Thing shuttle, MapParent map, string inSignal = null, bool requireAllColonistsOnMap = false, int requiredColonistCount = -1)
		{
			QuestPart_RequiredShuttleThings questPart_RequiredShuttleThings = new QuestPart_RequiredShuttleThings();
			questPart_RequiredShuttleThings.mapParent = map;
			questPart_RequiredShuttleThings.shuttle = shuttle;
			questPart_RequiredShuttleThings.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_RequiredShuttleThings.requireAllColonistsOnMap = requireAllColonistsOnMap;
			questPart_RequiredShuttleThings.requiredColonistCount = requiredColonistCount;
			quest.AddPart(questPart_RequiredShuttleThings);
			return questPart_RequiredShuttleThings;
		}

		// Token: 0x06014522 RID: 83234 RVA: 0x00612DE0 File Offset: 0x00610FE0
		public static QuestPart_PawnsNoLongerRequiredForShuttleOnRescue RemoveFromRequiredPawnsOnRescue(this Quest quest, Thing shuttle, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_PawnsNoLongerRequiredForShuttleOnRescue questPart_PawnsNoLongerRequiredForShuttleOnRescue = new QuestPart_PawnsNoLongerRequiredForShuttleOnRescue();
			questPart_PawnsNoLongerRequiredForShuttleOnRescue.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PawnsNoLongerRequiredForShuttleOnRescue.shuttle = shuttle;
			if (pawns != null)
			{
				questPart_PawnsNoLongerRequiredForShuttleOnRescue.pawns.AddRange(pawns);
			}
			quest.AddPart(questPart_PawnsNoLongerRequiredForShuttleOnRescue);
			return questPart_PawnsNoLongerRequiredForShuttleOnRescue;
		}
	}
}
