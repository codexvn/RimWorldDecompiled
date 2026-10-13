using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FAC RID: 8108
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x0600B2F9 RID: 45817 RVA: 0x0033461D File Offset: 0x0033281D
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		// Token: 0x0600B2FA RID: 45818 RVA: 0x00334624 File Offset: 0x00332824
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_FindPlayerStartSpot.<>c__DisplayClass3_0 CS$<>8__locals1 = new GenStep_FindPlayerStartSpot.<>c__DisplayClass3_0();
			CS$<>8__locals1.map = map;
			if (CS$<>8__locals1.map.wasSpawnedViaGravShipLanding)
			{
				return;
			}
			DeepProfiler.Start("RebuildAllRegions");
			CS$<>8__locals1.map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			if (MapGenerator.PlayerStartSpotValid)
			{
				return;
			}
			CS$<>8__locals1.largestOpenArea = this.FindLargestContiguousOpenArea(CS$<>8__locals1.map);
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(CS$<>8__locals1.map, 7, 10, new Predicate<IntVec3>(CS$<>8__locals1.<Generate>g__Validator|0), false);
		}

		// Token: 0x0600B2FB RID: 45819 RVA: 0x003346B8 File Offset: 0x003328B8
		private HashSet<IntVec3> FindLargestContiguousOpenArea(Map map)
		{
			HashSet<IntVec3> checkedCells = new HashSet<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			HashSet<IntVec3> current = new HashSet<IntVec3>();
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			using (IEnumerator<IntVec3> enumerator = map.AllCells.GetEnumerator())
			{
				Action<IntVec3> <>9__1;
				while (enumerator.MoveNext())
				{
					IntVec3 cell = enumerator.Current;
					if (cell.GetEdifice(map) == null && !checkedCells.Contains(cell))
					{
						current.Clear();
						FloodFiller floodFiller = map.floodFiller;
						IntVec3 cell2 = cell;
						Predicate<IntVec3> passCheck = (IntVec3 x) => cell.GetEdifice(map) == null;
						Action<IntVec3> processor;
						if ((processor = <>9__1) == null)
						{
							processor = (<>9__1 = delegate(IntVec3 x)
							{
								current.Add(x);
								checkedCells.Add(x);
							});
						}
						floodFiller.FloodFill(cell2, passCheck, processor, int.MaxValue, false, null);
						if (current.Count > hashSet.Count)
						{
							hashSet.Clear();
							hashSet.AddRange(current);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x04007A98 RID: 31384
		private const int MinRoomCellCount = 10;
	}
}
