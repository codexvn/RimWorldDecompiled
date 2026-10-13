using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E3 RID: 6115
	public class ThoughtWorker_Precept_Pain : ThoughtWorker_Precept
	{
		// Token: 0x06008E37 RID: 36407 RVA: 0x002A268F File Offset: 0x002A088F
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Pain.CurrentThoughtState(p);
		}
	}
}
