using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F57 RID: 8023
	public class RoomPart_DormantThreatCluster : RoomPartWorker
	{
		// Token: 0x0600B150 RID: 45392 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_DormantThreatCluster(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B151 RID: 45393 RVA: 0x0032ABF4 File Offset: 0x00328DF4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomGenUtility.SpawnDormantThreatCluster(room, map, threatPoints, room.ThreatSignal);
		}
	}
}
