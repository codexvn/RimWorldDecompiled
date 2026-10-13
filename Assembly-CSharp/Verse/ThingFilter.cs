using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D66 RID: 3430
	public class ThingFilter : IExposable
	{
		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x0600573C RID: 22332 RVA: 0x001C11A3 File Offset: 0x001BF3A3
		public TreeNode_ThingCategory OverrideRootNode
		{
			get
			{
				if (this.overrideRootDef == null)
				{
					return null;
				}
				return this.overrideRootDef.treeNode;
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x0600573D RID: 22333 RVA: 0x001C11BA File Offset: 0x001BF3BA
		public TreeNode_ThingCategory RootNode
		{
			get
			{
				if (this.overrideRootDef == null)
				{
					return ThingCategoryNodeDatabase.RootNode;
				}
				return this.overrideRootDef.treeNode;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x0600573E RID: 22334 RVA: 0x001C11D5 File Offset: 0x001BF3D5
		public bool OnlySpecialFilters
		{
			get
			{
				return this.onlySpecialFilters;
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x0600573F RID: 22335 RVA: 0x001C11E0 File Offset: 0x001BF3E0
		public string Summary
		{
			get
			{
				if (!this.customSummary.NullOrEmpty())
				{
					return this.customSummary;
				}
				if (this.thingDefs != null && this.thingDefs.Count == 1 && this.categories.NullOrEmpty<string>() && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && !this.disallowMedicalDrugs && !this.disallowDoesntProduceMeat && !this.disallowNotEverStorable && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return this.thingDefs[0].label;
				}
				if (this.thingDefs.NullOrEmpty<ThingDef>() && this.categories != null && this.categories.Count == 1 && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && !this.disallowMedicalDrugs && !this.disallowDoesntProduceMeat && !this.disallowNotEverStorable && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.4028235E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return DefDatabase<ThingCategoryDef>.GetNamed(this.categories[0], true).label;
				}
				if (this.allowedDefs.Count == 1)
				{
					return this.allowedDefs.First<ThingDef>().label;
				}
				return "UsableIngredients".Translate();
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06005740 RID: 22336 RVA: 0x001C1484 File Offset: 0x001BF684
		public ThingRequest BestThingRequest
		{
			get
			{
				if (this.allowedDefs.Count == 1)
				{
					return ThingRequest.ForDef(this.allowedDefs.First<ThingDef>());
				}
				bool flag = true;
				bool flag2 = true;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (!thingDef.EverHaulable)
					{
						flag = false;
					}
					if (thingDef.category != ThingCategory.Pawn)
					{
						flag2 = false;
					}
				}
				if (flag)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
				}
				if (flag2)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.Everything);
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06005741 RID: 22337 RVA: 0x001C1524 File Offset: 0x001BF724
		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault<ThingDef>();
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x001C1531 File Offset: 0x001BF731
		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x001C1539 File Offset: 0x001BF739
		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStorable(true)
				select def;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06005744 RID: 22340 RVA: 0x001C1564 File Offset: 0x001BF764
		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06005745 RID: 22341 RVA: 0x001C1571 File Offset: 0x001BF771
		// (set) Token: 0x06005746 RID: 22342 RVA: 0x001C1579 File Offset: 0x001BF779
		public FloatRange AllowedHitPointsPercents
		{
			get
			{
				return this.allowedHitPointsPercents;
			}
			set
			{
				if (this.allowedHitPointsPercents == value)
				{
					return;
				}
				this.allowedHitPointsPercents = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06005747 RID: 22343 RVA: 0x001C15A4 File Offset: 0x001BF7A4
		// (set) Token: 0x06005748 RID: 22344 RVA: 0x001C15AC File Offset: 0x001BF7AC
		public FloatRange AllowedMentalBreakChance
		{
			get
			{
				return this.allowedMentalBreakChance;
			}
			set
			{
				if (this.allowedMentalBreakChance == value)
				{
					return;
				}
				this.allowedMentalBreakChance = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06005749 RID: 22345 RVA: 0x001C15D7 File Offset: 0x001BF7D7
		// (set) Token: 0x0600574A RID: 22346 RVA: 0x001C15DF File Offset: 0x001BF7DF
		public QualityRange AllowedQualityLevels
		{
			get
			{
				return this.allowedQualities;
			}
			set
			{
				if (this.allowedQualities == value)
				{
					return;
				}
				this.allowedQualities = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x0600574B RID: 22347 RVA: 0x001C160A File Offset: 0x001BF80A
		// (set) Token: 0x0600574C RID: 22348 RVA: 0x001C162E File Offset: 0x001BF82E
		public TreeNode_ThingCategory DisplayRootCategory
		{
			get
			{
				if (this.displayRootCategoryInt == null)
				{
					this.RecalculateDisplayRootCategory();
				}
				if (this.displayRootCategoryInt == null)
				{
					return ThingCategoryNodeDatabase.RootNode;
				}
				return this.displayRootCategoryInt;
			}
			set
			{
				if (value == this.displayRootCategoryInt)
				{
					return;
				}
				this.displayRootCategoryInt = value;
				this.RecalculateSpecialFilterConfigurability();
			}
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x0600574D RID: 22349 RVA: 0x001C1647 File Offset: 0x001BF847
		public bool CaresAboutHitPoints
		{
			get
			{
				return this.allowedHitPointsConfigurable && this.allowedHitPointsPercents != FloatRange.ZeroToOne;
			}
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x001C1664 File Offset: 0x001BF864
		public ThingFilter()
		{
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x001C16D4 File Offset: 0x001BF8D4
		public ThingFilter(ThingCategoryDef overrideRootDef, bool onlySpecialFilters = false)
		{
			this.overrideRootDef = overrideRootDef;
			this.onlySpecialFilters = onlySpecialFilters;
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x001C1750 File Offset: 0x001BF950
		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x001C17C8 File Offset: 0x001BF9C8
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<FloatRange>(ref this.allowedMentalBreakChance, "allowedMentalBreakChance", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
			Scribe_Values.Look<bool>(ref this.onlySpecialFilters, "onlySpecialFilters", false, false);
			Scribe_Defs.Look<ThingCategoryDef>(ref this.overrideRootDef, "overrideRootDef");
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x001C186C File Offset: 0x001BFA6C
		public void ResolveReferences()
		{
			for (int i = 0; i < DefDatabase<SpecialThingFilterDef>.AllDefsListForReading.Count; i++)
			{
				SpecialThingFilterDef specialThingFilterDef = DefDatabase<SpecialThingFilterDef>.AllDefsListForReading[i];
				if (!specialThingFilterDef.allowedByDefault)
				{
					this.SetAllow(specialThingFilterDef, false);
				}
			}
			if (this.thingDefs != null)
			{
				for (int j = 0; j < this.thingDefs.Count; j++)
				{
					if (this.thingDefs[j] != null)
					{
						this.SetAllow(this.thingDefs[j], true);
					}
					else
					{
						string str = "ThingFilter could not find thing def named ";
						ThingDef thingDef = this.thingDefs[j];
						Log.Error(str + ((thingDef != null) ? thingDef.ToString() : null));
					}
				}
			}
			if (this.categories != null)
			{
				for (int k = 0; k < this.categories.Count; k++)
				{
					ThingCategoryDef named = DefDatabase<ThingCategoryDef>.GetNamed(this.categories[k], false);
					if (named != null)
					{
						this.SetAllow(named, true, null, null);
					}
				}
			}
			if (this.tradeTagsToAllow != null)
			{
				for (int l = 0; l < this.tradeTagsToAllow.Count; l++)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int m = 0; m < allDefsListForReading.Count; m++)
					{
						ThingDef thingDef2 = allDefsListForReading[m];
						if (thingDef2.tradeTags != null && thingDef2.tradeTags.Contains(this.tradeTagsToAllow[l]))
						{
							this.SetAllow(thingDef2, true);
						}
					}
				}
			}
			if (this.tradeTagsToDisallow != null)
			{
				for (int n = 0; n < this.tradeTagsToDisallow.Count; n++)
				{
					List<ThingDef> allDefsListForReading2 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num = 0; num < allDefsListForReading2.Count; num++)
					{
						ThingDef thingDef3 = allDefsListForReading2[num];
						if (thingDef3.tradeTags != null && thingDef3.tradeTags.Contains(this.tradeTagsToDisallow[n]))
						{
							this.SetAllow(thingDef3, false);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToAllow != null)
			{
				for (int num2 = 0; num2 < this.thingSetMakerTagsToAllow.Count; num2++)
				{
					List<ThingDef> allDefsListForReading3 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num3 = 0; num3 < allDefsListForReading3.Count; num3++)
					{
						ThingDef thingDef4 = allDefsListForReading3[num3];
						if (thingDef4.thingSetMakerTags != null && thingDef4.thingSetMakerTags.Contains(this.thingSetMakerTagsToAllow[num2]))
						{
							this.SetAllow(thingDef4, true);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToDisallow != null)
			{
				for (int num4 = 0; num4 < this.thingSetMakerTagsToDisallow.Count; num4++)
				{
					List<ThingDef> allDefsListForReading4 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num5 = 0; num5 < allDefsListForReading4.Count; num5++)
					{
						ThingDef thingDef5 = allDefsListForReading4[num5];
						if (thingDef5.thingSetMakerTags != null && thingDef5.thingSetMakerTags.Contains(this.thingSetMakerTagsToDisallow[num4]))
						{
							this.SetAllow(thingDef5, false);
						}
					}
				}
			}
			if (this.disallowedCategories != null)
			{
				for (int num6 = 0; num6 < this.disallowedCategories.Count; num6++)
				{
					ThingCategoryDef named2 = DefDatabase<ThingCategoryDef>.GetNamed(this.disallowedCategories[num6], false);
					if (named2 != null)
					{
						this.SetAllow(named2, false, null, null);
					}
				}
			}
			if (this.specialFiltersToAllow != null)
			{
				for (int num7 = 0; num7 < this.specialFiltersToAllow.Count; num7++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToAllow[num7]), true);
				}
			}
			if (this.specialFiltersToDisallow != null)
			{
				for (int num8 = 0; num8 < this.specialFiltersToDisallow.Count; num8++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToDisallow[num8]), false);
				}
			}
			if (this.stuffCategoriesToAllow != null)
			{
				for (int num9 = 0; num9 < this.stuffCategoriesToAllow.Count; num9++)
				{
					this.SetAllow(this.stuffCategoriesToAllow[num9], true);
				}
			}
			if (this.allowAllWhoCanMake != null)
			{
				for (int num10 = 0; num10 < this.allowAllWhoCanMake.Count; num10++)
				{
					this.SetAllowAllWhoCanMake(this.allowAllWhoCanMake[num10]);
				}
			}
			if (this.disallowWorsePreferability != FoodPreferability.Undefined)
			{
				List<ThingDef> allDefsListForReading5 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num11 = 0; num11 < allDefsListForReading5.Count; num11++)
				{
					ThingDef thingDef6 = allDefsListForReading5[num11];
					if (thingDef6.IsIngestible && thingDef6.ingestible.preferability != FoodPreferability.Undefined && thingDef6.ingestible.preferability < this.disallowWorsePreferability)
					{
						this.SetAllow(thingDef6, false);
					}
				}
			}
			if (this.disallowInedibleByHuman)
			{
				List<ThingDef> allDefsListForReading6 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num12 = 0; num12 < allDefsListForReading6.Count; num12++)
				{
					ThingDef thingDef7 = allDefsListForReading6[num12];
					if (thingDef7.IsIngestible && !ThingDefOf.Human.race.CanEverEat(thingDef7))
					{
						this.SetAllow(thingDef7, false);
					}
				}
			}
			if (this.disallowMedicalDrugs)
			{
				List<ThingDef> allDefsListForReading7 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num13 = 0; num13 < allDefsListForReading7.Count; num13++)
				{
					ThingDef thingDef8 = allDefsListForReading7[num13];
					if (!thingDef8.IsIngestible || (thingDef8.IsDrug && thingDef8.ingestible.drugCategory == DrugCategory.Medical))
					{
						this.SetAllow(thingDef8, false);
					}
				}
			}
			if (this.disallowDoesntProduceMeat)
			{
				List<ThingDef> allDefsListForReading8 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num14 = 0; num14 < allDefsListForReading8.Count; num14++)
				{
					ThingDef thingDef9 = allDefsListForReading8[num14];
					if (thingDef9.race != null && !thingDef9.race.hasMeat)
					{
						this.SetAllow(thingDef9, false);
					}
				}
			}
			if (this.disallowNotEverStorable)
			{
				foreach (ThingDef thingDef10 in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (!thingDef10.EverStorable(true))
					{
						this.SetAllow(thingDef10, false);
					}
				}
			}
			if (this.allowWithComp != null)
			{
				List<ThingDef> allDefsListForReading9 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num15 = 0; num15 < allDefsListForReading9.Count; num15++)
				{
					ThingDef thingDef11 = allDefsListForReading9[num15];
					if (thingDef11.HasComp(this.allowWithComp))
					{
						this.SetAllow(thingDef11, true);
					}
				}
			}
			if (this.disallowWithComp != null)
			{
				List<ThingDef> allDefsListForReading10 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num16 = 0; num16 < allDefsListForReading10.Count; num16++)
				{
					ThingDef thingDef12 = allDefsListForReading10[num16];
					if (thingDef12.HasComp(this.disallowWithComp))
					{
						this.SetAllow(thingDef12, false);
					}
				}
			}
			if (this.disallowCheaperThan != -3.4028235E+38f)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef13 in this.allowedDefs)
				{
					if (thingDef13.BaseMarketValue < this.disallowCheaperThan)
					{
						list.Add(thingDef13);
					}
				}
				for (int num17 = 0; num17 < list.Count; num17++)
				{
					this.SetAllow(list[num17], false);
				}
			}
			if (this.disallowedThingDefs != null)
			{
				for (int num18 = 0; num18 < this.disallowedThingDefs.Count; num18++)
				{
					if (this.disallowedThingDefs[num18] != null)
					{
						this.SetAllow(this.disallowedThingDefs[num18], false);
					}
					else
					{
						string str2 = "ThingFilter could not find excepted thing def named ";
						ThingDef thingDef14 = this.disallowedThingDefs[num18];
						Log.Error(str2 + ((thingDef14 != null) ? thingDef14.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x001C1FB0 File Offset: 0x001C01B0
		public void RecalculateDisplayRootCategory()
		{
			if (ThingCategoryNodeDatabase.allThingCategoryNodes == null)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			int lastFoundCategory = -1;
			object lockObject = new object();
			GenThreading.ParallelFor(0, ThingCategoryNodeDatabase.allThingCategoryNodes.Count, delegate(int index)
			{
				TreeNode_ThingCategory treeNode_ThingCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[index];
				bool flag = false;
				bool flag2 = false;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (treeNode_ThingCategory.catDef.ContainedInThisOrDescendant(thingDef))
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
				}
				if (!flag && flag2)
				{
					object lockObject = lockObject;
					lock (lockObject)
					{
						if (index > lastFoundCategory)
						{
							lastFoundCategory = index;
						}
					}
				}
			}, -1);
			if (lastFoundCategory == -1)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			this.DisplayRootCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[lastFoundCategory];
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x001C2038 File Offset: 0x001C0238
		private void RecalculateSpecialFilterConfigurability()
		{
			if (this.DisplayRootCategory == null)
			{
				this.allowedHitPointsConfigurable = true;
				this.allowedQualitiesConfigurable = true;
				return;
			}
			this.allowedHitPointsConfigurable = false;
			this.allowedQualitiesConfigurable = false;
			foreach (ThingDef thingDef in this.DisplayRootCategory.catDef.DescendantThingDefs)
			{
				if (thingDef.useHitPoints)
				{
					this.allowedHitPointsConfigurable = true;
				}
				if (thingDef.HasComp(typeof(CompQuality)))
				{
					this.allowedQualitiesConfigurable = true;
				}
				if (this.allowedHitPointsConfigurable && this.allowedQualitiesConfigurable)
				{
					break;
				}
			}
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x001C20E8 File Offset: 0x001C02E8
		public bool IsAlwaysDisallowedDueToSpecialFilters(ThingDef def)
		{
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.AlwaysMatches(def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x001C2128 File Offset: 0x001C0328
		public virtual void CopyAllowancesFrom(ThingFilter other)
		{
			this.allowedDefs.Clear();
			this.allowedDefs.AddRange(other.allowedDefs);
			this.disallowedSpecialFilters = other.disallowedSpecialFilters.ListFullCopyOrNull<SpecialThingFilterDef>();
			this.allowedHitPointsPercents = other.allowedHitPointsPercents;
			this.allowedMentalBreakChance = other.allowedMentalBreakChance;
			this.allowedHitPointsConfigurable = other.allowedHitPointsConfigurable;
			this.allowedQualities = other.allowedQualities;
			this.allowedQualitiesConfigurable = other.allowedQualitiesConfigurable;
			this.thingDefs = other.thingDefs.ListFullCopyOrNull<ThingDef>();
			this.categories = other.categories.ListFullCopyOrNull<string>();
			this.tradeTagsToAllow = other.tradeTagsToAllow.ListFullCopyOrNull<string>();
			this.tradeTagsToDisallow = other.tradeTagsToDisallow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToAllow = other.thingSetMakerTagsToAllow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToDisallow = other.thingSetMakerTagsToDisallow.ListFullCopyOrNull<string>();
			this.disallowedCategories = other.disallowedCategories.ListFullCopyOrNull<string>();
			this.specialFiltersToAllow = other.specialFiltersToAllow.ListFullCopyOrNull<string>();
			this.specialFiltersToDisallow = other.specialFiltersToDisallow.ListFullCopyOrNull<string>();
			this.stuffCategoriesToAllow = other.stuffCategoriesToAllow.ListFullCopyOrNull<StuffCategoryDef>();
			this.allowAllWhoCanMake = other.allowAllWhoCanMake.ListFullCopyOrNull<ThingDef>();
			this.disallowWorsePreferability = other.disallowWorsePreferability;
			this.disallowInedibleByHuman = other.disallowInedibleByHuman;
			this.disallowMedicalDrugs = other.disallowMedicalDrugs;
			this.disallowNotEverStorable = other.disallowNotEverStorable;
			this.allowWithComp = other.allowWithComp;
			this.disallowWithComp = other.disallowWithComp;
			this.disallowCheaperThan = other.disallowCheaperThan;
			this.disallowedThingDefs = other.disallowedThingDefs.ListFullCopyOrNull<ThingDef>();
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x001C22D4 File Offset: 0x001C04D4
		public void SetAllow(ThingDef thingDef, bool allow)
		{
			if (allow == this.Allows(thingDef))
			{
				return;
			}
			if (allow)
			{
				this.allowedDefs.Add(thingDef);
			}
			else
			{
				this.allowedDefs.Remove(thingDef);
			}
			foreach (ThingDef thingDef2 in thingDef.virtualDefs)
			{
				this.SetAllow(thingDef2, allow);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x001C236C File Offset: 0x001C056C
		public void SetAllow(SpecialThingFilterDef sfDef, bool allow)
		{
			if (allow == this.Allows(sfDef))
			{
				return;
			}
			if (allow)
			{
				if (this.disallowedSpecialFilters.Contains(sfDef))
				{
					this.disallowedSpecialFilters.Remove(sfDef);
				}
			}
			else if (!this.disallowedSpecialFilters.Contains(sfDef))
			{
				this.disallowedSpecialFilters.Add(sfDef);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001C23D8 File Offset: 0x001C05D8
		public void SetAllow(ThingCategoryDef categoryDef, bool allow, IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			if (!ThingCategoryNodeDatabase.initialized)
			{
				Log.Error("SetAllow categories won't work before ThingCategoryDatabase is initialized.");
			}
			foreach (ThingDef thingDef in categoryDef.DescendantThingDefs)
			{
				if (exceptedDefs == null || !exceptedDefs.Contains(thingDef))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in categoryDef.DescendantSpecialThingFilterDefs)
			{
				if (specialThingFilterDef.configurable && (exceptedFilters == null || !exceptedFilters.Contains(specialThingFilterDef)))
				{
					this.SetAllow(specialThingFilterDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x001C24B0 File Offset: 0x001C06B0
		public void SetAllow(StuffCategoryDef cat, bool allow)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.categories.Contains(cat))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x001C251C File Offset: 0x001C071C
		public void SetAllowAllWhoCanMake(ThingDef thing)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.CanMake(thing))
				{
					this.SetAllow(thingDef, true);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x001C2584 File Offset: 0x001C0784
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Manufactured, true, null, null);
				this.SetAllow(ThingCategoryDefOf.ResourcesRaw, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Items, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Buildings, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Weapons, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
				this.SetAllow(ThingCategoryDefOf.BodyParts, true, null, null);
				if (ModsConfig.BiotechActive)
				{
					this.SetAllow(ThingDefOf.Wastepack, false);
				}
			}
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Corpses, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Chunks, true, null, null);
				if (ModsConfig.BiotechActive)
				{
					this.SetAllow(ThingDefOf.Wastepack, true);
				}
			}
			if (preset == StorageSettingsPreset.CorpseStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Corpses, true, null, null);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x001C267C File Offset: 0x001C087C
		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d));
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf)));
			if (this.onlySpecialFilters)
			{
				this.DisableSpecialFilters(this.RootNode);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x001C26FC File Offset: 0x001C08FC
		private void DisableSpecialFilters(TreeNode_ThingCategory category)
		{
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in category.ChildCategoryNodesAndThis)
			{
				foreach (SpecialThingFilterDef item in treeNode_ThingCategory.catDef.childSpecialFilters)
				{
					this.disallowedSpecialFilters.Add(item);
				}
			}
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x001C278C File Offset: 0x001C098C
		public void SetAllowAll(ThingFilter parentFilter, bool includeNonStorable = false)
		{
			this.allowedDefs.Clear();
			if (parentFilter != null)
			{
				using (HashSet<ThingDef>.Enumerator enumerator = parentFilter.allowedDefs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef item = enumerator.Current;
						this.allowedDefs.Add(item);
					}
					goto IL_B9;
				}
			}
			if (includeNonStorable)
			{
				using (IEnumerator<ThingDef> enumerator2 = DefDatabase<ThingDef>.AllDefs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDef item2 = enumerator2.Current;
						this.allowedDefs.Add(item2);
					}
					goto IL_B9;
				}
			}
			foreach (ThingDef item3 in ThingFilter.AllStorableThingDefs)
			{
				this.allowedDefs.Add(item3);
			}
			IL_B9:
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable);
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x001C28C0 File Offset: 0x001C0AC0
		public virtual bool Allows(Thing t)
		{
			t = t.GetInnerIfMinified();
			if (!this.Allows(t.def) && !this.onlySpecialFilters)
			{
				return false;
			}
			if (t.def.useHitPoints && this.CaresAboutHitPoints)
			{
				float num = (float)t.HitPoints / (float)t.MaxHitPoints;
				num = GenMath.RoundedHundredth(num);
				if (!this.allowedHitPointsPercents.IncludesEpsilon(Mathf.Clamp01(num)))
				{
					return false;
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Book book = t as Book;
				if (book != null && !this.allowedMentalBreakChance.IncludesEpsilon(Mathf.Clamp01(book.MentalBreakChancePerHour)))
				{
					return false;
				}
			}
			QualityCategory p;
			if (this.allowedQualities != QualityRange.All && t.def.FollowQualityThingFilter() && t.TryGetQuality(out p) && !this.allowedQualities.Includes(p))
			{
				return false;
			}
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.Matches(t) && (t.def.IsWithinCategory(this.disallowedSpecialFilters[i].parentCategory) || this.onlySpecialFilters))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x001C29EA File Offset: 0x001C0BEA
		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x001C29F8 File Offset: 0x001C0BF8
		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x001C2A09 File Offset: 0x001C0C09
		public ThingRequest GetThingRequest()
		{
			if (this.AllowedThingDefs.Any((ThingDef def) => !def.alwaysHaulable))
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
			return ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways);
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x001C2A44 File Offset: 0x001C0C44
		public override string ToString()
		{
			return this.Summary;
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x001C2A4C File Offset: 0x001C0C4C
		public static ThingFilter CreateOnlyEverStorableThingFilter()
		{
			ThingFilter thingFilter = new ThingFilter();
			thingFilter.categories = new List<string>();
			thingFilter.categories.Add(ThingCategoryDefOf.Root.defName);
			thingFilter.disallowNotEverStorable = true;
			thingFilter.ResolveReferences();
			thingFilter.hiddenSpecialFilters = new List<SpecialThingFilterDef>
			{
				SpecialThingFilterDefOf.AllowLargeCorpses
			};
			return thingFilter;
		}

		// Token: 0x04003F91 RID: 16273
		[Unsaved(false)]
		private Action settingsChangedCallback;

		// Token: 0x04003F92 RID: 16274
		[Unsaved(false)]
		private TreeNode_ThingCategory displayRootCategoryInt;

		// Token: 0x04003F93 RID: 16275
		[Unsaved(false)]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		// Token: 0x04003F94 RID: 16276
		[Unsaved(false)]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04003F95 RID: 16277
		[Unsaved(false)]
		public List<SpecialThingFilterDef> hiddenSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x04003F96 RID: 16278
		private ThingCategoryDef overrideRootDef;

		// Token: 0x04003F97 RID: 16279
		private bool onlySpecialFilters;

		// Token: 0x04003F98 RID: 16280
		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		// Token: 0x04003F99 RID: 16281
		private FloatRange allowedMentalBreakChance = FloatRange.ZeroToOne;

		// Token: 0x04003F9A RID: 16282
		public bool allowedHitPointsConfigurable = true;

		// Token: 0x04003F9B RID: 16283
		private QualityRange allowedQualities = QualityRange.All;

		// Token: 0x04003F9C RID: 16284
		public bool allowedQualitiesConfigurable = true;

		// Token: 0x04003F9D RID: 16285
		[MustTranslate]
		public string customSummary;

		// Token: 0x04003F9E RID: 16286
		private List<ThingDef> thingDefs;

		// Token: 0x04003F9F RID: 16287
		[NoTranslate]
		private List<string> categories;

		// Token: 0x04003FA0 RID: 16288
		[NoTranslate]
		private List<string> tradeTagsToAllow;

		// Token: 0x04003FA1 RID: 16289
		[NoTranslate]
		private List<string> tradeTagsToDisallow;

		// Token: 0x04003FA2 RID: 16290
		[NoTranslate]
		private List<string> thingSetMakerTagsToAllow;

		// Token: 0x04003FA3 RID: 16291
		[NoTranslate]
		private List<string> thingSetMakerTagsToDisallow;

		// Token: 0x04003FA4 RID: 16292
		[NoTranslate]
		private List<string> disallowedCategories;

		// Token: 0x04003FA5 RID: 16293
		[NoTranslate]
		private List<string> specialFiltersToAllow;

		// Token: 0x04003FA6 RID: 16294
		[NoTranslate]
		private List<string> specialFiltersToDisallow;

		// Token: 0x04003FA7 RID: 16295
		private List<StuffCategoryDef> stuffCategoriesToAllow;

		// Token: 0x04003FA8 RID: 16296
		private List<ThingDef> allowAllWhoCanMake;

		// Token: 0x04003FA9 RID: 16297
		private FoodPreferability disallowWorsePreferability;

		// Token: 0x04003FAA RID: 16298
		private bool disallowInedibleByHuman;

		// Token: 0x04003FAB RID: 16299
		private bool disallowDoesntProduceMeat;

		// Token: 0x04003FAC RID: 16300
		private bool disallowMedicalDrugs;

		// Token: 0x04003FAD RID: 16301
		private bool disallowNotEverStorable;

		// Token: 0x04003FAE RID: 16302
		private Type allowWithComp;

		// Token: 0x04003FAF RID: 16303
		private Type disallowWithComp;

		// Token: 0x04003FB0 RID: 16304
		private float disallowCheaperThan = float.MinValue;

		// Token: 0x04003FB1 RID: 16305
		private List<ThingDef> disallowedThingDefs;
	}
}
