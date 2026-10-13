using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186E RID: 6254
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x06008FC2 RID: 36802 RVA: 0x002A70C0 File Offset: 0x002A52C0
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (Find.TaleManager.GetLatestTale(this.def.taleDef, other) == null)
			{
				return false;
			}
			return true;
		}
	}
}
