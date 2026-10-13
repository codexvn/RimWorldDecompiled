using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001838 RID: 6200
	public class ThoughtWorker_Precept_PreferredXenotype_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06008F3F RID: 36671 RVA: 0x002A5538 File Offset: 0x002A3738
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || otherPawn.genes == null)
			{
				return ThoughtState.Inactive;
			}
			if (!p.Ideo.PreferredXenotypes.Any<XenotypeDef>() && !p.Ideo.PreferredCustomXenotypes.Any<CustomXenotype>())
			{
				return ThoughtState.Inactive;
			}
			if (p.Ideo.IsPreferredXenotype(otherPawn))
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}
	}
}
