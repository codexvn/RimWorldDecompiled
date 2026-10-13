using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F09 RID: 7945
	public class RoomContents_Etchings : RoomContentsWorker
	{
		// Token: 0x0600AFE3 RID: 45027 RVA: 0x003221E0 File Offset: 0x003203E0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 1, 0, null, false))
			{
				return;
			}
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.FloorEtching, null), loc, map, WipeMode.Vanish);
		}
	}
}
