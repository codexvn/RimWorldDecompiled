using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002724 RID: 10020
	public class RitualObligationTrigger_MemberCorpseDestroyed : RitualObligationTrigger
	{
		// Token: 0x0600DBF5 RID: 56309 RVA: 0x0040CFBC File Offset: 0x0040B1BC
		public override void Notify_MemberCorpseDestroyed(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if ((this.mustBePlayerIdeo && !Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || p.HomeFaction != Faction.OfPlayer || !p.IsFreeNonSlaveColonist || p.IsKidnapped())
			{
				return;
			}
			if (ModsConfig.AnomalyActive)
			{
				if (p.IsCreepJoiner)
				{
					return;
				}
				if (p.IsSubhuman)
				{
					return;
				}
				if (p.health.hediffSet.HasHediff(HediffDefOf.ShamblerCorpse, false))
				{
					return;
				}
			}
			Pawn_IdeoTracker ideo = p.ideo;
			object obj;
			if (ideo == null)
			{
				obj = null;
			}
			else
			{
				Ideo ideo2 = ideo.Ideo;
				obj = ((ideo2 != null) ? ideo2.GetPrecept(PreceptDefOf.Funeral) : null);
			}
			Precept_Ritual precept_Ritual = (Precept_Ritual)obj;
			if (precept_Ritual != null && !precept_Ritual.activeObligations.NullOrEmpty<RitualObligation>() && !precept_Ritual.completedObligations.NullOrEmpty<RitualObligation>() && precept_Ritual.completedObligations.Any((RitualObligation o) => o.FirstValidTarget.Thing == p))
			{
				return;
			}
			this.ritual.AddObligation(new RitualObligation(this.ritual, p, true));
		}
	}
}
