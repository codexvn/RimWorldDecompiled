using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002422 RID: 9250
	public class PawnRelationWorker_Grandparent : PawnRelationWorker
	{
		// Token: 0x0600C850 RID: 51280 RVA: 0x003A807F File Offset: 0x003A627F
		public override bool InRelation(Pawn me, Pawn other)
		{
			return me != other && PawnRelationDefOf.Grandchild.Worker.InRelation(other, me);
		}
	}
}
