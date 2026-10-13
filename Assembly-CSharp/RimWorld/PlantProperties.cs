using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019BB RID: 6587
	public class PlantProperties
	{
		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x060094DD RID: 38109 RVA: 0x002BA93A File Offset: 0x002B8B3A
		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x060094DE RID: 38110 RVA: 0x002BA94A File Offset: 0x002B8B4A
		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x060094DF RID: 38111 RVA: 0x002BA959 File Offset: 0x002B8B59
		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x060094E0 RID: 38112 RVA: 0x002BA96B File Offset: 0x002B8B6B
		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood" || this.forceIsTree;
			}
		}

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x060094E1 RID: 38113 RVA: 0x002BA987 File Offset: 0x002B8B87
		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanDaysPerGrowDays;
			}
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x060094E2 RID: 38114 RVA: 0x002BA996 File Offset: 0x002B8B96
		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x060094E3 RID: 38115 RVA: 0x002BA9A5 File Offset: 0x002B8BA5
		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanDaysPerGrowDays > 0f;
			}
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x060094E4 RID: 38116 RVA: 0x002BA9B4 File Offset: 0x002B8BB4
		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x060094E5 RID: 38117 RVA: 0x002BA9D1 File Offset: 0x002B8BD1
		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x060094E6 RID: 38118 RVA: 0x002BA9DC File Offset: 0x002B8BDC
		public bool RequiresPollution
		{
			get
			{
				return this.pollution == Pollution.PollutedOnly;
			}
		}

		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x060094E7 RID: 38119 RVA: 0x002BA9E7 File Offset: 0x002B8BE7
		public bool RequiresNoPollution
		{
			get
			{
				return this.pollution == Pollution.CleanOnly;
			}
		}

		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x060094E8 RID: 38120 RVA: 0x002BA9F4 File Offset: 0x002B8BF4
		public HashSet<string> WildTerrainTags
		{
			get
			{
				HashSet<string> result;
				if ((result = this.wildTerrainTagsSet) == null)
				{
					IEnumerable<string> enumerable = this.wildTerrainTags;
					result = (this.wildTerrainTagsSet = new HashSet<string>(enumerable ?? Enumerable.Empty<string>()));
				}
				return result;
			}
		}

		// Token: 0x060094E9 RID: 38121 RVA: 0x002BAA2C File Offset: 0x002B8C2C
		public void PostLoadSpecial(ThingDef parentDef)
		{
			if (!this.leaflessGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.immatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (ModsConfig.BiotechActive && !this.pollutedGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.pollutedGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.pollutedGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
			if (!this.leaflessImmatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessImmatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessImmatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo, null);
				});
			}
		}

		// Token: 0x060094EA RID: 38122 RVA: 0x002BAACC File Offset: 0x002B8CCC
		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount > 25)
			{
				yield return "maxMeshCount > MaxMaxMeshCount";
			}
			yield break;
		}

		// Token: 0x060094EB RID: 38123 RVA: 0x002BAADC File Offset: 0x002B8CDC
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHarvestYieldHyperlinks()
		{
			yield return new Dialog_InfoCard.Hyperlink(this.harvestedThingDef, -1);
			yield break;
		}

		// Token: 0x060094EC RID: 38124 RVA: 0x002BAAEC File Offset: 0x002B8CEC
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.sowMinSkill > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowingSkillToSow".Translate(), this.sowMinSkill.ToString(), "Stat_Thing_Plant_MinGrowingSkillToSow_Desc".Translate(), 4151, null, null, false, false);
			}
			string attributes = "";
			if (this.Harvestable)
			{
				string text = "Harvestable".Translate();
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
					text = text.UncapitalizeFirst();
				}
				attributes += text;
			}
			if (this.LimitedLifespan)
			{
				string text2 = "LimitedLifespan".Translate();
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
					text2 = text2.UncapitalizeFirst();
				}
				attributes += text2;
			}
			if (!this.isStump)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "GrowingTime".Translate(), this.growDays.ToString("0.##") + " " + "Days".Translate(), "GrowingTimeDesc".Translate(), 4158, null, null, false, false);
				if (!this.completelyIgnoreFertility)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilityRequirement".Translate(), this.fertilityMin.ToStringPercent(), "Stat_Thing_Plant_FertilityRequirement_Desc".Translate(), 4156, null, null, false, false);
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilitySensitivity".Translate(), this.fertilitySensitivity.ToStringPercent(), "Stat_Thing_Plant_FertilitySensitivity_Desc".Translate(), 4155, null, null, false, false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LightRequirement".Translate(), this.growMinGlow.ToStringPercent(), "Stat_Thing_Plant_LightRequirement_Desc".Translate(), 4154, null, null, false, false);
			}
			if (!attributes.NullOrEmpty())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Attributes".Translate(), attributes, "Stat_Thing_Plant_Attributes_Desc".Translate(), 4157, null, null, false, false);
			}
			if (this.LimitedLifespan)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LifeSpan".Translate(), this.LifespanDays.ToString("0.##") + " " + "Days".Translate(), "Stat_Thing_Plant_LifeSpan_Desc".Translate(), 4150, null, null, false, false);
			}
			if (this.harvestYield > 0f)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Stat_Thing_Plant_HarvestYield_Desc".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.cropYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HarvestYield".Translate(), Mathf.CeilToInt(this.harvestYield * Find.Storyteller.difficulty.cropYieldFactor).ToString("F0"), stringBuilder.ToString(), 4150, null, this.GetHarvestYieldHyperlinks(), false, false);
			}
			if (!this.isStump)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowthTemperature".Translate(), this.minGrowthTemperature.ToStringTemperature("F1"), "Stat_Thing_Plant_MinGrowthTemperature_Desc".Translate(), 4152, null, null, false, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MaxGrowthTemperature".Translate(), this.maxGrowthTemperature.ToStringTemperature("F1"), "Stat_Thing_Plant_MaxGrowthTemperature_Desc".Translate(), 4153, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x040060B5 RID: 24757
		public List<PlantBiomeRecord> wildBiomes;

		// Token: 0x040060B6 RID: 24758
		public int wildClusterRadius = -1;

		// Token: 0x040060B7 RID: 24759
		public float wildClusterWeight = 15f;

		// Token: 0x040060B8 RID: 24760
		public float wildOrder = 2f;

		// Token: 0x040060B9 RID: 24761
		public bool wildEqualLocalDistribution = true;

		// Token: 0x040060BA RID: 24762
		public bool cavePlant;

		// Token: 0x040060BB RID: 24763
		public float cavePlantWeight = 1f;

		// Token: 0x040060BC RID: 24764
		public List<string> wildTerrainTags;

		// Token: 0x040060BD RID: 24765
		public bool wildPlantUseDistanceToShore;

		// Token: 0x040060BE RID: 24766
		public float plantRespawningCommonalityFactor = 1f;

		// Token: 0x040060BF RID: 24767
		[NoTranslate]
		public List<string> sowTags = new List<string>();

		// Token: 0x040060C0 RID: 24768
		public float sowWork = 10f;

		// Token: 0x040060C1 RID: 24769
		public int sowMinSkill;

		// Token: 0x040060C2 RID: 24770
		public bool blockAdjacentSow;

		// Token: 0x040060C3 RID: 24771
		public List<ResearchProjectDef> sowResearchPrerequisites;

		// Token: 0x040060C4 RID: 24772
		public bool mustBeWildToSow;

		// Token: 0x040060C5 RID: 24773
		public float harvestWork = 10f;

		// Token: 0x040060C6 RID: 24774
		public float harvestYield;

		// Token: 0x040060C7 RID: 24775
		public ThingDef harvestedThingDef;

		// Token: 0x040060C8 RID: 24776
		[NoTranslate]
		public string harvestTag;

		// Token: 0x040060C9 RID: 24777
		public float harvestMinGrowth = 0.65f;

		// Token: 0x040060CA RID: 24778
		public float harvestAfterGrowth;

		// Token: 0x040060CB RID: 24779
		public bool harvestFailable = true;

		// Token: 0x040060CC RID: 24780
		public bool harvestYieldAffectedByDifficulty = true;

		// Token: 0x040060CD RID: 24781
		public SoundDef soundHarvesting;

		// Token: 0x040060CE RID: 24782
		public SoundDef soundHarvestFinish;

		// Token: 0x040060CF RID: 24783
		public bool autoHarvestable = true;

		// Token: 0x040060D0 RID: 24784
		public bool skipDeteriorationMessage;

		// Token: 0x040060D1 RID: 24785
		public float growDays = 2f;

		// Token: 0x040060D2 RID: 24786
		public float lifespanDaysPerGrowDays = 8f;

		// Token: 0x040060D3 RID: 24787
		public float growMinGlow = 0.51f;

		// Token: 0x040060D4 RID: 24788
		public float growOptimalGlow = 1f;

		// Token: 0x040060D5 RID: 24789
		public bool diesToLight;

		// Token: 0x040060D6 RID: 24790
		public bool vacuumResistant;

		// Token: 0x040060D7 RID: 24791
		public bool terraformable;

		// Token: 0x040060D8 RID: 24792
		public Pollution pollution;

		// Token: 0x040060D9 RID: 24793
		public float minGrowthTemperature;

		// Token: 0x040060DA RID: 24794
		public float minOptimalGrowthTemperature = 6f;

		// Token: 0x040060DB RID: 24795
		public float maxOptimalGrowthTemperature = 42f;

		// Token: 0x040060DC RID: 24796
		public float maxGrowthTemperature = 58f;

		// Token: 0x040060DD RID: 24797
		public float fertilityMin = 0.9f;

		// Token: 0x040060DE RID: 24798
		public float fertilitySensitivity = 0.5f;

		// Token: 0x040060DF RID: 24799
		public bool completelyIgnoreFertility;

		// Token: 0x040060E0 RID: 24800
		public List<TerrainDef> terrainBlacklist;

		// Token: 0x040060E1 RID: 24801
		public bool dieIfLeafless;

		// Token: 0x040060E2 RID: 24802
		public bool neverBlightable;

		// Token: 0x040060E3 RID: 24803
		public bool interferesWithRoof;

		// Token: 0x040060E4 RID: 24804
		public bool dieIfNoSunlight = true;

		// Token: 0x040060E5 RID: 24805
		public bool dieFromToxicFallout = true;

		// Token: 0x040060E6 RID: 24806
		public PlantPurpose purpose = PlantPurpose.Misc;

		// Token: 0x040060E7 RID: 24807
		public bool humanFoodPlant;

		// Token: 0x040060E8 RID: 24808
		public bool treeLoversCareIfChopped = true;

		// Token: 0x040060E9 RID: 24809
		public bool allowAutoCut = true;

		// Token: 0x040060EA RID: 24810
		public bool drugForHarvestPurposes;

		// Token: 0x040060EB RID: 24811
		public TreeCategory treeCategory;

		// Token: 0x040060EC RID: 24812
		public ThingDef burnedThingDef;

		// Token: 0x040060ED RID: 24813
		public ThingDef choppedThingDef;

		// Token: 0x040060EE RID: 24814
		public ThingDef smashedThingDef;

		// Token: 0x040060EF RID: 24815
		public bool canDeteriorate;

		// Token: 0x040060F0 RID: 24816
		public bool showGrowthInInspectPane = true;

		// Token: 0x040060F1 RID: 24817
		public float minSpacingBetweenSamePlant;

		// Token: 0x040060F2 RID: 24818
		public bool forceIsTree;

		// Token: 0x040060F3 RID: 24819
		public bool warnIfMarkedForCut;

		// Token: 0x040060F4 RID: 24820
		public bool isStump;

		// Token: 0x040060F5 RID: 24821
		public float topWindExposure = 0.25f;

		// Token: 0x040060F6 RID: 24822
		public int maxMeshCount = 1;

		// Token: 0x040060F7 RID: 24823
		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x040060F8 RID: 24824
		public bool showInFrozenWater = true;

		// Token: 0x040060F9 RID: 24825
		public bool destroyedByFlooding = true;

		// Token: 0x040060FA RID: 24826
		[NoTranslate]
		private string leaflessGraphicPath;

		// Token: 0x040060FB RID: 24827
		[Unsaved(false)]
		public Graphic leaflessGraphic;

		// Token: 0x040060FC RID: 24828
		[NoTranslate]
		private string immatureGraphicPath;

		// Token: 0x040060FD RID: 24829
		[Unsaved(false)]
		public Graphic immatureGraphic;

		// Token: 0x040060FE RID: 24830
		public bool dropLeaves;

		// Token: 0x040060FF RID: 24831
		[NoTranslate]
		private string pollutedGraphicPath;

		// Token: 0x04006100 RID: 24832
		[Unsaved(false)]
		public Graphic pollutedGraphic;

		// Token: 0x04006101 RID: 24833
		[NoTranslate]
		private string leaflessImmatureGraphicPath;

		// Token: 0x04006102 RID: 24834
		[Unsaved(false)]
		public Graphic leaflessImmatureGraphic;

		// Token: 0x04006103 RID: 24835
		public const int MaxMaxMeshCount = 25;

		// Token: 0x04006104 RID: 24836
		private HashSet<string> wildTerrainTagsSet;
	}
}
