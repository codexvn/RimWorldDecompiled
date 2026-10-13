using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

// Token: 0x02003EF6 RID: 16118
internal static class $BurstDirectCallInitializer
{
	// Token: 0x0601618C RID: 90508 RVA: 0x0069C74F File Offset: 0x0069A94F
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.Initialize();
		MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.Initialize();
		FastTileFinder.Initialize$ComputeQueryJob_SphericalDistance_00014E3B$BurstDirectCall();
		PlanetLayer.CalculateAverageTileSize_000152FC$BurstDirectCall.Initialize();
		PlanetLayer.IntGetTileSize_000152FE$BurstDirectCall.Initialize();
		PlanetLayer.IntGetTileCenter_00015301$BurstDirectCall.Initialize();
	}
}
