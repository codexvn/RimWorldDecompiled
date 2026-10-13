using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C7 RID: 6087
	public class ThinkNode_ConditionalWantsLookChange : ThinkNode_Conditional
	{
		// Token: 0x06008DE0 RID: 36320 RVA: 0x002A1723 File Offset: 0x0029F923
		protected override bool Satisfied(Pawn pawn)
		{
			return ModsConfig.IdeologyActive && pawn.style != null && pawn.style.LookChangeDesired;
		}
	}
}
