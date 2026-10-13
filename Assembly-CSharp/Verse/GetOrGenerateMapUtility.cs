using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000411 RID: 1041
	public static class GetOrGenerateMapUtility
	{
		// Token: 0x06001A60 RID: 6752 RVA: 0x00085780 File Offset: 0x00083980
		public static Map GetOrGenerateMap(PlanetTile tile, IntVec3 size, WorldObjectDef suggestedMapParentDef, IEnumerable<GenStepWithParams> extraGenStepDefs = null, bool stepDebugger = false)
		{
			Map map = Current.Game.FindMap(tile);
			if (map == null)
			{
				MapParent mapParent = Find.WorldObjects.MapParentAt(tile);
				if (mapParent == null)
				{
					if (suggestedMapParentDef == null)
					{
						Log.Error(string.Format("Tried to get or generate map at {0}, but there isn't any MapParent world object here and map parent def argument is null.", tile));
						return null;
					}
					mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(suggestedMapParentDef);
					mapParent.Tile = tile;
					Find.WorldObjects.Add(mapParent);
				}
				if (!tile.Valid)
				{
					map = PocketMapUtility.GeneratePocketMap(size, mapParent.MapGeneratorDef, extraGenStepDefs, Find.AnyPlayerHomeMap);
				}
				else
				{
					map = MapGenerator.GenerateMap(size, mapParent, mapParent.MapGeneratorDef, mapParent.ExtraGenStepDefs.ConcatIfNotNull(extraGenStepDefs), null, false, stepDebugger);
				}
			}
			return map;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00085825 File Offset: 0x00083A25
		public static Map GetOrGenerateMap(PlanetTile tile, WorldObjectDef suggestedMapParentDef, IEnumerable<GenStepWithParams> extraGenStepDefs = null)
		{
			return GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, suggestedMapParentDef, extraGenStepDefs, false);
		}
	}
}
