using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000188 RID: 392
	public class WaterBody : IExposable
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x000352AA File Offset: 0x000334AA
		public bool HasFish
		{
			get
			{
				return (this.commonFish.Count > 0 || this.uncommonFish.Count > 0) && this.population > 0f;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x000352D7 File Offset: 0x000334D7
		public float PollutionPct
		{
			get
			{
				return (float)this.numCellsPolluted / (float)this.cellCount;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x000352E8 File Offset: 0x000334E8
		public int Size
		{
			get
			{
				return this.cellCount;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x000352F0 File Offset: 0x000334F0
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x000352F8 File Offset: 0x000334F8
		public float Population
		{
			get
			{
				return this.population;
			}
			set
			{
				this.population = value;
				this.population = Mathf.Clamp(this.population, 0f, this.MaxPopulation);
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0003531D File Offset: 0x0003351D
		public float MaxPopulation
		{
			get
			{
				return Mathf.Max(this.map.TileInfo.MaxFishPopulation * this.PopulationFactor, 20f);
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00035340 File Offset: 0x00033540
		public float PopulationFactor
		{
			get
			{
				if (this.cachedPopulationFactor < 0f)
				{
					this.cachedPopulationFactor = FishingUtility.FishPopulationFactorPerBodySizeCurve.Evaluate((float)this.Size);
					if (this.cachedPopulationFactor < 0f)
					{
						this.cachedPopulationFactor = 0f;
					}
				}
				return this.cachedPopulationFactor;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x000352E8 File Offset: 0x000334E8
		// (set) Token: 0x06000B1F RID: 2847 RVA: 0x0003538F File Offset: 0x0003358F
		public int CellCount
		{
			get
			{
				return this.cellCount;
			}
			set
			{
				this.cellCount = value;
				this.cachedPopulationFactor = -1f;
				this.cachedBounds = null;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x000353B0 File Offset: 0x000335B0
		public CellRect Bounds
		{
			get
			{
				CellRect value = this.cachedBounds.GetValueOrDefault();
				if (this.cachedBounds == null)
				{
					value = CellRect.FromCellList(this.cells);
					this.cachedBounds = new CellRect?(value);
				}
				return this.cachedBounds.Value;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x000353F9 File Offset: 0x000335F9
		public IEnumerable<ThingDef> CommonFish
		{
			get
			{
				return this.commonFish;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00035401 File Offset: 0x00033601
		public IEnumerable<ThingDef> UncommonFish
		{
			get
			{
				return this.uncommonFish;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x00035409 File Offset: 0x00033609
		public IEnumerable<ThingDef> CommonFishIncludingExtras
		{
			get
			{
				if (this.commonFish.NullOrEmpty<ThingDef>())
				{
					yield break;
				}
				foreach (ThingDef thingDef in this.commonFish)
				{
					yield return thingDef;
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				if (ModsConfig.BiotechActive && FishingUtility.PollutionToxfishChanceCurve.Evaluate(this.PollutionPct) > 0f)
				{
					yield return ThingDefOf.Fish_Toxfish;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00035419 File Offset: 0x00033619
		public WaterBody()
		{
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00035450 File Offset: 0x00033650
		public WaterBody(Map map, IntVec3 rootCell)
		{
			this.map = map;
			this.rootCell = rootCell;
			this.cellCount = 1;
			this.waterBodyType = rootCell.GetWaterBodyType(map);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x000354B1 File Offset: 0x000336B1
		public void Initialize()
		{
			this.InitializePopulation();
			this.SetFishTypes();
			this.RecacheState();
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x000354C5 File Offset: 0x000336C5
		public void InitializePopulation()
		{
			this.population = this.MaxPopulation;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000354D4 File Offset: 0x000336D4
		public void SetFishTypes()
		{
			if (this.map.Biome.fishTypes == null)
			{
				return;
			}
			this.commonFish.Clear();
			this.uncommonFish.Clear();
			this.shouldHaveFish = Rand.Chance(FishingUtility.ChanceForCommonFishFromWaterBodySizeCurve.Evaluate((float)this.Size));
			if (!this.shouldHaveFish)
			{
				return;
			}
			bool flag = Rand.Chance(FishingUtility.ChanceForUncommonFishFromWaterBodySizeCurve.Evaluate((float)this.Size));
			WaterBodyType waterBodyType = this.waterBodyType;
			if (waterBodyType != WaterBodyType.Freshwater)
			{
				if (waterBodyType != WaterBodyType.Saltwater)
				{
					return;
				}
				FishChance fishChance;
				if (this.map.Biome.fishTypes.saltwater_Common.TryRandomElement(out fishChance))
				{
					this.commonFish.Add(fishChance.fishDef);
				}
				FishChance fishChance2;
				if (flag && this.map.Biome.fishTypes.saltwater_Uncommon.TryRandomElement(out fishChance2))
				{
					this.uncommonFish.Add(fishChance2.fishDef);
				}
			}
			else
			{
				FishChance fishChance3;
				if (this.map.Biome.fishTypes.freshwater_Common.TryRandomElement(out fishChance3))
				{
					this.commonFish.Add(fishChance3.fishDef);
				}
				FishChance fishChance4;
				if (flag && this.map.Biome.fishTypes.freshwater_Uncommon.TryRandomElement(out fishChance4))
				{
					this.uncommonFish.Add(fishChance4.fishDef);
					return;
				}
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00035620 File Offset: 0x00033820
		public void RecacheState()
		{
			this.numCellsPolluted = 0;
			this.numCellsFrozen = 0;
			foreach (IntVec3 c in this.cells)
			{
				if (!FreezeManager.CanEverFreeze(c, this.map))
				{
					this.numCellsFrozen++;
				}
				if (c.IsPolluted(this.map))
				{
					this.numCellsPolluted++;
				}
			}
			if (this.shouldHaveFish && this.commonFish.NullOrEmpty<ThingDef>())
			{
				this.SetFishTypes();
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000356CC File Offset: 0x000338CC
		public void Notify_TerrainChanged(IntVec3 cell, TerrainDef oldTerr, TerrainDef newTerr)
		{
			if (!this.cells.Contains(cell))
			{
				return;
			}
			if (newTerr == TerrainDefOf.ThinIce && oldTerr != TerrainDefOf.ThinIce)
			{
				this.numCellsFrozen++;
			}
			if (newTerr != TerrainDefOf.ThinIce && oldTerr == TerrainDefOf.ThinIce)
			{
				this.numCellsFrozen--;
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00035724 File Offset: 0x00033924
		public void Notify_PollutionChanged(IntVec3 cell, bool isPolluted)
		{
			if (!this.cells.Contains(cell))
			{
				return;
			}
			if (isPolluted)
			{
				this.numCellsPolluted++;
				return;
			}
			this.numCellsPolluted--;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00035755 File Offset: 0x00033955
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(base.GetHashCode(), this.rootCell.GetHashCode()), this.cellCount), this.waterBodyType.GetHashCode());
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00035794 File Offset: 0x00033994
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.rootCell, "rootCell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.cellCount, "cellCount", 0, false);
			Scribe_Values.Look<WaterBodyType>(ref this.waterBodyType, "fishType", WaterBodyType.None, false);
			Scribe_Values.Look<float>(ref this.population, "population", 0f, false);
			Scribe_Values.Look<bool>(ref this.shouldHaveFish, "shouldHaveFish", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.commonFish, "commonFish", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.uncommonFish, "uncommonFish", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.commonFish.NullOrEmpty<ThingDef>())
				{
					this.shouldHaveFish = true;
				}
				if (this.shouldHaveFish && this.commonFish == null)
				{
					this.commonFish = new List<ThingDef>();
					this.uncommonFish = new List<ThingDef>();
					this.SetFishTypes();
				}
				if (this.shouldHaveFish && this.Population > 0f && this.commonFish.Count == 0)
				{
					this.SetFishTypes();
				}
				this.commonFish.RemoveAll((ThingDef x) => x == null);
				this.uncommonFish.RemoveAll((ThingDef x) => x == null);
			}
		}

		// Token: 0x0400065D RID: 1629
		public Map map;

		// Token: 0x0400065E RID: 1630
		public IntVec3 rootCell;

		// Token: 0x0400065F RID: 1631
		public WaterBodyType waterBodyType;

		// Token: 0x04000660 RID: 1632
		[Unsaved(false)]
		public HashSet<IntVec3> cells = new HashSet<IntVec3>();

		// Token: 0x04000661 RID: 1633
		[Unsaved(false)]
		public CellRect? cachedBounds;

		// Token: 0x04000662 RID: 1634
		private bool shouldHaveFish;

		// Token: 0x04000663 RID: 1635
		private List<ThingDef> commonFish = new List<ThingDef>();

		// Token: 0x04000664 RID: 1636
		private List<ThingDef> uncommonFish = new List<ThingDef>();

		// Token: 0x04000665 RID: 1637
		private int cellCount;

		// Token: 0x04000666 RID: 1638
		private float population;

		// Token: 0x04000667 RID: 1639
		[Unsaved(false)]
		private float cachedPopulationFactor = -1f;

		// Token: 0x04000668 RID: 1640
		public int numCellsFrozen;

		// Token: 0x04000669 RID: 1641
		public int numCellsPolluted;
	}
}
