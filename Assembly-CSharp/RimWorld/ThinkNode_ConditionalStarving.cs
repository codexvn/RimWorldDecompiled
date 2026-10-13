using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A8 RID: 6056
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		// Token: 0x06008D9B RID: 36251 RVA: 0x002A0DDF File Offset: 0x0029EFDF
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && pawn.needs.food.CurCategory >= HungerCategory.Starving;
		}
	}
}
