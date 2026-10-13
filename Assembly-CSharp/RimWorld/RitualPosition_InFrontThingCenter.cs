using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200278E RID: 10126
	public class RitualPosition_InFrontThingCenter : RitualPosition_VerticalThingCenter
	{
		// Token: 0x0600DDE7 RID: 56807 RVA: 0x00414EC4 File Offset: 0x004130C4
		protected override CellRect GetRect(CellRect thingRect)
		{
			IntVec3 intVec = IntVec3.South + this.offset;
			return new CellRect(thingRect.minX + intVec.x, thingRect.minZ + intVec.z, thingRect.Width, 1);
		}
	}
}
