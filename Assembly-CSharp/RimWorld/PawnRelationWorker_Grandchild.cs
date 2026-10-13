using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002420 RID: 9248
	public class PawnRelationWorker_Grandchild : PawnRelationWorker
	{
		// Token: 0x0600C84C RID: 51276 RVA: 0x003A7FE0 File Offset: 0x003A61E0
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
