using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200178A RID: 6026
	public class ThinkNode_ConditionalCanCrawl : ThinkNode_Conditional
	{
		// Token: 0x06008D5D RID: 36189 RVA: 0x002A0A20 File Offset: 0x0029EC20
		protected override bool Satisfied(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return (lord == null || !(lord.LordJob is LordJob_PsychicRitual)) && pawn.health.CanCrawl;
		}
	}
}
