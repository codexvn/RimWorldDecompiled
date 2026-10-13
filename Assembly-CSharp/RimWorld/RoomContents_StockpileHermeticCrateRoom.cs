using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EEA RID: 7914
	public class RoomContents_StockpileHermeticCrateRoom : RoomContentsWorker
	{
		// Token: 0x0600AF46 RID: 44870 RVA: 0x00320340 File Offset: 0x0031E540
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_StockpileHermeticCrateRoom.SpawnCrate(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF47 RID: 44871 RVA: 0x00320354 File Offset: 0x0031E554
		private static void SpawnCrate(Map map, LayoutRoom room)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 3, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			RoomGenUtility.SpawnHermeticCrate(centerCell, map, Rot4.Random, ThingSetMakerDefOf.MapGen_HighValueCrate, true, null);
		}
	}
}
