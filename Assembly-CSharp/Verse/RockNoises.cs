using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x020004C0 RID: 1216
	public static class RockNoises
	{
		// Token: 0x06001F84 RID: 8068 RVA: 0x000A5BB4 File Offset: 0x000A3DB4
		public static void Init(Map map)
		{
			RockNoises.rockNoises = new List<RockNoises.RockNoise>();
			foreach (ThingDef rockDef in Find.World.NaturalRockTypesIn(map.Tile))
			{
				RockNoises.RockNoise rockNoise = new RockNoises.RockNoise();
				rockNoise.rockDef = rockDef;
				rockNoise.noise = new Perlin(0.004999999888241291, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.Medium);
				RockNoises.rockNoises.Add(rockNoise);
				ModuleBase noise = rockNoise.noise;
				ThingDef rockDef2 = rockNoise.rockDef;
				NoiseDebugUI.StoreNoiseRender(noise, ((rockDef2 != null) ? rockDef2.ToString() : null) + " score", map.Size.ToIntVec2);
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x000A5C94 File Offset: 0x000A3E94
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04001B26 RID: 6950
		public static List<RockNoises.RockNoise> rockNoises;

		// Token: 0x04001B27 RID: 6951
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x020004C1 RID: 1217
		public class RockNoise
		{
			// Token: 0x04001B28 RID: 6952
			public ThingDef rockDef;

			// Token: 0x04001B29 RID: 6953
			public ModuleBase noise;
		}
	}
}
