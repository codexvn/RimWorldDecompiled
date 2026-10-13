using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001524 RID: 5412
	public class JobGiver_Berserk_Warcall : JobGiver_Berserk
	{
		// Token: 0x060080D5 RID: 32981 RVA: 0x0026CDBB File Offset: 0x0026AFBB
		protected override bool IsGoodTarget(Thing thing)
		{
			return base.IsGoodTarget(thing) || (thing.Spawned && !(thing is Pawn));
		}
	}
}
