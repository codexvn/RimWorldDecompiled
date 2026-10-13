using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F52 RID: 8018
	public class RoomPart_CornerThing : RoomPartWorker
	{
		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x0600B143 RID: 45379 RVA: 0x0032AAB7 File Offset: 0x00328CB7
		public new RoomPart_ThingDef def
		{
			get
			{
				return (RoomPart_ThingDef)this.def;
			}
		}

		// Token: 0x0600B144 RID: 45380 RVA: 0x00329C8D File Offset: 0x00327E8D
		public RoomPart_CornerThing(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x0600B145 RID: 45381 RVA: 0x0032AAC4 File Offset: 0x00328CC4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			RoomGenUtility.TryPlaceInRandomCorner(map, room, this.def.thingDef, faction ?? Faction.OfAncientsHostile, this.def.stuffDef);
		}
	}
}
