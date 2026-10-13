using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001865 RID: 6245
	public class ThoughtWorker_Drunk : ThoughtWorker
	{
		// Token: 0x06008FB0 RID: 36784 RVA: 0x002A6C30 File Offset: 0x002A4E30
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			Hediff firstHediffOfDef = other.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
			if (firstHediffOfDef == null || !firstHediffOfDef.Visible)
			{
				return false;
			}
			return true;
		}
	}
}
