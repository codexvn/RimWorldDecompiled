using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A2 RID: 6050
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		// Token: 0x06008D8F RID: 36239 RVA: 0x002A0D44 File Offset: 0x0029EF44
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
