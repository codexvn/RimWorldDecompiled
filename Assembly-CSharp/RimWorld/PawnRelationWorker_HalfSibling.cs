using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002426 RID: 9254
	public class PawnRelationWorker_HalfSibling : PawnRelationWorker
	{
		// Token: 0x0600C858 RID: 51288 RVA: 0x003A8164 File Offset: 0x003A6364
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && !PawnRelationDefOf.Sibling.Worker.InRelation(me, other) && (me.HasSameMother(other) || me.HasSameFather(other));
		}
	}
}
