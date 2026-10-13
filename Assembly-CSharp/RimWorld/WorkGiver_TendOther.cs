using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161F RID: 5663
	public class WorkGiver_TendOther : WorkGiver_Tend
	{
		// Token: 0x06008596 RID: 34198 RVA: 0x0027E940 File Offset: 0x0027CB40
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && pawn != t;
		}
	}
}
