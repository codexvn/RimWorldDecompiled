using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020020B7 RID: 8375
	public class TileMutatorDef : Def
	{
		// Token: 0x17001BE0 RID: 7136
		// (get) Token: 0x0600B745 RID: 46917 RVA: 0x0034C84C File Offset: 0x0034AA4C
		public TileMutatorWorker Worker
		{
			get
			{
				if (this.mutatorWorker != null)
				{
					return this.mutatorWorker;
				}
				if (this.workerClass != null)
				{
					return this.mutatorWorker = (TileMutatorWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					});
				}
				return null;
			}
		}

		// Token: 0x17001BE1 RID: 7137
		// (get) Token: 0x0600B746 RID: 46918 RVA: 0x0034C89B File Offset: 0x0034AA9B
		public bool IsCave
		{
			get
			{
				return this.categories.Contains("Caves");
			}
		}

		// Token: 0x0600B747 RID: 46919 RVA: 0x0034C8AD File Offset: 0x0034AAAD
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.biomeWhitelist != null)
			{
				this.biomeWhitelistSet = new HashSet<BiomeDef>(this.biomeWhitelist);
			}
			if (this.biomeBlacklist != null)
			{
				this.biomeBlacklistSet = new HashSet<BiomeDef>(this.biomeBlacklist);
			}
		}

		// Token: 0x0600B748 RID: 46920 RVA: 0x0034C8E7 File Offset: 0x0034AAE7
		public string Label(PlanetTile tileID)
		{
			TileMutatorWorker worker = this.Worker;
			return ((worker != null) ? worker.GetLabel(tileID) : null) ?? this.label;
		}

		// Token: 0x0600B749 RID: 46921 RVA: 0x0034C906 File Offset: 0x0034AB06
		public string Description(PlanetTile tileID)
		{
			TileMutatorWorker worker = this.Worker;
			return ((worker != null) ? worker.GetDescription(tileID) : null) ?? this.description;
		}

		// Token: 0x0600B74A RID: 46922 RVA: 0x0034C925 File Offset: 0x0034AB25
		public bool EverValid()
		{
			return this.requiresFactionOfDef == null || Find.FactionManager.FirstFactionOfDef(this.requiresFactionOfDef) != null;
		}

		// Token: 0x0600B74B RID: 46923 RVA: 0x0034C944 File Offset: 0x0034AB44
		public bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			if (!this.EverValid())
			{
				return false;
			}
			Tile tile2 = layer[tile];
			BiomeDef primaryBiome = tile2.PrimaryBiome;
			if (tile2.Mutators.Contains(this))
			{
				return false;
			}
			if (this.Worker != null && !this.Worker.IsValidTile(tile, layer))
			{
				return false;
			}
			foreach (string item in this.categories)
			{
				foreach (TileMutatorDef tileMutatorDef in tile2.Mutators)
				{
					if (tileMutatorDef.categories.Contains(item) && tileMutatorDef.priority >= this.priority)
					{
						return false;
					}
					if (tileMutatorDef.overrideCategories.Contains(item))
					{
						return false;
					}
				}
			}
			if (!this.animalDensityRange.Includes(primaryBiome.animalDensity))
			{
				return false;
			}
			if (!this.plantDensityRange.Includes(primaryBiome.plantDensity))
			{
				return false;
			}
			if (!this.pollutionRange.Includes(tile2.pollution))
			{
				return false;
			}
			if (!this.averageTemperatureRange.Includes(tile2.temperature))
			{
				return false;
			}
			if (this.minHilliness != Hilliness.Undefined && tile2.hilliness < this.minHilliness)
			{
				return false;
			}
			if (this.maxHilliness != Hilliness.Undefined && tile2.hilliness > this.maxHilliness)
			{
				return false;
			}
			if (this.biomeWhitelistSet != null && !this.biomeWhitelistSet.Contains(primaryBiome))
			{
				return false;
			}
			if (this.biomeBlacklistSet != null && this.biomeBlacklistSet.Contains(primaryBiome))
			{
				return false;
			}
			if (this.coastSidesRange != IntRange.Invalid)
			{
				TileMutatorDef.tmpTileNeighbors.Clear();
				layer.GetTileNeighbors(tile, TileMutatorDef.tmpTileNeighbors);
				bool flag = this.coastSidesRange.max == 0;
				int num = 0;
				for (int i = 0; i < TileMutatorDef.tmpTileNeighbors.Count; i++)
				{
					BiomeDef primaryBiome2 = layer[TileMutatorDef.tmpTileNeighbors[i]].PrimaryBiome;
					if (primaryBiome2 == BiomeDefOf.Ocean || primaryBiome2 == BiomeDefOf.Lake)
					{
						if (flag)
						{
							return false;
						}
						num++;
					}
				}
				if (!this.coastSidesRange.Includes(num))
				{
					return false;
				}
			}
			SurfaceTile surfaceTile = tile2 as SurfaceTile;
			if (surfaceTile != null)
			{
				if (!surfaceTile.Rivers.NullOrEmpty<SurfaceTile.RiverLink>() && !this.canSpawnOnRiver)
				{
					return false;
				}
				if (!surfaceTile.Roads.NullOrEmpty<SurfaceTile.RoadLink>() && !this.canSpawnOnRoad)
				{
					return false;
				}
			}
			return tile2.Landmark == null || this.canSpawnOnLandmark;
		}

		// Token: 0x04007DB9 RID: 32185
		private Type workerClass;

		// Token: 0x04007DBA RID: 32186
		public int genOrder;

		// Token: 0x04007DBB RID: 32187
		[NoTranslate]
		public List<string> categories = new List<string>();

		// Token: 0x04007DBC RID: 32188
		[NoTranslate]
		public List<string> overrideCategories = new List<string>();

		// Token: 0x04007DBD RID: 32189
		public int priority;

		// Token: 0x04007DBE RID: 32190
		public int displayPriority;

		// Token: 0x04007DBF RID: 32191
		public float chanceOnNonLandmarkTile;

		// Token: 0x04007DC0 RID: 32192
		public List<GenStepDef> extraGenSteps = new List<GenStepDef>();

		// Token: 0x04007DC1 RID: 32193
		public List<GenStepDef> preventGenSteps = new List<GenStepDef>();

		// Token: 0x04007DC2 RID: 32194
		public TerrainDef overrideCoastalBeachTerrain;

		// Token: 0x04007DC3 RID: 32195
		public TerrainDef overrideLakeBeachTerrain;

		// Token: 0x04007DC4 RID: 32196
		public TerrainDef overrideRiverbankTerrain;

		// Token: 0x04007DC5 RID: 32197
		public TerrainDef overrideMudTerrain;

		// Token: 0x04007DC6 RID: 32198
		public bool preventsPondGeneration;

		// Token: 0x04007DC7 RID: 32199
		public bool preventsLandmarks;

		// Token: 0x04007DC8 RID: 32200
		public bool preventNaturalElevation;

		// Token: 0x04007DC9 RID: 32201
		public bool preventPatches;

		// Token: 0x04007DCA RID: 32202
		public float animalDensityFactor = 1f;

		// Token: 0x04007DCB RID: 32203
		public float plantDensityFactor = 1f;

		// Token: 0x04007DCC RID: 32204
		public float junkDensityFactor = 1f;

		// Token: 0x04007DCD RID: 32205
		public float geyserCountFactor = 1f;

		// Token: 0x04007DCE RID: 32206
		public float chunkDensityFactor = 1f;

		// Token: 0x04007DCF RID: 32207
		public float fishPopulationFactor = 1f;

		// Token: 0x04007DD0 RID: 32208
		public List<BiomePlantRecord> additionalWildPlants = new List<BiomePlantRecord>();

		// Token: 0x04007DD1 RID: 32209
		public SimpleCurve overrideDensityForFertilityCurve;

		// Token: 0x04007DD2 RID: 32210
		public bool allowRoofedEdgeWalkIn;

		// Token: 0x04007DD3 RID: 32211
		public List<RaidStrategyDef> blacklistedRaidStrategies = new List<RaidStrategyDef>();

		// Token: 0x04007DD4 RID: 32212
		public List<GameConditionDef> additionalGameConditions = new List<GameConditionDef>();

		// Token: 0x04007DD5 RID: 32213
		public Hilliness hillinessForElevationGen;

		// Token: 0x04007DD6 RID: 32214
		public Hilliness hillinessForOreGeneration;

		// Token: 0x04007DD7 RID: 32215
		public Hilliness hillinessLabel;

		// Token: 0x04007DD8 RID: 32216
		public List<BiomeDef> biomeWhitelist;

		// Token: 0x04007DD9 RID: 32217
		public List<BiomeDef> biomeBlacklist;

		// Token: 0x04007DDA RID: 32218
		public FloatRange animalDensityRange = new FloatRange(0f, float.MaxValue);

		// Token: 0x04007DDB RID: 32219
		public FloatRange plantDensityRange = new FloatRange(0f, float.MaxValue);

		// Token: 0x04007DDC RID: 32220
		public Hilliness minHilliness;

		// Token: 0x04007DDD RID: 32221
		public Hilliness maxHilliness;

		// Token: 0x04007DDE RID: 32222
		public IntRange coastSidesRange = IntRange.Invalid;

		// Token: 0x04007DDF RID: 32223
		public bool canSpawnOnRiver = true;

		// Token: 0x04007DE0 RID: 32224
		public bool canSpawnOnRoad = true;

		// Token: 0x04007DE1 RID: 32225
		public bool canSpawnOnLandmark = true;

		// Token: 0x04007DE2 RID: 32226
		public FactionDef requiresFactionOfDef;

		// Token: 0x04007DE3 RID: 32227
		public FloatRange pollutionRange = new FloatRange(0f, float.MaxValue);

		// Token: 0x04007DE4 RID: 32228
		public FloatRange averageTemperatureRange = new FloatRange(float.MinValue, float.MaxValue);

		// Token: 0x04007DE5 RID: 32229
		public float weatherFrequencyFactor = 1f;

		// Token: 0x04007DE6 RID: 32230
		public float weatherFrequencyOffset;

		// Token: 0x04007DE7 RID: 32231
		public List<WeatherDef> weathersToAffect = new List<WeatherDef>();

		// Token: 0x04007DE8 RID: 32232
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x04007DE9 RID: 32233
		public List<ThingDef> plantKinds = new List<ThingDef>();

		// Token: 0x04007DEA RID: 32234
		public AncientStructureGenParms structureGenParms;

		// Token: 0x04007DEB RID: 32235
		public List<ThingDef> resourceBlacklist = new List<ThingDef>();

		// Token: 0x04007DEC RID: 32236
		private HashSet<BiomeDef> biomeWhitelistSet;

		// Token: 0x04007DED RID: 32237
		private HashSet<BiomeDef> biomeBlacklistSet;

		// Token: 0x04007DEE RID: 32238
		private TileMutatorWorker mutatorWorker;

		// Token: 0x04007DEF RID: 32239
		private static readonly List<PlanetTile> tmpTileNeighbors = new List<PlanetTile>();
	}
}
