using RimWorld;
using RimWorld.Planet;
using UnityEngine;

internal static class _0024BurstDirectCallInitializer
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		MapGenUtility.ComputeLargestRects_0000B772_0024BurstDirectCall.Initialize();
		MapGenUtility.RectsComputeSpaces_0000B773_0024BurstDirectCall.Initialize();
		FastTileFinder.Initialize_0024ComputeQueryJob_SphericalDistance_00014F7D_0024BurstDirectCall();
		PlanetLayer.CalculateAverageTileSize_00015440_0024BurstDirectCall.Initialize();
		PlanetLayer.IntGetTileSize_00015442_0024BurstDirectCall.Initialize();
		PlanetLayer.IntGetTileCenter_00015445_0024BurstDirectCall.Initialize();
	}
}
