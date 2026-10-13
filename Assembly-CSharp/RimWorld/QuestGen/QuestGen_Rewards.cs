using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020039D5 RID: 14805
	public static class QuestGen_Rewards
	{
		// Token: 0x06014504 RID: 83204 RVA: 0x00611C60 File Offset: 0x0060FE60
		public static QuestPart_Choice GiveRewards(this Quest quest, RewardsGeneratorParams parms, string inSignal = null, string customLetterLabel = null, string customLetterText = null, RulePack customLetterLabelRules = null, RulePack customLetterTextRules = null, bool? useDifficultyFactor = null, Action runIfChosenPawnSignalUsed = null, int? variants = null, bool addCampLootReward = false, Pawn asker = null, bool addShuttleLootReward = false, bool addPossibleFutureReward = false, float? overridePopulationIntent = null)
		{
			QuestPart_Choice result;
			try
			{
				Slate slate = QuestGen.slate;
				RewardsGeneratorParams rewardsGeneratorParams = parms;
				rewardsGeneratorParams.rewardValue = ((rewardsGeneratorParams.rewardValue == 0f) ? slate.Get<float>("rewardValue", 0f, false) : rewardsGeneratorParams.rewardValue);
				if (useDifficultyFactor ?? true)
				{
					rewardsGeneratorParams.rewardValue *= Find.Storyteller.difficulty.EffectiveQuestRewardValueFactor;
					rewardsGeneratorParams.rewardValue = Math.Max(1f, rewardsGeneratorParams.rewardValue);
				}
				if (slate.Get<bool>("debugDontGenerateRewardThings", false, false))
				{
					DebugActionsQuests.lastQuestGeneratedRewardValue += Mathf.Max(rewardsGeneratorParams.rewardValue, 250f);
					result = null;
				}
				else
				{
					rewardsGeneratorParams.minGeneratedRewardValue = 250f;
					rewardsGeneratorParams.giverFaction = (rewardsGeneratorParams.giverFaction ?? ((asker != null) ? asker.Faction : null));
					rewardsGeneratorParams.populationIntent = (overridePopulationIntent ?? QuestTuning.PopIncreasingRewardWeightByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest));
					if (rewardsGeneratorParams.giverFaction == null || rewardsGeneratorParams.giverFaction.def.permanentEnemy)
					{
						rewardsGeneratorParams.allowGoodwill = false;
					}
					if (rewardsGeneratorParams.giverFaction == null || asker.royalty == null || !asker.royalty.HasAnyTitleIn(asker.Faction) || rewardsGeneratorParams.giverFaction.HostileTo(Faction.OfPlayer))
					{
						rewardsGeneratorParams.allowRoyalFavor = false;
					}
					if (!ModsConfig.IdeologyActive || Faction.OfPlayer.ideos.FluidIdeo == null)
					{
						rewardsGeneratorParams.allowDevelopmentPoints = false;
					}
					if (!ModsConfig.BiotechActive)
					{
						rewardsGeneratorParams.allowXenogermReimplantation = false;
					}
					Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo("inSignal");
					if (inSignal.NullOrEmpty())
					{
						inSignal = slate.Get<string>("inSignal", null, false);
					}
					else
					{
						slate.Set<string>("inSignal", QuestGenUtility.HardcodedSignalWithQuestID(inSignal), false);
					}
					try
					{
						QuestPart_Choice questPart_Choice = new QuestPart_Choice();
						questPart_Choice.inSignalChoiceUsed = slate.Get<string>("inSignal", null, false);
						bool flag = false;
						int num;
						if (rewardsGeneratorParams.allowGoodwill && rewardsGeneratorParams.giverFaction != null && rewardsGeneratorParams.giverFaction.HostileTo(Faction.OfPlayer))
						{
							num = 1;
						}
						else
						{
							num = (variants ?? (QuestGen.quest.root.autoAccept ? 1 : 3));
						}
						QuestGen_Rewards.generatedRewards.Clear();
						for (int i = 0; i < num; i++)
						{
							QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
							List<Reward> list = QuestGen_Rewards.GenerateRewards(rewardsGeneratorParams, slate, i == 0, ref flag, choice, num, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules);
							if (list != null)
							{
								questPart_Choice.choices.Add(choice);
								QuestGen_Rewards.generatedRewards.Add(list);
							}
						}
						QuestGen_Rewards.generatedRewards.Clear();
						if (addCampLootReward)
						{
							for (int j = 0; j < questPart_Choice.choices.Count; j++)
							{
								questPart_Choice.choices[j].rewards.Add(new Reward_CampLoot());
							}
						}
						if (addShuttleLootReward)
						{
							for (int k = 0; k < questPart_Choice.choices.Count; k++)
							{
								questPart_Choice.choices[k].rewards.Add(new Reward_ShuttleLoot());
							}
						}
						if (addPossibleFutureReward)
						{
							for (int l = 0; l < questPart_Choice.choices.Count; l++)
							{
								questPart_Choice.choices[l].rewards.Add(new Reward_PossibleFutureReward());
							}
						}
						if (rewardsGeneratorParams.allowDevelopmentPoints)
						{
							if (questPart_Choice.choices.Count <= 0)
							{
								QuestPart_Choice.Choice choice2 = new QuestPart_Choice.Choice();
								choice2.rewards.Add(new Reward_DevelopmentPoints(quest));
								questPart_Choice.choices.Add(choice2);
							}
							else
							{
								for (int m = 0; m < questPart_Choice.choices.Count; m++)
								{
									questPart_Choice.choices[m].rewards.Add(new Reward_DevelopmentPoints(quest));
								}
							}
						}
						if (rewardsGeneratorParams.allowXenogermReimplantation)
						{
							QuestPart_Choice.Choice choice3 = new QuestPart_Choice.Choice();
							Reward_ReimplantXenogerm reward_ReimplantXenogerm = new Reward_ReimplantXenogerm();
							choice3.rewards.Add(reward_ReimplantXenogerm);
							questPart_Choice.choices.Add(choice3);
							foreach (QuestPart questPart in reward_ReimplantXenogerm.GenerateQuestParts(0, parms, null, null, null, null))
							{
								QuestGen.quest.AddPart(questPart);
								choice3.questParts.Add(questPart);
							}
						}
						questPart_Choice.choices.SortByDescending(new Func<QuestPart_Choice.Choice, int>(QuestGen_Rewards.GetDisplayPriority));
						QuestGen.quest.AddPart(questPart_Choice);
						if (flag && runIfChosenPawnSignalUsed != null)
						{
							QuestGen_Rewards.tmpPrevQuestParts.Clear();
							QuestGen_Rewards.tmpPrevQuestParts.AddRange(QuestGen.quest.PartsListForReading);
							runIfChosenPawnSignalUsed();
							List<QuestPart> partsListForReading = QuestGen.quest.PartsListForReading;
							for (int n = 0; n < partsListForReading.Count; n++)
							{
								if (!QuestGen_Rewards.tmpPrevQuestParts.Contains(partsListForReading[n]))
								{
									for (int num2 = 0; num2 < questPart_Choice.choices.Count; num2++)
									{
										bool flag2 = false;
										for (int num3 = 0; num3 < questPart_Choice.choices[num2].rewards.Count; num3++)
										{
											if (questPart_Choice.choices[num2].rewards[num3].MakesUseOfChosenPawnSignal)
											{
												flag2 = true;
												break;
											}
										}
										if (flag2)
										{
											questPart_Choice.choices[num2].questParts.Add(partsListForReading[n]);
										}
									}
								}
							}
							QuestGen_Rewards.tmpPrevQuestParts.Clear();
						}
						result = questPart_Choice;
					}
					finally
					{
						slate.Restore(restoreInfo);
					}
				}
			}
			finally
			{
				QuestGen_Rewards.generatedRewards.Clear();
			}
			return result;
		}

		// Token: 0x06014505 RID: 83205 RVA: 0x00612260 File Offset: 0x00610460
		private static List<Reward> GenerateRewards(RewardsGeneratorParams parmsResolved, Slate slate, bool addDescriptionRules, ref bool chosenPawnSignalUsed, QuestPart_Choice.Choice choice, int variants, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			List<string> list;
			List<string> list2;
			if (addDescriptionRules)
			{
				list = new List<string>();
				list2 = new List<string>();
			}
			else
			{
				list = null;
				list2 = null;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < QuestGen_Rewards.generatedRewards.Count; i++)
			{
				for (int j = 0; j < QuestGen_Rewards.generatedRewards[i].Count; j++)
				{
					if (QuestGen_Rewards.generatedRewards[i][j] is Reward_Pawn)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					break;
				}
			}
			if (flag2)
			{
				parmsResolved.thingRewardItemsOnly = true;
			}
			List<Reward> list3 = null;
			if (variants >= 2 && !QuestGen_Rewards.generatedRewards.Any<List<Reward>>() && parmsResolved.allowGoodwill && !parmsResolved.thingRewardRequired)
			{
				list3 = QuestGen_Rewards.TryGenerateRewards_SocialOnly(parmsResolved, variants >= 3);
				if (list3.NullOrEmpty<Reward>() && variants >= 3)
				{
					list3 = QuestGen_Rewards.TryGenerateRewards_ThingsOnly(parmsResolved);
				}
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else if (variants >= 3 && QuestGen_Rewards.generatedRewards.Count == 1 && parmsResolved.allowRoyalFavor && !parmsResolved.thingRewardRequired)
			{
				list3 = QuestGen_Rewards.TryGenerateRewards_RoyalFavorOnly(parmsResolved);
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateRewards_ThingsOnly(parmsResolved);
				}
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else if (variants >= 2 && QuestGen_Rewards.generatedRewards.Any<List<Reward>>() && !parmsResolved.thingRewardDisallowed)
			{
				list3 = QuestGen_Rewards.TryGenerateRewards_ThingsOnly(parmsResolved);
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else
			{
				list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
			}
			if (list3.NullOrEmpty<Reward>())
			{
				return null;
			}
			Reward_Items reward_Items = (Reward_Items)list3.Find((Reward x) => x is Reward_Items);
			if (reward_Items == null)
			{
				List<Type> b = (from x in list3
				select x.GetType()).ToList<Type>();
				for (int k = 0; k < QuestGen_Rewards.generatedRewards.Count; k++)
				{
					if ((from x in QuestGen_Rewards.generatedRewards[k]
					select x.GetType()).ToList<Type>().SetsEqual(b))
					{
						return null;
					}
				}
			}
			else if (list3.Count == 1)
			{
				List<ThingDef> b2 = (from x in reward_Items.ItemsListForReading
				select x.def).ToList<ThingDef>();
				for (int l = 0; l < QuestGen_Rewards.generatedRewards.Count; l++)
				{
					Reward_Items reward_Items2 = (Reward_Items)QuestGen_Rewards.generatedRewards[l].Find((Reward x) => x is Reward_Items);
					if (reward_Items2 != null)
					{
						if ((from x in reward_Items2.ItemsListForReading
						select x.def).ToList<ThingDef>().SetsEqual(b2))
						{
							return null;
						}
					}
				}
			}
			list3.SortBy((Reward x) => x is Reward_Items);
			choice.rewards.AddRange(list3);
			for (int m = 0; m < list3.Count; m++)
			{
				if (addDescriptionRules)
				{
					list.Add(list3[m].GetDescription(parmsResolved));
					if (!(list3[m] is Reward_Items))
					{
						list2.Add(list3[m].GetDescription(parmsResolved));
					}
					else if (m == list3.Count - 1)
					{
						flag = true;
					}
				}
				foreach (QuestPart questPart in list3[m].GenerateQuestParts(m, parmsResolved, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules))
				{
					QuestGen.quest.AddPart(questPart);
					choice.questParts.Add(questPart);
				}
				if (!parmsResolved.chosenPawnSignal.NullOrEmpty() && list3[m].MakesUseOfChosenPawnSignal)
				{
					chosenPawnSignalUsed = true;
				}
			}
			if (addDescriptionRules)
			{
				string text = list.AsEnumerable<string>().ToList<string>().ToClauseSequence().Resolve().UncapitalizeFirst();
				if (flag)
				{
					text = text.TrimEnd('.');
				}
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("allRewardsDescriptions", text.UncapitalizeFirst()),
					new Rule_String("allRewardsDescriptionsExceptItems", list2.Any<string>() ? list2.AsEnumerable<string>().ToList<string>().ToClauseSequence().Resolve().UncapitalizeFirst() : "")
				});
			}
			return list3;
		}

		// Token: 0x06014506 RID: 83206 RVA: 0x00612734 File Offset: 0x00610934
		private static List<Reward> TryGenerateRewards_SocialOnly(RewardsGeneratorParams parms, bool disallowRoyalFavor)
		{
			parms.thingRewardDisallowed = true;
			if (disallowRoyalFavor)
			{
				parms.allowRoyalFavor = false;
			}
			return QuestGen_Rewards.TryGenerateNonRepeatingRewards(parms);
		}

		// Token: 0x06014507 RID: 83207 RVA: 0x0061274F File Offset: 0x0061094F
		private static List<Reward> TryGenerateRewards_RoyalFavorOnly(RewardsGeneratorParams parms)
		{
			parms.allowGoodwill = false;
			parms.thingRewardDisallowed = true;
			return QuestGen_Rewards.TryGenerateNonRepeatingRewards(parms);
		}

		// Token: 0x06014508 RID: 83208 RVA: 0x00612767 File Offset: 0x00610967
		private static List<Reward> TryGenerateRewards_ThingsOnly(RewardsGeneratorParams parms)
		{
			if (parms.thingRewardDisallowed)
			{
				return null;
			}
			parms.allowGoodwill = false;
			parms.allowRoyalFavor = false;
			return QuestGen_Rewards.TryGenerateNonRepeatingRewards(parms);
		}

		// Token: 0x06014509 RID: 83209 RVA: 0x0061278C File Offset: 0x0061098C
		private static List<Reward> TryGenerateNonRepeatingRewards(RewardsGeneratorParams parms)
		{
			List<Reward> list = null;
			int i = 0;
			while (i < 10)
			{
				list = RewardsGenerator.Generate(parms);
				if (list.Any((Reward x) => x is Reward_Pawn))
				{
					return list;
				}
				if (list.Any((Reward x) => x is Reward_ReimplantXenogerm))
				{
					return list;
				}
				Reward_Items reward_Items = (Reward_Items)list.FirstOrDefault((Reward x) => x is Reward_Items);
				if (reward_Items != null)
				{
					bool flag = false;
					for (int j = 0; j < QuestGen_Rewards.generatedRewards.Count; j++)
					{
						Reward_Items otherGeneratedItems = null;
						for (int l = 0; l < QuestGen_Rewards.generatedRewards[j].Count; l++)
						{
							otherGeneratedItems = (QuestGen_Rewards.generatedRewards[j][l] as Reward_Items);
							if (otherGeneratedItems != null)
							{
								break;
							}
						}
						if (otherGeneratedItems != null)
						{
							int k2;
							int k;
							Predicate<Thing> <>9__3;
							for (k = 0; k < otherGeneratedItems.items.Count; k = k2 + 1)
							{
								List<Thing> items = reward_Items.items;
								Predicate<Thing> predicate;
								if ((predicate = <>9__3) == null)
								{
									predicate = (<>9__3 = ((Thing x) => x.GetInnerIfMinified().def == otherGeneratedItems.items[k].GetInnerIfMinified().def));
								}
								if (items.Any(predicate))
								{
									flag = true;
									break;
								}
								k2 = k;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						i++;
						continue;
					}
				}
				return list;
			}
			return list;
		}

		// Token: 0x0601450A RID: 83210 RVA: 0x00612930 File Offset: 0x00610B30
		private static int GetDisplayPriority(QuestPart_Choice.Choice choice)
		{
			for (int i = 0; i < choice.rewards.Count; i++)
			{
				if (choice.rewards[i] is Reward_ReimplantXenogerm)
				{
					return 2;
				}
			}
			for (int j = 0; j < choice.rewards.Count; j++)
			{
				if (choice.rewards[j] is Reward_RoyalFavor)
				{
					return 1;
				}
			}
			for (int k = 0; k < choice.rewards.Count; k++)
			{
				if (choice.rewards[k] is Reward_Goodwill)
				{
					return -1;
				}
			}
			return 0;
		}

		// Token: 0x0400E02B RID: 57387
		private const float MinRewardValue = 250f;

		// Token: 0x0400E02C RID: 57388
		private const int DefaultVariants = 3;

		// Token: 0x0400E02D RID: 57389
		private static List<List<Reward>> generatedRewards = new List<List<Reward>>();

		// Token: 0x0400E02E RID: 57390
		private static List<QuestPart> tmpPrevQuestParts = new List<QuestPart>();
	}
}
