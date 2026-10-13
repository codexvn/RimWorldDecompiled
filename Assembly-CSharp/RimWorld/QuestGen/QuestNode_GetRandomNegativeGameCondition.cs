using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A7A RID: 14970
	public class QuestNode_GetRandomNegativeGameCondition : QuestNode
	{
		// Token: 0x06014745 RID: 83781 RVA: 0x0061BF54 File Offset: 0x0061A154
		public static void ResetStaticData()
		{
			QuestNode_GetRandomNegativeGameCondition.options = new List<QuestNode_GetRandomNegativeGameCondition.Option>
			{
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.VolcanicWinter, new FloatRange(10f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.WeatherController, new FloatRange(5f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.HeatWave, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ColdSnap, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ToxicFallout, new FloatRange(5f, 20f), 0.8f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicSuppression, new FloatRange(4f, 8f), 1.5f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.EMIField, new FloatRange(4f, 8f), 1.8f, 3),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicDrone, new FloatRange(4f, 8f), 2f, 3)
			};
		}

		// Token: 0x06014746 RID: 83782 RVA: 0x0061C093 File Offset: 0x0061A293
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null && this.DoWork(slate);
		}

		// Token: 0x06014747 RID: 83783 RVA: 0x0061C0AD File Offset: 0x0061A2AD
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06014748 RID: 83784 RVA: 0x0061C0BC File Offset: 0x0061A2BC
		private bool DoWork(Slate slate)
		{
			QuestNode_GetRandomNegativeGameCondition.Option option;
			if (QuestGen.Working)
			{
				if (!(from x in QuestNode_GetRandomNegativeGameCondition.options
				where x.challengeRating == QuestGen.quest.challengeRating && this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option) && !(from x in QuestNode_GetRandomNegativeGameCondition.options
				where x.challengeRating < QuestGen.quest.challengeRating && this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option) && !(from x in QuestNode_GetRandomNegativeGameCondition.options
				where this.PossibleNow(x.gameCondition, slate)
				select x).TryRandomElement(out option))
				{
					return false;
				}
			}
			else if (!(from x in QuestNode_GetRandomNegativeGameCondition.options
			where this.PossibleNow(x.gameCondition, slate)
			select x).TryRandomElement(out option))
			{
				return false;
			}
			int var = (int)(option.durationDaysRange.RandomInRange * 60000f);
			slate.Set<GameConditionDef>(this.storeGameConditionAs.GetValue(slate), option.gameCondition, false);
			slate.Set<int>(this.storeGameConditionDurationAs.GetValue(slate), var, false);
			slate.Set<float>(this.storeGameConditionDifficultyAs.GetValue(slate), option.difficulty, false);
			return true;
		}

		// Token: 0x06014749 RID: 83785 RVA: 0x0061C1E0 File Offset: 0x0061A3E0
		private bool PossibleNow(GameConditionDef def, Slate slate)
		{
			if (def == null)
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			if (map.gameConditionManager.ConditionIsActive(def))
			{
				return false;
			}
			IncidentDef incidentDef = null;
			List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].Worker is IncidentWorker_MakeGameCondition && allDefsListForReading[i].gameCondition == def)
				{
					incidentDef = allDefsListForReading[i];
					break;
				}
			}
			if (incidentDef != null)
			{
				if (!Find.Storyteller.difficulty.AllowedBy(incidentDef.disabledWhen))
				{
					return false;
				}
				if (GenDate.DaysPassedSinceSettle < incidentDef.earliestDay)
				{
					return false;
				}
				if (incidentDef.Worker.FiredTooRecently(map))
				{
					return false;
				}
			}
			return (def != GameConditionDefOf.ColdSnap || IncidentWorker_ColdSnap.IsTemperatureAppropriate(map)) && (def != GameConditionDefOf.HeatWave || IncidentWorker_HeatWave.IsTemperatureAppropriate(map));
		}

		// Token: 0x0400E25B RID: 57947
		[NoTranslate]
		public SlateRef<string> storeGameConditionAs;

		// Token: 0x0400E25C RID: 57948
		[NoTranslate]
		public SlateRef<string> storeGameConditionDurationAs;

		// Token: 0x0400E25D RID: 57949
		[NoTranslate]
		public SlateRef<string> storeGameConditionDifficultyAs;

		// Token: 0x0400E25E RID: 57950
		private static List<QuestNode_GetRandomNegativeGameCondition.Option> options;

		// Token: 0x02003A7B RID: 14971
		private struct Option
		{
			// Token: 0x0601474B RID: 83787 RVA: 0x0061C2B5 File Offset: 0x0061A4B5
			public Option(GameConditionDef gameCondition, FloatRange durationDaysRange, float difficulty, int challengeRating)
			{
				this.gameCondition = gameCondition;
				this.durationDaysRange = durationDaysRange;
				this.difficulty = difficulty;
				this.challengeRating = challengeRating;
			}

			// Token: 0x0400E25F RID: 57951
			public GameConditionDef gameCondition;

			// Token: 0x0400E260 RID: 57952
			public FloatRange durationDaysRange;

			// Token: 0x0400E261 RID: 57953
			public float difficulty;

			// Token: 0x0400E262 RID: 57954
			public int challengeRating;
		}
	}
}
