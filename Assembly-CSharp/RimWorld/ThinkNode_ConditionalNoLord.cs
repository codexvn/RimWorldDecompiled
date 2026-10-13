using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020017A1 RID: 6049
	public class ThinkNode_ConditionalNoLord : ThinkNode_Conditional
	{
		// Token: 0x06008D8D RID: 36237 RVA: 0x002A0D39 File Offset: 0x0029EF39
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() == null;
		}
	}
}
