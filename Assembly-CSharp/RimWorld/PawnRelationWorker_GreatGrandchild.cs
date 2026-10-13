using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002424 RID: 9252
	public class PawnRelationWorker_GreatGrandchild : PawnRelationWorker
	{
		// Token: 0x0600C854 RID: 51284 RVA: 0x003A80FC File Offset: 0x003A62FC
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Grandchild.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
