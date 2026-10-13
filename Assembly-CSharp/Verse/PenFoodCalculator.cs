using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003BF RID: 959
	public class PenFoodCalculator
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x0007C1EA File Offset: 0x0007A3EA
		public float NutritionPerDayToday
		{
			get
			{
				return this.sumNutritionPerDayToday;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x0007C1F2 File Offset: 0x0007A3F2
		public List<PenFoodCalculator.PenAnimalInfo> ActualAnimalInfos
		{
			get
			{
				return this.animals;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x0007C1FA File Offset: 0x0007A3FA
		public List<PenFoodCalculator.PenFoodItemInfo> AllStockpiledInfos
		{
			get
			{
				return this.stockpiled;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001880 RID: 6272 RVA: 0x0007C202 File Offset: 0x0007A402
		public bool Unenclosed
		{
			get
			{
				return this.numCells == 0;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x0007C210 File Offset: 0x0007A410
		public float SumNutritionConsumptionPerDay
		{
			get
			{
				float num = 0f;
				foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
				{
					num += penAnimalInfo.TotalNutritionConsumptionPerDay;
				}
				return num;
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0007C26C File Offset: 0x0007A46C
		public PenFoodCalculator.PenAnimalInfo GetAnimalInfo(ThingDef animalDef)
		{
			foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
			{
				if (penAnimalInfo.animalDef == animalDef)
				{
					return penAnimalInfo;
				}
			}
			PenFoodCalculator.PenAnimalInfo penAnimalInfo2 = new PenFoodCalculator.PenAnimalInfo(animalDef);
			this.animals.Add(penAnimalInfo2);
			return penAnimalInfo2;
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0007C2DC File Offset: 0x0007A4DC
		public PenFoodCalculator.PenFoodItemInfo GetStockpiledInfo(ThingDef itemDef)
		{
			foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
			{
				if (penFoodItemInfo.itemDef == itemDef)
				{
					return penFoodItemInfo;
				}
			}
			PenFoodCalculator.PenFoodItemInfo penFoodItemInfo2 = new PenFoodCalculator.PenFoodItemInfo(itemDef);
			this.stockpiled.Add(penFoodItemInfo2);
			return penFoodItemInfo2;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0007C34C File Offset: 0x0007A54C
		public string NaturalGrowthRateTooltip()
		{
			if (this.cachedNaturalGrowthRateTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateDescription".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateSeasonal".Translate());
				stringBuilder.AppendLine();
				stringBuilder.Append("PenFoodTab_GrowthPerSeason".Translate()).AppendLine(":");
				Vector2 vector = Find.WorldGrid.LongLatOf(this.mapCalc.map.Tile);
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum = (Quadrum)i;
					stringBuilder.Append("- ").Append(quadrum.Label()).Append(" (").Append(quadrum.GetSeason(vector.y).Label()).Append("): ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionPerDayToString(this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum), false));
				}
				this.cachedNaturalGrowthRateTooltip = stringBuilder.ToString();
			}
			return this.cachedNaturalGrowthRateTooltip;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0007C45C File Offset: 0x0007A65C
		public string TotalConsumedToolTop()
		{
			if (this.cachedTotalConsumedTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NutritionConsumptionDescription".Translate());
				this.cachedTotalConsumedTooltip = stringBuilder.ToString();
			}
			return this.cachedTotalConsumedTooltip;
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0007C4A0 File Offset: 0x0007A6A0
		public string StockpileToolTip()
		{
			if (this.cachedStockpileTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_StockpileTotalDescription".Translate());
				stringBuilder.AppendLine();
				foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
				{
					stringBuilder.Append("- ").Append(penFoodItemInfo.itemDef.LabelCap).Append(" x").Append(penFoodItemInfo.totalCount).Append(": ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionToString(penFoodItemInfo.totalNutritionAvailable, true));
				}
				this.cachedStockpileTooltip = stringBuilder.ToString();
			}
			return this.cachedStockpileTooltip;
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0007C580 File Offset: 0x0007A780
		private void Reset(Map map)
		{
			this.mapCalc.Reset(map);
			this.animals.Clear();
			this.stockpiled.Clear();
			this.cachedNaturalGrowthRateTooltip = null;
			this.cachedTotalConsumedTooltip = null;
			this.cachedStockpileTooltip = null;
			this.nutritionPerDayPerQuadrum = default(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum);
			this.sumNutritionPerDayToday = 0f;
			this.sumStockpiledNutritionAvailableNow = 0f;
			this.numCells = 0;
			this.numCellsSoil = 0;
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0007C5F4 File Offset: 0x0007A7F4
		public List<PenFoodCalculator.PenAnimalInfo> ComputeExampleAnimals(List<ThingDef> animalDefs)
		{
			this.tmpAddedExampleAnimals.Clear();
			foreach (ThingDef thingDef in animalDefs)
			{
				LifeStageAge lifeStageAge = thingDef.race.lifeStageAges.Last<LifeStageAge>();
				PenFoodCalculator.PenAnimalInfo penAnimalInfo = new PenFoodCalculator.PenAnimalInfo(thingDef);
				penAnimalInfo.example = true;
				penAnimalInfo.nutritionConsumptionPerDay = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(thingDef, lifeStageAge.def);
				this.tmpAddedExampleAnimals.Add(penAnimalInfo);
			}
			PenFoodCalculator.SortAnimals(this.tmpAddedExampleAnimals);
			return this.tmpAddedExampleAnimals;
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0007C694 File Offset: 0x0007A894
		public void ResetAndProcessPen(CompAnimalPenMarker marker)
		{
			this.ResetAndProcessPen(marker.parent.Position, marker.parent.Map, false);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0007C6B3 File Offset: 0x0007A8B3
		public void ResetAndProcessPen(IntVec3 position, Map map, bool considerBlueprints)
		{
			this.Reset(map);
			if (map == null)
			{
				return;
			}
			if (considerBlueprints)
			{
				this.ProcessBlueprintPen(position, map);
			}
			else
			{
				this.ProcessRealPen(position, map);
			}
			this.SortResults(map);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0007C6DC File Offset: 0x0007A8DC
		private void SortResults(Map map)
		{
			this.stockpiled.Sort(new Comparison<PenFoodCalculator.PenFoodItemInfo>(PenFoodCalculator.<SortResults>g__FoodSorter|36_0));
			PenFoodCalculator.SortAnimals(this.animals);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0007C700 File Offset: 0x0007A900
		private static void SortAnimals(List<PenFoodCalculator.PenAnimalInfo> infos)
		{
			infos.Sort(new Comparison<PenFoodCalculator.PenAnimalInfo>(PenFoodCalculator.<SortAnimals>g__AnimalSorter|37_0));
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0007C714 File Offset: 0x0007A914
		private void ProcessBlueprintPen(IntVec3 markerPos, Map map)
		{
			this.blueprintEnclosureCalc.VisitPen(markerPos, map);
			if (!this.blueprintEnclosureCalc.isEnclosed)
			{
				return;
			}
			foreach (IntVec3 c in this.blueprintEnclosureCalc.cellsFound)
			{
				this.ProcessCell(c, map);
			}
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x0007C788 File Offset: 0x0007A988
		private void ProcessRealPen(IntVec3 markerPos, Map map)
		{
			using (ProfilerBlock.Scope("PenFoodCalculator.ProcessRealPen"))
			{
				foreach (District district in this.connectedDistrictsCalc.CalculateConnectedDistricts(markerPos, map))
				{
					foreach (Region region in district.Regions)
					{
						this.ProcessRegion(region);
					}
				}
				this.connectedDistrictsCalc.Reset();
			}
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0007C850 File Offset: 0x0007AA50
		private void ProcessRegion(Region region)
		{
			using (ProfilerBlock.Scope("PenFoodCalculator.ProcessRegion"))
			{
				foreach (IntVec3 c in region.Cells)
				{
					this.ProcessCell(c, region.Map);
				}
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0007C8C8 File Offset: 0x0007AAC8
		private void ProcessCell(IntVec3 c, Map map)
		{
			using (ProfilerBlock.Scope("ProcessTerrain"))
			{
				this.ProcessTerrain(c, map);
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.IsAnimal)
				{
					this.ProcessAnimal(pawn);
				}
				else if (thing.def.category == ThingCategory.Item && thing.IngestibleNow && MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thing.def))
				{
					this.ProcessStockpiledFood(thing);
				}
			}
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0007C98C File Offset: 0x0007AB8C
		private void ProcessTerrain(IntVec3 c, Map map)
		{
			this.numCells++;
			if (c.GetEdifice(map) != null)
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(map);
			if (terrain.IsSoil)
			{
				this.numCellsSoil++;
			}
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.mapCalc.CalculateAverageNutritionPerDay(terrain);
			this.nutritionPerDayPerQuadrum.AddFrom(other);
			this.sumNutritionPerDayToday += this.mapCalc.GetAverageNutritionPerDayToday(terrain);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0007CA04 File Offset: 0x0007AC04
		private void ProcessStockpiledFood(Thing thing)
		{
			PenFoodCalculator.PenFoodItemInfo stockpiledInfo = this.GetStockpiledInfo(thing.def);
			float num = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount;
			stockpiledInfo.totalCount += thing.stackCount;
			stockpiledInfo.totalNutritionAvailable += num;
			this.sumStockpiledNutritionAvailableNow += num;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0007CA64 File Offset: 0x0007AC64
		private void ProcessAnimal(Pawn animal)
		{
			if (!MapPlantGrowthRateCalculator.IsPastureAnimal(animal.def) || !animal.Spawned)
			{
				return;
			}
			PenFoodCalculator.PenAnimalInfo animalInfo = this.GetAnimalInfo(animal.def);
			animalInfo.count++;
			animalInfo.nutritionConsumptionPerDay += SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0007CAB4 File Offset: 0x0007ACB4
		public static string NutritionToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_Nutrition_Unit".Translate();
			}
			return text;
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0007CAF0 File Offset: 0x0007ACF0
		public static string NutritionPerDayToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_NutritionPerDay_Unit".Translate();
			}
			return text;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x0007CB2A File Offset: 0x0007AD2A
		public float CapacityOf(Quadrum q, ThingDef animal)
		{
			return this.nutritionPerDayPerQuadrum.ForQuadrum(q) / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0007CB40 File Offset: 0x0007AD40
		public Quadrum GetSummerOrBestQuadrum()
		{
			Vector2 location = Find.WorldGrid.LongLatOf(this.mapCalc.map.Tile);
			Quadrum? quadrum = null;
			float num = 0f;
			foreach (Quadrum quadrum2 in QuadrumUtility.Quadrums)
			{
				if (quadrum2.GetSeason(location) == Season.Summer)
				{
					return quadrum2;
				}
				float num2 = this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum2);
				if (quadrum == null || num2 > num)
				{
					quadrum = new Quadrum?(quadrum2);
					num = num2;
				}
			}
			return quadrum.Value;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0007CBF8 File Offset: 0x0007ADF8
		public string PenSizeDescription()
		{
			string result;
			if (this.Unenclosed)
			{
				result = "PenSizeDesc_Unenclosed".Translate();
			}
			else if (this.numCellsSoil < 50)
			{
				result = "PenSizeDesc_VerySmall".Translate();
			}
			else if (this.numCellsSoil < 100)
			{
				result = "PenSizeDesc_Small".Translate();
			}
			else if (this.numCellsSoil < 400)
			{
				result = "PenSizeDesc_Medium".Translate();
			}
			else
			{
				result = "PenSizeDesc_Large".Translate();
			}
			return result;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0007CCDD File Offset: 0x0007AEDD
		[CompilerGenerated]
		internal static int <SortResults>g__FoodSorter|36_0(PenFoodCalculator.PenFoodItemInfo a, PenFoodCalculator.PenFoodItemInfo b)
		{
			return -1 * a.totalNutritionAvailable.CompareTo(b.totalNutritionAvailable);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0007CCF4 File Offset: 0x0007AEF4
		[CompilerGenerated]
		internal static int <SortAnimals>g__AnimalSorter|37_0(PenFoodCalculator.PenAnimalInfo a, PenFoodCalculator.PenAnimalInfo b)
		{
			return -1 * a.TotalNutritionConsumptionPerDay.CompareTo(b.TotalNutritionConsumptionPerDay);
		}

		// Token: 0x040015C9 RID: 5577
		public const ToStringStyle NutritionStringStyle = ToStringStyle.FloatMaxTwo;

		// Token: 0x040015CA RID: 5578
		private readonly AnimalPenConnectedDistrictsCalculator connectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();

		// Token: 0x040015CB RID: 5579
		private readonly AnimalPenBlueprintEnclosureCalculator blueprintEnclosureCalc = new AnimalPenBlueprintEnclosureCalculator();

		// Token: 0x040015CC RID: 5580
		private readonly MapPastureNutritionCalculator mapCalc = new MapPastureNutritionCalculator();

		// Token: 0x040015CD RID: 5581
		private readonly List<PenFoodCalculator.PenAnimalInfo> animals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x040015CE RID: 5582
		private readonly List<PenFoodCalculator.PenFoodItemInfo> stockpiled = new List<PenFoodCalculator.PenFoodItemInfo>();

		// Token: 0x040015CF RID: 5583
		private string cachedNaturalGrowthRateTooltip;

		// Token: 0x040015D0 RID: 5584
		private string cachedTotalConsumedTooltip;

		// Token: 0x040015D1 RID: 5585
		private string cachedStockpileTooltip;

		// Token: 0x040015D2 RID: 5586
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;

		// Token: 0x040015D3 RID: 5587
		public float sumStockpiledNutritionAvailableNow;

		// Token: 0x040015D4 RID: 5588
		public int numCells;

		// Token: 0x040015D5 RID: 5589
		public int numCellsSoil;

		// Token: 0x040015D6 RID: 5590
		private float sumNutritionPerDayToday;

		// Token: 0x040015D7 RID: 5591
		private readonly List<PenFoodCalculator.PenAnimalInfo> tmpAddedExampleAnimals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x020003C0 RID: 960
		public class PenAnimalInfo
		{
			// Token: 0x17000475 RID: 1141
			// (get) Token: 0x0600189C RID: 6300 RVA: 0x0007CD17 File Offset: 0x0007AF17
			public int TotalCount
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x17000476 RID: 1142
			// (get) Token: 0x0600189D RID: 6301 RVA: 0x0007CD1F File Offset: 0x0007AF1F
			public float TotalNutritionConsumptionPerDay
			{
				get
				{
					return this.nutritionConsumptionPerDay;
				}
			}

			// Token: 0x0600189E RID: 6302 RVA: 0x0007CD27 File Offset: 0x0007AF27
			public PenAnimalInfo(ThingDef animalDef)
			{
				this.animalDef = animalDef;
			}

			// Token: 0x0600189F RID: 6303 RVA: 0x0007CD38 File Offset: 0x0007AF38
			public string ToolTip(PenFoodCalculator calc)
			{
				if (this.cachedToolTip == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					int value = Mathf.FloorToInt(calc.NutritionPerDayToday / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef));
					stringBuilder.Append("PenFoodTab_AnimalTypeAnimalCapacity".Translate()).Append(": ").Append(value).AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("PenFoodTab_NutritionConsumedPerDay".Translate(this.animalDef.Named("ANIMALDEF"))).AppendLine(":");
					List<LifeStageAge> lifeStageAges = this.animalDef.race.lifeStageAges;
					for (int i = 0; i < lifeStageAges.Count; i++)
					{
						LifeStageDef def = lifeStageAges[i].def;
						float value2 = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef, def);
						stringBuilder.Append("- ").Append(def.LabelCap).Append(": ").AppendLine(PenFoodCalculator.NutritionPerDayToString(value2, false));
					}
					this.cachedToolTip = stringBuilder.ToString();
				}
				return this.cachedToolTip;
			}

			// Token: 0x040015D8 RID: 5592
			public ThingDef animalDef;

			// Token: 0x040015D9 RID: 5593
			public bool example;

			// Token: 0x040015DA RID: 5594
			public int count;

			// Token: 0x040015DB RID: 5595
			public float nutritionConsumptionPerDay;

			// Token: 0x040015DC RID: 5596
			private string cachedToolTip;
		}

		// Token: 0x020003C1 RID: 961
		public class PenFoodItemInfo
		{
			// Token: 0x060018A0 RID: 6304 RVA: 0x0007CE55 File Offset: 0x0007B055
			public PenFoodItemInfo(ThingDef itemDef)
			{
				this.itemDef = itemDef;
			}

			// Token: 0x040015DD RID: 5597
			public ThingDef itemDef;

			// Token: 0x040015DE RID: 5598
			public int totalCount;

			// Token: 0x040015DF RID: 5599
			public float totalNutritionAvailable;
		}
	}
}
