using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F61 RID: 8033
	public class RoomPart_SentryDrone : RoomPartWorker
	{
		// Token: 0x0600B16F RID: 45423 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_SentryDrone(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B170 RID: 45424 RVA: 0x0032B1D0 File Offset: 0x003293D0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomPart_SentryDrone.<>c__DisplayClass1_0 CS$<>8__locals1 = new RoomPart_SentryDrone.<>c__DisplayClass1_0();
			CS$<>8__locals1.map = map;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(CS$<>8__locals1.map, out loc, 0, 0, new Func<IntVec3, bool>(CS$<>8__locals1.<FillRoom>g__Validator|0), false))
			{
				Log.Error("Failed to find cell to spawn sentry drone.");
				return;
			}
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(PawnKindDefOf.Drone_Sentry, faction, null), loc, CS$<>8__locals1.map, WipeMode.Vanish);
		}
	}
}
