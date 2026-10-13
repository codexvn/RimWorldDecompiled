using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;

namespace Verse
{
	// Token: 0x02000A6F RID: 2671
	public static class DebugOutputsEcology
	{
		// Token: 0x0600420A RID: 16906 RVA: 0x00157BA4 File Offset: 0x00155DA4
		[DebugOutput]
		public static void Plants()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[12];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("grow days", (ThingDef d) => d.plant.growDays.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => DebugOutputsEcology.<Plants>g__Nutrition|0_0(d).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("nutrition\n/day", (ThingDef d) => (DebugOutputsEcology.<Plants>g__Nutrition|0_0(d) / d.plant.growDays).ToString("F4"));
			array[4] = new TableDataGetter<ThingDef>("fertility\nmin", (ThingDef d) => d.plant.fertilityMin.ToString("F2"));
			array[5] = new TableDataGetter<ThingDef>("fertility\nsensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("harvest\nnutrition", (ThingDef d) => DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d).ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("nutrition\n/ harvest nutrition", delegate(ThingDef d)
			{
				if (DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d) > 0f)
				{
					return (DebugOutputsEcology.<Plants>g__Nutrition|0_0(d) / DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d)).ToString("F2");
				}
				return "";
			});
			array[8] = new TableDataGetter<ThingDef>("tree", delegate(ThingDef d)
			{
				if (!d.plant.IsTree)
				{
					return "";
				}
				return "yes";
			});
			array[9] = new TableDataGetter<ThingDef>("farm animal edible", delegate(ThingDef d)
			{
				if (!MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(d))
				{
					return "";
				}
				return "yes";
			});
			array[10] = new TableDataGetter<ThingDef>("minifiable", (ThingDef d) => d.Minifiable);
			array[11] = new TableDataGetter<ThingDef>("treelovers\ncare", (ThingDef d) => d.plant.treeLoversCareIfChopped);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x00157E1D File Offset: 0x0015601D
		[DebugOutput(true)]
		public static void PlantCurrentProportions()
		{
			PlantUtility.LogPlantProportions();
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x00157E24 File Offset: 0x00156024
		[DebugOutput]
		public static void Biomes()
		{
			IEnumerable<BiomeDef> dataSources = from d in DefDatabase<BiomeDef>.AllDefs
			orderby d.plantDensity descending
			select d;
			TableDataGetter<BiomeDef>[] array = new TableDataGetter<BiomeDef>[12];
			array[0] = new TableDataGetter<BiomeDef>("defName", (BiomeDef d) => d.defName);
			array[1] = new TableDataGetter<BiomeDef>("animalDensity", (BiomeDef d) => d.animalDensity.ToString("F2"));
			array[2] = new TableDataGetter<BiomeDef>("plantDensity", (BiomeDef d) => d.plantDensity.ToString("F2"));
			array[3] = new TableDataGetter<BiomeDef>("tree density", (BiomeDef d) => d.TreeDensity.ToStringPercent());
			array[4] = new TableDataGetter<BiomeDef>("tree sightings\nper hour", (BiomeDef d) => d.TreeSightingsPerHourFromCaravan);
			array[5] = new TableDataGetter<BiomeDef>("diseaseMtbDays", (BiomeDef d) => d.diseaseMtbDays.ToString("F0"));
			array[6] = new TableDataGetter<BiomeDef>("movementDifficulty", delegate(BiomeDef d)
			{
				if (!d.impassable)
				{
					return d.movementDifficulty.ToString("F1");
				}
				return "-";
			});
			array[7] = new TableDataGetter<BiomeDef>("forageability", (BiomeDef d) => d.forageability.ToStringPercent());
			array[8] = new TableDataGetter<BiomeDef>("forageFood", delegate(BiomeDef d)
			{
				if (d.foragedFood == null)
				{
					return "";
				}
				return d.foragedFood.label;
			});
			array[9] = new TableDataGetter<BiomeDef>("forageable plants", (BiomeDef d) => (from pd in d.AllWildPlants
			where pd.plant.harvestedThingDef != null && pd.plant.harvestedThingDef.IsNutritionGivingIngestible
			select pd.defName).ToCommaList(false, false));
			array[10] = new TableDataGetter<BiomeDef>("wildPlantRegrowDays", (BiomeDef d) => d.wildPlantRegrowDays.ToString("F0"));
			array[11] = new TableDataGetter<BiomeDef>("wildPlantsCareAboutLocalFertility", (BiomeDef d) => d.wildPlantsCareAboutLocalFertility.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<BiomeDef>(dataSources, array);
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x00158079 File Offset: 0x00156279
		[DebugOutput]
		public static void BiomeAnimalsSpawnChances()
		{
			DebugOutputsEcology.BiomeAnimalsInternal(delegate(PawnKindDef k, BiomeDef b)
			{
				float num = b.CommonalityOfAnimal(k);
				if (num == 0f)
				{
					return "";
				}
				return (num / DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki))).ToStringPercent("F1");
			});
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x0015809F File Offset: 0x0015629F
		[DebugOutput]
		public static void BiomeAnimalsTypicalCounts()
		{
			DebugOutputsEcology.BiomeAnimalsInternal((PawnKindDef k, BiomeDef b) => DebugOutputsEcology.ExpectedAnimalCount(k, b, null).ToStringEmptyZero("F2"));
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x001580C8 File Offset: 0x001562C8
		private static float ExpectedAnimalCount(PawnKindDef k, BiomeDef b, Map m)
		{
			float num = b.CommonalityOfAnimal(k);
			if (m != null && m.TileInfo.IsCoastal)
			{
				num += b.CommonalityOfCoastalAnimal(k);
			}
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
			float num3 = num / num2;
			float num4 = 10000f / b.animalDensity;
			float num5 = 62500f / num4;
			float totalCommonality = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
			float num6 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => k.ecoSystemWeight * (b.CommonalityOfAnimal(ki) / totalCommonality));
			return num5 / num6 * num3;
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x0015819C File Offset: 0x0015639C
		private static void BiomeAnimalsInternal(Func<PawnKindDef, BiomeDef, string> densityInBiomeOutputter)
		{
			List<TableDataGetter<PawnKindDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.animalDensity
			select new TableDataGetter<PawnKindDef>(b.defName, (PawnKindDef k) => densityInBiomeOutputter(k, b))).ToList<TableDataGetter<PawnKindDef>>();
			list.Insert(0, new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName + (k.race.race.predator ? " (P)" : "")));
			DebugTables.MakeTablesDialog<PawnKindDef>(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x001582A4 File Offset: 0x001564A4
		[DebugOutput]
		public static void BiomePlantsExpectedCount()
		{
			Func<ThingDef, BiomeDef, string> expectedCountInBiomeOutputter = (ThingDef p, BiomeDef b) => (b.CommonalityOfPlant(p) * b.plantDensity * 4000f).ToString("F0");
			List<TableDataGetter<ThingDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.plantDensity
			select new TableDataGetter<ThingDef>(b.defName + " (" + b.plantDensity.ToString("F2") + ")", (ThingDef k) => expectedCountInBiomeOutputter(k, b))).ToList<TableDataGetter<ThingDef>>();
			list.Insert(0, new TableDataGetter<ThingDef>("plant", (ThingDef k) => k.defName));
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x001583C8 File Offset: 0x001565C8
		[DebugOutput]
		public static void AnimalWildCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<PawnKindDef> dataSources = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.race != null && k.RaceProps.Animal && DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome, map) > 0f
			orderby DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome, map) descending
			select k;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[3];
			array[0] = new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("expected count on map (inaccurate)", (PawnKindDef k) => DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome, map).ToString("F2"));
			array[2] = new TableDataGetter<PawnKindDef>("actual count on map", (PawnKindDef k) => map.mapPawns.AllPawnsSpawned.Count((Pawn p) => p.kindDef == k).ToString());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x00158478 File Offset: 0x00156678
		[DebugOutput]
		public static void PlantCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<ThingDef> dataSources = from p in DefDatabase<ThingDef>.AllDefs
			where p.category == ThingCategory.Plant && map.Biome.CommonalityOfPlant(p) > 0f
			orderby map.Biome.CommonalityOfPlant(p) descending
			select p;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("plant", (ThingDef p) => p.defName);
			array[1] = new TableDataGetter<ThingDef>("biome-defined commonality", (ThingDef p) => map.Biome.CommonalityOfPlant(p).ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("expected count (rough)", (ThingDef p) => (map.Biome.CommonalityOfPlant(p) * map.Biome.plantDensity * 4000f).ToString("F0"));
			array[3] = new TableDataGetter<ThingDef>("actual count on map", (ThingDef p) => map.AllCells.Count((IntVec3 c) => c.GetPlant(map) != null && c.GetPlant(map).def == p).ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x0015853F File Offset: 0x0015673F
		[CompilerGenerated]
		internal static float <Plants>g__Nutrition|0_0(ThingDef d)
		{
			if (d.ingestible == null)
			{
				return 0f;
			}
			return d.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x0015855B File Offset: 0x0015675B
		[CompilerGenerated]
		internal static float <Plants>g__HarvestNutrition|0_1(ThingDef d)
		{
			if (d.plant.harvestedThingDef == null)
			{
				return 0f;
			}
			return d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}
	}
}
