using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002957 RID: 10583
	public class Blueprint_StorageWithRoomHighlight : Blueprint_Storage
	{
		// Token: 0x0600E819 RID: 59417 RVA: 0x00439604 File Offset: 0x00437804
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (Find.Selector.SingleSelectedThing == this)
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room != null && room.ProperRoom && !room.PsychologicallyOutdoors)
				{
					room.DrawFieldEdges();
				}
			}
		}
	}
}
