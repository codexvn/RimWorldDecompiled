using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001835 RID: 6197
	public class ThoughtWorker_Precept_IsBloodfeeder : ThoughtWorker_Precept
	{
		// Token: 0x06008F39 RID: 36665 RVA: 0x002A5394 File Offset: 0x002A3594
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return p.IsBloodfeeder();
		}
	}
}
