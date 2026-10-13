using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C9 RID: 713
	public class RecipeDef : Def
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00051406 File Offset: 0x0004F606
		public ThingDef UIIconThing
		{
			get
			{
				return this.uiIconThing ?? this.ProducedThingDef;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x00051418 File Offset: 0x0004F618
		public Texture2D UIIcon
		{
			get
			{
				if (!this.resolvedCachedIcon)
				{
					this.resolvedCachedIcon = true;
					if (!this.uiIconPath.NullOrEmpty())
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x0005144E File Offset: 0x0004F64E
		public RecipeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecipeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.recipe = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x00051480 File Offset: 0x0004F680
		public RecipeWorkerCounter WorkerCounter
		{
			get
			{
				if (this.workerCounterInt == null)
				{
					this.workerCounterInt = (RecipeWorkerCounter)Activator.CreateInstance(this.workerCounterClass);
					this.workerCounterInt.recipe = this;
				}
				return this.workerCounterInt;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x000514B2 File Offset: 0x0004F6B2
		public IngredientValueGetter IngredientValueGetter
		{
			get
			{
				if (this.ingredientValueGetterInt == null)
				{
					this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
				}
				return this.ingredientValueGetterInt;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x000514D8 File Offset: 0x0004F6D8
		public bool AvailableNow
		{
			get
			{
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					return false;
				}
				if (this.memePrerequisitesAny != null)
				{
					bool flag = false;
					foreach (MemeDef meme in this.memePrerequisitesAny)
					{
						if (Faction.OfPlayer.ideos.HasAnyIdeoWithMeme(meme))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (this.researchPrerequisites != null)
				{
					if (this.researchPrerequisites.Any((ResearchProjectDef r) => !r.IsFinished))
					{
						return false;
					}
				}
				if (this.factionPrerequisiteTags != null)
				{
					if (this.factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
					{
						RecipeDef.<>c__DisplayClass91_0 CS$<>8__locals1;
						CS$<>8__locals1.unlockedByIdeo = false;
						this.<get_AvailableNow>g__Check|91_2(ref CS$<>8__locals1);
						if (!CS$<>8__locals1.unlockedByIdeo)
						{
							return false;
						}
					}
				}
				return !this.fromIdeoBuildingPreceptOnly || (ModsConfig.IdeologyActive && IdeoUtility.PlayerHasPreceptForBuilding(this.ProducedThingDef));
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x00051604 File Offset: 0x0004F804
		public string MinSkillString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (this.skillRequirements != null)
				{
					for (int i = 0; i < this.skillRequirements.Count; i++)
					{
						SkillRequirement skillRequirement = this.skillRequirements[i];
						stringBuilder.AppendLine("   " + skillRequirement.skill.skillLabel.CapitalizeFirst() + ": " + skillRequirement.minLevel.ToString());
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x000516AA File Offset: 0x0004F8AA
		public IEnumerable<ThingDef> AllRecipeUsers
		{
			get
			{
				int num;
				if (this.recipeUsers != null)
				{
					for (int i = 0; i < this.recipeUsers.Count; i = num + 1)
					{
						yield return this.recipeUsers[i];
						num = i;
					}
				}
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < thingDefs.Count; i = num + 1)
				{
					if (thingDefs[i].recipes != null && thingDefs[i].recipes.Contains(this))
					{
						yield return thingDefs[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x000516BA File Offset: 0x0004F8BA
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x000516C8 File Offset: 0x0004F8C8
		public bool IsSurgery
		{
			get
			{
				if (this.isSurgeryCached == null)
				{
					this.isSurgeryCached = new bool?(false);
					using (IEnumerator<ThingDef> enumerator = this.AllRecipeUsers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.category == ThingCategory.Pawn)
							{
								this.isSurgeryCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isSurgeryCached.Value;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x00051748 File Offset: 0x0004F948
		public ThingDef ProducedThingDef
		{
			get
			{
				if (this.specialProducts != null)
				{
					return null;
				}
				if (this.products == null || this.products.Count != 1)
				{
					return null;
				}
				return this.products[0].thingDef;
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0005177D File Offset: 0x0004F97D
		public bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return this.Worker.AvailableOnNow(thing, part);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0005178C File Offset: 0x0004F98C
		public float WorkAmountTotal(Thing thing)
		{
			if (thing == null)
			{
				return this.WorkAmountForStuff(null);
			}
			if (this == RecipeDefOf.SmeltOrDestroyThing && thing.Smeltable)
			{
				return this.smeltingWorkAmount;
			}
			return this.WorkAmountForStuff(thing.Stuff);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000517BC File Offset: 0x0004F9BC
		public float WorkAmountForStuff(ThingDef stuff)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuff);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000517EE File Offset: 0x0004F9EE
		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			int num;
			for (int i = 0; i < this.ingredients.Count; i = num + 1)
			{
				IngredientCount ingredientCount = this.ingredients[i];
				bool flag = false;
				List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing = list[j];
					if ((billDoer == null || !thing.IsForbidden(billDoer)) && !thing.Position.Fogged(map) && (ingredientCount.IsFixedIngredient || this.fixedIngredientFilter.Allows(thing)) && ingredientCount.filter.Allows(thing))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (ingredientCount.IsFixedIngredient)
					{
						yield return ingredientCount.filter.AllowedThingDefs.First<ThingDef>();
					}
					else
					{
						ThingDef thingDef = (from x in ingredientCount.filter.AllowedThingDefs
						orderby x.BaseMarketValue
						select x).FirstOrDefault((ThingDef x) => this.fixedIngredientFilter.Allows(x));
						if (thingDef != null)
						{
							yield return thingDef;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0005180C File Offset: 0x0004FA0C
		public bool IsIngredient(ThingDef th)
		{
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (this.ingredients[i].filter.Allows(th) && (this.ingredients[i].IsFixedIngredient || this.fixedIngredientFilter.Allows(th)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0005186C File Offset: 0x0004FA6C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0005187C File Offset: 0x0004FA7C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			DeepProfiler.Start("Stat refs");
			try
			{
				if (this.workTableSpeedStat == null)
				{
					this.workTableSpeedStat = StatDefOf.WorkTableWorkSpeedFactor;
				}
				if (this.workTableEfficiencyStat == null)
				{
					this.workTableEfficiencyStat = StatDefOf.WorkTableEfficiencyFactor;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ingredients reference resolve");
			try
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					this.ingredients[i].ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("fixedIngredientFilter.ResolveReferences()");
			try
			{
				ThingFilter thingFilter = this.fixedIngredientFilter;
				if (thingFilter != null)
				{
					thingFilter.ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter setup");
			try
			{
				if (this.defaultIngredientFilter == null)
				{
					this.defaultIngredientFilter = new ThingFilter();
					if (this.fixedIngredientFilter != null)
					{
						this.defaultIngredientFilter.CopyAllowancesFrom(this.fixedIngredientFilter);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter.ResolveReferences()");
			try
			{
				this.defaultIngredientFilter.ResolveReferences();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000519C0 File Offset: 0x0004FBC0
		public bool CompatibleWithHediff(HediffDef hediffDef)
		{
			if (this.incompatibleWithHediffTags.NullOrEmpty<string>() || hediffDef.tags.NullOrEmpty<string>())
			{
				return true;
			}
			for (int i = 0; i < this.incompatibleWithHediffTags.Count; i++)
			{
				for (int j = 0; j < hediffDef.tags.Count; j++)
				{
					if (this.incompatibleWithHediffTags[i].Equals(hediffDef.tags[j], StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00051A38 File Offset: 0x0004FC38
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00051A44 File Offset: 0x0004FC44
		public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
		{
			if (this.skillRequirements == null)
			{
				return null;
			}
			for (int i = 0; i < this.skillRequirements.Count; i++)
			{
				if (!this.skillRequirements[i].PawnSatisfies(pawn))
				{
					return this.skillRequirements[i];
				}
			}
			return null;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00051A94 File Offset: 0x0004FC94
		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			if (this.premultipliedSmallIngredients != null)
			{
				return this.premultipliedSmallIngredients;
			}
			this.premultipliedSmallIngredients = (from td in this.ingredients.SelectMany((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs)
			where td.smallVolume
			select td).Distinct<ThingDef>().ToList<ThingDef>();
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (this.ingredients[i].filter.AllowedThingDefs.Any((ThingDef td) => !this.premultipliedSmallIngredients.Contains(td)))
					{
						foreach (ThingDef item in this.ingredients[i].filter.AllowedThingDefs)
						{
							flag |= this.premultipliedSmallIngredients.Remove(item);
						}
					}
				}
			}
			return this.premultipliedSmallIngredients;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00051BC0 File Offset: 0x0004FDC0
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00051C44 File Offset: 0x0004FE44
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.products
			select i.thingDef);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00051C75 File Offset: 0x0004FE75
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, "Stat_Recipe_Skill_Desc".Translate(), 4404, null, null, false, false);
			}
			if (!this.ingredients.NullOrEmpty<IngredientCount>())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
				select ic.SummaryFor(this)).ToCommaList(false, false), "Stat_Recipe_Ingredients_Desc".Translate(), 4405, null, this.GetIngredientsHyperlinks(), false, false);
			}
			if (!this.skillRequirements.NullOrEmpty<SkillRequirement>())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
				select sr.Summary).ToCommaList(false, false), "Stat_Recipe_SkillRequirements_Desc".Translate(), 4403, null, null, false, false);
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
				select pr.Summary).ToCommaList(false, false), "Stat_Recipe_Products_Desc".Translate(), 4405, null, this.GetProductsHyperlinks(), false, false);
				float num = this.WorkAmountTotal(null);
				if (num > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, StatDefOf.WorkToMake.LabelCap, num.ToStringWorkAmount(), StatDefOf.WorkToMake.description, StatDefOf.WorkToMake.displayPriorityInCategory, null, null, false, false);
				}
			}
			if (this.workSpeedStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, "Stat_Recipe_WorkSpeedStat_Desc".Translate(), 4402, null, null, false, false);
			}
			if (this.efficiencyStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, "Stat_Recipe_EfficiencyStat_Desc".Translate(), 4401, null, null, false, false);
			}
			if (this.IsSurgery)
			{
				if (this.surgerySuccessChanceFactor >= 99999f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail_Desc".Translate(), 4102, null, null, false, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), "Stat_Thing_Surgery_SuccessChanceFactor_Desc".Translate(), 4102, null, null, false, false);
					if (this.deathOnFailedSurgeryChance >= 99999f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), "100%", "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false, false);
					}
					else
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), this.deathOnFailedSurgeryChance.ToStringPercent(), "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false, false);
					}
				}
			}
			if (this.addsHediff != null)
			{
				foreach (StatDrawEntry statDrawEntry in MedicalRecipesUtility.GetMedicalStatsFromRecipeDefs(Gen.YieldSingle<RecipeDef>(this)))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00051D84 File Offset: 0x0004FF84
		[CompilerGenerated]
		private void <get_AvailableNow>g__Check|91_2(ref RecipeDef.<>c__DisplayClass91_0 A_1)
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept_Role precept_Role in ideo.RolesListForReading)
				{
					if (precept_Role.apparelRequirements != null)
					{
						foreach (PreceptApparelRequirement preceptApparelRequirement in precept_Role.apparelRequirements)
						{
							ThingDef thingDef = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None).FirstOrDefault<ThingDef>();
							if (thingDef == null)
							{
								Log.Error("Apparel requirement for role " + precept_Role.Label + " is null");
							}
							using (List<ThingDefCountClass>.Enumerator enumerator4 = this.products.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									if (enumerator4.Current.thingDef == thingDef)
									{
										A_1.unlockedByIdeo = true;
										return;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000F7F RID: 3967
		public Type workerClass = typeof(RecipeWorker);

		// Token: 0x04000F80 RID: 3968
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		// Token: 0x04000F81 RID: 3969
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		// Token: 0x04000F82 RID: 3970
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x04000F83 RID: 3971
		public float workAmount = -1f;

		// Token: 0x04000F84 RID: 3972
		public StatDef workSpeedStat;

		// Token: 0x04000F85 RID: 3973
		public StatDef efficiencyStat;

		// Token: 0x04000F86 RID: 3974
		public StatDef workTableEfficiencyStat;

		// Token: 0x04000F87 RID: 3975
		public StatDef workTableSpeedStat;

		// Token: 0x04000F88 RID: 3976
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		// Token: 0x04000F89 RID: 3977
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		// Token: 0x04000F8A RID: 3978
		public ThingFilter defaultIngredientFilter;

		// Token: 0x04000F8B RID: 3979
		public bool allowMixingIngredients;

		// Token: 0x04000F8C RID: 3980
		public bool ignoreIngredientCountTakeEntireStacks;

		// Token: 0x04000F8D RID: 3981
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		// Token: 0x04000F8E RID: 3982
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		// Token: 0x04000F8F RID: 3983
		public bool autoStripCorpses = true;

		// Token: 0x04000F90 RID: 3984
		public bool interruptIfIngredientIsRotting;

		// Token: 0x04000F91 RID: 3985
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		// Token: 0x04000F92 RID: 3986
		public List<SpecialProductType> specialProducts;

		// Token: 0x04000F93 RID: 3987
		public bool productHasIngredientStuff;

		// Token: 0x04000F94 RID: 3988
		public bool useIngredientsForColor = true;

		// Token: 0x04000F95 RID: 3989
		public int targetCountAdjustment = 1;

		// Token: 0x04000F96 RID: 3990
		public ThingDef unfinishedThingDef;

		// Token: 0x04000F97 RID: 3991
		public List<SkillRequirement> skillRequirements;

		// Token: 0x04000F98 RID: 3992
		public SkillDef workSkill;

		// Token: 0x04000F99 RID: 3993
		public float workSkillLearnFactor = 1f;

		// Token: 0x04000F9A RID: 3994
		public EffecterDef effectWorking;

		// Token: 0x04000F9B RID: 3995
		public SoundDef soundWorking;

		// Token: 0x04000F9C RID: 3996
		private ThingDef uiIconThing;

		// Token: 0x04000F9D RID: 3997
		[NoTranslate]
		private string uiIconPath;

		// Token: 0x04000F9E RID: 3998
		public List<ThingDef> recipeUsers;

		// Token: 0x04000F9F RID: 3999
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		// Token: 0x04000FA0 RID: 4000
		public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x04000FA1 RID: 4001
		public HediffDef addsHediff;

		// Token: 0x04000FA2 RID: 4002
		public HediffDef removesHediff;

		// Token: 0x04000FA3 RID: 4003
		public HediffDef addsHediffOnFailure;

		// Token: 0x04000FA4 RID: 4004
		public HediffDef changesHediffLevel;

		// Token: 0x04000FA5 RID: 4005
		public List<string> incompatibleWithHediffTags;

		// Token: 0x04000FA6 RID: 4006
		public int hediffLevelOffset;

		// Token: 0x04000FA7 RID: 4007
		public bool hideBodyPartNames;

		// Token: 0x04000FA8 RID: 4008
		public bool isViolation;

		// Token: 0x04000FA9 RID: 4009
		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		// Token: 0x04000FAA RID: 4010
		public float surgerySuccessChanceFactor = 1f;

		// Token: 0x04000FAB RID: 4011
		public float deathOnFailedSurgeryChance;

		// Token: 0x04000FAC RID: 4012
		public bool targetsBodyPart = true;

		// Token: 0x04000FAD RID: 4013
		public bool anesthetize = true;

		// Token: 0x04000FAE RID: 4014
		public int minPartCount = -1;

		// Token: 0x04000FAF RID: 4015
		public bool surgeryIgnoreEnvironment;

		// Token: 0x04000FB0 RID: 4016
		public SurgeryOutcomeEffectDef surgeryOutcomeEffect;

		// Token: 0x04000FB1 RID: 4017
		public Gender? genderPrerequisite;

		// Token: 0x04000FB2 RID: 4018
		public bool mustBeFertile;

		// Token: 0x04000FB3 RID: 4019
		public bool allowedForQuestLodgers = true;

		// Token: 0x04000FB4 RID: 4020
		public int minAllowedAge = -1;

		// Token: 0x04000FB5 RID: 4021
		public DevelopmentalStage? developmentalStageFilter;

		// Token: 0x04000FB6 RID: 4022
		public bool humanlikeOnly;

		// Token: 0x04000FB7 RID: 4023
		public List<MutantDef> mutantPrerequisite;

		// Token: 0x04000FB8 RID: 4024
		public List<MutantDef> mutantBlacklist;

		// Token: 0x04000FB9 RID: 4025
		public float smeltingWorkAmount = -1f;

		// Token: 0x04000FBA RID: 4026
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x04000FBB RID: 4027
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x04000FBC RID: 4028
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000FBD RID: 4029
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x04000FBE RID: 4030
		public bool fromIdeoBuildingPreceptOnly;

		// Token: 0x04000FBF RID: 4031
		public ConceptDef conceptLearned;

		// Token: 0x04000FC0 RID: 4032
		public bool dontShowIfAnyIngredientMissing;

		// Token: 0x04000FC1 RID: 4033
		public int displayPriority = 99999;

		// Token: 0x04000FC2 RID: 4034
		public int formingTicks;

		// Token: 0x04000FC3 RID: 4035
		public bool mechanitorOnlyRecipe;

		// Token: 0x04000FC4 RID: 4036
		public int gestationCycles;

		// Token: 0x04000FC5 RID: 4037
		public bool mechResurrection;

		// Token: 0x04000FC6 RID: 4038
		[Unsaved(false)]
		private RecipeWorker workerInt;

		// Token: 0x04000FC7 RID: 4039
		[Unsaved(false)]
		private RecipeWorkerCounter workerCounterInt;

		// Token: 0x04000FC8 RID: 4040
		[Unsaved(false)]
		private IngredientValueGetter ingredientValueGetterInt;

		// Token: 0x04000FC9 RID: 4041
		[Unsaved(false)]
		private List<ThingDef> premultipliedSmallIngredients;

		// Token: 0x04000FCA RID: 4042
		[Unsaved(false)]
		public bool regenerateOnDifficultyChange;

		// Token: 0x04000FCB RID: 4043
		[Unsaved(false)]
		public int adjustedCount = 1;

		// Token: 0x04000FCC RID: 4044
		[Unsaved(false)]
		private Texture2D cachedIcon;

		// Token: 0x04000FCD RID: 4045
		[Unsaved(false)]
		private bool resolvedCachedIcon;

		// Token: 0x04000FCE RID: 4046
		[Unsaved(false)]
		private bool? isSurgeryCached;
	}
}
