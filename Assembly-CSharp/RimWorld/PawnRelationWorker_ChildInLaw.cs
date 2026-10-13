using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200241A RID: 9242
	public class PawnRelationWorker_ChildInLaw : PawnRelationWorker
	{
		// Token: 0x0600C83B RID: 51259 RVA: 0x003A7D34 File Offset: 0x003A5F34
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (other.GetSpouseCount(true) == 0)
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Child.Worker;
			if (worker.InRelation(me, other))
			{
				return false;
			}
			foreach (Pawn other2 in other.GetSpouses(true))
			{
				if (worker.InRelation(me, other2))
				{
					return true;
				}
			}
			return false;
		}
	}
}
