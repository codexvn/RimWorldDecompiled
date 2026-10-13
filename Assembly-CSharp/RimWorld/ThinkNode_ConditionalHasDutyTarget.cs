using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017AD RID: 6061
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06008DA7 RID: 36263 RVA: 0x002A0F28 File Offset: 0x0029F128
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
