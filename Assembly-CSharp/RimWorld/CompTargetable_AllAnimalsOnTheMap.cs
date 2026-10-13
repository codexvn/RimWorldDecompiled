using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F20 RID: 12064
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		// Token: 0x0601108C RID: 69772 RVA: 0x004E61D0 File Offset: 0x004E43D0
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Thing as Pawn;
			return pawn != null && pawn.IsAnimal && base.ValidateTarget(target.Thing, showMessages);
		}
	}
}
