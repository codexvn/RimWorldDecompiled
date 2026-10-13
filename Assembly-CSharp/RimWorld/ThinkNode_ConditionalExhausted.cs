using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017AC RID: 6060
	public class ThinkNode_ConditionalExhausted : ThinkNode_Conditional
	{
		// Token: 0x06008DA5 RID: 36261 RVA: 0x002A0F01 File Offset: 0x0029F101
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.Exhausted;
		}
	}
}
