using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A5 RID: 6053
	public class ThinkNode_ConditionalBleeding : ThinkNode_Conditional
	{
		// Token: 0x06008D95 RID: 36245 RVA: 0x002A0D8C File Offset: 0x0029EF8C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.health.hediffSet.BleedRateTotal > 0.001f;
		}
	}
}
