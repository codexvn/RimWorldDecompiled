using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003926 RID: 14630
	public class SymbolResolver_FloorFill : SymbolResolver
	{
		// Token: 0x06014287 RID: 82567 RVA: 0x00601A70 File Offset: 0x005FFC70
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			TerrainDef terrainDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			bool valueOrDefault = rp.floorOnlyIfTerrainSupports.GetValueOrDefault();
			bool valueOrDefault2 = rp.allowBridgeOnAnyImpassableTerrain.GetValueOrDefault();
			foreach (IntVec3 c in rp.rect)
			{
				if ((rp.chanceToSkipFloor == null || !Rand.Chance(rp.chanceToSkipFloor.Value)) && (!valueOrDefault || GenConstruct.CanBuildOnTerrain(terrainDef, c, map, Rot4.North, null, null) || (valueOrDefault2 && c.GetTerrain(map).passability == Traversability.Impassable)))
				{
					terrainGrid.SetTerrain(c, terrainDef);
					if (rp.filthDef != null)
					{
						FilthMaker.TryMakeFilth(c, map, rp.filthDef, (rp.filthDensity != null) ? Mathf.RoundToInt(rp.filthDensity.Value.RandomInRange) : 1, FilthSourceFlags.None, true);
					}
				}
			}
		}
	}
}
