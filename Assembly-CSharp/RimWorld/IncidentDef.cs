using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A52 RID: 6738
	public class IncidentDef : Def
	{
		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x06009774 RID: 38772 RVA: 0x002C37BB File Offset: 0x002C19BB
		public bool ShouldIgnoreRecentWeighting
		{
			get
			{
				return this.ignoreRecentSelectionWeighting || this.category == IncidentCategoryDefOf.Special || this.category == IncidentCategoryDefOf.GiveQuest;
			}
		}

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x06009775 RID: 38773 RVA: 0x002C37E1 File Offset: 0x002C19E1
		public IncidentWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (IncidentWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x06009776 RID: 38774 RVA: 0x002C3814 File Offset: 0x002C1A14
		public List<IncidentDef> RefireCheckIncidents
		{
			get
			{
				if (this.refireCheckTags == null)
				{
					return null;
				}
				if (this.cachedRefireCheckIncidents == null)
				{
					this.cachedRefireCheckIncidents = new List<IncidentDef>();
					List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (this.ShouldDoRefireCheckWith(allDefsListForReading[i]))
						{
							this.cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedRefireCheckIncidents;
			}
		}

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x06009777 RID: 38775 RVA: 0x002C387C File Offset: 0x002C1A7C
		public bool IsAnomalyIncident
		{
			get
			{
				return this.minAnomalyThreatLevel >= 0;
			}
		}

		// Token: 0x06009778 RID: 38776 RVA: 0x002C388A File Offset: 0x002C1A8A
		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		// Token: 0x06009779 RID: 38777 RVA: 0x002C3894 File Offset: 0x002C1A94
		private bool ShouldDoRefireCheckWith(IncidentDef other)
		{
			if (other.tags == null)
			{
				return false;
			}
			if (other == this)
			{
				return false;
			}
			for (int i = 0; i < other.tags.Count; i++)
			{
				for (int j = 0; j < this.refireCheckTags.Count; j++)
				{
					if (other.tags[i] == this.refireCheckTags[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600977A RID: 38778 RVA: 0x002C38FF File Offset: 0x002C1AFF
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category == null)
			{
				yield return "category is undefined.";
			}
			if (this.targetTags == null || this.targetTags.Count == 0)
			{
				yield return "no target type";
			}
			if (this.TargetTagAllowed(IncidentTargetTagDefOf.World))
			{
				if (this.targetTags.Any((IncidentTargetTagDef tt) => tt != IncidentTargetTagDefOf.World))
				{
					yield return "allows world target type along with other targets. World targeting incidents should only target the world.";
				}
			}
			if (this.TargetTagAllowed(IncidentTargetTagDefOf.World) && (this.allowedBiomes != null || this.disallowedBiomes != null))
			{
				yield return "world-targeting incident has a biome restriction list";
			}
			if (!this.allowedBiomes.NullOrEmpty<BiomeDef>() && !this.disallowedBiomes.NullOrEmpty<BiomeDef>())
			{
				yield return "allowedBiomes (white list) and disallowedBiomes (black list) are both defined.";
			}
			if (this.questScriptDef != null && this.questScriptDef.rootSelectionWeight != 0f)
			{
				yield return "quest is run from both incident and random quest.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600977B RID: 38779 RVA: 0x002C390F File Offset: 0x002C1B0F
		public bool TargetTagAllowed(IncidentTargetTagDef target)
		{
			return this.targetTags.Contains(target);
		}

		// Token: 0x0600977C RID: 38780 RVA: 0x002C391D File Offset: 0x002C1B1D
		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTags.Intersect(target.IncidentTargetTags()).Any<IncidentTargetTagDef>();
		}

		// Token: 0x04006488 RID: 25736
		public Type workerClass;

		// Token: 0x04006489 RID: 25737
		public IncidentCategoryDef category;

		// Token: 0x0400648A RID: 25738
		public List<IncidentTargetTagDef> targetTags;

		// Token: 0x0400648B RID: 25739
		public float baseChance;

		// Token: 0x0400648C RID: 25740
		public float baseChanceWithRoyalty = -1f;

		// Token: 0x0400648D RID: 25741
		public IncidentPopulationEffect populationEffect;

		// Token: 0x0400648E RID: 25742
		public int earliestDay;

		// Token: 0x0400648F RID: 25743
		public int minPopulation;

		// Token: 0x04006490 RID: 25744
		public bool requireColonistsPresent;

		// Token: 0x04006491 RID: 25745
		public float minRefireDays;

		// Token: 0x04006492 RID: 25746
		[Obsolete]
		public int minDifficulty;

		// Token: 0x04006493 RID: 25747
		public DifficultyConditionConfig disabledWhen = new DifficultyConditionConfig();

		// Token: 0x04006494 RID: 25748
		public bool pointsScaleable;

		// Token: 0x04006495 RID: 25749
		public float minThreatPoints = float.MinValue;

		// Token: 0x04006496 RID: 25750
		public float maxThreatPoints = float.MaxValue;

		// Token: 0x04006497 RID: 25751
		public List<BiomeDef> allowedBiomes;

		// Token: 0x04006498 RID: 25752
		public List<BiomeDef> disallowedBiomes;

		// Token: 0x04006499 RID: 25753
		[NoTranslate]
		public List<string> tags;

		// Token: 0x0400649A RID: 25754
		[NoTranslate]
		public List<string> refireCheckTags;

		// Token: 0x0400649B RID: 25755
		public SimpleCurve chanceFactorByPopulationCurve;

		// Token: 0x0400649C RID: 25756
		public TaleDef tale;

		// Token: 0x0400649D RID: 25757
		public int minAnomalyThreatLevel = -1;

		// Token: 0x0400649E RID: 25758
		public int minGreatestPopulation = -1;

		// Token: 0x0400649F RID: 25759
		private bool ignoreRecentSelectionWeighting;

		// Token: 0x040064A0 RID: 25760
		public bool hidden;

		// Token: 0x040064A1 RID: 25761
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040064A2 RID: 25762
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x040064A3 RID: 25763
		public bool canOccurOnAllPlanetLayers;

		// Token: 0x040064A4 RID: 25764
		public EntityCodexEntryDef codexEntry;

		// Token: 0x040064A5 RID: 25765
		[MustTranslate]
		public string letterText;

		// Token: 0x040064A6 RID: 25766
		[MustTranslate]
		public string letterLabel;

		// Token: 0x040064A7 RID: 25767
		public LetterDef letterDef;

		// Token: 0x040064A8 RID: 25768
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x040064A9 RID: 25769
		public bool letterSingularForm;

		// Token: 0x040064AA RID: 25770
		public PawnKindDef pawnKind;

		// Token: 0x040064AB RID: 25771
		public bool pawnMustBeCapableOfViolence;

		// Token: 0x040064AC RID: 25772
		public Gender pawnFixedGender;

		// Token: 0x040064AD RID: 25773
		public HediffDef pawnHediff;

		// Token: 0x040064AE RID: 25774
		public GameConditionDef gameCondition;

		// Token: 0x040064AF RID: 25775
		public FloatRange durationDays;

		// Token: 0x040064B0 RID: 25776
		public HediffDef diseaseIncident;

		// Token: 0x040064B1 RID: 25777
		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		// Token: 0x040064B2 RID: 25778
		public int diseaseMaxVictims = 99999;

		// Token: 0x040064B3 RID: 25779
		public List<BiomeDiseaseRecord> diseaseBiomeRecords;

		// Token: 0x040064B4 RID: 25780
		public List<BodyPartDef> diseasePartsToAffect;

		// Token: 0x040064B5 RID: 25781
		[MustTranslate]
		public string diseaseLethalLetterText;

		// Token: 0x040064B6 RID: 25782
		public DevelopmentalStage diseaseDevelopmentStage = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x040064B7 RID: 25783
		public ThingDef mechClusterBuilding;

		// Token: 0x040064B8 RID: 25784
		public List<MTBByBiome> mtbDaysByBiome;

		// Token: 0x040064B9 RID: 25785
		public QuestScriptDef questScriptDef;

		// Token: 0x040064BA RID: 25786
		public ThingDef treeDef;

		// Token: 0x040064BB RID: 25787
		public GenStepDef treeGenStepDef;

		// Token: 0x040064BC RID: 25788
		public string letterTextPlural;

		// Token: 0x040064BD RID: 25789
		public float treeGrowth;

		// Token: 0x040064BE RID: 25790
		[Unsaved(false)]
		private IncidentWorker workerInt;

		// Token: 0x040064BF RID: 25791
		[Unsaved(false)]
		private List<IncidentDef> cachedRefireCheckIncidents;
	}
}
