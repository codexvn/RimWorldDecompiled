using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001870 RID: 6256
	public class ThoughtWorker_TaleDoublePawn : ThoughtWorker
	{
		// Token: 0x06008FC6 RID: 36806 RVA: 0x002A7154 File Offset: 0x002A5354
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
			Tale_DoublePawn tale_DoublePawn = Find.TaleManager.GetLatestTale(this.def.taleDef, other) as Tale_DoublePawn;
			if (tale_DoublePawn == null)
			{
				return false;
			}
			return (tale_DoublePawn.firstPawnData.pawn == p && tale_DoublePawn.secondPawnData.pawn == other) || (tale_DoublePawn.firstPawnData.pawn == other && tale_DoublePawn.secondPawnData.pawn == p);
		}
	}
}
