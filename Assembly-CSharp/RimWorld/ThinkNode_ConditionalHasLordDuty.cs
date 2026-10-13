using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200179A RID: 6042
	public class ThinkNode_ConditionalHasLordDuty : ThinkNode_Conditional
	{
		// Token: 0x06008D7E RID: 36222 RVA: 0x002A0BE5 File Offset: 0x0029EDE5
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null && pawn.GetLord().CurLordToil.AssignsDuties;
		}
	}
}
