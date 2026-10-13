using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038CC RID: 14540
	public class SketchResolver_FloorFill : SketchResolver
	{
		// Token: 0x0601411E RID: 82206 RVA: 0x005F7524 File Offset: 0x005F5724
		protected override void ResolveInt(SketchResolveParams parms)
		{
			CellRect cellRect = parms.rect ?? parms.sketch.OccupiedRect;
			TerrainDef terrainDef;
			TerrainDef terrainDef2;
			if (!SketchResolver_FloorFill.TryFindFloors(out terrainDef, out terrainDef2, parms))
			{
				return;
			}
			bool valueOrDefault = parms.floorFillRoomsOnly.GetValueOrDefault();
			bool valueOrDefault2 = parms.singleFloorType.GetValueOrDefault();
			if (valueOrDefault)
			{
				SketchResolver_FloorFill.tmpWalls.Clear();
				for (int i = 0; i < parms.sketch.Things.Count; i++)
				{
					SketchThing sketchThing = parms.sketch.Things[i];
					if (sketchThing.def.passability == Traversability.Impassable && sketchThing.def.Fillage == FillCategory.Full)
					{
						foreach (IntVec3 item in sketchThing.OccupiedRect)
						{
							SketchResolver_FloorFill.tmpWalls.Add(item);
						}
					}
				}
				SketchResolver_FloorFill.tmpVisited.Clear();
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 intVec = enumerator.Current;
						if (!SketchResolver_FloorFill.tmpWalls.Contains(intVec))
						{
							this.FloorFillRoom(intVec, SketchResolver_FloorFill.tmpWalls, SketchResolver_FloorFill.tmpVisited, parms.sketch, terrainDef, terrainDef2, cellRect, valueOrDefault2);
						}
					}
					return;
				}
			}
			bool[,] array = AbstractShapeGenerator.Generate(cellRect.Width, cellRect.Height, true, true, false, true, false, 0f);
			foreach (IntVec3 intVec2 in cellRect)
			{
				if (!parms.sketch.ThingsAt(intVec2).Any((SketchThing x) => x.def.Fillage == FillCategory.Full))
				{
					if (array[intVec2.x - cellRect.minX, intVec2.z - cellRect.minZ] || valueOrDefault2)
					{
						parms.sketch.AddTerrain(terrainDef, intVec2, false);
					}
					else
					{
						parms.sketch.AddTerrain(terrainDef2, intVec2, false);
					}
				}
			}
		}

		// Token: 0x0601411F RID: 82207 RVA: 0x005F7780 File Offset: 0x005F5980
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			TerrainDef terrainDef;
			TerrainDef terrainDef2;
			return SketchResolver_FloorFill.TryFindFloors(out terrainDef, out terrainDef2, parms);
		}

		// Token: 0x06014120 RID: 82208 RVA: 0x005F779C File Offset: 0x005F599C
		private static bool TryFindFloors(out TerrainDef floor1, out TerrainDef floor2, SketchResolveParams parms)
		{
			Predicate<TerrainDef> validator = (TerrainDef x) => SketchGenUtility.IsFloorAllowed(x, parms.allowWood ?? true, parms.allowConcrete ?? true, parms.useOnlyStonesAvailableOnMap, parms.onlyBuildableByPlayer.GetValueOrDefault(), parms.onlyStoneFloors ?? true);
			if (!BaseGenUtility.TryRandomInexpensiveFloor(out floor1, validator))
			{
				floor2 = null;
				return false;
			}
			if (parms.singleFloorType.GetValueOrDefault())
			{
				floor2 = null;
				return true;
			}
			TerrainDef floor1Local = floor1;
			return BaseGenUtility.TryRandomInexpensiveFloor(out floor2, (TerrainDef x) => x != floor1Local && (validator == null || validator(x)));
		}

		// Token: 0x06014121 RID: 82209 RVA: 0x005F780C File Offset: 0x005F5A0C
		private void FloorFillRoom(IntVec3 c, HashSet<IntVec3> walls, HashSet<IntVec3> visited, Sketch sketch, TerrainDef def1, TerrainDef def2, CellRect outerRect, bool singleFloorType)
		{
			if (visited.Contains(c))
			{
				return;
			}
			SketchResolver_FloorFill.tmpCells.Clear();
			SketchResolver_FloorFill.tmpStack.Clear();
			SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(c.x, c.z));
			visited.Add(c);
			int num = c.x;
			int num2 = c.x;
			int num3 = c.z;
			int num4 = c.z;
			while (SketchResolver_FloorFill.tmpStack.Count != 0)
			{
				Pair<int, int> pair = SketchResolver_FloorFill.tmpStack.Pop();
				int first = pair.First;
				int second = pair.Second;
				SketchResolver_FloorFill.tmpCells.Add(new IntVec3(first, 0, second));
				num = Mathf.Min(num, first);
				num2 = Mathf.Max(num2, first);
				num3 = Mathf.Min(num3, second);
				num4 = Mathf.Max(num4, second);
				if (first > outerRect.minX && !walls.Contains(new IntVec3(first - 1, 0, second)) && !visited.Contains(new IntVec3(first - 1, 0, second)))
				{
					visited.Add(new IntVec3(first - 1, 0, second));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first - 1, second));
				}
				if (second > outerRect.minZ && !walls.Contains(new IntVec3(first, 0, second - 1)) && !visited.Contains(new IntVec3(first, 0, second - 1)))
				{
					visited.Add(new IntVec3(first, 0, second - 1));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first, second - 1));
				}
				if (first < outerRect.maxX && !walls.Contains(new IntVec3(first + 1, 0, second)) && !visited.Contains(new IntVec3(first + 1, 0, second)))
				{
					visited.Add(new IntVec3(first + 1, 0, second));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first + 1, second));
				}
				if (second < outerRect.maxZ && !walls.Contains(new IntVec3(first, 0, second + 1)) && !visited.Contains(new IntVec3(first, 0, second + 1)))
				{
					visited.Add(new IntVec3(first, 0, second + 1));
					SketchResolver_FloorFill.tmpStack.Push(new Pair<int, int>(first, second + 1));
				}
			}
			for (int i = 0; i < SketchResolver_FloorFill.tmpCells.Count; i++)
			{
				if (outerRect.IsOnEdge(SketchResolver_FloorFill.tmpCells[i]))
				{
					return;
				}
			}
			CellRect cellRect = CellRect.FromLimits(num, num3, num2, num4);
			bool[,] array = AbstractShapeGenerator.Generate(cellRect.Width, cellRect.Height, true, true, false, true, false, 0f);
			for (int j = 0; j < SketchResolver_FloorFill.tmpCells.Count; j++)
			{
				IntVec3 intVec = SketchResolver_FloorFill.tmpCells[j];
				if (!sketch.ThingsAt(intVec).Any((SketchThing x) => x.def.passability == Traversability.Impassable && x.def.Fillage == FillCategory.Full))
				{
					if (array[intVec.x - cellRect.minX, intVec.z - cellRect.minZ] || singleFloorType)
					{
						sketch.AddTerrain(def1, intVec, false);
					}
					else
					{
						sketch.AddTerrain(def2, intVec, false);
					}
				}
			}
		}

		// Token: 0x0400DDA4 RID: 56740
		private static HashSet<IntVec3> tmpWalls = new HashSet<IntVec3>();

		// Token: 0x0400DDA5 RID: 56741
		private static HashSet<IntVec3> tmpVisited = new HashSet<IntVec3>();

		// Token: 0x0400DDA6 RID: 56742
		private static Stack<Pair<int, int>> tmpStack = new Stack<Pair<int, int>>();

		// Token: 0x0400DDA7 RID: 56743
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
