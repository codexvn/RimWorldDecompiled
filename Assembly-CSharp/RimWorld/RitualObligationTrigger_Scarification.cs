using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002728 RID: 10024
	public class RitualObligationTrigger_Scarification : RitualObligationTrigger_EveryMember
	{
		// Token: 0x170021B3 RID: 8627
		// (get) Token: 0x0600DC05 RID: 56325 RVA: 0x0040D13C File Offset: 0x0040B33C
		public override string TriggerExtraDesc
		{
			get
			{
				return "RitualScarificationTriggerExtraDesc".Translate(this.ritual.ideo.RequiredScars, this.ritual.ideo.memberName.Named("IDEOMEMBER"), this.ritual.ideo.MemberNamePlural.Named("IDEOMEMBERPLURAL"));
			}
		}

		// Token: 0x0600DC06 RID: 56326 RVA: 0x0040D1A4 File Offset: 0x0040B3A4
		protected override void Recache()
		{
			try
			{
				if (this.ritual.activeObligations != null)
				{
					this.ritual.activeObligations.RemoveAll(delegate(RitualObligation o)
					{
						Pawn pawn2 = o.targetA.Thing as Pawn;
						return pawn2 != null && pawn2.Ideo != this.ritual.ideo;
					});
					foreach (RitualObligation ritualObligation in this.ritual.activeObligations)
					{
						RitualObligationTrigger_Scarification.existingObligations.Add(ritualObligation.targetA.Thing as Pawn);
					}
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					if (!RitualObligationTrigger_Scarification.existingObligations.Contains(pawn) && pawn.Ideo != null && pawn.Ideo == this.ritual.ideo && !pawn.IsPrisoner)
					{
						int hediffCount = pawn.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
						if (pawn.Ideo.RequiredScars > hediffCount)
						{
							this.ritual.AddObligation(new RitualObligation(this.ritual, pawn, false));
						}
					}
				}
			}
			finally
			{
				RitualObligationTrigger_Scarification.existingObligations.Clear();
			}
		}

		// Token: 0x040094A1 RID: 38049
		private static List<Pawn> existingObligations = new List<Pawn>();
	}
}
