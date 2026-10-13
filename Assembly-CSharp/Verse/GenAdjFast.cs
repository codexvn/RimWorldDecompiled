using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000F0 RID: 240
	public static class GenAdjFast
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x00021573 File Offset: 0x0001F773
		public static List<IntVec3> AdjacentCells8Way(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCells8Way((Thing)pack);
			}
			return GenAdjFast.AdjacentCells8Way((IntVec3)pack);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00021598 File Offset: 0x0001F798
		public static List<IntVec3> AdjacentCells8Way(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 8; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.AdjacentCells[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x000215F9 File Offset: 0x0001F7F9
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00021618 File Offset: 0x0001F818
		public static List<IntVec3> AdjacentCells8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCells8Way(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num - 1, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2);
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4);
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num);
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00021749 File Offset: 0x0001F949
		public static List<IntVec3> AdjacentCellsCardinal(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCellsCardinal((Thing)pack);
			}
			return GenAdjFast.AdjacentCellsCardinal((IntVec3)pack);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002176C File Offset: 0x0001F96C
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 4; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.CardinalDirections[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000217CD File Offset: 0x0001F9CD
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x000217EC File Offset: 0x0001F9EC
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCellsCardinal(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2 - 1);
			intVec.x++;
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4 - 1);
			intVec.z++;
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num + 1);
			intVec.x--;
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00021948 File Offset: 0x0001FB48
		public static void AdjacentThings8Way(Thing thing, List<Thing> outThings)
		{
			outThings.Clear();
			if (!thing.Spawned)
			{
				return;
			}
			Map map = thing.Map;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(thing);
			for (int i = 0; i < list.Count; i++)
			{
				List<Thing> thingList = list[i].GetThingList(map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (!outThings.Contains(thingList[j]))
					{
						outThings.Add(thingList[j]);
					}
				}
			}
		}

		// Token: 0x040003A6 RID: 934
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x040003A7 RID: 935
		private static bool working = false;
	}
}
