using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001834 RID: 6196
	public class ThoughtWorker_Precept_Bloodfeeder_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008F37 RID: 36663 RVA: 0x002A5373 File Offset: 0x002A3573
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return otherPawn.IsBloodfeeder();
		}
	}
}
