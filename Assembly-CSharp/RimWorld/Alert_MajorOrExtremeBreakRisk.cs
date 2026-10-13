using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FC5 RID: 12229
	public class Alert_MajorOrExtremeBreakRisk : Alert_Critical
	{
		// Token: 0x17002CAF RID: 11439
		// (get) Token: 0x060113CA RID: 70602 RVA: 0x004F04C2 File Offset: 0x004EE6C2
		private List<Pawn> Culprits
		{
			get
			{
				this.culpritsResult.Clear();
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskExtreme);
				this.culpritsResult.AddRange(BreakRiskAlertUtility.PawnsAtRiskMajor);
				return this.culpritsResult;
			}
		}

		// Token: 0x060113CB RID: 70603 RVA: 0x004F04F5 File Offset: 0x004EE6F5
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x060113CC RID: 70604 RVA: 0x004F04FC File Offset: 0x004EE6FC
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x060113CD RID: 70605 RVA: 0x004F0508 File Offset: 0x004EE708
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Culprits);
		}

		// Token: 0x0400B2F1 RID: 45809
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
