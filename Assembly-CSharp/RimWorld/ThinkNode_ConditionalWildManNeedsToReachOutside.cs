using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017BE RID: 6078
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		// Token: 0x06008DCD RID: 36301 RVA: 0x002A138A File Offset: 0x0029F58A
		protected override bool Satisfied(Pawn pawn)
		{
			return WildManUtility.WildManShouldReachOutsideNow(pawn);
		}
	}
}
