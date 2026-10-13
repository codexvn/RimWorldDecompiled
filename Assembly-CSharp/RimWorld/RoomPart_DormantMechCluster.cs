using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F56 RID: 8022
	public class RoomPart_DormantMechCluster : RoomPartWorker
	{
		// Token: 0x0600B14E RID: 45390 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_DormantMechCluster(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B14F RID: 45391 RVA: 0x0032ABDC File Offset: 0x00328DDC
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (Faction.OfMechanoids != null)
			{
				RoomGenUtility.SpawnDormantMechCluster(room, map, threatPoints, room.ThreatSignal);
			}
		}
	}
}
