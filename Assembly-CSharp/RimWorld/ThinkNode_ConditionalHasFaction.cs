using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A0 RID: 6048
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x06008D8B RID: 36235 RVA: 0x002A0D2E File Offset: 0x0029EF2E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
