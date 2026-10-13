using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C52 RID: 11346
	public static class DeepDrillUtility
	{
		// Token: 0x17002811 RID: 10257
		// (get) Token: 0x0600FE75 RID: 65141 RVA: 0x0049C52C File Offset: 0x0049A72C
		public static List<ThingDef> Rocks
		{
			get
			{
				if (DeepDrillUtility.rocks == null)
				{
					DeepDrillUtility.rocks = (from def in DefDatabase<ThingDef>.AllDefs
					where def.IsNonResourceNaturalRock
					select def).ToList<ThingDef>();
				}
				return DeepDrillUtility.rocks;
			}
		}

		// Token: 0x0600FE76 RID: 65142 RVA: 0x0049C578 File Offset: 0x0049A778
		public static ThingDef RockForTerrain(TerrainDef terrain)
		{
			foreach (ThingDef thingDef in DeepDrillUtility.Rocks)
			{
				if (thingDef.building.naturalTerrain == terrain)
				{
					return thingDef;
				}
			}
			return null;
		}

		// Token: 0x0600FE77 RID: 65143 RVA: 0x0049C5D8 File Offset: 0x0049A7D8
		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

		// Token: 0x0600FE78 RID: 65144 RVA: 0x0049C5F4 File Offset: 0x0049A7F4
		public static bool GetNextResource(IntVec3 p, Map map, out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			for (int i = 0; i < 21; i++)
			{
				IntVec3 intVec = p + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					ThingDef thingDef = map.deepResourceGrid.ThingDefAt(intVec);
					if (thingDef != null)
					{
						resDef = thingDef;
						countPresent = map.deepResourceGrid.CountAt(intVec);
						cell = intVec;
						return true;
					}
				}
			}
			resDef = DeepDrillUtility.GetBaseResource(map, p);
			countPresent = int.MaxValue;
			cell = p;
			return false;
		}

		// Token: 0x0600FE79 RID: 65145 RVA: 0x0049C670 File Offset: 0x0049A870
		public static ThingDef GetBaseResource(Map map, IntVec3 cell)
		{
			if (!map.Biome.hasBedrock)
			{
				return null;
			}
			ThingDef thingDef = DeepDrillUtility.RockForTerrain(map.terrainGrid.BaseTerrainAt(cell));
			if (thingDef != null)
			{
				return thingDef.building.mineableThing;
			}
			Rand.PushState();
			Rand.Seed = cell.GetHashCode();
			ThingDef result = (from rock in Find.World.NaturalRockTypesIn(map.Tile)
			select rock.building.mineableThing).RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x0400A6BC RID: 42684
		private static List<ThingDef> rocks;

		// Token: 0x0400A6BD RID: 42685
		public const int NumCellsToScan = 21;
	}
}
