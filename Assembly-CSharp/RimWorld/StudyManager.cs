using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E8A RID: 7818
	public sealed class StudyManager : IExposable
	{
		// Token: 0x0600AD94 RID: 44436 RVA: 0x00318A80 File Offset: 0x00316C80
		public void UpdateStudiableCache(Thing thing, Map map)
		{
			if (map == null)
			{
				return;
			}
			if (!this.studiableThingsCache.ContainsKey(map))
			{
				this.studiableThingsCache.Add(map, new HashSet<Thing>());
			}
			if (thing.def.IsStudiable)
			{
				CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
				if (compStudiable != null && compStudiable.EverStudiableCached())
				{
					goto IL_5D;
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.HeldPawn != null)
				{
					goto IL_5D;
				}
			}
			if (!this.studiableThingsCache[map].Contains(thing))
			{
				return;
			}
			this.studiableThingsCache[map].Remove(thing);
			if (this.studiableThingsCache[map].NullOrEmpty<Thing>())
			{
				this.studiableThingsCache.Remove(map);
				return;
			}
			return;
			IL_5D:
			if (!this.studiableThingsCache[map].Contains(thing))
			{
				this.studiableThingsCache[map].Add(thing);
				return;
			}
		}

		// Token: 0x0600AD95 RID: 44437 RVA: 0x00318B59 File Offset: 0x00316D59
		public HashSet<Thing> GetStudiableThingsAndPlatforms(Map map)
		{
			if (!this.studiableThingsCache.ContainsKey(map))
			{
				return new HashSet<Thing>();
			}
			return this.studiableThingsCache[map];
		}

		// Token: 0x0600AD96 RID: 44438 RVA: 0x00318B7C File Offset: 0x00316D7C
		public void Study(Thing studiedThing, Pawn studier, float studyAmount)
		{
			if (!studiedThing.def.IsStudiable)
			{
				Log.Error("Tried to study " + studiedThing.def.label + " which is not studiable.");
				return;
			}
			CompStudiable compStudiable = studiedThing.TryGetComp<CompStudiable>();
			compStudiable.studyPoints += studyAmount;
			if (compStudiable.Props.studyAmountToComplete > 0f && compStudiable.studyPoints >= compStudiable.Props.studyAmountToComplete)
			{
				compStudiable.studyPoints = compStudiable.Props.studyAmountToComplete;
			}
			studiedThing.Notify_Studied(studier, studyAmount, null);
			Find.SignalManager.SendSignal(new Signal("ThingStudied", true));
		}

		// Token: 0x0600AD97 RID: 44439 RVA: 0x00318C20 File Offset: 0x00316E20
		public void StudyAnomaly(Thing studiedThing, Pawn studier, float knowledgeAmount, KnowledgeCategoryDef knowledgeCategory)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (knowledgeAmount <= 0f)
			{
				return;
			}
			Thing thing = studiedThing;
			if (thing.Map == null)
			{
				thing = (thing.ParentHolder as Thing);
			}
			if (thing != null)
			{
				MoteMaker.ThrowText(thing.DrawPos, thing.Map, string.Format("{0} +{1:0.00}", knowledgeCategory.LabelCap, knowledgeAmount), 3f);
			}
			Find.ResearchManager.ApplyKnowledge(knowledgeCategory, knowledgeAmount);
			studiedThing.Notify_Studied(studier, knowledgeAmount, knowledgeCategory);
			Find.SignalManager.SendSignal(new Signal("ThingStudied", true));
		}

		// Token: 0x0600AD98 RID: 44440 RVA: 0x00318CB8 File Offset: 0x00316EB8
		public float GetKnowledgeAmount(KnowledgeCategoryDef knowledgeCategory)
		{
			float num = 0f;
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				if (researchProjectDef.knowledgeCategory == knowledgeCategory)
				{
					num += Find.ResearchManager.GetKnowledge(researchProjectDef);
				}
			}
			return num;
		}

		// Token: 0x0600AD99 RID: 44441 RVA: 0x00318D24 File Offset: 0x00316F24
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Dictionary<ThingDef, float> dictionary = new Dictionary<ThingDef, float>();
				Scribe_Collections.Look<ThingDef, float>(ref dictionary, "studyProgress", LookMode.Def, LookMode.Value);
				if (dictionary != null)
				{
					foreach (ThingDef thingDef in dictionary.Keys)
					{
						if (thingDef.GetCompProperties<CompProperties_Studiable>() != null)
						{
							this.backCompatStudyProgress[thingDef] = dictionary[thingDef];
						}
						CompProperties_CompAnalyzableUnlockResearch compProperties = thingDef.GetCompProperties<CompProperties_CompAnalyzableUnlockResearch>();
						if (compProperties != null && dictionary[thingDef] >= 1f)
						{
							AnalysisManager analysisManager = Find.AnalysisManager;
							if (analysisManager != null)
							{
								analysisManager.ForceCompleteAnalysisProgress(compProperties.analysisID);
							}
						}
					}
				}
			}
		}

		// Token: 0x04007771 RID: 30577
		public const float DefaultStudyPerTick = 0.08f;

		// Token: 0x04007772 RID: 30578
		public const float DefaultStudyPerInteraction = 0.87f;

		// Token: 0x04007773 RID: 30579
		public const int DefaultStudyInteractions = 5;

		// Token: 0x04007774 RID: 30580
		public const string ThingStudiedSignal = "ThingStudied";

		// Token: 0x04007775 RID: 30581
		private Dictionary<Map, HashSet<Thing>> studiableThingsCache = new Dictionary<Map, HashSet<Thing>>();

		// Token: 0x04007776 RID: 30582
		public Dictionary<ThingDef, float> backCompatStudyProgress = new Dictionary<ThingDef, float>();
	}
}
