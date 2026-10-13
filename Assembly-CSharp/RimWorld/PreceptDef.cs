using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ABF RID: 6847
	public class PreceptDef : Def
	{
		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x06009947 RID: 39239 RVA: 0x002CA828 File Offset: 0x002C8A28
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					if (this.iconPath != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
					else if (!this.issue.iconPath.NullOrEmpty())
					{
						this.icon = this.issue.Icon;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x06009948 RID: 39240 RVA: 0x002CA888 File Offset: 0x002C8A88
		public PreceptWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (PreceptWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x06009949 RID: 39241 RVA: 0x002CA8BC File Offset: 0x002C8ABC
		public List<TraitRequirement> TraitsAffecting
		{
			get
			{
				if (this.traitsAffectingCached == null)
				{
					this.traitsAffectingCached = new List<TraitRequirement>();
					for (int i = 0; i < this.comps.Count; i++)
					{
						this.traitsAffectingCached.AddRange(this.comps[i].TraitsAffecting);
					}
					this.traitsAffectingCached.RemoveDuplicates((TraitRequirement a, TraitRequirement b) => a.def == b.def && ((a.degree != null) ? a.degree.Value : 0) == ((b.degree != null) ? b.degree.Value : 0));
				}
				return this.traitsAffectingCached;
			}
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x0600994A RID: 39242 RVA: 0x002CA940 File Offset: 0x002C8B40
		public List<string> RequiredMemeLabels
		{
			get
			{
				if (this.requiredMemeLabels == null && !this.requiredMemes.NullOrEmpty<MemeDef>())
				{
					this.requiredMemeLabels = new List<string>();
					foreach (MemeDef memeDef in this.requiredMemes)
					{
						this.requiredMemeLabels.Add(memeDef.label);
					}
				}
				return this.requiredMemeLabels;
			}
		}

		// Token: 0x0600994B RID: 39243 RVA: 0x002CA9C4 File Offset: 0x002C8BC4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.preceptClass == typeof(Precept_Ritual) && this.ritualPatternBase == null)
			{
				yield return "Ritual doesn't define a ritualPatternBase, was it meant to be abstract?";
			}
			if (typeof(Precept_ThingStyle).IsAssignableFrom(this.preceptClass))
			{
				foreach (PreceptThingChance preceptThingChance in this.Worker.ThingDefs)
				{
					ThingDef def = preceptThingChance.def;
					if (!def.CanBeStyled())
					{
						yield return string.Concat(new string[]
						{
							"ThingDef ",
							def.defName,
							" is on available things list of ",
							this.defName,
							" precept, but missing CompStyleable thing comp!"
						});
					}
				}
				IEnumerator<PreceptThingChance> enumerator2 = null;
			}
			foreach (PreceptComp preceptComp in this.comps)
			{
				foreach (string text2 in preceptComp.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			List<PreceptComp>.Enumerator enumerator3 = default(List<PreceptComp>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600994C RID: 39244 RVA: 0x002CA9D4 File Offset: 0x002C8BD4
		public override void PostLoad()
		{
			base.PostLoad();
			foreach (PreceptComp preceptComp in this.comps)
			{
				preceptComp.preceptDef = this;
			}
		}

		// Token: 0x0400683D RID: 26685
		public Type preceptClass = typeof(Precept);

		// Token: 0x0400683E RID: 26686
		public IssueDef issue;

		// Token: 0x0400683F RID: 26687
		public List<PreceptComp> comps = new List<PreceptComp>();

		// Token: 0x04006840 RID: 26688
		public List<AbilityStatModifiers> abilityStatFactors;

		// Token: 0x04006841 RID: 26689
		public List<StatModifier> statOffsets;

		// Token: 0x04006842 RID: 26690
		public List<StatModifier> statFactors;

		// Token: 0x04006843 RID: 26691
		public List<ConditionalStatAffecter> conditionalStatAffecters;

		// Token: 0x04006844 RID: 26692
		public float selectionWeight = 1f;

		// Token: 0x04006845 RID: 26693
		public List<WorkTypeDef> opposedWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04006846 RID: 26694
		public PreceptImpact impact;

		// Token: 0x04006847 RID: 26695
		public List<MemeDef> associatedMemes = new List<MemeDef>();

		// Token: 0x04006848 RID: 26696
		public List<MemeDef> conflictingMemes = new List<MemeDef>();

		// Token: 0x04006849 RID: 26697
		public List<MemeDef> requiredMemes = new List<MemeDef>();

		// Token: 0x0400684A RID: 26698
		public bool visible = true;

		// Token: 0x0400684B RID: 26699
		public bool visibleOnAddFloatMenu = true;

		// Token: 0x0400684C RID: 26700
		public bool listedForRoles = true;

		// Token: 0x0400684D RID: 26701
		public PreceptDef takeNameFrom;

		// Token: 0x0400684E RID: 26702
		public PreceptDef alsoAdds;

		// Token: 0x0400684F RID: 26703
		public int maxCount = 1;

		// Token: 0x04006850 RID: 26704
		[NoTranslate]
		public List<string> exclusionTags = new List<string>();

		// Token: 0x04006851 RID: 26705
		public bool allowDuplicates;

		// Token: 0x04006852 RID: 26706
		public bool ignoreLimitsInEditMode;

		// Token: 0x04006853 RID: 26707
		public bool canUseAlreadyUsedThingDef;

		// Token: 0x04006854 RID: 26708
		public bool classic;

		// Token: 0x04006855 RID: 26709
		public bool classicExtra;

		// Token: 0x04006856 RID: 26710
		public float defaultSelectionWeight;

		// Token: 0x04006857 RID: 26711
		public bool enabledForNPCFactions = true;

		// Token: 0x04006858 RID: 26712
		public bool countsTowardsPreceptLimit = true;

		// Token: 0x04006859 RID: 26713
		public bool canGenerateAsSpecialPrecept = true;

		// Token: 0x0400685A RID: 26714
		public RulePackDef nameMaker;

		// Token: 0x0400685B RID: 26715
		public int nameMaxLength = 16;

		// Token: 0x0400685C RID: 26716
		public SimpleCurve preceptInstanceCountCurve;

		// Token: 0x0400685D RID: 26717
		public RitualPatternDef ritualPatternBase;

		// Token: 0x0400685E RID: 26718
		public bool receivesExpectationsQualityOffset;

		// Token: 0x0400685F RID: 26719
		public bool usesIdeoVisualEffects = true;

		// Token: 0x04006860 RID: 26720
		public bool mergeRitualGizmosFromAllIdeos;

		// Token: 0x04006861 RID: 26721
		public bool allowSpectatorsFromOtherIdeos = true;

		// Token: 0x04006862 RID: 26722
		public bool allowOptionalRitualObligations;

		// Token: 0x04006863 RID: 26723
		public bool classicModeOnly;

		// Token: 0x04006864 RID: 26724
		public List<PreceptThingChanceClass> buildingDefChances;

		// Token: 0x04006865 RID: 26725
		public List<ExpectationDef> buildingMinExpectations;

		// Token: 0x04006866 RID: 26726
		public List<RoomRequirement> buildingRoomRequirements;

		// Token: 0x04006867 RID: 26727
		public List<RoomRequirement> buildingRoomRequirementsFixed;

		// Token: 0x04006868 RID: 26728
		public SimpleCurve roomRequirementCountCurve;

		// Token: 0x04006869 RID: 26729
		public bool leaderRole;

		// Token: 0x0400686A RID: 26730
		public int activationBelieverCount = -1;

		// Token: 0x0400686B RID: 26731
		public int deactivationBelieverCount = -1;

		// Token: 0x0400686C RID: 26732
		public List<RoleRequirement> roleRequirements;

		// Token: 0x0400686D RID: 26733
		public WorkTags roleDisabledWorkTags;

		// Token: 0x0400686E RID: 26734
		public WorkTags roleRequiredWorkTags;

		// Token: 0x0400686F RID: 26735
		public WorkTags roleRequiredWorkTagAny;

		// Token: 0x04006870 RID: 26736
		public List<PreceptApparelRequirement> roleApparelRequirements;

		// Token: 0x04006871 RID: 26737
		public SimpleCurve roleApparelRequirementCountCurve;

		// Token: 0x04006872 RID: 26738
		public List<AbilityDef> grantedAbilities;

		// Token: 0x04006873 RID: 26739
		public List<RoleEffect> roleEffects;

		// Token: 0x04006874 RID: 26740
		public List<string> roleTags;

		// Token: 0x04006875 RID: 26741
		public string iconPath;

		// Token: 0x04006876 RID: 26742
		public float restrictToSupremeGenderChance;

		// Token: 0x04006877 RID: 26743
		public float certaintyLossFactor = 1f;

		// Token: 0x04006878 RID: 26744
		public float convertPowerFactor = 1f;

		// Token: 0x04006879 RID: 26745
		public int expectationsOffset;

		// Token: 0x0400687A RID: 26746
		public bool createsRoleEmptyThought = true;

		// Token: 0x0400687B RID: 26747
		public bool roleCanBeChild;

		// Token: 0x0400687C RID: 26748
		public bool disallowLoggingCamps;

		// Token: 0x0400687D RID: 26749
		public bool disallowMiningCamps;

		// Token: 0x0400687E RID: 26750
		public bool disallowHuntingCamps;

		// Token: 0x0400687F RID: 26751
		public bool disallowFarmingCamps;

		// Token: 0x04006880 RID: 26752
		public bool approvesOfSlavery;

		// Token: 0x04006881 RID: 26753
		public bool prefersNudity;

		// Token: 0x04006882 RID: 26754
		public Gender genderPrefersNudity;

		// Token: 0x04006883 RID: 26755
		public bool useChoicesFromBuildingDefs;

		// Token: 0x04006884 RID: 26756
		public int displayOrderInImpact;

		// Token: 0x04006885 RID: 26757
		public int displayOrderInIssue;

		// Token: 0x04006886 RID: 26758
		public bool proselytizes;

		// Token: 0x04006887 RID: 26759
		public int requiredScars;

		// Token: 0x04006888 RID: 26760
		public bool approvesOfCharity;

		// Token: 0x04006889 RID: 26761
		public float blindPawnChance = -1f;

		// Token: 0x0400688A RID: 26762
		public bool approvesOfBlindness;

		// Token: 0x0400688B RID: 26763
		public bool canRemoveInUI = true;

		// Token: 0x0400688C RID: 26764
		public bool prefersDarkness;

		// Token: 0x0400688D RID: 26765
		public List<NeedDef> enablesNeeds;

		// Token: 0x0400688E RID: 26766
		public List<NeedDef> disablesNeeds;

		// Token: 0x0400688F RID: 26767
		public float blindPsychicSensitivityOffset;

		// Token: 0x04006890 RID: 26768
		public DrugPolicyDef defaultDrugPolicyOverride;

		// Token: 0x04006891 RID: 26769
		public bool warnPlayerOnDesignateChopTree;

		// Token: 0x04006892 RID: 26770
		public bool warnPlayerOnDesignateMine;

		// Token: 0x04006893 RID: 26771
		public bool willingToConstructOtherIdeoBuildables;

		// Token: 0x04006894 RID: 26772
		public float biosculpterPodCycleSpeedFactor = 1f;

		// Token: 0x04006895 RID: 26773
		public float growthVatSpeedFactor = 1f;

		// Token: 0x04006896 RID: 26774
		public bool prefersSlabBed;

		// Token: 0x04006897 RID: 26775
		public bool useRepeatPenalty = true;

		// Token: 0x04006898 RID: 26776
		public bool showRitualFloatMenuOption = true;

		// Token: 0x04006899 RID: 26777
		[MustTranslate]
		public string tipLabelOverride;

		// Token: 0x0400689A RID: 26778
		public bool capitalizeAsTitle = true;

		// Token: 0x0400689B RID: 26779
		public bool ignoreNameUniqueness;

		// Token: 0x0400689C RID: 26780
		[MustTranslate]
		public string extraTextPawnDeathLetter;

		// Token: 0x0400689D RID: 26781
		public PreceptDef apparelPreceptSwapDef;

		// Token: 0x0400689E RID: 26782
		public AbilityGroupDef useCooldownFromAbilityGroupDef;

		// Token: 0x0400689F RID: 26783
		public bool iconIgnoresIdeoColor;

		// Token: 0x040068A0 RID: 26784
		public PreceptDef sourcePawnRoleDef;

		// Token: 0x040068A1 RID: 26785
		public AbilityDef sourceAbilityDef;

		// Token: 0x040068A2 RID: 26786
		public bool likesHumanLeatherApparel;

		// Token: 0x040068A3 RID: 26787
		public bool approvesOfRaiding;

		// Token: 0x040068A4 RID: 26788
		public float skipOpportunityLettersBeforeDay = 10f;

		// Token: 0x040068A5 RID: 26789
		public bool notifyPlayerOnOpportunity = true;

		// Token: 0x040068A6 RID: 26790
		public Type workerClass = typeof(PreceptWorker);

		// Token: 0x040068A7 RID: 26791
		private PreceptWorker worker;

		// Token: 0x040068A8 RID: 26792
		[Unsaved(false)]
		private List<TraitRequirement> traitsAffectingCached;

		// Token: 0x040068A9 RID: 26793
		[Unsaved(false)]
		private List<string> requiredMemeLabels;

		// Token: 0x040068AA RID: 26794
		private Texture2D icon;
	}
}
