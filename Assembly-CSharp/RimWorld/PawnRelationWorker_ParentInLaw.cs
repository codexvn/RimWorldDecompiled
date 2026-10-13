using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200242C RID: 9260
	public class PawnRelationWorker_ParentInLaw : PawnRelationWorker
	{
		// Token: 0x0600C86A RID: 51306 RVA: 0x003A85EC File Offset: 0x003A67EC
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
			PawnRelationWorker worker = PawnRelationDefOf.Parent.Worker;
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
