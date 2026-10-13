using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ECD RID: 7885
	public class RoomContents_ComputerRoom : RoomContentsWorker
	{
		// Token: 0x0600AECE RID: 44750 RVA: 0x0031E2CC File Offset: 0x0031C4CC
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			IntVec3 loc;
			if (room.TryGetRandomCellInRoom(ThingDefOf.AncientMachine, map, out loc, null, 0, 1, null, false))
			{
				GenSpawn.Spawn(ThingDefOf.AncientMachine, loc, map, WipeMode.Vanish);
			}
			base.FillRoom(map, room, faction, threatPoints);
		}
	}
}
