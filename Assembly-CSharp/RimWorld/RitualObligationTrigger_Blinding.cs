using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200272A RID: 10026
	public class RitualObligationTrigger_Blinding : RitualObligationTrigger_EveryMember
	{
		// Token: 0x170021B4 RID: 8628
		// (get) Token: 0x0600DC0B RID: 56331 RVA: 0x0040D389 File Offset: 0x0040B589
		public override string TriggerExtraDesc
		{
			get
			{
				return "RitualBlindingTriggerExtraDesc".Translate(this.ritual.ideo.memberName.Named("IDEOMEMBER"));
			}
		}

		// Token: 0x0600DC0C RID: 56332 RVA: 0x0040D3B4 File Offset: 0x0040B5B4
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
						RitualObligationTrigger_Blinding.existingObligations.Add(ritualObligation.targetA.Thing as Pawn);
					}
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					if (!RitualObligationTrigger_Blinding.existingObligations.Contains(pawn) && pawn.Ideo != null && pawn.Ideo == this.ritual.ideo && !pawn.IsPrisoner)
					{
						if (pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Eye))
						{
							this.ritual.AddObligation(new RitualObligation(this.ritual, pawn, false));
						}
					}
				}
			}
			finally
			{
				RitualObligationTrigger_Blinding.existingObligations.Clear();
			}
		}

		// Token: 0x040094A2 RID: 38050
		private static List<Pawn> existingObligations = new List<Pawn>();
	}
}
