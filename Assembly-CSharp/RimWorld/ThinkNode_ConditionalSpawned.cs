using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D0 RID: 6096
	public class ThinkNode_ConditionalSpawned : ThinkNode_Conditional
	{
		// Token: 0x06008DF4 RID: 36340 RVA: 0x0020FBF8 File Offset: 0x0020DDF8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Spawned;
		}
	}
}
