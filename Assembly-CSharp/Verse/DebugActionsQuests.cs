using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.QuestGen;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000996 RID: 2454
	public static class DebugActionsQuests
	{
		// Token: 0x06003E5A RID: 15962 RVA: 0x00146760 File Offset: 0x00144960
		[DebugAction("Quests", "Generate quest", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> GenerateQuest()
		{
			return DebugActionsQuests.GenerateQuests(1, false);
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x0014676C File Offset: 0x0014496C
		[DebugAction("Quests", "Generate quests x#", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> GenerateQuests()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < DebugActionsQuests.QuestNumOptions.Length; i++)
			{
				int count = DebugActionsQuests.QuestNumOptions[i];
				DebugActionNode debugActionNode = new DebugActionNode(count.ToString(), DebugActionType.Action, null, null);
				foreach (DebugActionNode child in DebugActionsQuests.GenerateQuests(count, false))
				{
					debugActionNode.AddChild(child);
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x001467FC File Offset: 0x001449FC
		[DebugAction("Quests", "Generate quests (1x for each points)", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> GenerateQuestsSamples()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			using (IEnumerator<QuestScriptDef> enumerator = (from x in DefDatabase<QuestScriptDef>.AllDefs
			where x.IsRootAny
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestScriptDef scriptDef = enumerator.Current;
					list.Add(new DebugActionNode(scriptDef.defName, DebugActionType.Action, delegate()
					{
						foreach (float num in DebugActionsUtility.PointsOptions(false))
						{
							try
							{
								QuestScriptDef scriptDef = scriptDef;
								float points = num;
								IIncidentTarget randomPlayerHomeMap = Find.RandomPlayerHomeMap;
								if (!scriptDef.CanRun(points, randomPlayerHomeMap ?? Find.World))
								{
									Log.Error(string.Concat(new string[]
									{
										"Cannot generate quest ",
										scriptDef.defName,
										" for ",
										num.ToString(),
										" points!"
									}));
								}
								else
								{
									Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(scriptDef, num);
									quest.name = num.ToString() + ": " + quest.name;
								}
							}
							catch (Exception ex)
							{
								QuestScriptDef scriptDef;
								Log.Error(string.Concat(new string[]
								{
									"Exception generating quest ",
									scriptDef.defName,
									" for ",
									num.ToString(),
									" points!\n\n",
									ex.Message,
									"\n-------------\n",
									ex.StackTrace
								}));
							}
						}
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x0014689C File Offset: 0x00144A9C
		private static List<DebugActionNode> GenerateQuests(int count, bool logDescOnly = false)
		{
			DebugActionsQuests.<>c__DisplayClass5_0 CS$<>8__locals1 = new DebugActionsQuests.<>c__DisplayClass5_0();
			CS$<>8__locals1.count = count;
			CS$<>8__locals1.logDescOnly = logDescOnly;
			List<DebugActionNode> list = new List<DebugActionNode>();
			list.Add(new DebugActionNode("*Natural random", DebugActionType.Action, delegate()
			{
				Slate slate = new Slate();
				QuestScriptDef script = NaturalRandomQuestChooser.ChooseNaturalRandomQuest(slate.Get<float>("points", 0f, false), Find.CurrentMap);
				base.<GenerateQuests>g__GetQuest|0(script, slate);
			}, null));
			foreach (QuestScriptDef questScriptDef in from x in DefDatabase<QuestScriptDef>.AllDefs
			where x.IsRootAny
			select x)
			{
				QuestScriptDef localScriptDef = questScriptDef;
				DebugActionNode debugActionNode = new DebugActionNode(localScriptDef.defName, DebugActionType.Action, null, null);
				Slate slate = new Slate();
				slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromDebug".Translate(), false);
				if (localScriptDef.affectedByPoints && localScriptDef.affectedByPopulation)
				{
					debugActionNode.childGetter = delegate()
					{
						List<DebugActionNode> list2 = new List<DebugActionNode>();
						foreach (float num in DebugActionsUtility.PointsOptions(false))
						{
							string text = string.Format("{0:F0} points", num);
							QuestScriptDef localScriptDef;
							Slate slate;
							DebugActionsQuests.<GenerateQuests>g__GetSlateValuesForPoints|5_1(localScriptDef, slate, num);
							localScriptDef = localScriptDef;
							slate = slate;
							IIncidentTarget currentMap = Find.CurrentMap;
							if (!localScriptDef.CanRun(slate, currentMap ?? Find.World))
							{
								text += " [not now]";
							}
							DebugActionNode debugActionNode2 = new DebugActionNode(text, DebugActionType.Action, null, null);
							Slate localSlate = slate.DeepCopy();
							debugActionNode2.childGetter = (() => CS$<>8__locals1.<GenerateQuests>g__SelectPopulation|3(localScriptDef, localSlate, true));
							list2.Add(debugActionNode2);
						}
						return list2;
					};
				}
				else if (questScriptDef.affectedByPoints)
				{
					debugActionNode.childGetter = (() => CS$<>8__locals1.<GenerateQuests>g__SelectPoints|2(localScriptDef, slate, true));
				}
				else if (localScriptDef.affectedByPopulation)
				{
					debugActionNode.childGetter = (() => CS$<>8__locals1.<GenerateQuests>g__SelectPopulation|3(localScriptDef, slate, true));
				}
				else
				{
					debugActionNode.action = delegate()
					{
						CS$<>8__locals1.<GenerateQuests>g__GetQuest|0(localScriptDef, slate);
					};
				}
				list.Add(debugActionNode);
			}
			return (from op in list
			orderby op.label
			select op).ToList<DebugActionNode>();
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x00146A54 File Offset: 0x00144C54
		[DebugAction("Quests", "QuestPart test", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static List<DebugActionNode> TestQuestPart()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localQuestPartType2 in typeof(QuestPart).AllSubclassesNonAbstract())
			{
				Type localQuestPartType = localQuestPartType2;
				list.Add(new DebugActionNode(localQuestPartType.Name, DebugActionType.Action, delegate()
				{
					Quest quest = Quest.MakeRaw();
					quest.name = "DEBUG QUEST (" + localQuestPartType.Name + ")";
					QuestPart questPart = (QuestPart)Activator.CreateInstance(localQuestPartType);
					quest.AddPart(questPart);
					questPart.AssignDebugData();
					quest.description = "A debug quest to test " + localQuestPartType.Name + "\n\n" + Scribe.saver.DebugOutputFor(questPart);
					Find.QuestManager.Add(quest);
					Find.LetterStack.ReceiveLetter("DEV: Quest", quest.description, LetterDefOf.PositiveEvent, LookTargets.Invalid, null, quest, null, null, 0, true);
				}, null));
			}
			return list;
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00146AE4 File Offset: 0x00144CE4
		[DebugAction("Quests", "Log generated quest savedata", false, false, false, false, false, 0, false, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		public static void QuestExample()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (QuestScriptDef localRuleDef2 in DefDatabase<QuestScriptDef>.AllDefs)
			{
				QuestScriptDef localRuleDef = localRuleDef2;
				list.Add(new DebugMenuOption(localRuleDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = localPoints2;
						list2.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
						{
							Slate slate = new Slate();
							slate.Set<float>("points", localPoints, false);
							Quest saveable = QuestGen.Generate(localRuleDef, slate);
							Log.Message(Scribe.saver.DebugOutputFor(saveable));
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00146B70 File Offset: 0x00144D70
		[DebugOutput]
		public static void QuestRewardsSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<QuestScriptDef>.Enumerator enumerator = DefDatabase<QuestScriptDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestScriptDef quest = enumerator.Current;
					if (quest.IsRootAny)
					{
						QuestScriptDef localQuest = quest;
						list.Add(new DebugMenuOption(quest.defName, DebugMenuOptionMode.Action, delegate()
						{
							Dictionary<float, int> numQuestsRating1 = new Dictionary<float, int>();
							Dictionary<float, int> numQuestsRating2 = new Dictionary<float, int>();
							Dictionary<float, int> numQuestsRating3 = new Dictionary<float, int>();
							Dictionary<float, float> rewardRating1min = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating1max = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating1accumulated = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating2min = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating2max = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating2accumulated = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating3min = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating3max = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating3accumulated = new Dictionary<float, float>();
							foreach (float num in DebugActionsQuests.QuestRewardDebugPointLevels)
							{
								Quest quest;
								if (num >= quest.rootMinPoints)
								{
									numQuestsRating1.Add(num, 0);
									numQuestsRating2.Add(num, 0);
									numQuestsRating3.Add(num, 0);
									Slate slate = new Slate();
									slate.Set<float>("points", num, false);
									slate.Set<bool>("debugDontGenerateRewardThings", true, false);
									rewardRating1min.Add(num, 9999999f);
									rewardRating2min.Add(num, 9999999f);
									rewardRating3min.Add(num, 9999999f);
									rewardRating1max.Add(num, -9999999f);
									rewardRating2max.Add(num, -9999999f);
									rewardRating3max.Add(num, -9999999f);
									rewardRating1accumulated.Add(num, 0f);
									rewardRating2accumulated.Add(num, 0f);
									rewardRating3accumulated.Add(num, 0f);
									for (int j = 0; j < 20; j++)
									{
										DebugActionsQuests.lastQuestGeneratedRewardValue = 0f;
										quest = QuestGen.Generate(localQuest, slate.DeepCopy());
										float num2 = DebugActionsQuests.lastQuestGeneratedRewardValue;
										if (quest.challengeRating == 1)
										{
											Dictionary<float, int> numQuestsRating = numQuestsRating1;
											float key = num;
											int num3 = numQuestsRating[key];
											numQuestsRating[key] = num3 + 1;
											rewardRating1min[num] = Mathf.Min(rewardRating1min[num], num2);
											rewardRating1max[num] = Mathf.Max(rewardRating1max[num], num2);
											Dictionary<float, float> dictionary = rewardRating1accumulated;
											key = num;
											dictionary[key] += num2;
										}
										else if (quest.challengeRating == 2)
										{
											Dictionary<float, int> numQuestsRating4 = numQuestsRating2;
											float key = num;
											int num3 = numQuestsRating4[key];
											numQuestsRating4[key] = num3 + 1;
											rewardRating2min[num] = Mathf.Min(rewardRating2min[num], num2);
											rewardRating2max[num] = Mathf.Max(rewardRating2max[num], num2);
											Dictionary<float, float> dictionary = rewardRating2accumulated;
											key = num;
											dictionary[key] += num2;
										}
										else if (quest.challengeRating == 3)
										{
											Dictionary<float, int> numQuestsRating5 = numQuestsRating3;
											float key = num;
											int num3 = numQuestsRating5[key];
											numQuestsRating5[key] = num3 + 1;
											rewardRating3min[num] = Mathf.Min(rewardRating3min[num], num2);
											rewardRating3max[num] = Mathf.Max(rewardRating3max[num], num2);
											Dictionary<float, float> dictionary = rewardRating3accumulated;
											key = num;
											dictionary[key] += num2;
										}
									}
								}
							}
							IEnumerable<float> questRewardDebugPointLevels2 = DebugActionsQuests.QuestRewardDebugPointLevels;
							TableDataGetter<float>[] array = new TableDataGetter<float>[13];
							array[0] = new TableDataGetter<float>("points", (float v) => v.ToString());
							array[1] = new TableDataGetter<float>("rating 1\nquest count\nof " + 20.ToString(), (float v) => numQuestsRating1[v].ToString());
							array[2] = new TableDataGetter<float>("rating 1\nrewardValue\nmin", delegate(float v)
							{
								if (rewardRating1min[v] != 9999999f)
								{
									return ((int)rewardRating1min[v]).ToString();
								}
								return "-";
							});
							array[3] = new TableDataGetter<float>("rating 1\nrewardValue\navg", delegate(float v)
							{
								if (rewardRating1accumulated[v] > 0f)
								{
									return ((int)(rewardRating1accumulated[v] / (float)numQuestsRating1[v])).ToString();
								}
								return "-";
							});
							array[4] = new TableDataGetter<float>("rating 1\nrewardValue\nmax", delegate(float v)
							{
								if (rewardRating1max[v] != -9999999f)
								{
									return ((int)rewardRating1max[v]).ToString();
								}
								return "-";
							});
							array[5] = new TableDataGetter<float>("rating 2\nquest count\nof " + 20.ToString(), (float v) => numQuestsRating2[v].ToString());
							array[6] = new TableDataGetter<float>("rating 2\nrewardValue\nmin", delegate(float v)
							{
								if (rewardRating2min[v] != 9999999f)
								{
									return ((int)rewardRating2min[v]).ToString();
								}
								return "-";
							});
							array[7] = new TableDataGetter<float>("rating 2\nrewardValue\navg", delegate(float v)
							{
								if (rewardRating2accumulated[v] > 0f)
								{
									return ((int)(rewardRating2accumulated[v] / (float)numQuestsRating2[v])).ToString();
								}
								return "-";
							});
							array[8] = new TableDataGetter<float>("rating 2\nrewardValue\nmax", delegate(float v)
							{
								if (rewardRating2max[v] != -9999999f)
								{
									return ((int)rewardRating2max[v]).ToString();
								}
								return "-";
							});
							array[9] = new TableDataGetter<float>("rating 3\nquest count\nof " + 20.ToString(), (float v) => numQuestsRating3[v].ToString());
							array[10] = new TableDataGetter<float>("rating 3\nrewardValue\nmin", delegate(float v)
							{
								if (rewardRating3min[v] != 9999999f)
								{
									return ((int)rewardRating3min[v]).ToString();
								}
								return "-";
							});
							array[11] = new TableDataGetter<float>("rating 3\nrewardValue\navg", delegate(float v)
							{
								if (rewardRating3accumulated[v] > 0f)
								{
									return ((int)(rewardRating3accumulated[v] / (float)numQuestsRating3[v])).ToString();
								}
								return "-";
							});
							array[12] = new TableDataGetter<float>("rating 3\nrewardValue\nmax", delegate(float v)
							{
								if (rewardRating3max[v] != -9999999f)
								{
									return ((int)rewardRating3max[v]).ToString();
								}
								return "-";
							});
							DebugTables.MakeTablesDialog<float>(questRewardDebugPointLevels2, array);
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x00146C1C File Offset: 0x00144E1C
		[DebugOutput]
		public static void QuestDefs()
		{
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(Find.World), false);
			IEnumerable<QuestScriptDef> dataSources = from x in DefDatabase<QuestScriptDef>.AllDefs
			orderby x.IsRootRandomSelected descending, x.IsRootDecree descending
			select x;
			TableDataGetter<QuestScriptDef>[] array = new TableDataGetter<QuestScriptDef>[14];
			array[0] = new TableDataGetter<QuestScriptDef>("defName", (QuestScriptDef d) => d.defName);
			array[1] = new TableDataGetter<QuestScriptDef>("points\nmin", delegate(QuestScriptDef d)
			{
				if (d.rootMinPoints <= 0f)
				{
					return "";
				}
				return d.rootMinPoints.ToString();
			});
			array[2] = new TableDataGetter<QuestScriptDef>("progress\nmin", delegate(QuestScriptDef d)
			{
				if (d.rootMinProgressScore <= 0f)
				{
					return "";
				}
				return d.rootMinProgressScore.ToString();
			});
			array[3] = new TableDataGetter<QuestScriptDef>("increases\npop", (QuestScriptDef d) => d.rootIncreasesPopulation.ToStringCheckBlank());
			array[4] = new TableDataGetter<QuestScriptDef>("root\nweight", delegate(QuestScriptDef d)
			{
				if (d.rootSelectionWeight <= 0f)
				{
					return "";
				}
				return d.rootSelectionWeight.ToString();
			});
			array[5] = new TableDataGetter<QuestScriptDef>("decree\nweight", delegate(QuestScriptDef d)
			{
				if (d.decreeSelectionWeight <= 0f)
				{
					return "";
				}
				return d.decreeSelectionWeight.ToString();
			});
			array[6] = new TableDataGetter<QuestScriptDef>("decree\ntags", (QuestScriptDef d) => d.decreeTags.ToCommaList(false, false));
			array[7] = new TableDataGetter<QuestScriptDef>("auto\naccept", (QuestScriptDef d) => d.autoAccept.ToStringCheckBlank());
			array[8] = new TableDataGetter<QuestScriptDef>("expiry\ndays", delegate(QuestScriptDef d)
			{
				if (d.expireDaysRange.TrueMax <= 0f)
				{
					return "";
				}
				return d.expireDaysRange.ToString("0.##");
			});
			array[9] = new TableDataGetter<QuestScriptDef>("refire\ndays", delegate(QuestScriptDef d)
			{
				if (d.minRefireDays <= 0f)
				{
					return "";
				}
				return d.minRefireDays.ToString("0.##");
			});
			array[10] = new TableDataGetter<QuestScriptDef>("CanRun\nnow", delegate(QuestScriptDef d)
			{
				Slate slate = slate;
				IIncidentTarget currentMap = Find.CurrentMap;
				return d.CanRun(slate, currentMap ?? Find.World).ToStringCheckBlank();
			});
			array[11] = new TableDataGetter<QuestScriptDef>("canGiveRoyalFavor", (QuestScriptDef d) => d.canGiveRoyalFavor.ToStringCheckBlank());
			array[12] = new TableDataGetter<QuestScriptDef>("possible rewards", delegate(QuestScriptDef d)
			{
				RewardsGeneratorParams? rewardsParams = null;
				bool multiple = false;
				Slate slate;
				slate.Set<Action<QuestNode, Slate>>("testRunCallback", delegate(QuestNode node, Slate curSlate)
				{
					QuestNode_GiveRewards questNode_GiveRewards = node as QuestNode_GiveRewards;
					if (questNode_GiveRewards != null)
					{
						if (rewardsParams != null)
						{
							multiple = true;
							return;
						}
						rewardsParams = new RewardsGeneratorParams?(questNode_GiveRewards.parms.GetValue(curSlate));
					}
				}, false);
				slate = slate;
				IIncidentTarget currentMap = Find.CurrentMap;
				bool flag = d.CanRun(slate, currentMap ?? Find.World);
				slate.Remove("testRunCallback", false);
				if (multiple)
				{
					return "complex";
				}
				if (rewardsParams != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (rewardsParams.Value.allowGoodwill)
					{
						stringBuilder.AppendWithComma("goodwill");
					}
					if (rewardsParams.Value.allowRoyalFavor)
					{
						stringBuilder.AppendWithComma("favor");
					}
					return stringBuilder.ToString();
				}
				if (!flag)
				{
					return "unknown";
				}
				return "";
			});
			array[13] = new TableDataGetter<QuestScriptDef>("weight histogram", delegate(QuestScriptDef d)
			{
				string text = "";
				for (float num = 0f; num < d.rootSelectionWeight; num += 0.05f)
				{
					text += "*";
					if (num > 3f)
					{
						text += "*";
						break;
					}
				}
				return text;
			});
			DebugTables.MakeTablesDialog<QuestScriptDef>(dataSources, array);
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00146EF8 File Offset: 0x001450F8
		[DebugOutput]
		public static void QuestSelectionWeightsNow()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
			{
				float localPoints = localPoints2;
				list.Add(new DebugMenuOption(localPoints.ToString() + " points", DebugMenuOptionMode.Action, delegate()
				{
					IIncidentTarget target = Find.CurrentMap;
					string label = string.Concat(new string[]
					{
						"selection weight now\ntarget: ",
						target.ToString(),
						"\npoints: ",
						localPoints.ToString("F0"),
						"\npopIntentQuest: ",
						StorytellerUtilityPopulation.PopulationIntentForQuest.ToString()
					});
					IEnumerable<QuestScriptDef> dataSources = from x in DefDatabase<QuestScriptDef>.AllDefsListForReading
					where x.IsRootRandomSelected
					select x;
					TableDataGetter<QuestScriptDef>[] array = new TableDataGetter<QuestScriptDef>[5];
					array[0] = new TableDataGetter<QuestScriptDef>("defName", (QuestScriptDef x) => x.defName);
					array[1] = new TableDataGetter<QuestScriptDef>(label, (QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(x, localPoints, target.StoryState).ToString("F3"));
					array[2] = new TableDataGetter<QuestScriptDef>("increases\npopulation", (QuestScriptDef x) => x.rootIncreasesPopulation.ToStringCheckBlank());
					array[3] = new TableDataGetter<QuestScriptDef>("recency\nindex", delegate(QuestScriptDef x)
					{
						if (!target.StoryState.RecentRandomQuests.Contains(x))
						{
							return "";
						}
						return target.StoryState.RecentRandomQuests.IndexOf(x).ToString();
					});
					array[4] = new TableDataGetter<QuestScriptDef>("total\nselection\nchance\nnow", (QuestScriptDef x) => NaturalRandomQuestChooser.DebugTotalNaturalRandomSelectionWeight(x, localPoints, target).ToString("F3"));
					DebugTables.MakeTablesDialog<QuestScriptDef>(dataSources, array);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(from op in list
			orderby op.label
			select op, null));
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00146FB4 File Offset: 0x001451B4
		[DebugOutput]
		public static void DecreeSelectionWeightsNow()
		{
			IIncidentTarget target = Find.CurrentMap;
			string label = "selection weight now\ntarget: " + target.ToString();
			IEnumerable<QuestScriptDef> dataSources = from x in DefDatabase<QuestScriptDef>.AllDefsListForReading
			where x.IsRootDecree
			select x;
			TableDataGetter<QuestScriptDef>[] array = new TableDataGetter<QuestScriptDef>[2];
			array[0] = new TableDataGetter<QuestScriptDef>("defName", (QuestScriptDef x) => x.defName);
			array[1] = new TableDataGetter<QuestScriptDef>(label, (QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalDecreeSelectionWeight(x, target.StoryState).ToString("F3"));
			DebugTables.MakeTablesDialog<QuestScriptDef>(dataSources, array);
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x0014708C File Offset: 0x0014528C
		[CompilerGenerated]
		internal static void <GenerateQuests>g__GetSlateValuesForPoints|5_1(QuestScriptDef script, Slate slate, float points)
		{
			if (script != null)
			{
				if (script.IsRootDecree)
				{
					slate.Set<Pawn>("asker", PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.RandomElement<Pawn>(), false);
				}
				if (script == QuestScriptDefOf.LongRangeMineralScannerLump)
				{
					slate.Set<ThingDef>("targetMineableThing", ThingDefOf.Gold, false);
					slate.Set<ThingDef>("targetMineable", ThingDefOf.MineableGold, false);
					slate.Set<Pawn>("worker", PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>(), false);
				}
				slate.Set<float>("points", points, false);
			}
		}

		// Token: 0x04002C9A RID: 11418
		public static float lastQuestGeneratedRewardValue;

		// Token: 0x04002C9B RID: 11419
		private static readonly int[] QuestNumOptions = new int[]
		{
			1,
			2,
			5,
			10,
			25,
			50,
			100
		};

		// Token: 0x04002C9C RID: 11420
		private static readonly float[] QuestRewardDebugPointLevels = new float[]
		{
			35f,
			100f,
			200f,
			400f,
			800f,
			1600f,
			3200f,
			6000f
		};
	}
}
