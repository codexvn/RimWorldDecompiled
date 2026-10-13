using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AC4 RID: 2756
	public static class DebugOutputsMisc
	{
		// Token: 0x0600462F RID: 17967 RVA: 0x00168348 File Offset: 0x00166548
		[DebugOutput]
		public static void DrawerTypes()
		{
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[2] = new TableDataGetter<ThingDef>("drawType", (ThingDef d) => d.drawerType.ToString());
			array[3] = new TableDataGetter<ThingDef>("thingClass", (ThingDef d) => d.thingClass.Name);
			array[4] = new TableDataGetter<ThingDef>("draw() users (comps)", (ThingDef d) => DebugOutputsMisc.<DrawerTypes>g__GetDrawUsers|0_0(d));
			array[5] = new TableDataGetter<ThingDef>("potential issue", delegate(ThingDef d)
			{
				if (d.drawerType != DrawerType.MapMeshOnly || string.IsNullOrWhiteSpace(DebugOutputsMisc.<DrawerTypes>g__GetDrawUsers|0_0(d)))
				{
					return "No";
				}
				return "Yes";
			});
			DebugTables.MakeTablesDialog<ThingDef>(allDefs, array);
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x00168470 File Offset: 0x00166670
		[DebugOutput]
		public static void DynamicDrawnThingsNow()
		{
			IEnumerable<IGrouping<ThingDef, Thing>> dataSources = (from t in Find.CurrentMap.dynamicDrawManager.DrawThings
			group t by t.def).ToList<IGrouping<ThingDef, Thing>>();
			TableDataGetter<IGrouping<ThingDef, Thing>>[] array = new TableDataGetter<IGrouping<ThingDef, Thing>>[2];
			array[0] = new TableDataGetter<IGrouping<ThingDef, Thing>>("defName", (IGrouping<ThingDef, Thing> d) => d.Key.defName);
			array[1] = new TableDataGetter<IGrouping<ThingDef, Thing>>("count", (IGrouping<ThingDef, Thing> d) => d.Count<Thing>());
			DebugTables.MakeTablesDialog<IGrouping<ThingDef, Thing>>(dataSources, array);
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x00168518 File Offset: 0x00166718
		[DebugOutput]
		public static void DynamicDrawnThingsByCategoryNow()
		{
			IEnumerable<IGrouping<ThingCategory, Thing>> dataSources = (from t in Find.CurrentMap.dynamicDrawManager.DrawThings
			group t by t.def.category).ToList<IGrouping<ThingCategory, Thing>>();
			TableDataGetter<IGrouping<ThingCategory, Thing>>[] array = new TableDataGetter<IGrouping<ThingCategory, Thing>>[2];
			array[0] = new TableDataGetter<IGrouping<ThingCategory, Thing>>("category", (IGrouping<ThingCategory, Thing> d) => d.Key.ToString());
			array[1] = new TableDataGetter<IGrouping<ThingCategory, Thing>>("count", (IGrouping<ThingCategory, Thing> d) => d.Count<Thing>());
			DebugTables.MakeTablesDialog<IGrouping<ThingCategory, Thing>>(dataSources, array);
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x001685C0 File Offset: 0x001667C0
		[DebugOutput]
		public static void MiningResourceGeneration()
		{
			Func<ThingDef, ThingDef> mineable = delegate(ThingDef d)
			{
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.mineableThing == d)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			};
			Func<ThingDef, float> mineableCommonality = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return mineable(d).building.mineableScatterCommonality;
				}
				return 0f;
			};
			Func<ThingDef, IntRange> mineableLumpSizeRange = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return mineable(d).building.mineableScatterLumpSizeRange;
				}
				return IntRange.Zero;
			};
			Func<ThingDef, float> mineableYield = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return (float)mineable(d).building.EffectiveMineableYield;
				}
				return 0f;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.deepCommonality > 0f || mineableCommonality(d) > 0f
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[14];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("stackLimit", (ThingDef d) => d.stackLimit);
			array[3] = new TableDataGetter<ThingDef>("deep\ncommonality", (ThingDef d) => d.deepCommonality.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("deep\nlump size", (ThingDef d) => d.deepLumpSizeRange);
			array[5] = new TableDataGetter<ThingDef>("deep lump\nvalue min", (ThingDef d) => ((float)d.deepLumpSizeRange.min * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[6] = new TableDataGetter<ThingDef>("deep lump\nvalue avg", (ThingDef d) => (d.deepLumpSizeRange.Average * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[7] = new TableDataGetter<ThingDef>("deep lump\nvalue max", (ThingDef d) => ((float)d.deepLumpSizeRange.max * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[8] = new TableDataGetter<ThingDef>("deep count\nper cell", (ThingDef d) => d.deepCountPerCell);
			array[9] = new TableDataGetter<ThingDef>("deep count\nper portion", (ThingDef d) => d.deepCountPerPortion);
			array[10] = new TableDataGetter<ThingDef>("deep portion\nvalue", (ThingDef d) => ((float)d.deepCountPerPortion * d.BaseMarketValue).ToStringMoney(null));
			array[11] = new TableDataGetter<ThingDef>("mineable\ncommonality", (ThingDef d) => mineableCommonality(d).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("mineable\nlump size", (ThingDef d) => mineableLumpSizeRange(d));
			array[13] = new TableDataGetter<ThingDef>("mineable yield\nper cell", (ThingDef d) => mineableYield(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x00168884 File Offset: 0x00166A84
		[DebugOutput]
		public static void NaturalRocks()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && (d.building.isNaturalRock || d.building.isResourceRock) && !d.IsSmoothed
			select d into x
			orderby x.building.isNaturalRock descending, x.building.isResourceRock descending
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[9];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("isNaturalRock", (ThingDef d) => d.building.isNaturalRock.ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("isResourceRock", (ThingDef d) => d.building.isResourceRock.ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("smoothed", delegate(ThingDef d)
			{
				if (d.building.smoothedThing == null)
				{
					return "";
				}
				return d.building.smoothedThing.defName;
			});
			array[4] = new TableDataGetter<ThingDef>("mineableThing", delegate(ThingDef d)
			{
				if (d.building.mineableThing == null)
				{
					return "";
				}
				return d.building.mineableThing.defName;
			});
			array[5] = new TableDataGetter<ThingDef>("mineableYield", (ThingDef d) => d.building.EffectiveMineableYield);
			array[6] = new TableDataGetter<ThingDef>("mineableYieldWasteable", (ThingDef d) => d.building.mineableYieldWasteable);
			array[7] = new TableDataGetter<ThingDef>("NaturalRockType\never possible", (ThingDef d) => d.IsNonResourceNaturalRock.ToStringCheckBlank());
			array[8] = new TableDataGetter<ThingDef>("NaturalRockType\nin CurrentMap", delegate(ThingDef d)
			{
				if (Find.CurrentMap == null)
				{
					return "";
				}
				return Find.World.NaturalRockTypesIn(Find.CurrentMap.Tile).Contains(d).ToStringCheckBlank();
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x00168A9C File Offset: 0x00166C9C
		[DebugOutput]
		public static void AncientJunk()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.defName.StartsWith("Ancient")
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("edifice", (ThingDef d) => d.IsEdifice());
			array[2] = new TableDataGetter<ThingDef>("scatterers", delegate(ThingDef d)
			{
				string text = null;
				foreach (GenStepDef genStepDef in DefDatabase<GenStepDef>.AllDefsListForReading)
				{
					GenStep_ScatterThings genStep_ScatterThings = genStepDef.genStep as GenStep_ScatterThings;
					if (genStep_ScatterThings != null && genStep_ScatterThings.thingDef == d)
					{
						if (text != null)
						{
							text += ", ";
						}
						text = text + "ScatterThings " + genStepDef.defName;
					}
					GenStep_ScatterGroup genStep_ScatterGroup = genStepDef.genStep as GenStep_ScatterGroup;
					if (genStep_ScatterGroup != null)
					{
						string text2 = null;
						for (int i = 0; i < genStep_ScatterGroup.groups.Count; i++)
						{
							GenStep_ScatterGroup.ScatterGroup scatterGroup = genStep_ScatterGroup.groups[i];
							for (int j = 0; j < scatterGroup.things.Count; j++)
							{
								GenStep_ScatterGroup.ThingWeight thingWeight = scatterGroup.things[j];
								if (thingWeight.thing == d)
								{
									if (text2 != null)
									{
										text2 += ", ";
									}
									text2 = text2 + i.ToString() + "|" + thingWeight.weight.ToString();
								}
							}
						}
						if (text2 != null)
						{
							if (text != null)
							{
								text += ", ";
							}
							text = string.Concat(new string[]
							{
								text,
								"ScatterGroup ",
								genStepDef.defName,
								" {",
								text2,
								"}"
							});
						}
					}
				}
				return text;
			});
			array[3] = new TableDataGetter<ThingDef>("rotatable", (ThingDef d) => d.rotatable);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x00168B90 File Offset: 0x00166D90
		[DebugOutput]
		public static void MeditationFoci()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.StatBaseDefined(StatDefOf.MeditationFocusStrength)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[13];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MeditationFocusStrength, null).ToStringPercent());
			array[2] = new TableDataGetter<ThingDef>("max\ntotal", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__TotalMax|6_3(d).ToStringPercent());
			array[3] = new TableDataGetter<ThingDef>("offset 0\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 0));
			array[4] = new TableDataGetter<ThingDef>("offset 0\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 0).ToStringPercentEmptyZero("F0"));
			array[5] = new TableDataGetter<ThingDef>("offset 1\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 1));
			array[6] = new TableDataGetter<ThingDef>("offset 1\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 1).ToStringPercentEmptyZero("F0"));
			array[7] = new TableDataGetter<ThingDef>("offset 2\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 2));
			array[8] = new TableDataGetter<ThingDef>("offset 2\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 2).ToStringPercentEmptyZero("F0"));
			array[9] = new TableDataGetter<ThingDef>("offset 3\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 3));
			array[10] = new TableDataGetter<ThingDef>("offset 3\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 3).ToStringPercentEmptyZero("F0"));
			array[11] = new TableDataGetter<ThingDef>("offset 4\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|6_1(d, 4));
			array[12] = new TableDataGetter<ThingDef>("offset 4\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, 4).ToStringPercentEmptyZero("F0"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x00168E14 File Offset: 0x00167014
		[DebugOutput]
		public static void DefaultStuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.MadeFromStuff && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("default stuff", (ThingDef d) => GenStuff.DefaultStuffFor(d).defName);
			array[3] = new TableDataGetter<ThingDef>("stuff categories", (ThingDef d) => (from c in d.stuffCategories
			select c.defName).ToCommaList(false, false));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x00168F08 File Offset: 0x00167108
		[DebugOutput]
		public static void Beauties()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>()).Where(delegate(BuildableDef d)
			{
				ThingDef thingDef = d as ThingDef;
				if (thingDef != null)
				{
					return BeautyUtility.BeautyRelevant(thingDef.category);
				}
				return d is TerrainDef;
			})
			orderby (int)d.GetStatValueAbstract(StatDefOf.Beauty, null) descending
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[7];
			array[0] = new TableDataGetter<BuildableDef>("category", delegate(BuildableDef d)
			{
				if (!(d is ThingDef))
				{
					return "Terrain";
				}
				return ((ThingDef)d).category.ToString();
			});
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("beauty", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<BuildableDef>("beauty outdoors", delegate(BuildableDef d)
			{
				if (!d.StatBaseDefined(StatDefOf.BeautyOutdoors))
				{
					return "";
				}
				return d.GetStatValueAbstract(StatDefOf.BeautyOutdoors, null).ToString();
			});
			array[4] = new TableDataGetter<BuildableDef>("market value", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[5] = new TableDataGetter<BuildableDef>("work to produce", (BuildableDef d) => DebugOutputsEconomy.WorkToProduceBest(d).ToString());
			array[6] = new TableDataGetter<BuildableDef>("beauty per market value", delegate(BuildableDef d)
			{
				if (d.GetStatValueAbstract(StatDefOf.Beauty, null) <= 0f)
				{
					return "";
				}
				return (d.GetStatValueAbstract(StatDefOf.Beauty, null) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5");
			});
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x001690B8 File Offset: 0x001672B8
		[DebugOutput]
		public static void StuffBeauty()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[5];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("beauty factor", (ThingDef d) => DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty).ToString());
			array[2] = new TableDataGetter<ThingDef>("beauty offset", (ThingDef d) => DebugOutputsMisc.<StuffBeauty>g__getStatOffsetVal|9_1(d, StatDefOf.Beauty).ToString());
			array[3] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("beauty factor per market value", delegate(ThingDef d)
			{
				if (DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty) <= 0f)
				{
					return "";
				}
				return (DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|9_0(d, StatDefOf.Beauty) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x001691FC File Offset: 0x001673FC
		[DebugOutput]
		public static void ThingsPowerAndHeat()
		{
			Func<ThingDef, CompProperties_HeatPusher> heatPusher = delegate(ThingDef d)
			{
				if (d.comps == null)
				{
					return null;
				}
				for (int i = 0; i < d.comps.Count; i++)
				{
					CompProperties_HeatPusher compProperties_HeatPusher = d.comps[i] as CompProperties_HeatPusher;
					if (compProperties_HeatPusher != null)
					{
						return compProperties_HeatPusher;
					}
				}
				return null;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Building || d.GetCompProperties<CompProperties_Power>() != null || heatPusher(d) != null) && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[10];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base\npower consumption", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() != null)
				{
					return d.GetCompProperties<CompProperties_Power>().PowerConsumption.ToString();
				}
				return "";
			});
			array[2] = new TableDataGetter<ThingDef>("short circuit\nin rain", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() == null)
				{
					return "";
				}
				if (!d.GetCompProperties<CompProperties_Power>().shortCircuitInRain)
				{
					return "";
				}
				return "rainfire";
			});
			array[3] = new TableDataGetter<ThingDef>("transmits\npower", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() == null)
				{
					return "";
				}
				if (!d.GetCompProperties<CompProperties_Power>().transmitsPower)
				{
					return "";
				}
				return "transmit";
			});
			array[4] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue);
			array[5] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, false));
			array[6] = new TableDataGetter<ThingDef>("heat pusher\ncompClass", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).compClass.ToString();
				}
				return "";
			});
			array[7] = new TableDataGetter<ThingDef>("heat pusher\nheat per sec", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPerSecond.ToString();
				}
				return "";
			});
			array[8] = new TableDataGetter<ThingDef>("heat pusher\nmin temp", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPushMinTemperature.ToStringTemperature("F1");
				}
				return "";
			});
			array[9] = new TableDataGetter<ThingDef>("heat pusher\nmax temp", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPushMaxTemperature.ToStringTemperature("F1");
				}
				return "";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x001693C4 File Offset: 0x001675C4
		[DebugOutput]
		public static void FoodPoisonChances()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsIngestible
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category);
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("food poison chance", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_FoodPoisonable>() != null)
				{
					return "poisonable by cook";
				}
				float statValueAbstract = d.GetStatValueAbstract(StatDefOf.FoodPoisonChanceFixedHuman, null);
				if (statValueAbstract != 0f)
				{
					return statValueAbstract.ToStringPercent();
				}
				return "";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x0016948C File Offset: 0x0016768C
		[DebugOutput]
		public static void TechLevels()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building || d.category == ThingCategory.Item
			where !d.IsFrame && (d.building == null || !d.building.isNaturalRock)
			orderby (int)d.techLevel descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("tech level", (ThingDef d) => d.techLevel.ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0016959C File Offset: 0x0016779C
		[DebugOutput]
		public static void Stuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[26];
			array[0] = new TableDataGetter<ThingDef>("fabric", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Fabric).ToStringCheckBlank());
			array[1] = new TableDataGetter<ThingDef>("leather", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Leathery).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("metal", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("stony", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony).ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("woody", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Woody).ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[6] = new TableDataGetter<ThingDef>("burnable", (ThingDef d) => d.burnableByRecipe.ToStringCheckBlank());
			array[7] = new TableDataGetter<ThingDef>("smeltable", (ThingDef d) => d.smeltable.ToStringCheckBlank());
			array[8] = new TableDataGetter<ThingDef>("base\nmarket\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
			array[9] = new TableDataGetter<ThingDef>("melee\ncooldown\nmultiplier", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.MeleeWeapon_CooldownMultiplier));
			array[10] = new TableDataGetter<ThingDef>("melee\nsharp\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("melee\nsharp\ndps factor\noverall", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__meleeDpsSharpFactorOverall|13_1(d).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("melee\nblunt\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("melee\nblunt\ndps factor\noverall", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__meleeDpsBluntFactorOverall|13_2(d).ToString("F2"));
			array[14] = new TableDataGetter<ThingDef>("armor power\nsharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp, null).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("armor power\nblunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt, null).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("armor power\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat, null).ToString("F2"));
			array[17] = new TableDataGetter<ThingDef>("insulation\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold, null).ToString("F2"));
			array[18] = new TableDataGetter<ThingDef>("insulation\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat, null).ToString("F2"));
			array[19] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Flammability, null).ToString("F2"));
			array[20] = new TableDataGetter<ThingDef>("factor\nFlammability", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.Flammability));
			array[21] = new TableDataGetter<ThingDef>("factor\nWorkToMake", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.WorkToMake));
			array[22] = new TableDataGetter<ThingDef>("factor\nWorkToBuild", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.WorkToBuild));
			array[23] = new TableDataGetter<ThingDef>("factor\nMaxHp", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.MaxHitPoints));
			array[24] = new TableDataGetter<ThingDef>("factor\nBeauty", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.Beauty));
			array[25] = new TableDataGetter<ThingDef>("factor\nDoorspeed", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|13_0(d, StatDefOf.DoorOpenSpeed));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x00169A8C File Offset: 0x00167C8C
		[DebugOutput]
		public static void BurningAndSmeltingThings()
		{
			List<RecipeDef> burnRecipes = new List<RecipeDef>();
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefsListForReading)
			{
				if (recipeDef.defName.Substring(0, 4).ToLower().Equals("burn"))
				{
					burnRecipes.Add(recipeDef);
				}
			}
			List<RecipeDef> smeltRecipes = new List<RecipeDef>();
			foreach (RecipeDef recipeDef2 in DefDatabase<RecipeDef>.AllDefsListForReading)
			{
				if (recipeDef2.defName.Substring(0, 5).ToLower().Equals("smelt"))
				{
					smeltRecipes.Add(recipeDef2);
				}
			}
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[8];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.BaseFlammability);
			array[2] = new TableDataGetter<ThingDef>("burn- or smeltable", (ThingDef d) => (smeltRecipes.Any((RecipeDef r) => r.IsIngredient(d)) || burnRecipes.Any((RecipeDef r) => r.IsIngredient(d))).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("burnable", (ThingDef d) => d.burnableByRecipe.ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("smeltable", (ThingDef d) => d.smeltable.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("burn recipe", delegate(ThingDef d)
			{
				string[] array2 = (from r in burnRecipes
				where r.IsIngredient(d)
				select r.ToString()).ToArray<string>();
				if (array2.Length != 0)
				{
					return string.Join(",", array2);
				}
				return "NONE";
			});
			array[6] = new TableDataGetter<ThingDef>("smelt recipe", delegate(ThingDef d)
			{
				string[] array2 = (from r in smeltRecipes
				where r.IsIngredient(d)
				select r.ToString()).ToArray<string>();
				if (array2.Length != 0)
				{
					return string.Join(",", array2);
				}
				return "NONE";
			});
			array[7] = new TableDataGetter<ThingDef>("category", delegate(ThingDef d)
			{
				if (d.thingCategories != null)
				{
					return string.Join(",", (from c in d.thingCategories
					select c.defName).ToArray<string>());
				}
				return "NULL";
			});
			DebugTables.MakeTablesDialog<ThingDef>(allDefs, array);
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x00169CB8 File Offset: 0x00167EB8
		[DebugOutput]
		public static void Medicines()
		{
			List<float> list = new List<float>();
			list.Add(0.3f);
			list.AddRange(from d in DefDatabase<ThingDef>.AllDefs
			where typeof(Medicine).IsAssignableFrom(d.thingClass)
			select d.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			SkillNeed_Direct skillNeed_Direct = (SkillNeed_Direct)StatDefOf.MedicalTendQuality.skillNeedFactors[0];
			TableDataGetter<float>[] array = new TableDataGetter<float>[21];
			array[0] = new TableDataGetter<float>("potency", (float p) => p.ToStringPercent());
			for (int i = 0; i < 20; i++)
			{
				float factor = skillNeed_Direct.valuesPerLevel[i];
				array[i + 1] = new TableDataGetter<float>((i + 1).ToString(), (float p) => (p * factor).ToStringPercent());
			}
			DebugTables.MakeTablesDialog<float>(list, array);
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x00169DC4 File Offset: 0x00167FC4
		[DebugOutput]
		public static void ShootingAccuracy()
		{
			StatDef stat = StatDefOf.ShootingAccuracyPawn;
			Func<StatModifier, bool> <>9__19;
			Func<int, float, int, float> accAtDistance = delegate(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					IEnumerable<StatModifier> statOffsets = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets;
					Func<StatModifier, bool> predicate;
					if ((predicate = <>9__19) == null)
					{
						predicate = (<>9__19 = ((StatModifier so) => so.stat == stat));
					}
					float value = statOffsets.First(predicate).value;
					num += value;
				}
				foreach (SkillNeed skillNeed in stat.skillNeedFactors)
				{
					SkillNeed_Direct skillNeed_Direct = skillNeed as SkillNeed_Direct;
					num *= skillNeed_Direct.valuesPerLevel[level];
				}
				num = stat.postProcessCurve.Evaluate(num);
				return Mathf.Pow(num, dist);
			};
			List<int> list = new List<int>();
			for (int i = 0; i <= 20; i++)
			{
				list.Add(i);
			}
			IEnumerable<int> dataSources = list;
			TableDataGetter<int>[] array = new TableDataGetter<int>[18];
			array[0] = new TableDataGetter<int>("No trait skill", (int lev) => lev.ToString());
			array[1] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 0).ToStringPercent("F2"));
			array[2] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 0).ToStringPercent("F2"));
			array[3] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 0).ToStringPercent("F2"));
			array[4] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 0).ToStringPercent("F2"));
			array[5] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 0).ToStringPercent("F2"));
			array[6] = new TableDataGetter<int>("Careful shooter skill", (int lev) => lev.ToString());
			array[7] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 1).ToStringPercent("F2"));
			array[8] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 1).ToStringPercent("F2"));
			array[9] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 1).ToStringPercent("F2"));
			array[10] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 1).ToStringPercent("F2"));
			array[11] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 1).ToStringPercent("F2"));
			array[12] = new TableDataGetter<int>("Trigger-happy skill", (int lev) => lev.ToString());
			array[13] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, -1).ToStringPercent("F2"));
			array[14] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, -1).ToStringPercent("F2"));
			array[15] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, -1).ToStringPercent("F2"));
			array[16] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, -1).ToStringPercent("F2"));
			array[17] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, -1).ToStringPercent("F2"));
			DebugTables.MakeTablesDialog<int>(dataSources, array);
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x0016A01F File Offset: 0x0016821F
		[DebugOutput(true)]
		public static void TemperatureData()
		{
			Find.CurrentMap.mapTemperature.DebugLogTemps();
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x0016A030 File Offset: 0x00168230
		[DebugOutput(false)]
		public static void TemperatureOverlayColors()
		{
			Find.CurrentMap.mapTemperature.DebugLogTemperatureOverlayColors();
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x0016A041 File Offset: 0x00168241
		[DebugOutput(true)]
		public static void WeatherChances()
		{
			Find.CurrentMap.weatherDecider.LogWeatherChances();
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x0016A054 File Offset: 0x00168254
		[DebugOutput]
		public static void WeatherCommonalities()
		{
			IEnumerable<TableDataGetter<BiomeDef>> source = DefDatabase<WeatherDef>.AllDefsListForReading.Select(delegate(WeatherDef w)
			{
				Predicate<WeatherCommonalityRecord> <>9__3;
				return new TableDataGetter<BiomeDef>(w.label, delegate(BiomeDef b)
				{
					List<WeatherCommonalityRecord> baseWeatherCommonalities = b.baseWeatherCommonalities;
					Predicate<WeatherCommonalityRecord> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = ((WeatherCommonalityRecord c) => c.weather == w));
					}
					WeatherCommonalityRecord weatherCommonalityRecord = baseWeatherCommonalities.FirstOrDefault(predicate);
					return ((weatherCommonalityRecord != null) ? weatherCommonalityRecord.commonality.ToString() : null) ?? "-";
				});
			});
			DebugTables.MakeTablesDialog<BiomeDef>(DefDatabase<BiomeDef>.AllDefs, source.Prepend(new TableDataGetter<BiomeDef>("biome", (BiomeDef w) => w.defName)).ToArray<TableDataGetter<BiomeDef>>());
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x0016A0C9 File Offset: 0x001682C9
		[DebugOutput(true)]
		public static void CelestialGlow()
		{
			GenCelestial.LogSunGlowForYear();
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x0016A0D0 File Offset: 0x001682D0
		[DebugOutput(true)]
		public static void SunAngle()
		{
			GenCelestial.LogSunAngleForYear();
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x0016A0D7 File Offset: 0x001682D7
		[DebugOutput(true)]
		public static void FallColor()
		{
			PlantUtility.LogFallColorForYear();
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x0016A0DE File Offset: 0x001682DE
		[DebugOutput(true)]
		public static void PawnsListAllOnMap()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x0016A0EF File Offset: 0x001682EF
		[DebugOutput(true)]
		public static void WindSpeeds()
		{
			Find.CurrentMap.windManager.LogWindSpeeds();
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x0016A0DE File Offset: 0x001682DE
		[DebugOutput(true)]
		public static void MapPawnsList()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x0016A100 File Offset: 0x00168300
		[DebugOutput]
		public static void Lords()
		{
			Find.CurrentMap.lordManager.LogLords();
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x0016A111 File Offset: 0x00168311
		[DebugOutput]
		public static void LogEnroute()
		{
			Find.CurrentMap.enrouteManager.LogEnroute();
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x0016A124 File Offset: 0x00168324
		[DebugOutput]
		public static void BodyPartTagGroups()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				FloatMenuOption item = new FloatMenuOption(localBd.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(localBd.defName + "\n----------------");
					using (IEnumerator<BodyPartTagDef> enumerator2 = localBd.AllParts.SelectMany((BodyPartRecord part) => part.def.tags).Distinct<BodyPartTagDef>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BodyPartTagDef tag = enumerator2.Current;
							stringBuilder.AppendLine(tag.defName);
							IEnumerable<BodyPartRecord> allParts = localBd.AllParts;
							Func<BodyPartRecord, bool> predicate;
							Func<BodyPartRecord, bool> <>9__2;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((BodyPartRecord part) => part.def.tags.Contains(tag)));
							}
							foreach (BodyPartRecord bodyPartRecord in from part in allParts.Where(predicate)
							orderby part.def.defName
							select part)
							{
								stringBuilder.AppendLine("  " + bodyPartRecord.def.defName);
							}
						}
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x0016A1C0 File Offset: 0x001683C0
		[DebugOutput]
		public static void MinifiableTags()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.Minifiable)
				{
					stringBuilder.Append(thingDef.defName);
					if (!thingDef.tradeTags.NullOrEmpty<string>())
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(thingDef.tradeTags.ToCommaList(false, false));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x0016A260 File Offset: 0x00168460
		[DebugOutput]
		public static void ThingSetMakerTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				Action<ThingSetMakerParams> <>9__1;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					DebugOutputsMisc.<>c__DisplayClass31_1 CS$<>8__locals2 = new DebugOutputsMisc.<>c__DisplayClass31_1();
					DebugOutputsMisc.<>c__DisplayClass31_1 CS$<>8__locals3 = CS$<>8__locals2;
					Action<ThingSetMakerParams> generate;
					if ((generate = <>9__1) == null)
					{
						generate = (<>9__1 = delegate(ThingSetMakerParams parms)
						{
							StringBuilder stringBuilder = new StringBuilder();
							float num = 0f;
							float num2 = 0f;
							for (int i = 0; i < 50; i++)
							{
								List<Thing> list3 = localDef.root.Generate(parms);
								if (stringBuilder.Length > 0)
								{
									stringBuilder.AppendLine();
								}
								if (list3.NullOrEmpty<Thing>())
								{
									stringBuilder.AppendLine("-(nothing generated)");
								}
								float num3 = 0f;
								float num4 = 0f;
								for (int j = 0; j < list3.Count; j++)
								{
									stringBuilder.AppendLine("-" + list3[j].LabelCap + " - $" + (list3[j].MarketValue * (float)list3[j].stackCount).ToString("F0"));
									num3 += list3[j].MarketValue * (float)list3[j].stackCount;
									if (!(list3[j] is Pawn))
									{
										num4 += list3[j].GetStatValue(StatDefOf.Mass, true, -1) * (float)list3[j].stackCount;
									}
									list3[j].Destroy(DestroyMode.Vanish);
								}
								num += num3;
								num2 += num4;
								stringBuilder.AppendLine("   Total market value: $" + num3.ToString("F0"));
								stringBuilder.AppendLine("   Total mass: " + num4.ToStringMass());
							}
							StringBuilder stringBuilder2 = new StringBuilder();
							stringBuilder2.AppendLine("Default thing sets generated by: " + localDef.defName);
							string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localDef.root.fixedParams);
							stringBuilder2.AppendLine("root fixedParams: " + (nonNullFieldsDebugInfo.NullOrEmpty() ? "none" : nonNullFieldsDebugInfo));
							string nonNullFieldsDebugInfo2 = Gen.GetNonNullFieldsDebugInfo(parms);
							if (!nonNullFieldsDebugInfo2.NullOrEmpty())
							{
								stringBuilder2.AppendLine("(used custom debug params: " + nonNullFieldsDebugInfo2 + ")");
							}
							stringBuilder2.AppendLine("Average market value: $" + (num / 50f).ToString("F1"));
							stringBuilder2.AppendLine("Average mass: " + (num2 / 50f).ToStringMass());
							stringBuilder2.AppendLine();
							stringBuilder2.Append(stringBuilder.ToString());
							Log.Message(stringBuilder2.ToString());
						});
					}
					CS$<>8__locals3.generate = generate;
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.makingFaction = localF;
											obj.traderDef = localKind;
											CS$<>8__locals2.generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
						return;
					}
					CS$<>8__locals2.generate(localDef.debugParams);
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x0016A2F0 File Offset: 0x001684F0
		[DebugOutput]
		public static void ThingSetMakerPossibleDefs()
		{
			Dictionary<ThingSetMakerDef, List<ThingDef>> generatableThings = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
			foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
				generatableThings[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
			}
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			list.Add(new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null)));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass()));
			list.Add(new TableDataGetter<ThingDef>("min\ncount", delegate(ThingDef d)
			{
				if (d.stackLimit == 1)
				{
					return "";
				}
				return d.minRewardCount.ToString();
			}));
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				list.Add(new TableDataGetter<ThingDef>(localDef.defName.Shorten(), (ThingDef d) => generatableThings[localDef].Contains(d).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && !d.IsCorpse && !d.isUnfinishedThing) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			orderby d.BaseMarketValue descending
			select d, list.ToArray());
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x0016A500 File Offset: 0x00168700
		[DebugOutput]
		public static void ThingSetMakerSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				Action<ThingSetMakerParams> <>9__1;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					DebugOutputsMisc.<>c__DisplayClass33_1 CS$<>8__locals2 = new DebugOutputsMisc.<>c__DisplayClass33_1();
					DebugOutputsMisc.<>c__DisplayClass33_1 CS$<>8__locals3 = CS$<>8__locals2;
					Action<ThingSetMakerParams> generate;
					if ((generate = <>9__1) == null)
					{
						generate = (<>9__1 = delegate(ThingSetMakerParams parms)
						{
							Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
							for (int i = 0; i < 500; i++)
							{
								List<Thing> list3 = localDef.root.Generate(parms);
								foreach (ThingDef thingDef in (from th in list3
								select th.GetInnerIfMinified().def).Distinct<ThingDef>())
								{
									if (!counts.ContainsKey(thingDef))
									{
										counts.Add(thingDef, 0);
									}
									Dictionary<ThingDef, int> counts2 = counts;
									ThingDef key = thingDef;
									int num = counts2[key];
									counts2[key] = num + 1;
								}
								for (int j = 0; j < list3.Count; j++)
								{
									list3[j].Destroy(DestroyMode.Vanish);
								}
							}
							IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
							where counts.ContainsKey(d)
							orderby counts[d] descending
							select d;
							TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
							array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
							array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
							array[2] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass());
							array[3] = new TableDataGetter<ThingDef>("appearance rate in " + localDef.defName, (ThingDef d) => ((float)counts[d] / 500f).ToStringPercent());
							DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
						});
					}
					CS$<>8__locals3.generate = generate;
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.makingFaction = localF;
											obj.traderDef = localKind;
											CS$<>8__locals2.generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3, null));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
						return;
					}
					CS$<>8__locals2.generate(localDef.debugParams);
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x0016A590 File Offset: 0x00168790
		[DebugOutput]
		public static void RewardsGeneration()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction != Faction.OfPlayer)
				{
					Faction localF = faction;
					list.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
						{
							float localPoints = localPoints2;
							list2.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int i = 0; i < 30; i++)
								{
									RewardsGeneratorParams rewardsGeneratorParams = new RewardsGeneratorParams
									{
										allowGoodwill = true,
										allowRoyalFavor = true,
										populationIntent = StorytellerUtilityPopulation.PopulationIntentForQuest,
										giverFaction = localF,
										rewardValue = localPoints
									};
									float f;
									List<Reward> source = RewardsGenerator.Generate(rewardsGeneratorParams, out f);
									StringBuilder stringBuilder2 = stringBuilder;
									string[] array = new string[8];
									array[0] = "giver: ";
									array[1] = rewardsGeneratorParams.giverFaction.Name;
									array[2] = ", input value: ";
									array[3] = rewardsGeneratorParams.rewardValue.ToStringMoney(null);
									array[4] = ", output value: ";
									array[5] = f.ToStringMoney(null);
									array[6] = "\n";
									array[7] = (from x in source
									select "-" + x.ToString()).ToLineList(null, false).Indented("  ");
									stringBuilder2.AppendLine(string.Concat(array));
									stringBuilder.AppendLine();
								}
								Log.Message(stringBuilder.ToString());
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, null));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x0016A648 File Offset: 0x00168848
		[DebugOutput]
		public static void RewardsGenerationSampled()
		{
			RewardsGeneratorParams parms = default(RewardsGeneratorParams);
			parms.allowGoodwill = true;
			parms.allowRoyalFavor = true;
			parms.populationIntent = StorytellerUtilityPopulation.PopulationIntentForQuest;
			parms.giverFaction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Undefined, false)
			where !x.Hidden && x.def.humanlikeFaction && !x.HostileTo(Faction.OfPlayer)
			select x).First<Faction>();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
			{
				float localPoints = localPoints2;
				list.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
				{
					parms.rewardValue = localPoints;
					Dictionary<Type, int> countByType = new Dictionary<Type, int>();
					Dictionary<ThingDef, int> countByThingDef = new Dictionary<ThingDef, int>();
					for (int i = 0; i < 1000; i++)
					{
						foreach (Reward reward in RewardsGenerator.Generate(parms))
						{
							countByType.Increment(reward.GetType());
							Reward_Items reward_Items = reward as Reward_Items;
							if (reward_Items != null)
							{
								foreach (ThingDef key in (from x in reward_Items.items
								select x.GetInnerIfMinified().def).Distinct<ThingDef>())
								{
									countByThingDef.Increment(key);
								}
							}
						}
					}
					Dictionary<ThingSetMakerDef, List<ThingDef>> dictionary = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
					foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
					{
						ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
						dictionary[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
					}
					List<TableDataGetter<object>> list2 = new List<TableDataGetter<object>>();
					list2.Add(new TableDataGetter<object>("defName", delegate(object d)
					{
						if (!(d is Type))
						{
							return ((ThingDef)d).defName;
						}
						return "*" + ((Type)d).Name;
					}));
					list2.Add(new TableDataGetter<object>("times appeared\nin " + 1000.ToString() + " rewards\nof value " + localPoints.ToString(), delegate(object d)
					{
						if (!(d is Type))
						{
							return countByThingDef.TryGetValue((ThingDef)d, 0);
						}
						return countByType.TryGetValue((Type)d, 0);
					}));
					list2.Add(new TableDataGetter<object>("reward tags", delegate(object d)
					{
						ThingDef thingDef = d as ThingDef;
						if (thingDef != null && !thingDef.thingSetMakerTags.NullOrEmpty<string>())
						{
							if (thingDef.thingSetMakerTags.Any((string x) => x.Contains("Reward")))
							{
								return thingDef.thingSetMakerTags.ToCommaList(false, false);
							}
						}
						return "-";
					}));
					list2.Add(new TableDataGetter<object>("market value", delegate(object d)
					{
						ThingDef thingDef = d as ThingDef;
						if (thingDef != null)
						{
							return thingDef.BaseMarketValue.ToStringMoney(null);
						}
						return "-";
					}));
					DebugTables.MakeTablesDialog<object>(typeof(Reward).AllSubclassesNonAbstract().Cast<object>().Union(countByThingDef.Keys.Cast<object>()).OrderByDescending(delegate(object d)
					{
						if (!(d is Type))
						{
							return countByThingDef.TryGetValue((ThingDef)d, 0);
						}
						return countByType.TryGetValue((Type)d, 0);
					}), list2.ToArray());
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x0016A760 File Offset: 0x00168960
		[DebugOutput]
		public static void WorkDisables()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef pkInner2 in from ki in DefDatabase<PawnKindDef>.AllDefs
			where ki.RaceProps.Humanlike
			select ki)
			{
				PawnKindDef pkInner = pkInner2;
				Faction faction = FactionUtility.DefaultFactionFrom(pkInner.defaultFactionDef);
				FloatMenuOption item = new FloatMenuOption(pkInner.defName, delegate()
				{
					int num = 500;
					DefMap<WorkTypeDef, int> defMap = new DefMap<WorkTypeDef, int>();
					for (int i = 0; i < num; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(pkInner, faction, null);
						if (pawn.workSettings != null)
						{
							foreach (WorkTypeDef workTypeDef in pawn.GetDisabledWorkTypes(true))
							{
								DefMap<WorkTypeDef, int> defMap2 = defMap;
								WorkTypeDef def = workTypeDef;
								int num2 = defMap2[def];
								defMap2[def] = num2 + 1;
							}
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"Generated ",
						num.ToString(),
						" pawns of kind ",
						pkInner.defName,
						" on faction ",
						faction.ToStringSafe<Faction>()
					}));
					stringBuilder.AppendLine("Work types disabled:");
					foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefs)
					{
						if (workTypeDef2.workTags != WorkTags.None)
						{
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"   ",
								workTypeDef2.defName,
								": ",
								defMap[workTypeDef2].ToString(),
								"        ",
								((float)defMap[workTypeDef2] / (float)num).ToStringPercent()
							}));
						}
					}
					IEnumerable<BackstoryDef> allDefs = DefDatabase<BackstoryDef>.AllDefs;
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTypeDef disable rates (there are " + allDefs.Count<BackstoryDef>().ToString() + " backstories):");
					using (IEnumerator<WorkTypeDef> enumerator3 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							WorkTypeDef wt = enumerator3.Current;
							int num3 = 0;
							Predicate<WorkTypeDef> <>9__2;
							foreach (BackstoryDef backstoryDef in allDefs)
							{
								List<WorkTypeDef> disabledWorkTypes = backstoryDef.DisabledWorkTypes;
								Predicate<WorkTypeDef> predicate;
								if ((predicate = <>9__2) == null)
								{
									predicate = (<>9__2 = ((WorkTypeDef wd) => wt == wd));
								}
								if (disabledWorkTypes.Any(predicate))
								{
									num3++;
								}
							}
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"   ",
								wt.defName,
								": ",
								num3.ToString(),
								"     ",
								((float)num3 / (float)allDefs.Count<BackstoryDef>()).ToStringPercent()
							}));
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTag disable rates (there are " + allDefs.Count<BackstoryDef>().ToString() + " backstories):");
					foreach (object obj in Enum.GetValues(typeof(WorkTags)))
					{
						WorkTags workTags = (WorkTags)obj;
						int num4 = 0;
						foreach (BackstoryDef backstoryDef2 in allDefs)
						{
							if ((workTags & backstoryDef2.workDisables) != WorkTags.None)
							{
								num4++;
							}
						}
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"   ",
							workTags.ToString(),
							": ",
							num4.ToString(),
							"     ",
							((float)num4 / (float)allDefs.Count<BackstoryDef>()).ToStringPercent()
						}));
					}
					Log.Message(stringBuilder.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x0016A834 File Offset: 0x00168A34
		[DebugOutput]
		public static void FoodPreferability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Food, ordered by preferability:");
			foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
			where td.ingestible != null
			orderby td.ingestible.preferability
			select td)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", thingDef.ingestible.preferability, thingDef.defName));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x0016A908 File Offset: 0x00168B08
		[DebugOutput]
		public static void IngestibleMaxSatisfiedTitle()
		{
			RoyalTitleUtility.DoTable_IngestibleMaxSatisfiedTitle();
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x0016A90F File Offset: 0x00168B0F
		[DebugOutput]
		public static void AbilityCosts()
		{
			AbilityUtility.DoTable_AbilityCosts();
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x0016A918 File Offset: 0x00168B18
		[DebugOutput]
		public static void SitePartDefs()
		{
			List<TableDataGetter<SitePartDef>> list = new List<TableDataGetter<SitePartDef>>();
			list.Add(new TableDataGetter<SitePartDef>("defName", (SitePartDef a) => a.defName));
			list.Add(new TableDataGetter<SitePartDef>("label", (SitePartDef a) => a.LabelCap));
			list.Add(new TableDataGetter<SitePartDef>("tags", (SitePartDef a) => string.Join(", ", a.tags)));
			list.Add(new TableDataGetter<SitePartDef>("excludeTags", (SitePartDef a) => string.Join(", ", a.excludesTags)));
			list.Add(new TableDataGetter<SitePartDef>("incompatible\nothers", (SitePartDef a) => string.Join<TaggedString>(", ", from def in DefDatabase<SitePartDef>.AllDefs
			where !def.CompatibleWith(a)
			select def.LabelCap)));
			DebugTables.MakeTablesDialog<SitePartDef>(DefDatabase<SitePartDef>.AllDefsListForReading, list.ToArray());
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x0016AA28 File Offset: 0x00168C28
		[DebugOutput]
		public static void MapDanger()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Map danger status:");
			foreach (Map map in Find.Maps)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", map, map.dangerWatcher.DangerRating));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x0016AAB4 File Offset: 0x00168CB4
		[DebugOutput]
		public static void GenSteps()
		{
			IEnumerable<GenStepDef> dataSources = from x in DefDatabase<GenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<GenStepDef>[] array = new TableDataGetter<GenStepDef>[4];
			array[0] = new TableDataGetter<GenStepDef>("defName", (GenStepDef x) => x.defName);
			array[1] = new TableDataGetter<GenStepDef>("order", (GenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<GenStepDef>("class", (GenStepDef x) => x.genStep.GetType().Name);
			array[3] = new TableDataGetter<GenStepDef>("site", delegate(GenStepDef x)
			{
				if (x.linkWithSite == null)
				{
					return "";
				}
				return x.linkWithSite.defName;
			});
			DebugTables.MakeTablesDialog<GenStepDef>(dataSources, array);
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0016ABCC File Offset: 0x00168DCC
		[DebugOutput]
		public static void WorldGenSteps()
		{
			IEnumerable<WorldGenStepDef> dataSources = from x in DefDatabase<WorldGenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<WorldGenStepDef>[] array = new TableDataGetter<WorldGenStepDef>[3];
			array[0] = new TableDataGetter<WorldGenStepDef>("defName", (WorldGenStepDef x) => x.defName);
			array[1] = new TableDataGetter<WorldGenStepDef>("order", (WorldGenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<WorldGenStepDef>("class", (WorldGenStepDef x) => x.worldGenStep.GetType().Name);
			DebugTables.MakeTablesDialog<WorldGenStepDef>(dataSources, array);
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x0016ACB8 File Offset: 0x00168EB8
		[DebugOutput]
		public static void ShuttleDefsToAvoid()
		{
			IEnumerable<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[2];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef x) => x.defName);
			array[1] = new TableDataGetter<ThingDef>("avoid", (ThingDef x) => (GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, x, false) || (x.plant != null && x.plant.IsTree) || x.category == ThingCategory.Item || x.category == ThingCategory.Building).ToStringCheckBlank());
			DebugTables.MakeTablesDialog<ThingDef>(allDefsListForReading, array);
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x0016AD30 File Offset: 0x00168F30
		[DebugOutput]
		public static void RitualDuration()
		{
			IEnumerable<RitualBehaviorDef> allDefsListForReading = DefDatabase<RitualBehaviorDef>.AllDefsListForReading;
			TableDataGetter<RitualBehaviorDef>[] array = new TableDataGetter<RitualBehaviorDef>[2];
			array[0] = new TableDataGetter<RitualBehaviorDef>("defName", (RitualBehaviorDef x) => x.defName);
			array[1] = new TableDataGetter<RitualBehaviorDef>("duration", (RitualBehaviorDef x) => x.durationTicks.max.ToStringTicksToPeriod(true, false, true, true, false));
			DebugTables.MakeTablesDialog<RitualBehaviorDef>(allDefsListForReading, array);
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x0016ADA5 File Offset: 0x00168FA5
		[DebugOutput]
		public static void PlayerWealth()
		{
			Log.Message("Player wealth: " + WealthUtility.PlayerWealth.ToStringMoney(null));
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x0016ADC4 File Offset: 0x00168FC4
		[DebugOutput]
		public static void MemesAndPrecepts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Memes:");
			foreach (MemeDef memeDef in from x in DefDatabase<MemeDef>.AllDefs
			orderby x.category
			select x)
			{
				stringBuilder.AppendLine(string.Format("{0}: \"{1}\"", memeDef.LabelCap, memeDef.description));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Precepts:");
			using (IEnumerator<IssueDef> enumerator2 = DefDatabase<IssueDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IssueDef i = enumerator2.Current;
					stringBuilder.AppendLine(i.LabelCap);
					IEnumerable<PreceptDef> allDefs = DefDatabase<PreceptDef>.AllDefs;
					Func<PreceptDef, bool> predicate;
					Func<PreceptDef, bool> <>9__1;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PreceptDef x) => x.issue == i && !x.label.NullOrEmpty() && !x.description.NullOrEmpty()));
					}
					foreach (PreceptDef preceptDef in allDefs.Where(predicate))
					{
						stringBuilder.AppendLine(string.Format("\t{0}: \"{1}\"", preceptDef.LabelCap, preceptDef.description));
					}
				}
			}
			Log.Message(GUIUtility.systemCopyBuffer = stringBuilder.ToString());
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x0016AF6C File Offset: 0x0016916C
		[DebugOutput]
		public static void GenerateGeneSetsX10()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 10; i++)
			{
				GeneSet geneSet = GeneUtility.GenerateGeneSet(null);
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(geneSet.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x0016AFC4 File Offset: 0x001691C4
		[DebugOutput]
		public static void Genes()
		{
			IEnumerable<GeneDef> allDefs = DefDatabase<GeneDef>.AllDefs;
			TableDataGetter<GeneDef>[] array = new TableDataGetter<GeneDef>[7];
			array[0] = new TableDataGetter<GeneDef>("defName", (GeneDef g) => g.defName);
			array[1] = new TableDataGetter<GeneDef>("label", (GeneDef g) => g.label);
			array[2] = new TableDataGetter<GeneDef>("labelShortAdj", (GeneDef g) => g.LabelShortAdj);
			array[3] = new TableDataGetter<GeneDef>("CPX", (GeneDef g) => g.biostatCpx);
			array[4] = new TableDataGetter<GeneDef>("MET", (GeneDef g) => g.biostatMet);
			array[5] = new TableDataGetter<GeneDef>("ARC", (GeneDef g) => g.biostatArc);
			array[6] = new TableDataGetter<GeneDef>("prereq", delegate(GeneDef g)
			{
				if (g.prerequisite != null)
				{
					return g.prerequisite.defName;
				}
				return string.Empty;
			});
			DebugTables.MakeTablesDialog<GeneDef>(allDefs, array);
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x0016B118 File Offset: 0x00169318
		[DebugOutput]
		public static void Xenotypes()
		{
			IEnumerable<XenotypeDef> allDefs = DefDatabase<XenotypeDef>.AllDefs;
			TableDataGetter<XenotypeDef>[] array = new TableDataGetter<XenotypeDef>[4];
			array[0] = new TableDataGetter<XenotypeDef>("defName", (XenotypeDef x) => x.defName);
			array[1] = new TableDataGetter<XenotypeDef>("CPX", (XenotypeDef x) => x.AllGenes.Sum((GeneDef y) => y.biostatCpx));
			array[2] = new TableDataGetter<XenotypeDef>("MET", (XenotypeDef x) => x.AllGenes.Sum((GeneDef y) => y.biostatMet));
			array[3] = new TableDataGetter<XenotypeDef>("ARC", (XenotypeDef x) => x.AllGenes.Sum((GeneDef y) => y.biostatArc));
			DebugTables.MakeTablesDialog<XenotypeDef>(allDefs, array);
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x0016B1E8 File Offset: 0x001693E8
		[DebugOutput]
		public static void PreceptDefs()
		{
			IEnumerable<PreceptDef> dataSources = from x in DefDatabase<PreceptDef>.AllDefs
			orderby x.defName
			select x;
			TableDataGetter<PreceptDef>[] array = new TableDataGetter<PreceptDef>[8];
			array[0] = new TableDataGetter<PreceptDef>("defName", (PreceptDef x) => x.defName);
			array[1] = new TableDataGetter<PreceptDef>("impact", (PreceptDef x) => x.impact);
			array[2] = new TableDataGetter<PreceptDef>("duplicates", (PreceptDef x) => x.allowDuplicates.ToStringCheckBlank());
			array[3] = new TableDataGetter<PreceptDef>("defaultSelectionWeight", (PreceptDef x) => x.defaultSelectionWeight.ToString());
			array[4] = new TableDataGetter<PreceptDef>("noExpansion", (PreceptDef x) => x.classic.ToStringCheckBlank());
			array[5] = new TableDataGetter<PreceptDef>("exclusionTags", (PreceptDef x) => x.exclusionTags.ToCommaList(false, true));
			array[6] = new TableDataGetter<PreceptDef>("effects", (PreceptDef x) => string.Join("\n", x.comps.SelectMany((PreceptComp y) => y.GetDescriptions()).Distinct<string>()));
			array[7] = new TableDataGetter<PreceptDef>("stats", delegate(PreceptDef x)
			{
				IEnumerable<string> first;
				if (x.statOffsets == null)
				{
					first = Enumerable.Empty<string>();
				}
				else
				{
					first = from y in x.statOffsets
					select y.stat.defName + ": " + y.ValueToStringAsOffset;
				}
				IEnumerable<string> second;
				if (x.statFactors == null)
				{
					second = Enumerable.Empty<string>();
				}
				else
				{
					second = from y in x.statFactors
					select y.stat.defName + ": " + y.ToStringAsFactor;
				}
				return first.Concat(second).ToCommaList(false, true);
			});
			DebugTables.MakeTablesDialog<PreceptDef>(dataSources, array);
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x0016B38C File Offset: 0x0016958C
		[DebugOutput]
		public static void ThingTradeTags()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<string> enumerator = (from t in (from k in DefDatabase<ThingDef>.AllDefs
			where k.tradeTags != null
			select k).SelectMany((ThingDef k) => k.tradeTags).Distinct<string>()
			orderby t
			select t).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string tag = enumerator.Current;
					Func<ThingDef, bool> <>9__5;
					list.Add(new DebugMenuOption(tag, DebugMenuOptionMode.Action, delegate()
					{
						List<TableDataGetter<ThingDef>> list2 = new List<TableDataGetter<ThingDef>>();
						list2.Add(new TableDataGetter<ThingDef>("defName", (ThingDef k) => k.defName));
						IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
						Func<ThingDef, bool> predicate;
						if ((predicate = <>9__5) == null)
						{
							predicate = (<>9__5 = ((ThingDef t) => t.tradeTags.NotNullAndContains(tag)));
						}
						DebugTables.MakeTablesDialog<ThingDef>(allDefs.Where(predicate), list2.ToArray());
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x0016B484 File Offset: 0x00169684
		[DebugOutput]
		public static void StructureColorDefs()
		{
			IEnumerable<ColorDef> dataSources = from x in DefDatabase<ColorDef>.AllDefs
			where x.colorType == ColorType.Structure
			orderby x.displayOrder
			select x;
			TableDataGetter<ColorDef>[] array = new TableDataGetter<ColorDef>[5];
			array[0] = new TableDataGetter<ColorDef>("defName", (ColorDef x) => x.defName);
			array[1] = new TableDataGetter<ColorDef>("label", (ColorDef x) => x.label);
			array[2] = new TableDataGetter<ColorDef>("rgb", (ColorDef x) => string.Format("({0}, {1}, {2})", Mathf.RoundToInt(x.color.r * 255f), Mathf.RoundToInt(x.color.r * 255f), Mathf.RoundToInt(x.color.r * 2555f)));
			array[3] = new TableDataGetter<ColorDef>("hsl", delegate(ColorDef x)
			{
				float num;
				float f;
				float f2;
				Color.RGBToHSV(x.color, out num, out f, out f2);
				return string.Format("({0}, {1}, {2})", Mathf.RoundToInt(num * 360f), f.ToStringPercent(), f2.ToStringPercent());
			});
			array[4] = new TableDataGetter<ColorDef>("order", (ColorDef x) => x.displayOrder);
			DebugTables.MakeTablesDialog<ColorDef>(dataSources, array);
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x0016B5C8 File Offset: 0x001697C8
		[DebugOutput]
		public static void PsychicRituals()
		{
			IEnumerable<PsychicRitualDef_InvocationCircle> allDefs = DefDatabase<PsychicRitualDef_InvocationCircle>.AllDefs;
			TableDataGetter<PsychicRitualDef_InvocationCircle>[] array = new TableDataGetter<PsychicRitualDef_InvocationCircle>[5];
			array[0] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("defName", (PsychicRitualDef_InvocationCircle g) => g.defName);
			array[1] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("label", (PsychicRitualDef_InvocationCircle g) => g.label);
			array[2] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("duration", (PsychicRitualDef_InvocationCircle g) => g.DurationLabel());
			array[3] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("cooldown", (PsychicRitualDef_InvocationCircle g) => g.CooldownLabel);
			array[4] = new TableDataGetter<PsychicRitualDef_InvocationCircle>("required offering", delegate(PsychicRitualDef_InvocationCircle g)
			{
				if (g.RequiredOffering == null)
				{
					return "-";
				}
				return g.RequiredOffering.SummaryFilterFirst;
			});
			DebugTables.MakeTablesDialog<PsychicRitualDef_InvocationCircle>(allDefs, array);
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x0016B6C4 File Offset: 0x001698C4
		[DebugOutput]
		public static void AnalysisDetails()
		{
			IEnumerable<AnalysisDetails> analysisDetailsForReading = Find.AnalysisManager.AnalysisDetailsForReading;
			TableDataGetter<AnalysisDetails>[] array = new TableDataGetter<AnalysisDetails>[3];
			array[0] = new TableDataGetter<AnalysisDetails>("id", (AnalysisDetails g) => g.id);
			array[1] = new TableDataGetter<AnalysisDetails>("times done", (AnalysisDetails g) => g.timesDone);
			array[2] = new TableDataGetter<AnalysisDetails>("required", (AnalysisDetails g) => g.required);
			DebugTables.MakeTablesDialog<AnalysisDetails>(analysisDetailsForReading, array);
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x0016B76C File Offset: 0x0016996C
		[DebugOutput]
		private static void RelicThings()
		{
			List<PreceptThingChance> relicDefs = PreceptDefOf.IdeoRelic.Worker.ThingDefs.ToList<PreceptThingChance>();
			IEnumerable<ThingDef> dataSources = from td in DefDatabase<ThingDef>.AllDefsListForReading
			where td.category == ThingCategory.Item
			select td;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[7];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("chance", delegate(ThingDef d)
			{
				PreceptThingChance preceptThingChance = relicDefs.FirstOrDefault((PreceptThingChance def) => def.def == d);
				if (preceptThingChance.chance <= 0f)
				{
					return "";
				}
				return preceptThingChance.chance;
			});
			array[2] = new TableDataGetter<ThingDef>("IsRangedWeapon", (ThingDef d) => d.IsRangedWeapon.ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("IsMeleeWeapon", (ThingDef d) => d.IsMeleeWeapon.ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("IsWeapon", (ThingDef d) => d.IsWeapon.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("weaponTags", delegate(ThingDef d)
			{
				if (d.weaponTags != null)
				{
					return string.Join(", ", d.weaponTags);
				}
				return "NULL";
			});
			array[6] = new TableDataGetter<ThingDef>("comps", delegate(ThingDef d)
			{
				if (d.comps != null)
				{
					return string.Join(", ", from c in d.comps
					select c.compClass.Name);
				}
				return "NULL";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x0016B8F0 File Offset: 0x00169AF0
		[DebugOutput]
		private static void RelicMarketValues()
		{
			IEnumerable<Tuple<ThingDef, ThingDef>> dataSources = (from td in PreceptDefOf.IdeoRelic.Worker.ThingDefs
			select td.def into td
			where td.category == ThingCategory.Item
			select td).SelectMany((ThingDef td) => from s in GenStuff.AllowedStuffsFor(td, TechLevel.Undefined, false)
			select new Tuple<ThingDef, ThingDef>(td, s));
			TableDataGetter<Tuple<ThingDef, ThingDef>>[] array = new TableDataGetter<Tuple<ThingDef, ThingDef>>[2];
			array[0] = new TableDataGetter<Tuple<ThingDef, ThingDef>>("Relic", (Tuple<ThingDef, ThingDef> d) => d.Item1.defName + "_" + d.Item2.defName);
			array[1] = new TableDataGetter<Tuple<ThingDef, ThingDef>>("market value", (Tuple<ThingDef, ThingDef> d) => d.Item1.GetStatValueAbstract(StatDefOf.MarketValue, d.Item2).ToStringMoney(null));
			DebugTables.MakeTablesDialog<Tuple<ThingDef, ThingDef>>(dataSources, array);
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x0016B9DC File Offset: 0x00169BDC
		[DebugOutput]
		private static void RelicStuffs()
		{
			List<ThingDef> list = (from td in PreceptDefOf.IdeoRelic.Worker.ThingDefs
			select td.def).ToList<ThingDef>();
			HashSet<ThingDef> hashSet = new HashSet<ThingDef>();
			Dictionary<ThingDef, Dictionary<ThingDef, int>> stuffSamples = new Dictionary<ThingDef, Dictionary<ThingDef, int>>();
			foreach (ThingDef thingDef in list)
			{
				Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
				if (thingDef.MadeFromStuff)
				{
					for (int i = 0; i < 1000; i++)
					{
						ThingDef thingDef2 = Precept_Relic.GenerateStuffFor(thingDef, null);
						hashSet.Add(thingDef2);
						int num;
						if (dictionary.TryGetValue(thingDef2, out num))
						{
							dictionary[thingDef2] = num + 1;
						}
						else
						{
							dictionary.Add(thingDef2, 1);
						}
					}
				}
				stuffSamples.Add(thingDef, dictionary);
			}
			List<TableDataGetter<ThingDef>> list2 = new List<TableDataGetter<ThingDef>>();
			list2.Add(new TableDataGetter<ThingDef>("relic", (ThingDef relic) => relic.LabelCap));
			using (IEnumerator<ThingDef> enumerator2 = (from td in DefDatabase<ThingDef>.AllDefsListForReading
			where td.IsStuff
			select td).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ThingDef stuff = enumerator2.Current;
					if (hashSet.Contains(stuff))
					{
						list2.Add(new TableDataGetter<ThingDef>(stuff.LabelCap, delegate(ThingDef relic)
						{
							int num2;
							if (stuffSamples[relic].TryGetValue(stuff, out num2))
							{
								return ((float)stuffSamples[relic][stuff] / 1000f).ToStringPercent();
							}
							return 0f.ToStringPercent();
						}));
					}
				}
			}
			list2.Add(new TableDataGetter<ThingDef>("Made from stuff", (ThingDef relic) => relic.MadeFromStuff));
			list2.Add(new TableDataGetter<ThingDef>("Stuff categories", delegate(ThingDef relic)
			{
				if (relic.stuffCategories != null)
				{
					return string.Join<StuffCategoryDef>(", ", relic.stuffCategories);
				}
				return "NULL";
			}));
			DebugTables.MakeTablesDialog<ThingDef>(list, list2.ToArray());
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x0016BC2C File Offset: 0x00169E2C
		[DebugOutput]
		public static void CollectionsMemoryUsage()
		{
			DebugOutputsMisc.<>c__DisplayClass59_0 CS$<>8__locals1;
			CS$<>8__locals1.collections = new List<ValueTuple<ICollection, string>>();
			CS$<>8__locals1.visited = new HashSet<object>();
			foreach (Type type in GenTypes.AllTypes)
			{
				if (!type.IsGenericType || !type.ContainsGenericParameters)
				{
					foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (typeof(ICollection).IsAssignableFrom(fieldInfo.FieldType))
						{
							ICollection collection = (ICollection)fieldInfo.GetValue(null);
							if (collection != null && CS$<>8__locals1.visited.Add(collection))
							{
								CS$<>8__locals1.collections.Add(new ValueTuple<ICollection, string>(collection, type.Name + "." + fieldInfo.Name + " (static collection)"));
							}
						}
					}
				}
			}
			foreach (Type type2 in GenTypes.AllTypes)
			{
				if ((!type2.IsGenericType || !type2.ContainsGenericParameters) && !typeof(ICollection).IsAssignableFrom(type2))
				{
					foreach (FieldInfo fieldInfo2 in type2.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						object value = fieldInfo2.GetValue(null);
						if (value != null)
						{
							DebugOutputsMisc.<CollectionsMemoryUsage>g__CheckRecursively|59_0(value, true, 0, type2.Name + "." + fieldInfo2.Name, ref CS$<>8__locals1);
						}
					}
				}
			}
			GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
			for (int i = 0; i < array.Length; i++)
			{
				DebugOutputsMisc.<CollectionsMemoryUsage>g__CheckRecursively|59_0(array[i], false, 0, null, ref CS$<>8__locals1);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Collections found: " + CS$<>8__locals1.collections.Count.ToString("#,##0").Replace(',', ' '));
			stringBuilder.AppendLine("Total elements: " + CS$<>8__locals1.collections.Sum((ValueTuple<ICollection, string> x) => x.Item1.Count).ToString("#,##0").Replace(',', ' '));
			ValueTuple<ICollection, string> valueTuple = CS$<>8__locals1.collections.MaxBy((ValueTuple<ICollection, string> x) => x.Item1.Count);
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Max collection size: ",
				valueTuple.Item1.Count.ToString("#,##0").Replace(',', ' '),
				" (",
				valueTuple.Item2,
				")"
			}));
			stringBuilder.AppendLine("30 largest collections:");
			foreach (ValueTuple<ICollection, string> valueTuple2 in (from x in CS$<>8__locals1.collections
			orderby x.Item1.Count descending
			select x).Take(30))
			{
				stringBuilder.AppendLine(string.Concat(new string[]
				{
					"- ",
					valueTuple2.Item2,
					" (",
					valueTuple2.Item1.Count.ToString("#,##0").Replace(',', ' '),
					")"
				}));
			}
			stringBuilder.AppendLine();
			List<ValueTuple<ICollection, string, int>> source = (from x in CS$<>8__locals1.collections
			where x.Item1 != null && x.Item1.GetType().GetProperty("Capacity") != null
			select new ValueTuple<ICollection, string, int>(x.Item1, x.Item2, (int)x.Item1.GetType().GetProperty("Capacity").GetValue(x.Item1))).ToList<ValueTuple<ICollection, string, int>>();
			ValueTuple<ICollection, string, int> valueTuple3 = source.MaxBy((ValueTuple<ICollection, string, int> x) => x.Item3);
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Max collection capacity: ",
				valueTuple3.Item3.ToString("#,##0").Replace(',', ' '),
				" (",
				valueTuple3.Item2,
				")"
			}));
			stringBuilder.AppendLine("30 largest collections by capacity (mostly only Lists define Capacity):");
			foreach (ValueTuple<ICollection, string, int> valueTuple4 in (from x in source
			orderby x.Item3 descending
			select x).Take(30))
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string[] array2 = new string[5];
				array2[0] = "- ";
				array2[1] = valueTuple4.Item2;
				array2[2] = " (";
				int num = 3;
				int i = valueTuple4.Item3;
				array2[num] = i.ToString("#,##0").Replace(',', ' ');
				array2[4] = ")";
				stringBuilder2.AppendLine(string.Concat(array2));
			}
			Log.Message(stringBuilder.ToString());
			if (CS$<>8__locals1.collections.Distinct<ValueTuple<ICollection, string>>().Count<ValueTuple<ICollection, string>>() != CS$<>8__locals1.collections.Count)
			{
				Log.Error("Duplicate collections found. Could be a bug in the algorithm.");
			}
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x0016C1C4 File Offset: 0x0016A3C4
		[DebugOutput]
		public static void Landmarks()
		{
			IEnumerable<LandmarkDef> allDefs = DefDatabase<LandmarkDef>.AllDefs;
			TableDataGetter<LandmarkDef>[] array = new TableDataGetter<LandmarkDef>[4];
			array[0] = new TableDataGetter<LandmarkDef>("defName", (LandmarkDef d) => d.defName);
			array[1] = new TableDataGetter<LandmarkDef>("commonality", (LandmarkDef d) => d.commonality);
			array[2] = new TableDataGetter<LandmarkDef>("mutators", (LandmarkDef d) => (from mc in d.mutatorChances
			orderby mc.chance descending
			select mc.mutator.defName + ((mc.chance < 1f) ? (" (" + mc.chance.ToStringPercent() + ")") : "")).ToCommaList(false, false));
			array[3] = new TableDataGetter<LandmarkDef>("biomes", (LandmarkDef d) => (from biome in DefDatabase<BiomeDef>.AllDefs
			where d.mutatorChances.All((MutatorChance mc) => mc.chance < 1f || mc.mutator.biomeWhitelist == null || mc.mutator.biomeWhitelist.Contains(biome)) && d.mutatorChances.All((MutatorChance mc) => mc.chance < 1f || mc.mutator.biomeBlacklist == null || !mc.mutator.biomeBlacklist.Contains(biome))
			select biome.defName).ToCommaList(false, false));
			DebugTables.MakeTablesDialog<LandmarkDef>(allDefs, array);
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x0016C294 File Offset: 0x0016A494
		[DebugOutput]
		public static void AdjacentDistanceBetweenLayerTiles()
		{
			if (Find.WorldGrid.PlanetLayers.Count < 2)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer from2;
				keyValuePair.Deconstruct(out num, out from2);
				PlanetLayer from = from2;
				list.Add(new DebugMenuOption(from.Def.label, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (KeyValuePair<int, PlanetLayer> keyValuePair2 in Find.WorldGrid.PlanetLayers)
					{
						int num2;
						PlanetLayer to2;
						keyValuePair2.Deconstruct(out num2, out to2);
						PlanetLayer to = to2;
						if (to != from)
						{
							list2.Add(new DebugMenuOption(to.Def.label, DebugMenuOptionMode.Action, delegate()
							{
								float num3 = 0f;
								for (int i = 0; i < 10; i++)
								{
									PlanetTile tile = from.Tiles.RandomElement<Tile>().tile;
									PlanetTile tileNeighbor = from.GetTileNeighbor(tile, 0);
									PlanetTile closestTile = to.GetClosestTile(tile);
									PlanetTile closestTile2 = to.GetClosestTile(tileNeighbor);
									num3 += to.ApproxDistanceInTiles(closestTile, closestTile2);
								}
								num3 /= 10f;
								Log.Message(string.Format("Approximate average distance between adjacent tiles on {0} to {1}: {2:0.###}", from.Def.label, to.Def.label, num3));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2, "Select destination layer"));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, "Select origin layer"));
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x0016C350 File Offset: 0x0016A550
		[DebugOutput]
		public static void LogAnyPlayerHomeMap()
		{
			Log.Message(Find.AnyPlayerHomeMap);
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x0016C35C File Offset: 0x0016A55C
		[DebugOutput]
		public static void GravshipBuildingChecklist()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[5];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("rotatable", delegate(ThingDef d)
			{
				if (!d.rotatable)
				{
					return " ";
				}
				return "✔";
			});
			array[2] = new TableDataGetter<ThingDef>("size", (ThingDef d) => d.size);
			array[3] = new TableDataGetter<ThingDef>("interaction", delegate(ThingDef d)
			{
				if (!d.hasInteractionCell)
				{
					return " ";
				}
				return "✔";
			});
			array[4] = new TableDataGetter<ThingDef>("valid for gravship", delegate(ThingDef d)
			{
				if ((d.size.x != d.size.z && !d.rotatable) || (!d.rotatable && d.hasInteractionCell))
				{
					return " ";
				}
				return "✔";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x0016C47C File Offset: 0x0016A67C
		[CompilerGenerated]
		internal static string <DrawerTypes>g__GetDrawUsers|0_0(ThingDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CompProperties compProperties in def.comps)
			{
				MethodInfo method = compProperties.compClass.GetMethod("PostDraw");
				if (method.DeclaringType != typeof(ThingComp))
				{
					stringBuilder.AppendLine(method.DeclaringType.Name);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines().Replace(Environment.NewLine, ",");
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x0016C520 File Offset: 0x0016A720
		[CompilerGenerated]
		internal static bool <MeditationFoci>g__TryGetOffset|6_0(ThingDef d, int index, out FocusStrengthOffset result)
		{
			result = null;
			CompProperties_MeditationFocus compProperties = d.GetCompProperties<CompProperties_MeditationFocus>();
			if (compProperties == null)
			{
				return false;
			}
			if (compProperties.offsets.Count <= index)
			{
				return false;
			}
			result = compProperties.offsets[index];
			return true;
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x0016C55C File Offset: 0x0016A75C
		[CompilerGenerated]
		internal static string <MeditationFoci>g__GetOffsetClassName|6_1(ThingDef d, int index)
		{
			FocusStrengthOffset focusStrengthOffset;
			if (!DebugOutputsMisc.<MeditationFoci>g__TryGetOffset|6_0(d, index, out focusStrengthOffset))
			{
				return "";
			}
			return focusStrengthOffset.GetType().Name;
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x0016C588 File Offset: 0x0016A788
		[CompilerGenerated]
		internal static float <MeditationFoci>g__GetOffsetMax|6_2(ThingDef d, int index)
		{
			FocusStrengthOffset focusStrengthOffset;
			if (!DebugOutputsMisc.<MeditationFoci>g__TryGetOffset|6_0(d, index, out focusStrengthOffset))
			{
				return 0f;
			}
			return Mathf.Max(focusStrengthOffset.MaxOffset(null), 0f);
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x0016C5B8 File Offset: 0x0016A7B8
		[CompilerGenerated]
		internal static float <MeditationFoci>g__TotalMax|6_3(ThingDef d)
		{
			float num = d.GetStatValueAbstract(StatDefOf.MeditationFocusStrength, null);
			for (int i = 0; i < 5; i++)
			{
				num += DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|6_2(d, i);
			}
			return num;
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x0016C5EC File Offset: 0x0016A7EC
		[CompilerGenerated]
		internal static float <StuffBeauty>g__getStatFactorVal|9_0(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statFactors == null)
			{
				return 0f;
			}
			StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return 0f;
			}
			return statModifier.value;
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x0016C648 File Offset: 0x0016A848
		[CompilerGenerated]
		internal static float <StuffBeauty>g__getStatOffsetVal|9_1(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statOffsets == null)
			{
				return 0f;
			}
			StatModifier statModifier = d.stuffProps.statOffsets.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return 0f;
			}
			return statModifier.value;
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x0016C6A4 File Offset: 0x0016A8A4
		[CompilerGenerated]
		internal static string <Stuffs>g__getStatFactorString|13_0(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statFactors == null)
			{
				return "";
			}
			StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return "";
			}
			return stat.ValueToString(statModifier.value, ToStringNumberSense.Absolute, true);
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x0016C70C File Offset: 0x0016A90C
		[CompilerGenerated]
		internal static float <Stuffs>g__meleeDpsSharpFactorOverall|13_1(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x0016C740 File Offset: 0x0016A940
		[CompilerGenerated]
		internal static float <Stuffs>g__meleeDpsBluntFactorOverall|13_2(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x0016C774 File Offset: 0x0016A974
		[CompilerGenerated]
		internal static void <CollectionsMemoryUsage>g__CheckRecursively|59_0(object obj, bool fromStatic, int depth = 0, string cameFrom = null, ref DebugOutputsMisc.<>c__DisplayClass59_0 A_4)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is Type || obj is Pointer || obj is IntPtr || obj is UIntPtr || obj is ICollection)
			{
				return;
			}
			if (!A_4.visited.Add(obj))
			{
				return;
			}
			foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (typeof(ICollection).IsAssignableFrom(fieldInfo.FieldType))
				{
					ICollection collection = (ICollection)fieldInfo.GetValue(obj);
					if (collection != null)
					{
						if (!A_4.visited.Add(collection))
						{
							return;
						}
						string str;
						if (obj.GetType().Name.Contains("HashSet"))
						{
							str = (cameFrom ?? "HashSet");
						}
						else
						{
							str = obj.GetType().Name + "." + fieldInfo.Name;
						}
						A_4.collections.Add(new ValueTuple<ICollection, string>(collection, str + (fromStatic ? " (from static object)" : " (from instance)")));
					}
				}
				else
				{
					object value = fieldInfo.GetValue(obj);
					if (value != null)
					{
						DebugOutputsMisc.<CollectionsMemoryUsage>g__CheckRecursively|59_0(value, fromStatic, depth + 1, obj.GetType().Name + "." + fieldInfo.Name, ref A_4);
					}
				}
			}
		}
	}
}
