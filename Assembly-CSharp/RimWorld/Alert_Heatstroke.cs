using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003000 RID: 12288
	public class Alert_Heatstroke : Alert
	{
		// Token: 0x060114C6 RID: 70854 RVA: 0x004F57C9 File Offset: 0x004F39C9
		public Alert_Heatstroke()
		{
			this.defaultLabel = "AlertHeatstroke".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17002CD3 RID: 11475
		// (get) Token: 0x060114C7 RID: 70855 RVA: 0x004F5804 File Offset: 0x004F3A04
		private List<Pawn> HeatstrokePawns
		{
			get
			{
				this.heatstrokePawnsResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					if (pawn.health != null && !pawn.IsAnimal && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, true) != null)
					{
						this.heatstrokePawnsResult.Add(pawn);
					}
				}
				return this.heatstrokePawnsResult;
			}
		}

		// Token: 0x060114C8 RID: 70856 RVA: 0x004F587C File Offset: 0x004F3A7C
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.HeatstrokePawns)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return string.Format("AlertHeatstrokeDesc".Translate(), this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x060114C9 RID: 70857 RVA: 0x004F5924 File Offset: 0x004F3B24
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HeatstrokePawns);
		}

		// Token: 0x0400B352 RID: 45906
		private List<Pawn> heatstrokePawnsResult = new List<Pawn>();

		// Token: 0x0400B353 RID: 45907
		private StringBuilder sb = new StringBuilder();
	}
}
