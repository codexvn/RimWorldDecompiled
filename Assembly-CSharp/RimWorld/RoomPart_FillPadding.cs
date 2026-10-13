using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F58 RID: 8024
	public class RoomPart_FillPadding : RoomPartWorker
	{
		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x0600B152 RID: 45394 RVA: 0x0032AAB7 File Offset: 0x00328CB7
		public new RoomPart_ThingDef def
		{
			get
			{
				return (RoomPart_ThingDef)this.def;
			}
		}

		// Token: 0x0600B153 RID: 45395 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_FillPadding(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B154 RID: 45396 RVA: 0x0032AC08 File Offset: 0x00328E08
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomPart_FillPadding.<>c__DisplayClass3_0 CS$<>8__locals1 = new RoomPart_FillPadding.<>c__DisplayClass3_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.map = map;
			ThingDef thingDef = this.def.thingDef;
			int count = 1;
			Map map2 = CS$<>8__locals1.map;
			Func<IntVec3, Rot4, CellRect, bool> validator = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<FillRoom>g__Validator|0);
			ThingDef stuffDef = this.def.stuffDef;
			RoomGenUtility.FillWithPadding(thingDef, count, room, map2, null, validator, null, 1, stuffDef, false, false, null, null);
		}
	}
}
