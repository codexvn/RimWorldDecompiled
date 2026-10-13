using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200278D RID: 10125
	public class RitualPosition_BehindThingCenter : RitualPosition_VerticalThingCenter
	{
		// Token: 0x0600DDE5 RID: 56805 RVA: 0x00414E74 File Offset: 0x00413074
		protected override CellRect GetRect(CellRect thingRect)
		{
			IntVec3 intVec = IntVec3.North + this.offset;
			return new CellRect(thingRect.minX + intVec.x, thingRect.maxZ + intVec.z, thingRect.Width, 1);
		}
	}
}
