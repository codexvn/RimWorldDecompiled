using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FCF RID: 12239
	public class Alert_CubeWithdrawal : Alert_Critical
	{
		// Token: 0x17002CBD RID: 11453
		// (get) Token: 0x060113FC RID: 70652 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002CBE RID: 11454
		// (get) Token: 0x060113FD RID: 70653 RVA: 0x004F1464 File Offset: 0x004EF664
		private List<Pawn> Withdrawal
		{
			get
			{
				this.inWithdrawal.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep)
				{
					if ((!ModsConfig.BiotechActive || !pawn.Deathresting) && Alert_CubeWithdrawal.InWithdrawal(pawn))
					{
						this.inWithdrawal.Add(pawn);
					}
				}
				return this.inWithdrawal;
			}
		}

		// Token: 0x060113FE RID: 70654 RVA: 0x004F14E4 File Offset: 0x004EF6E4
		public Alert_CubeWithdrawal()
		{
			this.defaultLabel = "AlertCubeWithdrawal".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x060113FF RID: 70655 RVA: 0x004F1514 File Offset: 0x004EF714
		private static bool InWithdrawal(ThingWithComps thing)
		{
			Pawn pawn = thing as Pawn;
			Hediff hediff;
			return pawn != null && pawn.health != null && pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeWithdrawal, out hediff) && hediff.Severity >= 0.8f;
		}

		// Token: 0x06011400 RID: 70656 RVA: 0x004F1560 File Offset: 0x004EF760
		public override string GetLabel()
		{
			if (this.inWithdrawal.Count == 1)
			{
				return this.inWithdrawal[0].Label + ": " + Alert_CubeWithdrawal.GetSeverity(this.inWithdrawal[0]).ToStringPercent("0");
			}
			return this.defaultLabel;
		}

		// Token: 0x06011401 RID: 70657 RVA: 0x004F15B8 File Offset: 0x004EF7B8
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.inWithdrawal)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve() + ": " + Alert_CubeWithdrawal.GetSeverity(pawn).ToStringPercent("0"));
			}
			return string.Format("{0}:\n{1}\n\n{2}", "AlertCubeWithdrawalDesc".Translate(), stringBuilder.ToString().TrimEndNewlines(), "AlertCubeWithdrawalDescAppended".Translate());
		}

		// Token: 0x06011402 RID: 70658 RVA: 0x004F1678 File Offset: 0x004EF878
		private static float GetSeverity(Pawn pawn)
		{
			return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.CubeWithdrawal, false).Severity;
		}

		// Token: 0x06011403 RID: 70659 RVA: 0x004F1695 File Offset: 0x004EF895
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Withdrawal);
		}

		// Token: 0x0400B2FC RID: 45820
		private readonly List<Pawn> inWithdrawal = new List<Pawn>();

		// Token: 0x0400B2FD RID: 45821
		private const float AlertPercentage = 0.8f;
	}
}
