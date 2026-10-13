using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001874 RID: 6260
	public class ThoughtWorker_Ghoul : ThoughtWorker
	{
		// Token: 0x06008FCF RID: 36815 RVA: 0x002A7371 File Offset: 0x002A5571
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			return other.IsGhoul;
		}
	}
}
