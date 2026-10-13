using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F55 RID: 8021
	public class RoomPart_DormantInsectCluster : RoomPartWorker
	{
		// Token: 0x0600B14C RID: 45388 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_DormantInsectCluster(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B14D RID: 45389 RVA: 0x0032ABC4 File Offset: 0x00328DC4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (Faction.OfInsects != null)
			{
				RoomGenUtility.SpawnDormantInsectCluster(room, map, threatPoints, room.ThreatSignal);
			}
		}
	}
}
