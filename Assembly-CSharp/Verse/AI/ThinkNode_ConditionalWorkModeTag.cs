using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FEB RID: 4075
	public class ThinkNode_ConditionalWorkModeTag : ThinkNode_Conditional
	{
		// Token: 0x06006462 RID: 25698 RVA: 0x001FD4D4 File Offset: 0x001FB6D4
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.RaceProps.IsMechanoid && pawn.Faction == Faction.OfPlayer && pawn.relations != null)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
				return ((firstDirectRelationPawn != null) ? firstDirectRelationPawn.mechanitor.GetControlGroup(pawn).GetTag(pawn) : null) == this.tag;
			}
			return false;
		}
	}
}
