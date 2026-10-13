using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE4 RID: 12260
	public class Alert_MinorBreakRisk : Alert
	{
		// Token: 0x06011457 RID: 70743 RVA: 0x004F2A9B File Offset: 0x004F0C9B
		public Alert_MinorBreakRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06011458 RID: 70744 RVA: 0x004F04F5 File Offset: 0x004EE6F5
		public override string GetLabel()
		{
			return BreakRiskAlertUtility.AlertLabel;
		}

		// Token: 0x06011459 RID: 70745 RVA: 0x004F04FC File Offset: 0x004EE6FC
		public override TaggedString GetExplanation()
		{
			return BreakRiskAlertUtility.AlertExplanation;
		}

		// Token: 0x0601145A RID: 70746 RVA: 0x004F2AAA File Offset: 0x004F0CAA
		public override AlertReport GetReport()
		{
			if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>() || BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
			{
				return false;
			}
			return AlertReport.CulpritsAre(BreakRiskAlertUtility.PawnsAtRiskMinor);
		}
	}
}
