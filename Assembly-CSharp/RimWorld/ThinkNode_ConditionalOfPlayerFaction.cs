using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001791 RID: 6033
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x06008D6B RID: 36203 RVA: 0x0028B748 File Offset: 0x00289948
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
