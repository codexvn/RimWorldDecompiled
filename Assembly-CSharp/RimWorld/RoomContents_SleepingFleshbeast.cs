using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F11 RID: 7953
	public class RoomContents_SleepingFleshbeast : RoomContentsWorker
	{
		// Token: 0x0600AFF9 RID: 45049 RVA: 0x00322680 File Offset: 0x00320880
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 1, 0, null, false))
			{
				return;
			}
			CompCanBeDormant compCanBeDormant;
			if (GenSpawn.Spawn(PawnGenerator.GeneratePawn(PawnKindDefOf.Fingerspike, Faction.OfEntities, null), loc, map, WipeMode.Vanish).TryGetComp(out compCanBeDormant))
			{
				compCanBeDormant.ToSleep();
			}
			for (int i = 0; i < 5; i++)
			{
				FilthMaker.TryMakeFilth(room.Cells.RandomElement<IntVec3>(), map, ThingDefOf.Filth_Blood, 1, FilthSourceFlags.None, true);
			}
		}
	}
}
