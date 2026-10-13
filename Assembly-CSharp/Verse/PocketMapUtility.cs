using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004CF RID: 1231
	public static class PocketMapUtility
	{
		// Token: 0x06001FE7 RID: 8167 RVA: 0x000A6E70 File Offset: 0x000A5070
		public static Map GeneratePocketMap(IntVec3 size, MapGeneratorDef generatorDef, IEnumerable<GenStepWithParams> extraGenStepDefs, Map sourceMap)
		{
			PocketMapParent pocketMapParent = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.PocketMap) as PocketMapParent;
			pocketMapParent.sourceMap = sourceMap;
			pocketMapParent.mapGenerator = generatorDef;
			Map result = MapGenerator.GenerateMap(size, pocketMapParent, generatorDef, extraGenStepDefs, null, true, false);
			Find.World.pocketMaps.Add(pocketMapParent);
			return result;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000A6EB8 File Offset: 0x000A50B8
		public static void DestroyPocketMap(Map map)
		{
			PocketMapParent pocketMapParent = ((map != null) ? map.Parent : null) as PocketMapParent;
			if (map == null || pocketMapParent == null)
			{
				return;
			}
			Find.World.pocketMaps.Remove(pocketMapParent);
			Current.Game.DeinitAndRemoveMap(map, true);
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x04001B4E RID: 6990
		public static MapPortal currentlyGeneratingPortal;
	}
}
