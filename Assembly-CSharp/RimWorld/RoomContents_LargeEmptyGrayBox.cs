using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F0D RID: 7949
	public class RoomContents_LargeEmptyGrayBox : RoomContents_LargeGrayBox
	{
		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x0600AFEE RID: 45038 RVA: 0x0002F443 File Offset: 0x0002D643
		protected override int MaxCrates
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x0600AFEF RID: 45039 RVA: 0x003223A3 File Offset: 0x003205A3
		protected override void SpawnBox(IntVec3 cell, Map map)
		{
			RoomContents_GrayBox.SpawnBoxInRoom(cell, map, null, false);
		}
	}
}
