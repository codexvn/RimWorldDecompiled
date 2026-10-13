using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld.Planet;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001F96 RID: 8086
	public class GenStep_Asteroid : GenStep
	{
		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x0600B29A RID: 45722 RVA: 0x003322A8 File Offset: 0x003304A8
		public override int SeedPart
		{
			get
			{
				return 1929282;
			}
		}

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x0600B29B RID: 45723 RVA: 0x003322AF File Offset: 0x003304AF
		protected virtual float Radius
		{
			get
			{
				return 0.224f;
			}
		}

		// Token: 0x0600B29C RID: 45724 RVA: 0x003322B8 File Offset: 0x003304B8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Asteroid"))
			{
				return;
			}
			this.GenerateAsteroidElevation(map, parms);
			this.GenerateCaveElevation(map, parms);
			GenStep_Asteroid.SpawnAsteroid(map);
			this.SpawnOres(map, parms);
			if (Rand.Chance(this.ruinsChance))
			{
				GenStep_Asteroid.GenerateRuins(map, parms);
			}
			if (Rand.Chance(this.archeanTreeChance))
			{
				GenStep_Asteroid.GenerateArcheanTree(map, parms);
			}
			map.OrbitalDebris = OrbitalDebrisDefOf.Asteroid;
		}

		// Token: 0x0600B29D RID: 45725 RVA: 0x00332324 File Offset: 0x00330524
		private static void SpawnAsteroid(Map map)
		{
			using (map.pathing.DisableIncrementalScope())
			{
				foreach (IntVec3 intVec in map.AllCells)
				{
					float num = MapGenerator.Elevation[intVec];
					float num2 = MapGenerator.Caves[intVec];
					if (num > 0.5f)
					{
						map.terrainGrid.SetTerrain(intVec, ThingDefOf.Vacstone.building.naturalTerrain);
					}
					if (num > 0.7f && num2 == 0f)
					{
						GenSpawn.Spawn(ThingDefOf.Vacstone, intVec, map, WipeMode.Vanish);
					}
					if (num > 0.7f)
					{
						map.roofGrid.SetRoof(intVec, RoofDefOf.RoofRockThin);
					}
				}
				HashSet<IntVec3> mainIsland = new HashSet<IntVec3>();
				map.floodFiller.FloodFill(map.Center, (IntVec3 x) => x.GetTerrain(map) != TerrainDefOf.Space, delegate(IntVec3 x)
				{
					mainIsland.Add(x);
				}, int.MaxValue, false, null);
				foreach (IntVec3 intVec2 in map.AllCells)
				{
					if (!mainIsland.Contains(intVec2))
					{
						map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.Space);
						map.roofGrid.SetRoof(intVec2, null);
						foreach (Thing thing in intVec2.GetThingList(map).ToList<Thing>())
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x0600B29E RID: 45726 RVA: 0x00332568 File Offset: 0x00330768
		private void GenerateAsteroidElevation(Map map, GenStepParams parms)
		{
			this.innerNoise = this.ConfigureNoise(map, parms);
			foreach (IntVec3 intVec in map.AllCells)
			{
				MapGenerator.Elevation[intVec] = this.innerNoise.GetValue(intVec);
			}
		}

		// Token: 0x0600B29F RID: 45727 RVA: 0x003325D4 File Offset: 0x003307D4
		protected virtual ModuleBase ConfigureNoise(Map map, GenStepParams parms)
		{
			ModuleBase moduleBase = new DistFromPoint((float)map.Size.x * this.Radius);
			moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			moduleBase = new Scale(0.6499999761581421, 1.0, 1.0, moduleBase);
			moduleBase = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, moduleBase);
			moduleBase = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), moduleBase);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "Base asteroid shape");
			moduleBase = new Blend(new Perlin(0.006000000052154064, 2.0, 2.0, 3, Rand.Int, QualityMode.Medium), moduleBase, new Const(0.800000011920929));
			moduleBase = new Blend(new Perlin(0.05000000074505806, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium), moduleBase, new Const(0.8500000238418579));
			moduleBase = new Power(moduleBase, new Const(0.20000000298023224));
			NoiseDebugUI.StoreNoiseRender(moduleBase, "Asteroid");
			return moduleBase;
		}

		// Token: 0x0600B2A0 RID: 45728 RVA: 0x0033272C File Offset: 0x0033092C
		private void SpawnOres(Map map, GenStepParams parms)
		{
			ThingDef thingDef = ((SpaceMapParent)map.ParentHolder).preciousResource ?? this.mineableCounts.RandomElement<GenStep_Asteroid.MineableCountConfig>().mineable;
			int num = 0;
			for (int i = 0; i < this.mineableCounts.Count; i++)
			{
				if (this.mineableCounts[i].mineable == thingDef)
				{
					num = this.mineableCounts[i].countRange.RandomInRange;
					break;
				}
			}
			if (num == 0)
			{
				string str = "No count found for resource ";
				ThingDef thingDef2 = thingDef;
				Debug.LogError(str + ((thingDef2 != null) ? thingDef2.ToString() : null));
				return;
			}
			int randomInRange = this.numChunks.RandomInRange;
			int forcedLumpSize = num / randomInRange;
			new GenStep_ScatterLumpsMineable
			{
				count = randomInRange,
				forcedDefToScatter = thingDef,
				forcedLumpSize = forcedLumpSize
			}.Generate(map, parms);
		}

		// Token: 0x0600B2A1 RID: 45729 RVA: 0x003327FC File Offset: 0x003309FC
		private void GenerateCaveElevation(Map map, GenStepParams parms)
		{
			GenStep_Asteroid.<>c__DisplayClass29_0 CS$<>8__locals1 = new GenStep_Asteroid.<>c__DisplayClass29_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			Perlin directionNoise = new Perlin(0.0020000000949949026, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
			CS$<>8__locals1.elevation = MapGenerator.Elevation;
			BoolGrid visited = new BoolGrid(CS$<>8__locals1.map);
			List<IntVec3> group = new List<IntVec3>();
			MapGenCavesUtility.CaveGenParms @default = MapGenCavesUtility.CaveGenParms.Default;
			@default.widthOffsetPerCell = 0.015f;
			@default.minTunnelWidth = 0.5f;
			@default.branchChance = 0.05f;
			@default.maxOpenTunnelsPerRockGroup = 2;
			@default.maxClosedTunnelsPerRockGroup = 2;
			@default.minTunnelWidth = 0.25f;
			@default.branchChance = 0.05f;
			@default.openTunnelsPer10k = 4f;
			@default.tunnelsWidthPerRockCount = new SimpleCurve
			{
				{
					new CurvePoint(100f, 1f),
					true
				},
				{
					new CurvePoint(300f, 1.5f),
					true
				},
				{
					new CurvePoint(3000f, 1.9f),
					true
				}
			};
			MapGenCavesUtility.GenerateCaves(CS$<>8__locals1.map, visited, group, directionNoise, @default, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateCaveElevation>g__Rock|0));
		}

		// Token: 0x0600B2A2 RID: 45730 RVA: 0x0033292B File Offset: 0x00330B2B
		private bool IsRock(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return c.InBounds(map) && elevation[c] > 0.7f;
		}

		// Token: 0x0600B2A3 RID: 45731 RVA: 0x00332948 File Offset: 0x00330B48
		private static void GenerateArcheanTree(Map map, GenStepParams parms)
		{
			GenStep_Asteroid.<>c__DisplayClass31_0 CS$<>8__locals1 = new GenStep_Asteroid.<>c__DisplayClass31_0();
			CS$<>8__locals1.map = map;
			if (!MapGenUtility.TryGetRandomClearRect(23, 23, out CS$<>8__locals1.rect, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<GenerateArcheanTree>g__RectValidator|0), 0.59999996f, 3.4028235E+38f, MapGenUtility.SearchWeightMode.Largest, 1) && !MapGenUtility.TryGetRandomClearRect(23, 23, out CS$<>8__locals1.rect, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<GenerateArcheanTree>g__RectValidator|0), -1f, 3.4028235E+38f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				return;
			}
			int num = 5;
			foreach (IntVec3 intVec in CS$<>8__locals1.rect.Cells)
			{
				float magnitude = (intVec - CS$<>8__locals1.rect.CenterCell).Magnitude;
				if (magnitude < 8f || (magnitude < 9f && Rand.Chance(0.25f)))
				{
					Building edifice = intVec.GetEdifice(CS$<>8__locals1.map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
					CS$<>8__locals1.map.roofGrid.SetRoof(intVec, null);
				}
				if (magnitude < (float)num || (magnitude < (float)num + 1.9f && Rand.Chance(0.25f)))
				{
					CS$<>8__locals1.map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoilRich);
				}
			}
			WildPlantSpawner.SpawnPlant(ThingDefOf.Plant_TreeArchean, CS$<>8__locals1.map, CS$<>8__locals1.rect.CenterCell, false).Growth = 1f;
			List<IntVec3> list;
			RCellFinder.TryFindRandomCellsNear(CS$<>8__locals1.rect.CenterCell, 6, CS$<>8__locals1.map, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateArcheanTree>g__Validator|1), out list, 4, 7);
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec2 = list[i];
				if (i == 0)
				{
					RoomGenUtility.SpawnCrate(ThingDefOf.SealedCrate, intVec2, CS$<>8__locals1.map, Rot4.North, ThingSetMakerDefOf.Reward_ArcheanSeed, true, null);
				}
				else
				{
					GenSpawn.Spawn(ThingDefOf.AncientSpacerCrate, intVec2, CS$<>8__locals1.map, Rot4.North, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600B2A4 RID: 45732 RVA: 0x00332B4C File Offset: 0x00330D4C
		private static void GenerateRuins(Map map, GenStepParams parms)
		{
			GenStep_Asteroid.<>c__DisplayClass32_0 CS$<>8__locals1;
			CS$<>8__locals1.size = new IntVec2(GenStep_Asteroid.RuinsSize.RandomInRange, GenStep_Asteroid.RuinsSize.RandomInRange);
			IntVec3 intVec = IntVec3.Invalid;
			Rot4 rot = Rot4.North;
			for (int i = 0; i < 10; i++)
			{
				float num = Rand.Range(0f, 0.2f);
				Vector3 vect = new Vector3((float)Mathf.RoundToInt((float)map.Size.x * num), 0f, 0f).RotatedBy(Rand.Range(0f, 360f));
				IntVec3 intVec2 = map.Center + vect.ToIntVec3();
				rot = Rot4.Random;
				for (int j = 0; j < 2; j++)
				{
					if (i == 1)
					{
						rot = rot.Rotated(RotationDirection.Clockwise);
					}
					if (GenStep_Asteroid.<GenerateRuins>g__IsValidPoint|32_0(intVec2, rot, ref CS$<>8__locals1))
					{
						intVec = intVec2;
						break;
					}
				}
			}
			if (intVec == IntVec3.Invalid)
			{
				return;
			}
			CellRect cellRect = intVec.RectAbout(CS$<>8__locals1.size, rot);
			StructureGenParams parms2 = new StructureGenParams
			{
				size = cellRect.Size
			};
			LayoutWorker worker = LayoutDefOf.SpaceRuins.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms2);
			SketchResolveParams parms3 = new SketchResolveParams
			{
				sketch = layoutStructureSketch.layoutSketch,
				destroyChanceExp = new float?(1.5f)
			};
			SketchResolverDefOf.DamageBuildingsLight.Resolve(parms3);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			worker.Spawn(layoutStructureSketch, map, cellRect.Min, null, null, true, false, null);
		}

		// Token: 0x0600B2A7 RID: 45735 RVA: 0x00332CEC File Offset: 0x00330EEC
		[CompilerGenerated]
		internal static bool <GenerateRuins>g__IsValidPoint|32_0(IntVec3 p, Rot4 rot, ref GenStep_Asteroid.<>c__DisplayClass32_0 A_2)
		{
			foreach (IntVec3 c in p.RectAbout(A_2.size, rot))
			{
				if (MapGenerator.Elevation[c] < 0.7f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04007A4D RID: 31309
		public List<GenStep_Asteroid.MineableCountConfig> mineableCounts;

		// Token: 0x04007A4E RID: 31310
		public IntRange numChunks;

		// Token: 0x04007A4F RID: 31311
		public float ruinsChance;

		// Token: 0x04007A50 RID: 31312
		public float archeanTreeChance;

		// Token: 0x04007A51 RID: 31313
		private const float FloorThreshold = 0.5f;

		// Token: 0x04007A52 RID: 31314
		private const float RuinsRadius = 0.2f;

		// Token: 0x04007A53 RID: 31315
		private const float MacroFrequency = 0.006f;

		// Token: 0x04007A54 RID: 31316
		private const float MicroFrequency = 0.05f;

		// Token: 0x04007A55 RID: 31317
		private const float MacroBlend = 0.8f;

		// Token: 0x04007A56 RID: 31318
		private const float MicroBlend = 0.85f;

		// Token: 0x04007A57 RID: 31319
		private const float SquashScale = 0.65f;

		// Token: 0x04007A58 RID: 31320
		private const float Exponent = 0.2f;

		// Token: 0x04007A59 RID: 31321
		private const float CaveBranchChance = 0.05f;

		// Token: 0x04007A5A RID: 31322
		private const float CaveWidthOffsetPerCell = 0.015f;

		// Token: 0x04007A5B RID: 31323
		private const float CaveMinTunnelWidth = 0.5f;

		// Token: 0x04007A5C RID: 31324
		private const int TreeAreaRadiusSize = 11;

		// Token: 0x04007A5D RID: 31325
		private const float CaveDirectionNoiseFrequency = 0.002f;

		// Token: 0x04007A5E RID: 31326
		private static readonly IntRange RuinsSize = new IntRange(6, 10);

		// Token: 0x04007A5F RID: 31327
		private ModuleBase innerNoise;

		// Token: 0x02001F97 RID: 8087
		public class MineableCountConfig
		{
			// Token: 0x0600B2A8 RID: 45736 RVA: 0x00332D5C File Offset: 0x00330F5C
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "mineable", xmlRoot.Name, null, null, null);
				this.countRange = IntRange.FromString(xmlRoot.FirstChild.Value);
			}

			// Token: 0x04007A60 RID: 31328
			public ThingDef mineable;

			// Token: 0x04007A61 RID: 31329
			public IntRange countRange;
		}
	}
}
