using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002425 RID: 9253
	public class PawnRelationWorker_GreatGrandparent : PawnRelationWorker
	{
		// Token: 0x0600C856 RID: 51286 RVA: 0x003A814B File Offset: 0x003A634B
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.GreatGrandchild.Worker.InRelation(other, me);
		}
	}
}
