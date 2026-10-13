using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200178E RID: 6030
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x06008D65 RID: 36197 RVA: 0x002A0ABF File Offset: 0x0029ECBF
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
