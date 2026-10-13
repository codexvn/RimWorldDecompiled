using System;

namespace Verse.AI
{
	// Token: 0x02000FDD RID: 4061
	public class ThinkNode_ChancePerHour_Forage : ThinkNode_ChancePerHour
	{
		// Token: 0x0600643D RID: 25661 RVA: 0x001FD1BC File Offset: 0x001FB3BC
		protected override float MtbHours(Pawn pawn)
		{
			if (pawn.Map.Biome.forageability == 0f)
			{
				return 0f;
			}
			return 72f / pawn.Map.Biome.forageability;
		}
	}
}
