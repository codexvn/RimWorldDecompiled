using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017AE RID: 6062
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x06008DA9 RID: 36265 RVA: 0x002A0F4E File Offset: 0x0029F14E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
