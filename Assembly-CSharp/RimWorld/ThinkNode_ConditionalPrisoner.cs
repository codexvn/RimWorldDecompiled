using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200178C RID: 6028
	public class ThinkNode_ConditionalPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06008D61 RID: 36193 RVA: 0x001F192B File Offset: 0x001EFB2B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
