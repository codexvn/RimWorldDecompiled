using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020034C4 RID: 13508
	public static class LessonAutoActivator
	{
		// Token: 0x17003170 RID: 12656
		// (get) Token: 0x06012FB4 RID: 77748 RVA: 0x005A1A0A File Offset: 0x0059FC0A
		private static float SecondsSinceLesson
		{
			get
			{
				return LessonAutoActivator.timeSinceLastLesson;
			}
		}

		// Token: 0x17003171 RID: 12657
		// (get) Token: 0x06012FB5 RID: 77749 RVA: 0x005A1A11 File Offset: 0x0059FC11
		private static float RelaxDesire
		{
			get
			{
				return 100f - LessonAutoActivator.SecondsSinceLesson * 0.11111111f;
			}
		}

		// Token: 0x06012FB6 RID: 77750 RVA: 0x005A1A24 File Offset: 0x0059FC24
		public static void Reset()
		{
			LessonAutoActivator.alertingConcepts.Clear();
		}

		// Token: 0x06012FB7 RID: 77751 RVA: 0x005A1A30 File Offset: 0x0059FC30
		public static void TeachOpportunity(ConceptDef conc, OpportunityType opp)
		{
			LessonAutoActivator.TeachOpportunity(conc, null, opp);
		}

		// Token: 0x06012FB8 RID: 77752 RVA: 0x005A1A3C File Offset: 0x0059FC3C
		public static void TeachOpportunity(ConceptDef conc, Thing subject, OpportunityType opp)
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return;
			}
			float value = 999f;
			switch (opp)
			{
			case OpportunityType.GoodToKnow:
				value = 60f;
				break;
			case OpportunityType.Important:
				value = 80f;
				break;
			case OpportunityType.Critical:
				value = 100f;
				break;
			default:
				Log.Error("Unknown need");
				break;
			}
			LessonAutoActivator.opportunities[conc] = value;
			if (opp >= OpportunityType.Important || Find.Tutor.learningReadout.ActiveConceptsCount < 4)
			{
				LessonAutoActivator.TryInitiateLesson(conc);
			}
		}

		// Token: 0x06012FB9 RID: 77753 RVA: 0x005A1ABD File Offset: 0x0059FCBD
		public static void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				LessonAutoActivator.opportunities[conc] = 0f;
			}
		}

		// Token: 0x06012FBA RID: 77754 RVA: 0x005A1AD8 File Offset: 0x0059FCD8
		public static void LessonAutoActivatorUpdate()
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || Current.Game == null || Find.Tutor.learningReadout.ShowAllMode)
			{
				return;
			}
			LessonAutoActivator.timeSinceLastLesson += RealTime.realDeltaTime;
			if (Current.ProgramState == ProgramState.Playing && (Time.timeSinceLevelLoad < 8f || Find.WindowStack.SecondsSinceClosedGameStartDialog < 8f || Find.TickManager.NotPlaying))
			{
				return;
			}
			for (int i = LessonAutoActivator.alertingConcepts.Count - 1; i >= 0; i--)
			{
				if (PlayerKnowledgeDatabase.IsComplete(LessonAutoActivator.alertingConcepts[i]))
				{
					LessonAutoActivator.alertingConcepts.RemoveAt(i);
				}
			}
			if (Time.frameCount % 15 == 0 && Find.ActiveLesson.Current == null)
			{
				for (int j = 0; j < DefDatabase<ConceptDef>.AllDefsListForReading.Count; j++)
				{
					ConceptDef conceptDef = DefDatabase<ConceptDef>.AllDefsListForReading[j];
					if (!PlayerKnowledgeDatabase.IsComplete(conceptDef))
					{
						float num = PlayerKnowledgeDatabase.GetKnowledge(conceptDef);
						num -= 0.00015f * Time.deltaTime * 15f;
						if (num < 0f)
						{
							num = 0f;
						}
						PlayerKnowledgeDatabase.SetKnowledge(conceptDef, num);
						if (conceptDef.opportunityDecays)
						{
							float num2 = LessonAutoActivator.GetOpportunity(conceptDef);
							num2 -= 0.4f * Time.deltaTime * 15f;
							if (num2 < 0f)
							{
								num2 = 0f;
							}
							LessonAutoActivator.opportunities[conceptDef] = num2;
						}
					}
				}
				if (Find.Tutor.learningReadout.ActiveConceptsCount < 3)
				{
					ConceptDef conceptDef2 = LessonAutoActivator.MostDesiredConcept();
					if (conceptDef2 != null)
					{
						float desire = LessonAutoActivator.GetDesire(conceptDef2);
						if (desire > 0.1f && LessonAutoActivator.RelaxDesire < desire)
						{
							LessonAutoActivator.TryInitiateLesson(conceptDef2);
							return;
						}
					}
				}
				else
				{
					LessonAutoActivator.SetLastLessonTimeToNow();
				}
			}
		}

		// Token: 0x06012FBB RID: 77755 RVA: 0x005A1C80 File Offset: 0x0059FE80
		private static ConceptDef MostDesiredConcept()
		{
			float num = -9999f;
			ConceptDef result = null;
			List<ConceptDef> allDefsListForReading = DefDatabase<ConceptDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ConceptDef conceptDef = allDefsListForReading[i];
				float desire = LessonAutoActivator.GetDesire(conceptDef);
				if (desire > num && (!conceptDef.needsOpportunity || LessonAutoActivator.GetOpportunity(conceptDef) >= 0.1f) && PlayerKnowledgeDatabase.GetKnowledge(conceptDef) <= 0.15f)
				{
					num = desire;
					result = conceptDef;
				}
			}
			return result;
		}

		// Token: 0x06012FBC RID: 77756 RVA: 0x005A1CF0 File Offset: 0x0059FEF0
		private static float GetDesire(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return 0f;
			}
			if (Find.Tutor.learningReadout.IsActive(conc))
			{
				return 0f;
			}
			if (Current.ProgramState != conc.gameMode)
			{
				return 0f;
			}
			if (conc.needsOpportunity && LessonAutoActivator.GetOpportunity(conc) < 0.1f)
			{
				return 0f;
			}
			return (0f + conc.priority + LessonAutoActivator.GetOpportunity(conc) / 100f * 60f) * (1f - PlayerKnowledgeDatabase.GetKnowledge(conc));
		}

		// Token: 0x06012FBD RID: 77757 RVA: 0x005A1D80 File Offset: 0x0059FF80
		private static float GetOpportunity(ConceptDef conc)
		{
			float result;
			if (LessonAutoActivator.opportunities.TryGetValue(conc, out result))
			{
				return result;
			}
			LessonAutoActivator.opportunities[conc] = 0f;
			return 0f;
		}

		// Token: 0x06012FBE RID: 77758 RVA: 0x005A1DB3 File Offset: 0x0059FFB3
		private static void TryInitiateLesson(ConceptDef conc)
		{
			if (Find.Tutor.learningReadout.TryActivateConcept(conc))
			{
				LessonAutoActivator.SetLastLessonTimeToNow();
			}
		}

		// Token: 0x06012FBF RID: 77759 RVA: 0x005A1DCC File Offset: 0x0059FFCC
		private static void SetLastLessonTimeToNow()
		{
			LessonAutoActivator.timeSinceLastLesson = 0f;
		}

		// Token: 0x06012FC0 RID: 77760 RVA: 0x005A1DD8 File Offset: 0x0059FFD8
		public static void Notify_TutorialEnding()
		{
			LessonAutoActivator.SetLastLessonTimeToNow();
		}

		// Token: 0x06012FC1 RID: 77761 RVA: 0x005A1DE0 File Offset: 0x0059FFE0
		public static string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("RelaxDesire: " + LessonAutoActivator.RelaxDesire.ToString());
			foreach (ConceptDef conceptDef in from co in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(co) descending
			select co)
			{
				if (PlayerKnowledgeDatabase.IsComplete(conceptDef))
				{
					stringBuilder.AppendLine(conceptDef.defName + " complete");
				}
				else
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						conceptDef.defName,
						"\n   know ",
						PlayerKnowledgeDatabase.GetKnowledge(conceptDef).ToString("F3"),
						"\n   need ",
						LessonAutoActivator.opportunities[conceptDef].ToString("F3"),
						"\n   des ",
						LessonAutoActivator.GetDesire(conceptDef).ToString("F3")
					}));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06012FC2 RID: 77762 RVA: 0x005A1F14 File Offset: 0x005A0114
		public static void DebugForceInitiateBestLessonNow()
		{
			LessonAutoActivator.TryInitiateLesson((from def in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(def) descending
			select def).First<ConceptDef>());
		}

		// Token: 0x0400C4F5 RID: 50421
		private static Dictionary<ConceptDef, float> opportunities = new Dictionary<ConceptDef, float>();

		// Token: 0x0400C4F6 RID: 50422
		private static float timeSinceLastLesson = 10000f;

		// Token: 0x0400C4F7 RID: 50423
		private static List<ConceptDef> alertingConcepts = new List<ConceptDef>();

		// Token: 0x0400C4F8 RID: 50424
		private const float MapStartGracePeriod = 8f;

		// Token: 0x0400C4F9 RID: 50425
		private const float KnowledgeDecayRate = 0.00015f;

		// Token: 0x0400C4FA RID: 50426
		private const float OpportunityDecayRate = 0.4f;

		// Token: 0x0400C4FB RID: 50427
		private const float OpportunityMaxDesireAdd = 60f;

		// Token: 0x0400C4FC RID: 50428
		private const int CheckInterval = 15;

		// Token: 0x0400C4FD RID: 50429
		private const float MaxLessonInterval = 900f;
	}
}
