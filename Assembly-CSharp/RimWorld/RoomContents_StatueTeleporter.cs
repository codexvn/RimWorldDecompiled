using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F14 RID: 7956
	public class RoomContents_StatueTeleporter : RoomContentsWorker
	{
		// Token: 0x0600B002 RID: 45058 RVA: 0x003227F4 File Offset: 0x003209F4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(ThingDefOf.GrayStatueTeleporter, map, out centerCell, null, 4, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GrayStatueTeleporter, null), centerCell, map, WipeMode.Vanish);
		}
	}
}
