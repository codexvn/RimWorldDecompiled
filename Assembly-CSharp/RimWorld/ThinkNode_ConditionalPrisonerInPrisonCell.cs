using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200178D RID: 6029
	public class ThinkNode_ConditionalPrisonerInPrisonCell : ThinkNode_Conditional
	{
		// Token: 0x06008D63 RID: 36195 RVA: 0x002A0A94 File Offset: 0x0029EC94
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.IsPrisoner)
			{
				return false;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			return room != null && room.IsPrisonCell;
		}
	}
}
