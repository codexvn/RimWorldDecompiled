using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003050 RID: 12368
	public class Alert_PsychicBondedSeparated : Alert
	{
		// Token: 0x06011603 RID: 71171 RVA: 0x004FC1EB File Offset: 0x004FA3EB
		public Alert_PsychicBondedSeparated()
		{
			this.defaultLabel = "AlertPsychicBondedPawnsSeparated".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x06011604 RID: 71172 RVA: 0x004FC21A File Offset: 0x004FA41A
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x06011605 RID: 71173 RVA: 0x004FC230 File Offset: 0x004FA430
		public override TaggedString GetExplanation()
		{
			return "AlertPsychicBondedPawnsSeparatedDesc".Translate() + ":\n" + (from x in this.targets
			select x.NameShortColored.Resolve()).ToLineList("  - ", true);
		}

		// Token: 0x06011606 RID: 71174 RVA: 0x004FC28C File Offset: 0x004FA48C
		private void GetTargets()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned)
			{
				if (pawn.RaceProps.Humanlike && pawn.Faction == Faction.OfPlayer)
				{
					Hediff_PsychicBond hediff_PsychicBond = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond != null && !ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(pawn, hediff_PsychicBond))
					{
						this.targets.Add(pawn);
					}
				}
			}
		}

		// Token: 0x0400B3E4 RID: 46052
		private List<Pawn> targets = new List<Pawn>();
	}
}
