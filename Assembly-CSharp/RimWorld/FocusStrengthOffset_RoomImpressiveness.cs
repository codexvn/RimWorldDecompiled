using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D51 RID: 11601
	public class FocusStrengthOffset_RoomImpressiveness : FocusStrengthOffset_Curve
	{
		// Token: 0x060104D2 RID: 66770 RVA: 0x004B5538 File Offset: 0x004B3738
		protected override float SourceValue(Thing parent)
		{
			Room room = parent.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return 0f;
			}
			return room.GetStat(RoomStatDefOf.Impressiveness);
		}

		// Token: 0x17002976 RID: 10614
		// (get) Token: 0x060104D3 RID: 66771 RVA: 0x004B5556 File Offset: 0x004B3756
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_RoomImpressiveness";
			}
		}
	}
}
