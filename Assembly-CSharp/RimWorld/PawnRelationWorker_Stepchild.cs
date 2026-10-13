using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002430 RID: 9264
	public class PawnRelationWorker_Stepchild : PawnRelationWorker
	{
		// Token: 0x0600C878 RID: 51320 RVA: 0x003A8D34 File Offset: 0x003A6F34
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (me.GetSpouseCount(true) == 0)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			if (worker.InRelation(me, other))
			{
				return false;
			}
			foreach (Pawn me2 in me.GetSpouses(true))
			{
				if (worker.InRelation(me2, other))
				{
					return true;
				}
			}
			return false;
		}
	}
}
