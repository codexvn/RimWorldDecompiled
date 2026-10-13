using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CE8 RID: 15592
	public static class WorldGenerator
	{
		// Token: 0x170034E7 RID: 13543
		// (get) Token: 0x0601538E RID: 86926 RVA: 0x00659244 File Offset: 0x00657444
		private static List<GameSetupStepDef> GameSetupStepsInOrder
		{
			get
			{
				List<GameSetupStepDef> result;
				if ((result = WorldGenerator.cachedGenSteps) == null)
				{
					result = (WorldGenerator.cachedGenSteps = (from x in DefDatabase<GameSetupStepDef>.AllDefs
					orderby x.order, x.index
					select x).ToList<GameSetupStepDef>());
				}
				return result;
			}
		}

		// Token: 0x0601538F RID: 86927 RVA: 0x006592B4 File Offset: 0x006574B4
		public static World GenerateWorld(float planetCoverage, string seedString, OverallRainfall overallRainfall, OverallTemperature overallTemperature, OverallPopulation population, LandmarkDensity landmarkDensity, List<FactionDef> factions = null, float pollution = 0f)
		{
			DeepProfiler.Start("GenerateWorld");
			Rand.PushState();
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.Seed = seedFromSeedString;
			World creatingWorld;
			try
			{
				Current.CreatingWorld = new World
				{
					info = 
					{
						seedString = seedString,
						planetCoverage = planetCoverage,
						overallRainfall = overallRainfall,
						overallTemperature = overallTemperature,
						overallPopulation = population,
						landmarkDensity = landmarkDensity,
						name = NameGenerator.GenerateName(RulePackDefOf.NamerWorld, null, false, null, null, null),
						factions = factions,
						pollution = pollution
					}
				};
				foreach (GameSetupStepDef gameSetupStepDef in WorldGenerator.GameSetupStepsInOrder)
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, gameSetupStepDef.setupStep.SeedPart);
					gameSetupStepDef.setupStep.GenerateFresh();
				}
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					WorldGenerator.GeneratePlanetLayer(planetLayer2, seedString, seedFromSeedString);
					if (planetLayer2.Tiles.Count == 0)
					{
						Log.Warning(string.Format("No tiles on layer {0}, layer should have a world gen step worker which initializes tiles such as WorldGenStep_Tiles", planetLayer2));
					}
				}
				Rand.Seed = seedFromSeedString;
				Current.CreatingWorld.grid.StandardizeTileData();
				Current.CreatingWorld.FinalizeInit(false);
				Find.Scenario.PostWorldGenerate();
				if (!ModsConfig.IdeologyActive)
				{
					Find.Scenario.PostIdeoChosen();
				}
				creatingWorld = Current.CreatingWorld;
			}
			finally
			{
				Rand.PopState();
				DeepProfiler.End();
				Current.CreatingWorld = null;
			}
			return creatingWorld;
		}

		// Token: 0x06015390 RID: 86928 RVA: 0x006594BC File Offset: 0x006576BC
		public static void GeneratePlanetLayer(PlanetLayer layer, string seedString, int seed)
		{
			List<WorldGenStepDef> genStepsInOrder = layer.Def.GenStepsInOrder;
			DeepProfiler.Start(string.Format("WorldGen - {0}", layer));
			for (int i = 0; i < genStepsInOrder.Count; i++)
			{
				DeepProfiler.Start(string.Format("WorldGenStep - {0}", genStepsInOrder[i]));
				try
				{
					Rand.Seed = Gen.HashCombineInt(seed, WorldGenerator.GetSeedPart(genStepsInOrder, i));
					genStepsInOrder[i].worldGenStep.GenerateFresh(seedString, layer);
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error in WorldGenStep: {0}", arg));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
		}

		// Token: 0x06015391 RID: 86929 RVA: 0x00659568 File Offset: 0x00657768
		public static void GenerateWithoutWorldData(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.PushState();
			foreach (GameSetupStepDef gameSetupStepDef in WorldGenerator.GameSetupStepsInOrder)
			{
				Rand.Seed = Gen.HashCombineInt(seedFromSeedString, gameSetupStepDef.setupStep.SeedPart);
				gameSetupStepDef.setupStep.GenerateWithoutWorldData();
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				List<WorldGenStepDef> genStepsInOrder = planetLayer2.Def.GenStepsInOrder;
				for (int i = 0; i < genStepsInOrder.Count; i++)
				{
					try
					{
						Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(genStepsInOrder, i));
						genStepsInOrder[i].worldGenStep.GenerateWithoutWorldData(seedString, planetLayer2);
					}
					catch (Exception arg)
					{
						Log.Error(string.Format("Error in WorldGenStep: {0}", arg));
					}
				}
			}
			Rand.PopState();
		}

		// Token: 0x06015392 RID: 86930 RVA: 0x006596A4 File Offset: 0x006578A4
		public static void GenerateFromScribe(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.PushState();
			foreach (GameSetupStepDef gameSetupStepDef in WorldGenerator.GameSetupStepsInOrder)
			{
				Rand.Seed = Gen.HashCombineInt(seedFromSeedString, gameSetupStepDef.setupStep.SeedPart);
				gameSetupStepDef.setupStep.GenerateFromScribe();
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				List<WorldGenStepDef> genStepsInOrder = planetLayer2.Def.GenStepsInOrder;
				for (int i = 0; i < genStepsInOrder.Count; i++)
				{
					try
					{
						Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(genStepsInOrder, i));
						genStepsInOrder[i].worldGenStep.GenerateFromScribe(seedString, planetLayer2);
					}
					catch (Exception arg)
					{
						Log.Error(string.Format("Error in WorldGenStep: {0}", arg));
					}
				}
			}
			Rand.PopState();
		}

		// Token: 0x06015393 RID: 86931 RVA: 0x006597E0 File Offset: 0x006579E0
		private static int GetSeedPart(List<WorldGenStepDef> genSteps, int index)
		{
			int seedPart = genSteps[index].worldGenStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (genSteps[i].worldGenStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x06015394 RID: 86932 RVA: 0x00659828 File Offset: 0x00657A28
		private static int GetSeedFromSeedString(string seedString)
		{
			return GenText.StableStringHash(seedString);
		}

		// Token: 0x0400EC9B RID: 60571
		public const float DefaultPlanetCoverage = 0.3f;

		// Token: 0x0400EC9C RID: 60572
		public const float DefaultPlanetCoverageOdyssey = 0.5f;

		// Token: 0x0400EC9D RID: 60573
		public const OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;

		// Token: 0x0400EC9E RID: 60574
		public const OverallPopulation DefaultOverallPopulation = OverallPopulation.Normal;

		// Token: 0x0400EC9F RID: 60575
		public const OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;

		// Token: 0x0400ECA0 RID: 60576
		public const LandmarkDensity DefaultLandmarkDensity = LandmarkDensity.Normal;

		// Token: 0x0400ECA1 RID: 60577
		public const float DefaultPollutionCoverage = 0.05f;

		// Token: 0x0400ECA2 RID: 60578
		private static List<GameSetupStepDef> cachedGenSteps;
	}
}
