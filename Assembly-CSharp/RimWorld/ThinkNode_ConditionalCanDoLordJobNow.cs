using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017B4 RID: 6068
	public class ThinkNode_ConditionalCanDoLordJobNow : ThinkNode_Conditional
	{
		// Token: 0x06008DB6 RID: 36278 RVA: 0x002A1173 File Offset: 0x0029F373
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake() && (!pawn.Downed || pawn.DutyActiveWhenDown(false));
		}
	}
}
