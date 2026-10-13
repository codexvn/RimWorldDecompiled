using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002024 RID: 8228
	public class GenStep_MetalHell : GenStep
	{
		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x0600B503 RID: 46339 RVA: 0x0033F931 File Offset: 0x0033DB31
		public override int SeedPart
		{
			get
			{
				return 41234756;
			}
		}

		// Token: 0x0600B504 RID: 46340 RVA: 0x0033F938 File Offset: 0x0033DB38
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckAnomaly("Metalhell"))
			{
				return;
			}
			List<ThingDef> source = new List<ThingDef>
			{
				ThingDefOf.VoidmetalMassSmall,
				ThingDefOf.VoidmetalMassMedium,
				ThingDefOf.VoidmetalMassLarge
			};
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.StoreNoiseRender(this.noise, "metal hell");
			foreach (IntVec3 intVec in map.AllCells)
			{
				float num = (float)(intVec.x - map.Center.x) / ((float)map.Size.x / 2f);
				float num2 = (float)(intVec.z - map.Center.z) / ((float)map.Size.z / 2f);
				float num3 = num * num + num2 * num2;
				float num4 = 1f / num3 + this.noise.GetValue(intVec) / 2f;
				map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Gravel);
				map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Voidmetal);
				if (num4 < 1.75f)
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.VoidmetalWall, null), intVec, map, WipeMode.Vanish);
				}
				else if (num4 < 1.9f)
				{
					ThingDef thingDef = source.RandomElement<ThingDef>();
					bool flag = true;
					foreach (IntVec3 c in GenAdj.CellsOccupiedBy(intVec, Rot4.North, thingDef.size))
					{
						if (!c.InBounds(map) || !map.thingGrid.ThingsListAt(c).NullOrEmpty<Thing>())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, null), intVec, map, WipeMode.Vanish);
					}
				}
				if (GenStep_MetalHell.InnerMassCutoffRange.Includes(num4) && Rand.Chance(0.05f))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(source.RandomElement<ThingDef>(), null), intVec, map, WipeMode.Vanish);
				}
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.VoidNode, null), map.Center, map, WipeMode.Vanish);
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MetalHellAmbience, null), map.Center, map, WipeMode.Vanish);
			foreach (IntVec3 intVec2 in GenAdj.CardinalDirections)
			{
				Rot4 opposite = Rot4.FromIntVec3(intVec2).Opposite;
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.TwistedArchotechSupport_Large, null), map.Center + intVec2 * 10, map, opposite, WipeMode.Vanish, false, false);
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.TwistedArchotechSupport_Small, null), map.Center + intVec2 * 16, map, opposite, WipeMode.Vanish, false, false);
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.TwistedArchotechSupport_Vertical, null), map.Center + intVec2 * 7 + intVec2.RotatedBy(RotationDirection.Clockwise) * 7, map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MetalHellFloorCracks, null), map.Center, map, WipeMode.Vanish);
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MetalHellFloorMarkings, null), map.Center, map, WipeMode.Vanish);
			MapGenerator.PlayerStartSpot = map.Center;
		}

		// Token: 0x04007C2E RID: 31790
		private const float WallNoiseCutoff = 1.75f;

		// Token: 0x04007C2F RID: 31791
		private const float MassNoiseCutoff = 1.9f;

		// Token: 0x04007C30 RID: 31792
		private static readonly FloatRange InnerMassCutoffRange = new FloatRange(2f, 3f);

		// Token: 0x04007C31 RID: 31793
		private const float InnerMassChancePerCell = 0.05f;

		// Token: 0x04007C32 RID: 31794
		public float perlinFrequency = 0.05f;

		// Token: 0x04007C33 RID: 31795
		public float perlinLacunarity = 2f;

		// Token: 0x04007C34 RID: 31796
		public float perlinPersistence = 0.5f;

		// Token: 0x04007C35 RID: 31797
		public int perlinOctaves = 1;

		// Token: 0x04007C36 RID: 31798
		[Unsaved(false)]
		private ModuleBase noise;
	}
}
