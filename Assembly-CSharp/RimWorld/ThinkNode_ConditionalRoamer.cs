using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C5 RID: 6085
	public class ThinkNode_ConditionalRoamer : ThinkNode_Conditional
	{
		// Token: 0x06008DDB RID: 36315 RVA: 0x002A169A File Offset: 0x0029F89A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Roamer;
		}
	}
}
