using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001A96 RID: 6806
	public class MemeDef : Def
	{
		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x0600986A RID: 39018 RVA: 0x002C75BF File Offset: 0x002C57BF
		public Texture2D Icon
		{
			get
			{
				if (!ModLister.CheckIdeology("Memes"))
				{
					return BaseContent.BadTex;
				}
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x0600986B RID: 39019 RVA: 0x002C75FC File Offset: 0x002C57FC
		public List<BuildableDef> AllDesignatorBuildables
		{
			get
			{
				if (this.cachedAllDesignatorBuildables == null)
				{
					this.cachedAllDesignatorBuildables = new List<BuildableDef>();
					if (this.addDesignators != null)
					{
						foreach (BuildableDef item in this.addDesignators)
						{
							this.cachedAllDesignatorBuildables.Add(item);
						}
					}
					if (this.addDesignatorGroups != null)
					{
						foreach (DesignatorDropdownGroupDef designatorDropdownGroupDef in this.addDesignatorGroups)
						{
							this.cachedAllDesignatorBuildables.AddRange(designatorDropdownGroupDef.BuildablesWithoutDefaultDesignators());
						}
					}
				}
				return this.cachedAllDesignatorBuildables;
			}
		}

		// Token: 0x0600986C RID: 39020 RVA: 0x002C76CC File Offset: 0x002C58CC
		public List<string> UnlockedRoles(Ideo ideo)
		{
			if (this.unlockedRoles == null || ideo != this.unlockedRolesCachedFor)
			{
				this.unlockedRoles = new List<string>();
				foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefsListForReading)
				{
					if (typeof(Precept_Role).IsAssignableFrom(preceptDef.preceptClass) && !preceptDef.requiredMemes.NullOrEmpty<MemeDef>() && preceptDef.requiredMemes.Contains(this))
					{
						this.unlockedRoles.Add(preceptDef.LabelCap.Resolve());
					}
				}
				this.unlockedRolesCachedFor = ideo;
			}
			return this.unlockedRoles;
		}

		// Token: 0x0600986D RID: 39021 RVA: 0x002C7790 File Offset: 0x002C5990
		public List<string> UnlockedRituals()
		{
			if (this.unlockedRituals == null)
			{
				this.unlockedRituals = new List<string>();
				if (this.consumableBuildings != null)
				{
					using (List<RitualPatternDef>.Enumerator enumerator = DefDatabase<RitualPatternDef>.AllDefsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RitualPatternDef p = enumerator.Current;
							if (!p.ignoreConsumableBuildingRequirement && p.ritualObligationTargetFilter != null && !p.ritualObligationTargetFilter.thingDefs.NullOrEmpty<ThingDef>() && p.ritualObligationTargetFilter.thingDefs.Any((ThingDef td) => this.consumableBuildings.Contains(td)))
							{
								string text;
								if ((text = p.shortDescOverride.CapitalizeFirst()) == null)
								{
									PreceptDef preceptDef = DefDatabase<PreceptDef>.AllDefsListForReading.FirstOrDefault((PreceptDef pr) => pr.ritualPatternBase == p);
									TaggedString? taggedString = (preceptDef != null) ? new TaggedString?(preceptDef.LabelCap) : null;
									text = ((taggedString != null) ? taggedString.GetValueOrDefault() : null);
								}
								string text2 = text;
								if (text2 != null)
								{
									this.unlockedRituals.Add(text2);
								}
							}
						}
					}
				}
			}
			return this.unlockedRituals;
		}

		// Token: 0x0600986E RID: 39022 RVA: 0x002C78E0 File Offset: 0x002C5AE0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.category == MemeCategory.Structure && string.IsNullOrWhiteSpace(this.worshipRoomLabel))
			{
				yield return "Structure meme has empty worshipRoomLabel";
			}
			if (this.fixedDeityNameTypes != null && this.category != MemeCategory.Structure)
			{
				yield return "fixedDeityNameTypes can only be used in structure memes.";
			}
			if (!this.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.priority <= 0f)
					{
						yield return "style category " + thingStyleCategoryWithPriority.category.LabelCap + " has <= 0 priority. It must be positive.";
					}
				}
				List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = default(List<ThingStyleCategoryWithPriority>.Enumerator);
			}
			if (this.category == MemeCategory.Structure && this.impact != 0)
			{
				yield return this.defName + " structure meme impact must be 0.";
			}
			if (this.category != MemeCategory.Structure && (this.impact < 1 || this.impact > 3))
			{
				yield return string.Format("{0} normal meme impact must be between 1 and {1}.", this.defName, 3);
			}
			if (!this.agreeableTraits.NullOrEmpty<TraitRequirement>() && !this.disagreeableTraits.NullOrEmpty<TraitRequirement>() && this.agreeableTraits.SharesElementWith(this.disagreeableTraits))
			{
				yield return this.defName + " agreeableTraits and disagreeableTraits share one or more values.";
			}
			if (this.category == MemeCategory.Structure)
			{
				if (this.descriptionMaker == null)
				{
					yield return "descriptionMaker is required for structure memes.";
				}
				else
				{
					if (this.descriptionMaker.patterns.NullOrEmpty<IdeoDescriptionMaker.PatternEntry>())
					{
						yield return "descriptionMaker must define at least one pattern for structure memes.";
					}
					using (List<IdeoDescriptionMaker.PatternEntry>.Enumerator enumerator3 = this.descriptionMaker.patterns.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.def == null)
							{
								yield return "descriptionMaker pattern has null def.";
							}
						}
					}
					List<IdeoDescriptionMaker.PatternEntry>.Enumerator enumerator3 = default(List<IdeoDescriptionMaker.PatternEntry>.Enumerator);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400667B RID: 26235
		public string iconPath;

		// Token: 0x0400667C RID: 26236
		public int renderOrder;

		// Token: 0x0400667D RID: 26237
		public int impact = -999;

		// Token: 0x0400667E RID: 26238
		public MemeCategory category;

		// Token: 0x0400667F RID: 26239
		public MemeGroupDef groupDef;

		// Token: 0x04006680 RID: 26240
		[NoTranslate]
		public List<string> exclusionTags = new List<string>();

		// Token: 0x04006681 RID: 26241
		public List<List<PreceptDef>> requireOne;

		// Token: 0x04006682 RID: 26242
		public PreceptsWithNoneChance selectOneOrNone;

		// Token: 0x04006683 RID: 26243
		public List<RequiredRitualAndBuilding> requiredRituals;

		// Token: 0x04006684 RID: 26244
		public IdeoWeaponClassPair preferredWeaponClasses;

		// Token: 0x04006685 RID: 26245
		public float randomizationSelectionWeightFactor = 1f;

		// Token: 0x04006686 RID: 26246
		public List<ThingDef> requireAnyRitualSeat;

		// Token: 0x04006687 RID: 26247
		public IntRange deityCount = new IntRange(-1, -1);

		// Token: 0x04006688 RID: 26248
		public bool allowDuringTutorial;

		// Token: 0x04006689 RID: 26249
		public List<FactionDef> factionWhitelist;

		// Token: 0x0400668A RID: 26250
		public bool hiddenInChooseMemes;

		// Token: 0x0400668B RID: 26251
		public RulePack generalRules;

		// Token: 0x0400668C RID: 26252
		public IdeoDescriptionMaker descriptionMaker;

		// Token: 0x0400668D RID: 26253
		public RulePackDef deityNameMakerOverride;

		// Token: 0x0400668E RID: 26254
		public RulePackDef deityTypeMakerOverride;

		// Token: 0x0400668F RID: 26255
		public bool allowSymbolsFromDeity = true;

		// Token: 0x04006690 RID: 26256
		public bool symbolPackOverride;

		// Token: 0x04006691 RID: 26257
		[MustTranslate]
		public List<IdeoSymbolPack> symbolPacks;

		// Token: 0x04006692 RID: 26258
		public List<DeityNameType> fixedDeityNameTypes;

		// Token: 0x04006693 RID: 26259
		public List<ThingStyleCategoryWithPriority> thingStyleCategories;

		// Token: 0x04006694 RID: 26260
		public IntRange ritualsToMake = IntRange.Zero;

		// Token: 0x04006695 RID: 26261
		public List<string> replaceRitualsWithTags = new List<string>();

		// Token: 0x04006696 RID: 26262
		public List<RitualPatternDef> replacementPatterns;

		// Token: 0x04006697 RID: 26263
		public List<ThingDef> consumableBuildings;

		// Token: 0x04006698 RID: 26264
		public int veneratedAnimalsCountOffset;

		// Token: 0x04006699 RID: 26265
		public int veneratedAnimalsCountOverride = -1;

		// Token: 0x0400669A RID: 26266
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x0400669B RID: 26267
		[MustTranslate]
		public string worshipRoomLabel;

		// Token: 0x0400669C RID: 26268
		public List<ResearchProjectDef> startingResearchProjects = new List<ResearchProjectDef>();

		// Token: 0x0400669D RID: 26269
		public List<BuildableDef> addDesignators;

		// Token: 0x0400669E RID: 26270
		public List<DesignatorDropdownGroupDef> addDesignatorGroups;

		// Token: 0x0400669F RID: 26271
		public List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x040066A0 RID: 26272
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> agreeableTraits;

		// Token: 0x040066A1 RID: 26273
		[XmlInheritanceAllowDuplicateNodes]
		public List<TraitRequirement> disagreeableTraits;

		// Token: 0x040066A2 RID: 26274
		public bool preventApparelRequirements;

		// Token: 0x040066A3 RID: 26275
		public XenotypeSet xenotypeSet;

		// Token: 0x040066A4 RID: 26276
		private Texture2D icon;

		// Token: 0x040066A5 RID: 26277
		private List<BuildableDef> cachedAllDesignatorBuildables;

		// Token: 0x040066A6 RID: 26278
		private List<string> unlockedRoles;

		// Token: 0x040066A7 RID: 26279
		private List<string> unlockedRituals;

		// Token: 0x040066A8 RID: 26280
		private Ideo unlockedRolesCachedFor;
	}
}
