using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A74 RID: 6772
	public class RoomPart_CrateDef : RoomPartDef
	{
		// Token: 0x060097FC RID: 38908 RVA: 0x002C5BCB File Offset: 0x002C3DCB
		public RoomPart_CrateDef()
		{
			this.workerClass = typeof(RoomPart_Crate);
		}

		// Token: 0x040065A8 RID: 26024
		public ThingDef crateDef;

		// Token: 0x040065A9 RID: 26025
		public ThingSetMakerDef thingSetMaker;

		// Token: 0x040065AA RID: 26026
		public RotEnum rotations = RotEnum.All;

		// Token: 0x040065AB RID: 26027
		public bool triggerThreatSignal = true;
	}
}
