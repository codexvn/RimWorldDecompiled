using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017AF RID: 6063
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x06008DAB RID: 36267 RVA: 0x002A0F7C File Offset: 0x0029F17C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
