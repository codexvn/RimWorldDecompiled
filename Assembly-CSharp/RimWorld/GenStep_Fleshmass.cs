using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001FB0 RID: 8112
	public class GenStep_Fleshmass : GenStep
	{
		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x0600B303 RID: 45827 RVA: 0x0033494F File Offset: 0x00332B4F
		public override int SeedPart
		{
			get
			{
				return 9872136;
			}
		}

		// Token: 0x0600B304 RID: 45828 RVA: 0x00334958 File Offset: 0x00332B58
		public override void Generate(Map map, GenStepParams parms)
		{
			Perlin perlin = new Perlin((double)this.fleshFrequency, 2.0, 0.5, this.noiseOctaves, Rand.Int, QualityMode.Medium);
			Perlin perlin2 = new Perlin((double)this.bloodFrequency, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (!map.generatorDef.isUnderground || caves[intVec] > 0f)
				{
					Building edifice = intVec.GetEdifice(map);
					if ((edifice == null || (this.fleshmassCanReplaceBuildings && !edifice.def.building.isNaturalRock)) && intVec.GetAffordances(map).Contains(ThingDefOf.Fleshmass.terrainAffordanceNeeded))
					{
						float num = (float)perlin.GetValue((double)intVec.x, 0.0, (double)intVec.z);
						if (this.fleshmassFalloffRadius > 0f)
						{
							float num2 = intVec.DistanceTo(map.Center);
							float num3 = 1f - Mathf.Clamp01((num2 - this.fleshmassFalloffRadius) / ((float)map.Size.x / 2f - this.fleshmassFalloffRadius));
							num *= num3;
						}
						if (num > this.fleshThreshold)
						{
							GenSpawn.Spawn(ThingDefOf.Fleshmass, intVec, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
						}
						if (num > this.fleshTerrainThreshold)
						{
							map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Flesh);
						}
						float num4 = (float)perlin2.GetValue((double)intVec.x, 0.0, (double)intVec.z);
						ThingDef filthDef = Rand.Bool ? ThingDefOf.Filth_TwistedFlesh : ThingDefOf.Filth_Blood;
						if (num4 > this.bloodThreshold)
						{
							FilthMaker.TryMakeFilth(intVec, map, filthDef, 1, FilthSourceFlags.None, true);
						}
					}
				}
			}
		}

		// Token: 0x04007AA2 RID: 31394
		private float fleshFrequency = 0.3f;

		// Token: 0x04007AA3 RID: 31395
		private float fleshThreshold = 0.2f;

		// Token: 0x04007AA4 RID: 31396
		private float fleshTerrainThreshold = 0.1f;

		// Token: 0x04007AA5 RID: 31397
		private bool fleshmassCanReplaceBuildings;

		// Token: 0x04007AA6 RID: 31398
		private float fleshmassFalloffRadius = -1f;

		// Token: 0x04007AA7 RID: 31399
		private int noiseOctaves = 6;

		// Token: 0x04007AA8 RID: 31400
		private float bloodFrequency = 0.3f;

		// Token: 0x04007AA9 RID: 31401
		private float bloodThreshold = 0.2f;
	}
}
