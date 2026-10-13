using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4D RID: 8013
	public class RoomPart_Barricades : RoomPartWorker
	{
		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x0600B12F RID: 45359 RVA: 0x00329FDB File Offset: 0x003281DB
		public new RoomPart_BarricadeDef def
		{
			get
			{
				return (RoomPart_BarricadeDef)this.def;
			}
		}

		// Token: 0x0600B130 RID: 45360 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_Barricades(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B131 RID: 45361 RVA: 0x00329FE8 File Offset: 0x003281E8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomGenUtility.SpawnDoorBarricades(this.def.wallDef, room, map, this.def.chancePerDoor, this.def.stuffDef, this.def.steps, this.def.offset);
		}
	}
}
