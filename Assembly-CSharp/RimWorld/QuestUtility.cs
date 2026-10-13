using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D09 RID: 7433
	public static class QuestUtility
	{
		// Token: 0x0600A612 RID: 42514 RVA: 0x002F8B2C File Offset: 0x002F6D2C
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return QuestUtility.GenerateQuestAndMakeAvailable(root, slate);
		}

		// Token: 0x0600A613 RID: 42515 RVA: 0x002F8B54 File Offset: 0x002F6D54
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, Slate vars)
		{
			Quest quest = QuestGen.Generate(root, vars);
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x0600A614 RID: 42516 RVA: 0x002F8B78 File Offset: 0x002F6D78
		public static void SendLetterQuestAvailable(Quest quest, string discoveryMethod = null)
		{
			TaggedString label = QuestUtility.QuestAvailableLetterLabel(quest);
			TaggedString text = QuestUtility.QuestAvailableLetterText(quest, discoveryMethod);
			QuestScriptDef root = quest.root;
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, ((root != null) ? root.questAvailableLetterDef : null) ?? IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest, null);
			choiceLetter.title = quest.name;
			Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
		}

		// Token: 0x0600A615 RID: 42517 RVA: 0x002F8BDC File Offset: 0x002F6DDC
		private static TaggedString QuestAvailableLetterLabel(Quest quest)
		{
			if (quest.root != null && !quest.root.questAvailableLetterLabel.NullOrEmpty())
			{
				return quest.root.questAvailableLetterLabel;
			}
			if (quest.initiallyAccepted)
			{
				return "LetterLabelQuestAutomaticallyAcceptedTitle".Translate(quest.name);
			}
			return "LetterLabelQuestAvailableTitle".Translate(quest.name);
		}

		// Token: 0x0600A616 RID: 42518 RVA: 0x002F8C48 File Offset: 0x002F6E48
		private static TaggedString QuestAvailableLetterText(Quest quest, string discoveryMethod = null)
		{
			TaggedString taggedString;
			if (quest.root != null && quest.root.questAvailableLetterTextIsDescription)
			{
				taggedString = quest.description;
			}
			else
			{
				if (!discoveryMethod.NullOrEmpty())
				{
					taggedString = "LetterNewQuest".Translate(discoveryMethod.Named("DISCOVEREDFROM"));
				}
				else
				{
					taggedString = "LetterNewQuestFromUnknown".Translate();
				}
				taggedString += "\n\n" + "LetterQuestIsNamed".Translate(quest.name);
			}
			if (quest.initiallyAccepted)
			{
				int questTicksRemaining = QuestUtility.GetQuestTicksRemaining(quest);
				if (questTicksRemaining > 0)
				{
					taggedString += "\n\n" + "LetterQuestActiveNowTime".Translate(questTicksRemaining.ToStringTicksToPeriod(false, false, false, true, false));
				}
			}
			else if (quest.TicksUntilExpiry >= 0)
			{
				taggedString += "\n\n" + "LetterQuestRequiresAcceptance".Translate(quest.TicksUntilExpiry.ToStringTicksToPeriod(false, false, false, true, false));
			}
			return taggedString;
		}

		// Token: 0x0600A617 RID: 42519 RVA: 0x002F8D40 File Offset: 0x002F6F40
		public static int GetQuestTicksRemaining(Quest quest)
		{
			foreach (QuestPart questPart in quest.PartsListForReading)
			{
				QuestPart_Delay questPart_Delay = questPart as QuestPart_Delay;
				if (questPart_Delay != null && questPart_Delay.State == QuestPartState.Enabled && questPart_Delay.isBad && !questPart_Delay.expiryInfoPart.NullOrEmpty())
				{
					return questPart_Delay.TicksLeft;
				}
			}
			return 0;
		}

		// Token: 0x0600A618 RID: 42520 RVA: 0x002F8DC0 File Offset: 0x002F6FC0
		public static void GenerateBackCompatibilityNameFor(Quest quest)
		{
			quest.name = NameGenerator.GenerateName(RulePackDefOf.NamerQuestDefault, from x in Find.QuestManager.QuestsListForReading
			select x.name, false, "defaultQuestName");
		}

		// Token: 0x0600A619 RID: 42521 RVA: 0x002F8E14 File Offset: 0x002F7014
		public static bool CanPawnAcceptQuest(Pawn p, Quest quest)
		{
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null && !questPart_RequirementsToAccept.CanPawnAccept(p))
				{
					return false;
				}
			}
			return !p.Destroyed && p.IsFreeColonist && !p.Downed && !p.Suspended && !p.IsQuestLodger();
		}

		// Token: 0x0600A61A RID: 42522 RVA: 0x002F8E84 File Offset: 0x002F7084
		public static AcceptanceReport CanAcceptQuest(Quest quest)
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			if (!Current.Game.PlayerHasControl)
			{
				return false;
			}
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null)
				{
					AcceptanceReport result = questPart_RequirementsToAccept.CanAccept();
					if (!result.Accepted)
					{
						return result;
					}
				}
			}
			return true;
		}

		// Token: 0x0600A61B RID: 42523 RVA: 0x002F8EF8 File Offset: 0x002F70F8
		public static Vector2 GetLocForDates()
		{
			if (Find.AnyPlayerHomeMap != null)
			{
				return Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
			}
			return default(Vector2);
		}

		// Token: 0x0600A61C RID: 42524 RVA: 0x002F8F2C File Offset: 0x002F712C
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, default(SignalArgs));
		}

		// Token: 0x0600A61D RID: 42525 RVA: 0x002F8F49 File Offset: 0x002F7149
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1));
		}

		// Token: 0x0600A61E RID: 42526 RVA: 0x002F8F58 File Offset: 0x002F7158
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2));
		}

		// Token: 0x0600A61F RID: 42527 RVA: 0x002F8F68 File Offset: 0x002F7168
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3));
		}

		// Token: 0x0600A620 RID: 42528 RVA: 0x002F8F7A File Offset: 0x002F717A
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3, arg4));
		}

		// Token: 0x0600A621 RID: 42529 RVA: 0x002F8F8E File Offset: 0x002F718E
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument[] args)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(args));
		}

		// Token: 0x0600A622 RID: 42530 RVA: 0x002F8FA0 File Offset: 0x002F71A0
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, SignalArgs args)
		{
			if (questTags == null)
			{
				return;
			}
			for (int i = 0; i < questTags.Count; i++)
			{
				Find.SignalManager.SendSignal(new Signal(questTags[i] + "." + signalPart, args, false));
			}
		}

		// Token: 0x0600A623 RID: 42531 RVA: 0x002F8FE5 File Offset: 0x002F71E5
		public static void AddQuestTag(ref List<string> questTags, string questTagToAdd)
		{
			if (questTags == null)
			{
				questTags = new List<string>();
			}
			if (questTags.Contains(questTagToAdd))
			{
				return;
			}
			questTags.Add(questTagToAdd);
		}

		// Token: 0x0600A624 RID: 42532 RVA: 0x002F9008 File Offset: 0x002F7208
		public static void AddQuestTag(object obj, string questTagToAdd)
		{
			if (questTagToAdd.NullOrEmpty())
			{
				return;
			}
			Thing thing = obj as Thing;
			if (thing != null)
			{
				QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				return;
			}
			WorldObject worldObject = obj as WorldObject;
			if (worldObject != null)
			{
				QuestUtility.AddQuestTag(ref worldObject.questTags, questTagToAdd);
				return;
			}
			Map map = obj as Map;
			if (map != null)
			{
				QuestUtility.AddQuestTag(ref map.Parent.questTags, questTagToAdd);
				return;
			}
			Lord lord = obj as Lord;
			if (lord != null)
			{
				QuestUtility.AddQuestTag(ref lord.questTags, questTagToAdd);
				return;
			}
			Faction faction = obj as Faction;
			if (faction != null)
			{
				QuestUtility.AddQuestTag(ref faction.questTags, questTagToAdd);
				return;
			}
			TransportShip transportShip = obj as TransportShip;
			if (transportShip != null)
			{
				QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
				return;
			}
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable != null)
			{
				foreach (object obj2 in enumerable)
				{
					Thing thing2 = obj2 as Thing;
					if (thing2 != null)
					{
						QuestUtility.AddQuestTag(ref thing2.questTags, questTagToAdd);
					}
					else
					{
						WorldObject worldObject2 = obj2 as WorldObject;
						if (worldObject2 != null)
						{
							QuestUtility.AddQuestTag(ref worldObject2.questTags, questTagToAdd);
						}
						else
						{
							Map map2 = obj2 as Map;
							if (map2 != null)
							{
								QuestUtility.AddQuestTag(ref map2.Parent.questTags, questTagToAdd);
							}
							else
							{
								Faction faction2 = obj2 as Faction;
								if (faction2 != null)
								{
									QuestUtility.AddQuestTag(ref faction2.questTags, questTagToAdd);
								}
								else
								{
									TransportShip transportShip2 = enumerable as TransportShip;
									if (transportShip2 != null)
									{
										QuestUtility.AddQuestTag(ref transportShip2.questTags, questTagToAdd);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600A625 RID: 42533 RVA: 0x002F919C File Offset: 0x002F739C
		public static bool TryGetIdealColonist(out Pawn pawn, Map idealMap = null, Func<Pawn, bool> validator = null)
		{
			QuestUtility.tmpPawns.Clear();
			if (idealMap != null)
			{
				for (int i = 0; i <= 3; i++)
				{
					QuestUtility.CacheIdealColonists(idealMap.mapPawns.AllHumanlikeSpawned, i, QuestUtility.tmpPawns, validator);
					if (!QuestUtility.tmpPawns.Empty<Pawn>())
					{
						pawn = QuestUtility.tmpPawns.RandomElement<Pawn>();
						QuestUtility.tmpPawns.Clear();
						return true;
					}
				}
			}
			for (int j = 0; j <= 3; j++)
			{
				foreach (Map map in Find.Maps)
				{
					if (idealMap == null || idealMap != map)
					{
						QuestUtility.CacheIdealColonists(map.mapPawns.AllHumanlikeSpawned, j, QuestUtility.tmpPawns, validator);
					}
				}
				if (!QuestUtility.tmpPawns.Empty<Pawn>())
				{
					pawn = QuestUtility.tmpPawns.RandomElement<Pawn>();
					QuestUtility.tmpPawns.Clear();
					return true;
				}
			}
			for (int k = 0; k <= 3; k++)
			{
				QuestUtility.CacheIdealColonists(Find.WorldPawns.AllPawnsAlive, k, QuestUtility.tmpPawns, validator);
				if (!QuestUtility.tmpPawns.Empty<Pawn>())
				{
					pawn = QuestUtility.tmpPawns.RandomElement<Pawn>();
					QuestUtility.tmpPawns.Clear();
					return true;
				}
			}
			QuestUtility.tmpPawns.Clear();
			pawn = null;
			return false;
		}

		// Token: 0x0600A626 RID: 42534 RVA: 0x002F92E4 File Offset: 0x002F74E4
		private static void CacheIdealColonists(List<Pawn> pawnsToScan, int level, List<Pawn> pawns, Func<Pawn, bool> validator = null)
		{
			foreach (Pawn pawn in pawnsToScan)
			{
				if (!pawn.DevelopmentalStage.Baby() && (validator == null || validator(pawn)) && (pawn.IsColonistPlayerControlled || (pawn.IsColonist && pawn.IsCaravanMember())) && (level > 2 || pawn.IsSlaveOfColony || pawn.IsColonist) && (level > 1 || pawn.ageTracker.AgeBiologicalYears >= 13) && (level > 0 || pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving)))
				{
					pawns.Add(pawn);
				}
			}
		}

		// Token: 0x0600A627 RID: 42535 RVA: 0x002F93AC File Offset: 0x002F75AC
		public static bool AnyMatchingTags(List<string> first, List<string> second)
		{
			if (first.NullOrEmpty<string>() || second.NullOrEmpty<string>())
			{
				return false;
			}
			for (int i = 0; i < first.Count; i++)
			{
				for (int j = 0; j < second.Count; j++)
				{
					if (first[i] == second[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600A628 RID: 42536 RVA: 0x002F9405 File Offset: 0x002F7605
		public static bool IsReservedByQuestOrQuestBeingGenerated(Pawn pawn)
		{
			return Find.QuestManager.IsReservedByAnyQuest(pawn) || (QuestGen.quest != null && QuestGen.quest.QuestReserves(pawn)) || QuestGen.WasGeneratedForQuestBeingGenerated(pawn);
		}

		// Token: 0x0600A629 RID: 42537 RVA: 0x002F9430 File Offset: 0x002F7630
		public static List<QuestPart_WorkDisabled> GetWorkDisabledQuestPart(Pawn p)
		{
			QuestUtility.tmpQuestWorkDisabled.Clear();
			List<Quest> activeQuestsListForReading = Find.QuestManager.ActiveQuestsListForReading;
			for (int i = 0; i < activeQuestsListForReading.Count; i++)
			{
				if (activeQuestsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = activeQuestsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_WorkDisabled questPart_WorkDisabled = partsListForReading[j] as QuestPart_WorkDisabled;
						if (questPart_WorkDisabled != null && questPart_WorkDisabled.pawns.Contains(p))
						{
							QuestUtility.tmpQuestWorkDisabled.Add(questPart_WorkDisabled);
						}
					}
				}
			}
			return QuestUtility.tmpQuestWorkDisabled;
		}

		// Token: 0x0600A62A RID: 42538 RVA: 0x002F94C2 File Offset: 0x002F76C2
		public static bool IsQuestLodger(this Pawn p)
		{
			return p.HasExtraHomeFaction(null) || p.HasExtraMiniFaction(null);
		}

		// Token: 0x0600A62B RID: 42539 RVA: 0x002F94D8 File Offset: 0x002F76D8
		public static bool IsQuestHelper(this Pawn p)
		{
			if (!p.IsQuestLodger())
			{
				return false;
			}
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_ExtraFaction questPart_ExtraFaction = partsListForReading[j] as QuestPart_ExtraFaction;
						if (questPart_ExtraFaction != null && questPart_ExtraFaction.affectedPawns.Contains(p) && questPart_ExtraFaction.areHelpers)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A62C RID: 42540 RVA: 0x002F9568 File Offset: 0x002F7768
		public static bool IsQuestReward(this Pawn pawn)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if ((questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing) && pawn.IsQuestReward(questsListForReading[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A62D RID: 42541 RVA: 0x002F95C0 File Offset: 0x002F77C0
		public static bool IsQuestReward(this Pawn pawn, Quest quest)
		{
			List<QuestPart> partsListForReading = quest.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				QuestPart_Choice questPart_Choice = partsListForReading[i] as QuestPart_Choice;
				if (questPart_Choice != null)
				{
					for (int j = 0; j < questPart_Choice.choices.Count; j++)
					{
						QuestPart_Choice.Choice choice = questPart_Choice.choices[j];
						for (int k = 0; k < choice.rewards.Count; k++)
						{
							Reward_Pawn reward_Pawn = choice.rewards[k] as Reward_Pawn;
							if (reward_Pawn != null && reward_Pawn.pawn == pawn)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A62E RID: 42542 RVA: 0x002F9660 File Offset: 0x002F7860
		public static bool LodgerAllowedDecrees(this Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_AllowDecreesForLodger questPart_AllowDecreesForLodger = partsListForReading[j] as QuestPart_AllowDecreesForLodger;
						if (questPart_AllowDecreesForLodger != null && questPart_AllowDecreesForLodger.lodger == p)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A62F RID: 42543 RVA: 0x002F96D5 File Offset: 0x002F78D5
		public static bool HasExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraHomeFaction(forQuest) != null;
		}

		// Token: 0x0600A630 RID: 42544 RVA: 0x002F96E1 File Offset: 0x002F78E1
		public static bool HasExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraMiniFaction(forQuest) != null;
		}

		// Token: 0x0600A631 RID: 42545 RVA: 0x002F96F0 File Offset: 0x002F78F0
		public static bool HasExtraHomeFaction(this Pawn p, Faction faction)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, null);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == ExtraFactionType.HomeFaction && QuestUtility.tmpExtraFactions[i].faction == faction)
				{
					QuestUtility.tmpExtraFactions.Clear();
					return true;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return false;
		}

		// Token: 0x0600A632 RID: 42546 RVA: 0x002F9764 File Offset: 0x002F7964
		public static bool HasExtraMiniFaction(this Pawn p, Faction faction)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, null);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == ExtraFactionType.MiniFaction && QuestUtility.tmpExtraFactions[i].faction == faction)
				{
					QuestUtility.tmpExtraFactions.Clear();
					return true;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return false;
		}

		// Token: 0x0600A633 RID: 42547 RVA: 0x002F97D9 File Offset: 0x002F79D9
		public static Faction GetExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HomeFaction, forQuest);
		}

		// Token: 0x0600A634 RID: 42548 RVA: 0x002F97E3 File Offset: 0x002F79E3
		public static Faction GetExtraHostFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HostFaction, forQuest);
		}

		// Token: 0x0600A635 RID: 42549 RVA: 0x002F97ED File Offset: 0x002F79ED
		public static Faction GetExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.MiniFaction, forQuest);
		}

		// Token: 0x0600A636 RID: 42550 RVA: 0x002F97F7 File Offset: 0x002F79F7
		public static bool InSameExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
		{
			return p.GetSharedExtraFaction(target, type, forQuest) != null;
		}

		// Token: 0x0600A637 RID: 42551 RVA: 0x002F9808 File Offset: 0x002F7A08
		public static Faction GetSharedExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
		{
			Faction extraFaction = p.GetExtraFaction(type, forQuest);
			Faction extraFaction2 = target.GetExtraFaction(type, forQuest);
			if (extraFaction != null && extraFaction == extraFaction2)
			{
				return extraFaction;
			}
			return null;
		}

		// Token: 0x0600A638 RID: 42552 RVA: 0x002F9834 File Offset: 0x002F7A34
		public static Faction GetExtraFaction(this Pawn p, ExtraFactionType extraFactionType, Quest forQuest = null)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, forQuest);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == extraFactionType)
				{
					Faction faction = QuestUtility.tmpExtraFactions[i].faction;
					QuestUtility.tmpExtraFactions.Clear();
					return faction;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return null;
		}

		// Token: 0x0600A639 RID: 42553 RVA: 0x002F98A8 File Offset: 0x002F7AA8
		public static void GetExtraFactionsFromQuestParts(Pawn pawn, List<ExtraFaction> outExtraFactions, Quest forQuest = null)
		{
			QuestUtility.<>c__DisplayClass116_0 CS$<>8__locals1;
			CS$<>8__locals1.outExtraFactions = outExtraFactions;
			CS$<>8__locals1.outExtraFactions.Clear();
			List<QuestPart_ExtraFaction> extraFactionQuestParts = Find.QuestManager.ExtraFactionQuestParts;
			for (int i = 0; i < extraFactionQuestParts.Count; i++)
			{
				ExtraFaction extraFaction = extraFactionQuestParts[i].extraFaction;
				if (((extraFaction != null) ? extraFaction.faction : null) != null)
				{
					Quest quest = extraFactionQuestParts[i].quest;
					if ((quest.State == QuestState.Ongoing || quest == forQuest || (quest.root == QuestScriptDefOf.Hospitality_Refugee && !pawn.IsColonist)) && !QuestUtility.<GetExtraFactionsFromQuestParts>g__AlreadyAdded|116_0(extraFactionQuestParts[i].extraFaction, ref CS$<>8__locals1) && extraFactionQuestParts[i].affectedPawns.Contains(pawn))
					{
						CS$<>8__locals1.outExtraFactions.Add(extraFactionQuestParts[i].extraFaction);
					}
				}
			}
		}

		// Token: 0x0600A63A RID: 42554 RVA: 0x002F9974 File Offset: 0x002F7B74
		public static bool IsBorrowedByAnyFaction(this Pawn pawn)
		{
			List<Quest> activeQuestsListForReading = Find.QuestManager.ActiveQuestsListForReading;
			int count = activeQuestsListForReading.Count;
			for (int i = 0; i < count; i++)
			{
				if (activeQuestsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = activeQuestsListForReading[i].PartsListForReading;
					int count2 = partsListForReading.Count;
					for (int j = 0; j < count2; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = partsListForReading[j] as QuestPart_LendColonistsToFaction;
						if (questPart_LendColonistsToFaction != null && questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A63B RID: 42555 RVA: 0x002F99FC File Offset: 0x002F7BFC
		public static int TotalBorrowedColonistCount()
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			int num = 0;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = partsListForReading[j] as QuestPart_LendColonistsToFaction;
						if (questPart_LendColonistsToFaction != null)
						{
							num += questPart_LendColonistsToFaction.LentColonistsListForReading.Count;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600A63C RID: 42556 RVA: 0x002F9A7B File Offset: 0x002F7C7B
		public static IEnumerable<T> GetAllQuestPartsOfType<T>(bool ongoingOnly = true) where T : class
		{
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (!ongoingOnly || quests[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partList = quests[i].PartsListForReading;
					for (int j = 0; j < partList.Count; j = num + 1)
					{
						T t = partList[j] as T;
						if (t != null)
						{
							yield return t;
						}
						num = j;
					}
					partList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600A63D RID: 42557 RVA: 0x002F9A8C File Offset: 0x002F7C8C
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target)
		{
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(sb, target, out num);
		}

		// Token: 0x0600A63E RID: 42558 RVA: 0x002F9AA2 File Offset: 0x002F7CA2
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target, out int count)
		{
			QuestUtility.AppendInspectStringsFromQuestParts(delegate(string str, Quest quest)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(str);
			}, target, out count);
		}

		// Token: 0x0600A63F RID: 42559 RVA: 0x002F9AC4 File Offset: 0x002F7CC4
		public static void AppendInspectStringsFromQuestParts(Action<string, Quest> func, ISelectable target, out int count)
		{
			count = 0;
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					QuestUtility.tmpQuestParts.Clear();
					QuestUtility.tmpQuestParts.AddRange(questsListForReading[i].PartsListForReading);
					QuestUtility.tmpQuestParts.SortBy(delegate(QuestPart x)
					{
						QuestPartActivable questPartActivable2 = x as QuestPartActivable;
						if (questPartActivable2 == null)
						{
							return 0;
						}
						return questPartActivable2.EnableTick;
					});
					for (int j = 0; j < QuestUtility.tmpQuestParts.Count; j++)
					{
						QuestPartActivable questPartActivable = QuestUtility.tmpQuestParts[j] as QuestPartActivable;
						if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
						{
							string str = questPartActivable.ExtraInspectString(target);
							if (!str.NullOrEmpty())
							{
								func(str.Formatted(target.Named("TARGET")), questsListForReading[i]);
								count++;
							}
						}
					}
					QuestUtility.tmpQuestParts.Clear();
				}
			}
		}

		// Token: 0x0600A640 RID: 42560 RVA: 0x002F9BC9 File Offset: 0x002F7DC9
		public static IEnumerable<Gizmo> GetQuestRelatedGizmos(Thing thing)
		{
			if (Find.Selector.SelectedObjects.Count == 1)
			{
				QuestUtility.<>c__DisplayClass124_0 CS$<>8__locals1 = new QuestUtility.<>c__DisplayClass124_0();
				CS$<>8__locals1.linkedQuest = null;
				List<Quest> quests = Find.QuestManager.QuestsListForReading;
				int num;
				for (int i = 0; i < quests.Count; i = num + 1)
				{
					if (!quests[i].hidden && !quests[i].Historical && !quests[i].dismissed)
					{
						if (quests[i].QuestLookTargets.Contains(thing) || quests[i].QuestSelectTargets.Contains(thing))
						{
							CS$<>8__locals1.linkedQuest = quests[i];
						}
						List<QuestPart> parts = quests[i].PartsListForReading;
						for (int j = 0; j < parts.Count; j = num + 1)
						{
							QuestPartActivable questPartActivable = parts[j] as QuestPartActivable;
							if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
							{
								IEnumerable<Gizmo> enumerable = questPartActivable.ExtraGizmos(thing);
								if (enumerable != null)
								{
									foreach (Gizmo gizmo in enumerable)
									{
										yield return gizmo;
									}
									IEnumerator<Gizmo> enumerator = null;
								}
							}
							num = j;
						}
						parts = null;
					}
					num = i;
				}
				if (CS$<>8__locals1.linkedQuest != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandOpenLinkedQuest".Translate(CS$<>8__locals1.linkedQuest.name),
						defaultDesc = "CommandOpenLinkedQuestDesc".Translate(),
						icon = TexCommand.OpenLinkedQuestTex,
						action = delegate()
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
							((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(CS$<>8__locals1.linkedQuest);
						}
					};
				}
				CS$<>8__locals1 = null;
				quests = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600A641 RID: 42561 RVA: 0x002F9BDC File Offset: 0x002F7DDC
		public static Gizmo GetSelectMonumentMarkerGizmo(Thing thing)
		{
			if (!thing.Spawned || !ModsConfig.RoyaltyActive)
			{
				return null;
			}
			List<Thing> list = thing.Map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
			for (int i = 0; i < list.Count; i++)
			{
				MonumentMarker m = (MonumentMarker)list[i];
				if (m.IsPart(thing))
				{
					return new Command_Action
					{
						defaultLabel = "CommandSelectMonumentMarker".Translate(),
						defaultDesc = "CommandSelectMonumentMarkerDesc".Translate(),
						icon = ThingDefOf.MonumentMarker.uiIcon,
						iconAngle = ThingDefOf.MonumentMarker.uiIconAngle,
						iconOffset = ThingDefOf.MonumentMarker.uiIconOffset,
						action = delegate()
						{
							CameraJumper.TrySelect(m, false);
						}
					};
				}
			}
			return null;
		}

		// Token: 0x0600A642 RID: 42562 RVA: 0x002F9CC0 File Offset: 0x002F7EC0
		public static bool AnyQuestDisablesRandomMoodCausedMentalBreaksFor(Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks = partsListForReading[j] as QuestPart_DisableRandomMoodCausedMentalBreaks;
						if (questPart_DisableRandomMoodCausedMentalBreaks != null && questPart_DisableRandomMoodCausedMentalBreaks.State == QuestPartState.Enabled && questPart_DisableRandomMoodCausedMentalBreaks.pawns.Contains(p))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A643 RID: 42563 RVA: 0x002F9D44 File Offset: 0x002F7F44
		public static IEnumerable<Quest> GetSubquests(this Quest quest, QuestState? state = null)
		{
			List<Quest> allQuests = Find.QuestManager.questsInDisplayOrder;
			int num;
			for (int i = 0; i < allQuests.Count; i = num + 1)
			{
				if (allQuests[i].parent == quest)
				{
					if (state != null)
					{
						QuestState state2 = allQuests[i].State;
						QuestState? questState = state;
						if (!(state2 == questState.GetValueOrDefault() & questState != null))
						{
							goto IL_B8;
						}
					}
					yield return allQuests[i];
				}
				IL_B8:
				num = i;
			}
			yield break;
		}

		// Token: 0x0600A644 RID: 42564 RVA: 0x002F9D5B File Offset: 0x002F7F5B
		public static bool IsSubquestOf(this Quest quest, Quest parent)
		{
			return quest.parent != null && quest.parent == parent;
		}

		// Token: 0x0600A645 RID: 42565 RVA: 0x002F9D70 File Offset: 0x002F7F70
		public static bool IsGoodwillLockedByQuest(Faction a, Faction b)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_FactionGoodwillLocked questPart_FactionGoodwillLocked = partsListForReading[j] as QuestPart_FactionGoodwillLocked;
						if (questPart_FactionGoodwillLocked != null && questPart_FactionGoodwillLocked.State == QuestPartState.Enabled && questPart_FactionGoodwillLocked.AppliesTo(a, b))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A646 RID: 42566 RVA: 0x002F9DF0 File Offset: 0x002F7FF0
		public static bool IsEndOnNewArchonexusSettlement(this Quest quest)
		{
			return quest.root.endOnColonyMove && (quest.State == QuestState.NotYetAccepted || quest.State == QuestState.Ongoing);
		}

		// Token: 0x0600A647 RID: 42567 RVA: 0x002F9E14 File Offset: 0x002F8014
		public static IEnumerable<QuestScriptDef> GetGiverQuests(QuestGiverTag tag)
		{
			if (!ModsConfig.OdysseyActive)
			{
				yield break;
			}
			IEnumerable<QuestScriptDef> allDefs = DefDatabase<QuestScriptDef>.AllDefs;
			Func<QuestScriptDef, bool> <>9__0;
			Func<QuestScriptDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((QuestScriptDef x) => x.givenBy.Contains(tag)));
			}
			foreach (QuestScriptDef questScriptDef in allDefs.Where(predicate))
			{
				yield return questScriptDef;
			}
			IEnumerator<QuestScriptDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600A649 RID: 42569 RVA: 0x002F9E50 File Offset: 0x002F8050
		[CompilerGenerated]
		internal static bool <GetExtraFactionsFromQuestParts>g__AlreadyAdded|116_0(ExtraFaction extraFaction, ref QuestUtility.<>c__DisplayClass116_0 A_1)
		{
			for (int i = 0; i < A_1.outExtraFactions.Count; i++)
			{
				if (A_1.outExtraFactions[i].faction == extraFaction.faction && A_1.outExtraFactions[i].factionType == extraFaction.factionType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040072CE RID: 29390
		public const string QuestTargetSignalPart_MapGenerated = "MapGenerated";

		// Token: 0x040072CF RID: 29391
		public const string QuestTargetSignalPart_MapRemoved = "MapRemoved";

		// Token: 0x040072D0 RID: 29392
		public const string QuestTargetSignalPart_MapSettled = "MapSettled";

		// Token: 0x040072D1 RID: 29393
		public const string QuestTargetSignalPart_Spawned = "Spawned";

		// Token: 0x040072D2 RID: 29394
		public const string QuestTargetSignalPart_Despawned = "Despawned";

		// Token: 0x040072D3 RID: 29395
		public const string QuestTargetSignalPart_Destroyed = "Destroyed";

		// Token: 0x040072D4 RID: 29396
		public const string QuestTargetSignalPart_Killed = "Killed";

		// Token: 0x040072D5 RID: 29397
		public const string QuestTargetSignalPart_TookDamage = "TookDamage";

		// Token: 0x040072D6 RID: 29398
		public const string QuestTargetSignalPart_TookDamageFromPlayer = "TookDamageFromPlayer";

		// Token: 0x040072D7 RID: 29399
		public const string QuestTargetSignalPart_ChangedFaction = "ChangedFaction";

		// Token: 0x040072D8 RID: 29400
		public const string QuestTargetSignalPart_ChangedFactionToPlayer = "ChangedFactionToPlayer";

		// Token: 0x040072D9 RID: 29401
		public const string QuestTargetSignalPart_ChangedFactionToNonPlayer = "ChangedFactionToNonPlayer";

		// Token: 0x040072DA RID: 29402
		public const string QuestTargetSignalPart_Hacked = "Hacked";

		// Token: 0x040072DB RID: 29403
		public const string QuestTargetSignalPart_HackingStarted = "HackingStarted";

		// Token: 0x040072DC RID: 29404
		public const string QuestTargetSignalPart_LockedOut = "LockedOut";

		// Token: 0x040072DD RID: 29405
		public const string QuestTargetSignalPart_StartedExtractingFromContainer = "StartedExtractingFromContainer";

		// Token: 0x040072DE RID: 29406
		public const string QuestTargetSignalPart_Studied = "Researched";

		// Token: 0x040072DF RID: 29407
		public const string QuestTargetSignalPart_KilledLeavingsLeft = "KilledLeavingsLeft";

		// Token: 0x040072E0 RID: 29408
		public const string QuestTargetSignalPart_Unfogged = "Unfogged";

		// Token: 0x040072E1 RID: 29409
		public const string QuestTargetSignalPart_Inspected = "Inspected";

		// Token: 0x040072E2 RID: 29410
		public const string QuestTargetSignalPart_SwappedMap = "SwappedMap";

		// Token: 0x040072E3 RID: 29411
		public const string QuestTargetSignalPart_LeftBehind = "LeftBehind";

		// Token: 0x040072E4 RID: 29412
		public const string QuestTargetSignalPart_LeftMap = "LeftMap";

		// Token: 0x040072E5 RID: 29413
		public const string QuestTargetSignalPart_SurgeryViolation = "SurgeryViolation";

		// Token: 0x040072E6 RID: 29414
		public const string QuestTargetSignalPart_Arrested = "Arrested";

		// Token: 0x040072E7 RID: 29415
		public const string QuestTargetSignalPart_Released = "Released";

		// Token: 0x040072E8 RID: 29416
		public const string QuestTargetSignalPart_Recruited = "Recruited";

		// Token: 0x040072E9 RID: 29417
		public const string QuestTargetSignalPart_Kidnapped = "Kidnapped";

		// Token: 0x040072EA RID: 29418
		public const string QuestTargetSignalPart_ChangedHostFaction = "ChangedHostFaction";

		// Token: 0x040072EB RID: 29419
		public const string QuestTargetSignalPart_NoLongerFactionLeader = "NoLongerFactionLeader";

		// Token: 0x040072EC RID: 29420
		public const string QuestTargetSignalPart_TitleChanged = "TitleChanged";

		// Token: 0x040072ED RID: 29421
		public const string QuestTargetSignalPart_TitleAwardedWhenUpdatingChanged = "TitleAwardedWhenUpdatingChanged";

		// Token: 0x040072EE RID: 29422
		public const string QuestTargetSignalPart_Banished = "Banished";

		// Token: 0x040072EF RID: 29423
		public const string QuestTargetSignalPart_Rescued = "Rescued";

		// Token: 0x040072F0 RID: 29424
		public const string QuestTargetSignalPart_RanWild = "RanWild";

		// Token: 0x040072F1 RID: 29425
		public const string QuestTargetSignalPart_Enslaved = "Enslaved";

		// Token: 0x040072F2 RID: 29426
		public const string QuestTargetSignalPart_PlayerTended = "PlayerTended";

		// Token: 0x040072F3 RID: 29427
		public const string QuestTargetSignalPart_XenogermAbsorbed = "XenogermAbsorbed";

		// Token: 0x040072F4 RID: 29428
		public const string QuestTargetSignalPart_XenogermReimplanted = "XenogermReimplanted";

		// Token: 0x040072F5 RID: 29429
		public const string QuestTargetSignalPart_BecameMutant = "BecameMutant";

		// Token: 0x040072F6 RID: 29430
		public const string QuestTargetSignalPart_PsychicRitualTarget = "PsychicRitualTarget";

		// Token: 0x040072F7 RID: 29431
		public const string QuestTargetSignalPart_ReceivedItems = "ReceivedItems";

		// Token: 0x040072F8 RID: 29432
		public const string QuestTargetSignalPart_ShuttleSentSatisfied = "SentSatisfied";

		// Token: 0x040072F9 RID: 29433
		public const string QuestTargetSignalPart_ShuttleSentUnsatisfied = "SentUnsatisfied";

		// Token: 0x040072FA RID: 29434
		public const string QuestTargetSignalPart_ShuttleSentWithExtraColonists = "SentWithExtraColonists";

		// Token: 0x040072FB RID: 29435
		public const string QuestTargetSignalPart_ShuttleUnloaded = "Unloaded";

		// Token: 0x040072FC RID: 29436
		public const string QuestTargetSignalPart_AllEnemiesDefeated = "AllEnemiesDefeated";

		// Token: 0x040072FD RID: 29437
		public const string QuestTargetSignalPart_TradeRequestFulfilled = "TradeRequestFulfilled";

		// Token: 0x040072FE RID: 29438
		public const string QuestTargetSignalPart_PeaceTalksResolved = "Resolved";

		// Token: 0x040072FF RID: 29439
		public const string QuestTargetSignalPart_LaunchedShip = "LaunchedShip";

		// Token: 0x04007300 RID: 29440
		public const string QuestTargetSignalPart_ReactorDestroyed = "ReactorDestroyed";

		// Token: 0x04007301 RID: 29441
		public const string QuestTargetSignalPart_MonumentCompleted = "MonumentCompleted";

		// Token: 0x04007302 RID: 29442
		public const string QuestTargetSignalPart_MonumentDestroyed = "MonumentDestroyed";

		// Token: 0x04007303 RID: 29443
		public const string QuestTargetSignalPart_MonumentCancelled = "MonumentCancelled";

		// Token: 0x04007304 RID: 29444
		public const string QuestTargetSignalPart_AllHivesDestroyed = "AllHivesDestroyed";

		// Token: 0x04007305 RID: 29445
		public const string QuestTargetSignalPart_ExitMentalState = "ExitMentalState";

		// Token: 0x04007306 RID: 29446
		public const string QuestTargetSignalPart_FactionBecameHostileToPlayer = "BecameHostileToPlayer";

		// Token: 0x04007307 RID: 29447
		public const string QuestTargetSignalPart_FactionBuiltBuilding = "BuiltBuilding";

		// Token: 0x04007308 RID: 29448
		public const string QuestTargetSignalPart_FactionPlacedBlueprint = "PlacedBlueprint";

		// Token: 0x04007309 RID: 29449
		public const string QuestTargetSignalPart_FactionMemberArrested = "FactionMemberArrested";

		// Token: 0x0400730A RID: 29450
		public const string QuestTargetSignalPart_CeremonyExpired = "CeremonyExpired";

		// Token: 0x0400730B RID: 29451
		public const string QuestTargetSignalPart_CeremonyFailed = "CeremonyFailed";

		// Token: 0x0400730C RID: 29452
		public const string QuestTargetSignalPart_CeremonyDone = "CeremonyDone";

		// Token: 0x0400730D RID: 29453
		public const string QuestTargetSignalPart_BeingAttacked = "BeingAttacked";

		// Token: 0x0400730E RID: 29454
		public const string QuestTargetSignalPart_Fleeing = "Fleeing";

		// Token: 0x0400730F RID: 29455
		public const string QuestTargetSignalPart_QuestEnded = "QuestEnded";

		// Token: 0x04007310 RID: 29456
		public const string QuestTargetSignalPart_AllPawnsLost = "AllPawnsLost";

		// Token: 0x04007311 RID: 29457
		public const string QuestTargetSignalPart_ShipDisposed = "Disposed";

		// Token: 0x04007312 RID: 29458
		public const string QuestTargetSignalPart_ShipArrived = "Arrived";

		// Token: 0x04007313 RID: 29459
		public const string QuestTargetSignalPart_ShipFlewAway = "FlewAway";

		// Token: 0x04007314 RID: 29460
		public const string QuestTargetSignalPart_ShipThingAdded = "ThingAdded";

		// Token: 0x04007315 RID: 29461
		public const string QuestTargetSignalPart_Activated = "Activated";

		// Token: 0x04007316 RID: 29462
		public const string QuestTargetSignalPart_NodeClosed = "NodeClosed";

		// Token: 0x04007317 RID: 29463
		public const string QuestTargetSignalPart_CoreDefeated = "CoreDefeated";

		// Token: 0x04007318 RID: 29464
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04007319 RID: 29465
		private static List<QuestPart_WorkDisabled> tmpQuestWorkDisabled = new List<QuestPart_WorkDisabled>();

		// Token: 0x0400731A RID: 29466
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x0400731B RID: 29467
		private static List<QuestPart> tmpQuestParts = new List<QuestPart>();
	}
}
