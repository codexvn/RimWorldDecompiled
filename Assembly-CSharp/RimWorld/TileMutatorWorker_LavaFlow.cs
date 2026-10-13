using System;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020FD RID: 8445
	public class TileMutatorWorker_LavaFlow : TileMutatorWorker
	{
		// Token: 0x0600B860 RID: 47200 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_LavaFlow(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B861 RID: 47201 RVA: 0x00352C34 File Offset: 0x00350E34
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.rockNoise = new RidgedMultifractal(0.029999999329447746, 2.0, 3, Rand.Int, QualityMode.High);
			this.rockNoise = new Clamp(0.0, 1.0, this.rockNoise);
			this.rockNoise = new Invert(this.rockNoise);
			this.rockNoise = new ScaleBias(1.0, 1.0, this.rockNoise);
			NoiseDebugUI.StoreNoiseRender(this.rockNoise, "lava flow");
		}

		// Token: 0x0600B862 RID: 47202 RVA: 0x00352CD4 File Offset: 0x00350ED4
		public override void GeneratePostElevationFertility(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.rockNoise.GetValue(intVec) <= 0.85f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x0600B863 RID: 47203 RVA: 0x00352D40 File Offset: 0x00350F40
		public override void GeneratePostTerrain(Map map)
		{
			TerrainPatchMaker terrainPatchMaker = map.Biome.terrainPatchMakers.FirstOrDefault((TerrainPatchMaker x) => x.isPond);
			foreach (IntVec3 intVec in map.AllCells)
			{
				TerrainDef terrain = intVec.GetTerrain(map);
				if (!terrain.IsWater)
				{
					if (terrain == TerrainDefOf.VolcanicRock)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Soil);
					}
					TerrainDef terrainDef = (terrainPatchMaker != null) ? terrainPatchMaker.TerrainAt(intVec, map, intVec.GetFertility(map)) : null;
					if (terrainDef != null)
					{
						map.terrainGrid.SetTerrain(intVec, terrainDef);
						if (terrainDef == TerrainDefOf.VolcanicRock)
						{
							map.terrainGrid.SetTerrain(intVec, TerrainDefOf.CooledLava);
						}
					}
					else if (this.rockNoise.GetValue(intVec) <= 0.85f && terrain != TerrainDefOf.LavaDeep)
					{
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.VolcanicRock);
						map.terrainGrid.SetTerrain(intVec, TerrainDefOf.CooledLava);
					}
				}
			}
			if (terrainPatchMaker != null)
			{
				terrainPatchMaker.Cleanup();
			}
		}

		// Token: 0x0600B864 RID: 47204 RVA: 0x00352E74 File Offset: 0x00351074
		public override void Tick(Map map)
		{
			IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentDefOf.LavaFlow.category, map);
			if (map.GameConditionManager.ConditionIsActive(GameConditionDefOf.LavaFlow))
			{
				return;
			}
			if (Rand.MTBEventOccurs(6f, 60000f, 1f) && IncidentDefOf.LavaFlow.Worker.CanFireNow(parms))
			{
				Find.Storyteller.incidentQueue.Add(IncidentDefOf.LavaFlow, Find.TickManager.TicksGame, parms, 0);
			}
		}

		// Token: 0x04007F10 RID: 32528
		private const int IncidentMTBDays = 6;

		// Token: 0x04007F11 RID: 32529
		private const float RidgedFreq = 0.03f;

		// Token: 0x04007F12 RID: 32530
		private const float RidgedLac = 2f;

		// Token: 0x04007F13 RID: 32531
		private const int RidgedOctaves = 3;

		// Token: 0x04007F14 RID: 32532
		private const float RockThreshold = 0.85f;

		// Token: 0x04007F15 RID: 32533
		private ModuleBase rockNoise;
	}
}
