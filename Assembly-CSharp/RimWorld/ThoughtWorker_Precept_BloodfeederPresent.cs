using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001837 RID: 6199
	public class ThoughtWorker_Precept_BloodfeederPresent : ThoughtWorker_Precept
	{
		// Token: 0x06008F3D RID: 36669 RVA: 0x002A549C File Offset: 0x002A369C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.IsBloodfeeder())
			{
				return ThoughtState.Inactive;
			}
			foreach (Pawn pawn in p.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (pawn.IsBloodfeeder() && (pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony || pawn.IsColonist))
				{
					return ThoughtState.ActiveDefault;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
