using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200241B RID: 9243
	public class PawnRelationWorker_Cousin : PawnRelationWorker
	{
		// Token: 0x0600C83D RID: 51261 RVA: 0x003A7DBC File Offset: 0x003A5FBC
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.UncleOrAunt.Worker;
			return (other.GetMother() != null && worker.InRelation(me, other.GetMother())) || (other.GetFather() != null && worker.InRelation(me, other.GetFather()));
		}
	}
}
