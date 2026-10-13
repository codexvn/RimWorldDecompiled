using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002429 RID: 9257
	public class PawnRelationWorker_NephewOrNiece : PawnRelationWorker
	{
		// Token: 0x0600C85F RID: 51295 RVA: 0x003A8224 File Offset: 0x003A6424
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Child.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Sibling.Worker;
			PawnRelationWorker worker2 = PawnRelationDefOf.HalfSibling.Worker;
			return (other.GetMother() != null && (worker.InRelation(me, other.GetMother()) || worker2.InRelation(me, other.GetMother()))) || (other.GetFather() != null && (worker.InRelation(me, other.GetFather()) || worker2.InRelation(me, other.GetFather())));
		}
	}
}
