using System;

namespace Verse.AI
{
	// Token: 0x02000FDA RID: 4058
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06006437 RID: 25655 RVA: 0x001FD122 File Offset: 0x001FB322
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
