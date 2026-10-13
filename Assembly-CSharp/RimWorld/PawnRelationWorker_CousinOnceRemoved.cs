using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200241C RID: 9244
	public class PawnRelationWorker_CousinOnceRemoved : PawnRelationWorker
	{
		// Token: 0x0600C83F RID: 51263 RVA: 0x003A7E0C File Offset: 0x003A600C
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Cousin.Worker;
			if ((other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather())))
			{
				return true;
			}
			PawnRelationWorker worker2 = PawnRelationDefOf.GranduncleOrGrandaunt.Worker;
			return (other.GetMother() != null && worker2.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker2.InRelation(me, other.GetFather()));
		}
	}
}
