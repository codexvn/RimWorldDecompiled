using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183E RID: 6206
	public class ThoughtWorker_Precept_NonImhumanized : ThoughtWorker_Precept
	{
		// Token: 0x06008F53 RID: 36691 RVA: 0x002A583A File Offset: 0x002A3A3A
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return ThoughtState.Inactive;
			}
			return !p.Inhumanized();
		}
	}
}
