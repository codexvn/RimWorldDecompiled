using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002785 RID: 10117
	public static class CommonRitualCellPredicates
	{
		// Token: 0x0600DDC7 RID: 56775 RVA: 0x00414874 File Offset: 0x00412A74
		public static bool RemoveLeastDesirableRitualCells(List<IntVec3> cells, IntVec3 spot, Map map, Pawn pawn, CellRect rect)
		{
			cells.RemoveAll(rect, CommonRitualCellPredicates.InsideRect, false);
			cells.RemoveAll(map, CommonRitualCellPredicates.Standable, true);
			cells.RemoveAll(new ValueTuple<Map, IntVec3>(map, spot), CommonRitualCellPredicates.InSameRoomAsSpot, true);
			cells.RemoveAll(pawn, CommonRitualCellPredicates.Reachable, true);
			return cells.RemoveAll_IfNotAll(map, CommonRitualCellPredicates.NotOnBed) && cells.RemoveAll_IfNotAll(map, CommonRitualCellPredicates.NotInDoor);
		}

		// Token: 0x0600DDC8 RID: 56776 RVA: 0x004148DC File Offset: 0x00412ADC
		public static Func<IntVec3, bool> DefaultValidator(IntVec3 spot, Map map, Pawn pawn, CellRect rect)
		{
			return (IntVec3 cell) => !CommonRitualCellPredicates.InsideRect(rect, cell) && CommonRitualCellPredicates.Standable(map, cell) && CommonRitualCellPredicates.InSameRoomAsSpot(new ValueTuple<Map, IntVec3>(map, spot), cell) && CommonRitualCellPredicates.Reachable(pawn, cell);
		}

		// Token: 0x0600DDC9 RID: 56777 RVA: 0x0041490C File Offset: 0x00412B0C
		private static bool _InSameRoomAsSpot(ValueTuple<Map, IntVec3> context, IntVec3 c)
		{
			Map item = context.Item1;
			IntVec3 item2 = context.Item2;
			return WanderUtility.InSameRoom(c, item2, item);
		}

		// Token: 0x0400958F RID: 38287
		public static readonly Predicate<CellRect, IntVec3> InsideRect = (CellRect rect, IntVec3 cell) => rect.Contains(cell);

		// Token: 0x04009590 RID: 38288
		public static readonly Predicate<Pawn, IntVec3> Reachable = (Pawn pawn, IntVec3 cell) => pawn.CanReserveAndReach(cell, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false);

		// Token: 0x04009591 RID: 38289
		public static readonly Predicate<Map, IntVec3> Standable = (Map map, IntVec3 cell) => cell.Standable(map);

		// Token: 0x04009592 RID: 38290
		public static readonly Predicate<Map, IntVec3> OnBed = delegate(Map map, IntVec3 cell)
		{
			Building building = map.edificeGrid[cell];
			if (building == null)
			{
				return false;
			}
			ThingDef def = building.def;
			bool? flag = (def != null) ? new bool?(def.IsBed) : null;
			bool flag2 = true;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		};

		// Token: 0x04009593 RID: 38291
		public static readonly Predicate<Map, IntVec3> NotOnBed = delegate(Map map, IntVec3 cell)
		{
			Building building = map.edificeGrid[cell];
			if (building == null)
			{
				return true;
			}
			ThingDef def = building.def;
			bool? flag = (def != null) ? new bool?(def.IsBed) : null;
			bool flag2 = true;
			return !(flag.GetValueOrDefault() == flag2 & flag != null);
		};

		// Token: 0x04009594 RID: 38292
		public static readonly Predicate<Map, IntVec3> InDoor = (Map map, IntVec3 cell) => cell.GetDoor(map) != null;

		// Token: 0x04009595 RID: 38293
		public static readonly Predicate<Map, IntVec3> NotInDoor = (Map map, IntVec3 cell) => cell.GetDoor(map) == null;

		// Token: 0x04009596 RID: 38294
		public static readonly Predicate<ValueTuple<Map, IntVec3>, IntVec3> InSameRoomAsSpot = new Predicate<ValueTuple<Map, IntVec3>, IntVec3>(CommonRitualCellPredicates._InSameRoomAsSpot);
	}
}
