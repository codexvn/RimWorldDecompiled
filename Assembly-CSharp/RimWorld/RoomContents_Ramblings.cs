using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F10 RID: 7952
	public class RoomContents_Ramblings : RoomContentsWorker
	{
		// Token: 0x0600AFF7 RID: 45047 RVA: 0x00322644 File Offset: 0x00320844
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 1, 0, null, false))
			{
				return;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.FloorEtchingRambling, null), loc, map, WipeMode.Vanish);
		}
	}
}
