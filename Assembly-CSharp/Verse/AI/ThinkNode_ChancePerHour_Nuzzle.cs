using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FD8 RID: 4056
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06006433 RID: 25651 RVA: 0x001FD0F5 File Offset: 0x001FB2F5
		protected override float MtbHours(Pawn pawn)
		{
			return NuzzleUtility.GetNuzzleMTBHours(pawn);
		}
	}
}
