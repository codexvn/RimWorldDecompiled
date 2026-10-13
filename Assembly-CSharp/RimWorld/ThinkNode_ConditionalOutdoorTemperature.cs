using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A7 RID: 6055
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		// Token: 0x06008D99 RID: 36249 RVA: 0x002A0DCC File Offset: 0x0029EFCC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
