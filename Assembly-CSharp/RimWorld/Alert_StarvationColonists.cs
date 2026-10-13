using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FFE RID: 12286
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x060114BE RID: 70846 RVA: 0x004F5505 File Offset: 0x004F3705
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17002CD1 RID: 11473
		// (get) Token: 0x060114BF RID: 70847 RVA: 0x004F5540 File Offset: 0x004F3740
		private List<Pawn> StarvingColonists
		{
			get
			{
				this.starvingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (pawn.needs.food != null && pawn.needs.food.Starving)
					{
						this.starvingColonistsResult.Add(pawn);
					}
				}
				return this.starvingColonistsResult;
			}
		}

		// Token: 0x060114C0 RID: 70848 RVA: 0x004F55C8 File Offset: 0x004F37C8
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.starvingColonistsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "StarvationDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x060114C1 RID: 70849 RVA: 0x004F5664 File Offset: 0x004F3864
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}

		// Token: 0x0400B34E RID: 45902
		private List<Pawn> starvingColonistsResult = new List<Pawn>();

		// Token: 0x0400B34F RID: 45903
		private StringBuilder sb = new StringBuilder();
	}
}
