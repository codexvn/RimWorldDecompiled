using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003020 RID: 12320
	public class Alert_StarvationAnimals : Alert
	{
		// Token: 0x06011544 RID: 70980 RVA: 0x004F84BE File Offset: 0x004F66BE
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		// Token: 0x17002CE9 RID: 11497
		// (get) Token: 0x06011545 RID: 70981 RVA: 0x004F84F4 File Offset: 0x004F66F4
		private List<Pawn> StarvingAnimals
		{
			get
			{
				this.starvingAnimalsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep)
				{
					if (pawn.HostFaction == null && !pawn.RaceProps.Humanlike && pawn.needs.food != null && (pawn.needs.food.TicksStarving > 30000 || (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true) && pawn.needs.food.TicksStarving > 5000)))
					{
						this.starvingAnimalsResult.Add(pawn);
					}
				}
				return this.starvingAnimalsResult;
			}
		}

		// Token: 0x06011546 RID: 70982 RVA: 0x004F85C8 File Offset: 0x004F67C8
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in from a in this.starvingAnimalsResult
			orderby a.def.label
			select a)
			{
				this.sb.Append("  - " + pawn.NameShortColored.Resolve());
				if (pawn.Name.IsValid && !pawn.Name.Numerical)
				{
					this.sb.Append(" (" + pawn.def.label + ")");
				}
				this.sb.AppendLine();
			}
			return "StarvationAnimalsDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x06011547 RID: 70983 RVA: 0x004F86D0 File Offset: 0x004F68D0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}

		// Token: 0x0400B386 RID: 45958
		private List<Pawn> starvingAnimalsResult = new List<Pawn>();

		// Token: 0x0400B387 RID: 45959
		private StringBuilder sb = new StringBuilder();
	}
}
