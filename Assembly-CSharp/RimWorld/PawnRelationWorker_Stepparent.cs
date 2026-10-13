using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002431 RID: 9265
	public class PawnRelationWorker_Stepparent : PawnRelationWorker
	{
		// Token: 0x0600C87A RID: 51322 RVA: 0x003A8DBC File Offset: 0x003A6FBC
		public override bool InRelation(Pawn me, Pawn other)
		{
			if (me == other)
			{
				return false;
			}
			if (PawnRelationDefOf.Parent.Worker.InRelation(me, other))
			{
				return false;
			}
			PawnRelationWorker worker = PawnRelationDefOf.Spouse.Worker;
			return (me.GetMother() != null && worker.InRelation(me.GetMother(), other)) || (me.GetFather() != null && worker.InRelation(me.GetFather(), other));
		}
	}
}
