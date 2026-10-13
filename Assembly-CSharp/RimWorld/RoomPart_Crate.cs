using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F54 RID: 8020
	public class RoomPart_Crate : RoomPartWorker
	{
		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x0600B149 RID: 45385 RVA: 0x0032AB58 File Offset: 0x00328D58
		public new RoomPart_CrateDef def
		{
			get
			{
				return (RoomPart_CrateDef)this.def;
			}
		}

		// Token: 0x0600B14A RID: 45386 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_Crate(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B14B RID: 45387 RVA: 0x0032AB68 File Offset: 0x00328D68
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			string signal = null;
			if (this.def.triggerThreatSignal)
			{
				signal = room.ThreatSignal;
			}
			RoomGenUtility.SpawnCratesInRoom(this.def.crateDef, room, map, this.def.thingSetMaker, IntRange.One, true, signal, new Rot4?(this.def.rotations.Random()));
		}
	}
}
