using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FED RID: 8173
	public class GenStep_RocksFromGrid : GenStep
	{
		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x0600B41A RID: 46106 RVA: 0x0033A570 File Offset: 0x00338770
		public override int SeedPart
		{
			get
			{
				return 1182952823;
			}
		}

		// Token: 0x0600B41B RID: 46107 RVA: 0x0033A578 File Offset: 0x00338778
		public static ThingDef RockDefAt(IntVec3 c)
		{
			ThingDef thingDef = null;
			float num = -999999f;
			for (int i = 0; i < RockNoises.rockNoises.Count; i++)
			{
				float value = RockNoises.rockNoises[i].noise.GetValue(c);
				if (value > num)
				{
					thingDef = RockNoises.rockNoises[i].rockDef;
					num = value;
				}
			}
			if (thingDef == null)
			{
				string str = "Did not get rock def to generate at ";
				IntVec3 intVec = c;
				Log.ErrorOnce(str + intVec.ToString(), 50812);
				thingDef = ThingDefOf.Sandstone;
			}
			return thingDef;
		}

		// Token: 0x0600B41C RID: 46108 RVA: 0x0033A600 File Offset: 0x00338800
		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.TileInfo.WaterCovered)
			{
				return;
			}
			map.regionAndRoomUpdater.Enabled = false;
			float num = 0.7f;
			List<GenStep_RocksFromGrid.RoofThreshold> list = new List<GenStep_RocksFromGrid.RoofThreshold>();
			list.Add(new GenStep_RocksFromGrid.RoofThreshold
			{
				roofDef = RoofDefOf.RoofRockThick,
				minGridVal = num * 1.14f
			});
			list.Add(new GenStep_RocksFromGrid.RoofThreshold
			{
				roofDef = RoofDefOf.RoofRockThin,
				minGridVal = num * 1.04f
			});
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 intVec in map.AllCells)
			{
				float num2 = elevation[intVec];
				if (num2 > num)
				{
					if (caves[intVec] <= 0f)
					{
						GenSpawn.Spawn(GenStep_RocksFromGrid.RockDefAt(intVec), intVec, map, WipeMode.Vanish);
					}
					for (int i = 0; i < list.Count; i++)
					{
						if (num2 > list[i].minGridVal)
						{
							map.roofGrid.SetRoof(intVec, list[i].roofDef);
							break;
						}
					}
				}
			}
			BoolGrid visited = new BoolGrid(map);
			List<IntVec3> toRemove = new List<IntVec3>();
			Predicate<IntVec3> <>9__0;
			Action<IntVec3> <>9__1;
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				if (!visited[intVec2] && this.IsNaturalRoofAt(intVec2, map))
				{
					toRemove.Clear();
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec2;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => this.IsNaturalRoofAt(x, map)));
					}
					Action<IntVec3> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(IntVec3 x)
						{
							visited[x] = true;
							toRemove.Add(x);
						});
					}
					floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
					if (toRemove.Count < 20)
					{
						for (int j = 0; j < toRemove.Count; j++)
						{
							map.roofGrid.SetRoof(toRemove[j], null);
						}
					}
				}
			}
			GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable();
			if (!ModsConfig.OdysseyActive || !this.ignoreMaxIfGravship || !map.wasSpawnedViaGravShipLanding)
			{
				genStep_ScatterLumpsMineable.maxValue = this.maxMineableValue;
			}
			genStep_ScatterLumpsMineable.useNomadicMineables = true;
			float num3 = GenStep_RocksFromGrid.GetResourceBlotchesPer10KCellsForMap(map);
			if (this.overrideBlotchesPer10kCells != null)
			{
				num3 = this.overrideBlotchesPer10kCells.Value;
			}
			genStep_ScatterLumpsMineable.countPer10kCellsRange = new FloatRange(num3, num3);
			genStep_ScatterLumpsMineable.Generate(map, parms);
			map.regionAndRoomUpdater.Enabled = true;
		}

		// Token: 0x0600B41D RID: 46109 RVA: 0x0033A938 File Offset: 0x00338B38
		public static float GetResourceBlotchesPer10KCellsForMap(Map map)
		{
			float result = 10f;
			switch (map.TileInfo.HillinessForOreGeneration)
			{
			case Hilliness.Flat:
				result = 4f;
				break;
			case Hilliness.SmallHills:
				result = 8f;
				break;
			case Hilliness.LargeHills:
				result = 11f;
				break;
			case Hilliness.Mountainous:
				result = 15f;
				break;
			case Hilliness.Impassable:
				result = 16f;
				break;
			}
			return result;
		}

		// Token: 0x0600B41E RID: 46110 RVA: 0x0033A99C File Offset: 0x00338B9C
		private bool IsNaturalRoofAt(IntVec3 c, Map map)
		{
			return c.Roofed(map) && c.GetRoof(map).isNatural;
		}

		// Token: 0x04007B88 RID: 31624
		private float maxMineableValue = float.MaxValue;

		// Token: 0x04007B89 RID: 31625
		private bool ignoreMaxIfGravship;

		// Token: 0x04007B8A RID: 31626
		private float? overrideBlotchesPer10kCells;

		// Token: 0x04007B8B RID: 31627
		private const int MinRoofedCellsPerGroup = 20;

		// Token: 0x02001FEE RID: 8174
		private class RoofThreshold
		{
			// Token: 0x04007B8C RID: 31628
			public RoofDef roofDef;

			// Token: 0x04007B8D RID: 31629
			public float minGridVal;
		}
	}
}
