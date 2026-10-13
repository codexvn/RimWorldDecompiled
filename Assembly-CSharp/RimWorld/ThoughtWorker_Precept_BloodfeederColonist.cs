using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001836 RID: 6198
	public class ThoughtWorker_Precept_BloodfeederColonist : ThoughtWorker_Precept
	{
		// Token: 0x06008F3B RID: 36667 RVA: 0x002A53B8 File Offset: 0x002A35B8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.IsBloodfeeder() || p.Faction == null)
			{
				return ThoughtState.Inactive;
			}
			Ideo ideo = p.Ideo;
			bool flag = false;
			foreach (Pawn pawn in p.MapHeld.mapPawns.SpawnedPawnsInFaction(p.Faction))
			{
				if (pawn.IsBloodfeeder())
				{
					flag = true;
					Ideo ideo2 = pawn.Ideo;
					Precept_Role precept_Role = (ideo2 != null) ? ideo2.GetRole(pawn) : null;
					if (precept_Role != null && precept_Role.ideo == p.Ideo && precept_Role.def == PreceptDefOf.IdeoRole_Leader)
					{
						return ThoughtState.ActiveAtStage(2);
					}
				}
			}
			if (flag)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
