using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB6 RID: 8118
	public class GenStep_FogSpace : GenStep
	{
		// Token: 0x17001B43 RID: 6979
		// (get) Token: 0x0600B315 RID: 45845 RVA: 0x003350C7 File Offset: 0x003332C7
		public override int SeedPart
		{
			get
			{
				return 492371731;
			}
		}

		// Token: 0x0600B316 RID: 45846 RVA: 0x003350D0 File Offset: 0x003332D0
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_FogSpace.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.map.fogGrid.SetAllFogged();
			foreach (IntVec3 intVec in CS$<>8__locals1.map.BoundsRect(0).Corners)
			{
				if (GenStep_FogSpace.<Generate>g__Validator|2_0(intVec, ref CS$<>8__locals1))
				{
					FloodFillerFog.FloodUnfog(intVec, CS$<>8__locals1.map);
				}
			}
			List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
			for (int i = 0; i < rootsToUnfog.Count; i++)
			{
				FloodFillerFog.FloodUnfog(rootsToUnfog[i], CS$<>8__locals1.map);
			}
		}

		// Token: 0x0600B318 RID: 45848 RVA: 0x00335188 File Offset: 0x00333388
		[CompilerGenerated]
		internal static bool <Generate>g__Validator|2_0(IntVec3 c, ref GenStep_FogSpace.<>c__DisplayClass2_0 A_1)
		{
			return c.GetEdifice(A_1.map) == null && !c.Roofed(A_1.map);
		}
	}
}
