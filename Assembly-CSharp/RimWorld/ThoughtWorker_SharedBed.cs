using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001889 RID: 6281
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		// Token: 0x06009001 RID: 36865 RVA: 0x002A7DA4 File Offset: 0x002A5FA4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			return LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p) != null;
		}
	}
}
