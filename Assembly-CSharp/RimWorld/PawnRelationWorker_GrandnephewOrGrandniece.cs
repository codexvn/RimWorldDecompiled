using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002421 RID: 9249
	public class PawnRelationWorker_GrandnephewOrGrandniece : PawnRelationWorker
	{
		// Token: 0x0600C84E RID: 51278 RVA: 0x003A8030 File Offset: 0x003A6230
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.NephewOrNiece.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
