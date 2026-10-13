using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017B1 RID: 6065
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06008DB0 RID: 36272 RVA: 0x002A102A File Offset: 0x0029F22A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
