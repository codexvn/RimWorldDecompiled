using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E81 RID: 7809
	public static class StorytellerUtilityPopulation
	{
		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x0600AD5D RID: 44381 RVA: 0x00317C10 File Offset: 0x00315E10
		private static StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x0600AD5E RID: 44382 RVA: 0x00317C1C File Offset: 0x00315E1C
		public static float PopulationIntent
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulation, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x0600AD5F RID: 44383 RVA: 0x00317C3C File Offset: 0x00315E3C
		public static float PopulationIntentForQuest
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulationIncludingQuests, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x0600AD60 RID: 44384 RVA: 0x00317C5C File Offset: 0x00315E5C
		public static float AdjustedPopulation
		{
			get
			{
				float num = 0f;
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
				{
					if (pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony)
					{
						num += StorytellerUtilityPopulation.AdjustedPopulationValue(pawn);
					}
				}
				num += (float)QuestUtility.TotalBorrowedColonistCount();
				return num;
			}
		}

		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x0600AD61 RID: 44385 RVA: 0x00317CD8 File Offset: 0x00315ED8
		public static float AdjustedPopulationIncludingQuests
		{
			get
			{
				float num = StorytellerUtilityPopulation.AdjustedPopulation;
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (!questsListForReading[i].Historical && questsListForReading[i].IncreasesPopulation)
					{
						num += 1f;
					}
				}
				return num;
			}
		}

		// Token: 0x0600AD62 RID: 44386 RVA: 0x00317D2C File Offset: 0x00315F2C
		private static float AdjustedPopulationValue(Pawn pawn)
		{
			if (pawn.DevelopmentalStage.Baby())
			{
				return 0f;
			}
			float num = pawn.DevelopmentalStage.Adult() ? 1f : Mathf.Lerp(StorytellerUtilityPopulation.PopulationValue_Child, 1f, (float)pawn.ageTracker.AgeBiologicalYears / pawn.ageTracker.AdultMinAge);
			if (pawn.IsPrisonerOfColony)
			{
				if (pawn.guest.Recruitable)
				{
					num *= StorytellerUtilityPopulation.PopulationValue_Prisoner;
				}
				else
				{
					num *= StorytellerUtilityPopulation.PopulationValue_PrisonerUnrecruitable;
				}
			}
			else if (pawn.IsSlaveOfColony)
			{
				num *= StorytellerUtilityPopulation.PopulationValue_Slave;
			}
			return num;
		}

		// Token: 0x0600AD63 RID: 44387 RVA: 0x00317DC4 File Offset: 0x00315FC4
		private static float CalculatePopulationIntent(StorytellerDef def, float curPop, float popAdaptation)
		{
			float num = def.populationIntentFactorFromPopCurve.Evaluate(curPop);
			if (num > 0f)
			{
				num *= def.populationIntentFactorFromPopAdaptDaysCurve.Evaluate(popAdaptation);
			}
			return num;
		}

		// Token: 0x0600AD64 RID: 44388 RVA: 0x00317DF8 File Offset: 0x00315FF8
		public static string DebugReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Population intent: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			stringBuilder.AppendLine("Population intent for quest: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntentForQuest.ToString("F2"));
			stringBuilder.AppendLine("Chance random quest increases population: ".PadRight(40) + NaturalRandomQuestChooser.PopulationIncreasingQuestChance().ToStringPercent());
			stringBuilder.AppendLine("Adjusted population: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Adjusted population including quests: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Pop adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherPopAdaptation.AdaptDays.ToString("F2"));
			return stringBuilder.ToString();
		}

		// Token: 0x0600AD65 RID: 44389 RVA: 0x00317F10 File Offset: 0x00316110
		[DebugOutput]
		public static void PopulationIntents()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add((float)i);
			}
			List<float> list2 = new List<float>();
			for (int j = 0; j < 40; j += 2)
			{
				list2.Add((float)j);
			}
			DebugTables.MakeTablesDialog<float, float>(list2, (float ds) => "d-" + ds.ToString("F0"), list, (float rv) => rv.ToString("F2"), (float ds, float p) => StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, p, (float)((int)ds)).ToString("F2"), "pop");
		}

		// Token: 0x0400774B RID: 30539
		private static float PopulationValue_Prisoner = 0.5f;

		// Token: 0x0400774C RID: 30540
		private static float PopulationValue_PrisonerUnrecruitable = 0.25f;

		// Token: 0x0400774D RID: 30541
		private static float PopulationValue_Slave = 0.75f;

		// Token: 0x0400774E RID: 30542
		private static float PopulationValue_Child = 0.3f;
	}
}
