using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002432 RID: 9266
	public class PawnRelationWorker_UncleOrAunt : PawnRelationWorker
	{
		// Token: 0x0600C87C RID: 51324 RVA: 0x003A8E24 File Offset: 0x003A7024
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
			PawnRelationWorker worker = PawnRelationDefOf.Grandparent.Worker;
			return (me.GetMother() != null && (other.HasSameMother(me.GetMother()) || other.HasSameFather(me.GetMother()))) || (me.GetFather() != null && (other.HasSameMother(me.GetFather()) || other.HasSameFather(me.GetFather())));
		}
	}
}
