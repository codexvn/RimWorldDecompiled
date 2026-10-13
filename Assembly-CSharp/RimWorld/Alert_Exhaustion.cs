using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FFF RID: 12287
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x060114C2 RID: 70850 RVA: 0x004F5671 File Offset: 0x004F3871
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17002CD2 RID: 11474
		// (get) Token: 0x060114C3 RID: 70851 RVA: 0x004F56AC File Offset: 0x004F38AC
		private List<Pawn> ExhaustedColonists
		{
			get
			{
				this.exhaustedColonistsResult.Clear();
				List<Pawn> allMaps_FreeColonistsSpawned = PawnsFinder.AllMaps_FreeColonistsSpawned;
				for (int i = 0; i < allMaps_FreeColonistsSpawned.Count; i++)
				{
					if (allMaps_FreeColonistsSpawned[i].needs.rest != null && allMaps_FreeColonistsSpawned[i].needs.rest.CurCategory == RestCategory.Exhausted)
					{
						this.exhaustedColonistsResult.Add(allMaps_FreeColonistsSpawned[i]);
					}
				}
				return this.exhaustedColonistsResult;
			}
		}

		// Token: 0x060114C4 RID: 70852 RVA: 0x004F5720 File Offset: 0x004F3920
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ExhaustionDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x060114C5 RID: 70853 RVA: 0x004F57BC File Offset: 0x004F39BC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}

		// Token: 0x0400B350 RID: 45904
		private List<Pawn> exhaustedColonistsResult = new List<Pawn>();

		// Token: 0x0400B351 RID: 45905
		private StringBuilder sb = new StringBuilder();
	}
}
