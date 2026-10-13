using System;

namespace Verse.AI
{
	// Token: 0x02000FDC RID: 4060
	public class ThinkNode_ChancePerHour_InsectDigChance : ThinkNode_ChancePerHour
	{
		// Token: 0x0600643B RID: 25659 RVA: 0x001FD164 File Offset: 0x001FB364
		protected override float MtbHours(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return 18f;
			}
			int num = room.IsHuge ? 9999 : room.CellCount;
			float num2 = GenMath.LerpDoubleClamped(2f, 25f, 6f, 1f, (float)num);
			return 18f / num2;
		}

		// Token: 0x040047CB RID: 18379
		private const float BaseMtbHours = 18f;
	}
}
