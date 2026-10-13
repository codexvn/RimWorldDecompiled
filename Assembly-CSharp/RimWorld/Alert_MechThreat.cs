using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FDD RID: 12253
	public class Alert_MechThreat : Alert_Scenario
	{
		// Token: 0x17002CC6 RID: 11462
		// (get) Token: 0x06011436 RID: 70710 RVA: 0x004F25E3 File Offset: 0x004F07E3
		private bool Red
		{
			get
			{
				return Find.TickManager.TicksGame > this.raidTick - 60000;
			}
		}

		// Token: 0x17002CC7 RID: 11463
		// (get) Token: 0x06011437 RID: 70711 RVA: 0x004F25FD File Offset: 0x004F07FD
		private bool Critical
		{
			get
			{
				return Find.TickManager.TicksGame > this.raidTick;
			}
		}

		// Token: 0x06011438 RID: 70712 RVA: 0x004F2611 File Offset: 0x004F0811
		public override AlertReport GetReport()
		{
			return AlertReport.Active;
		}

		// Token: 0x06011439 RID: 70713 RVA: 0x004F2618 File Offset: 0x004F0818
		public override string GetLabel()
		{
			if (this.Critical)
			{
				return "AlertMechanoidThreatCritical".Translate();
			}
			return "AlertMechanoidThreat".Translate() + ": " + (this.raidTick - Find.TickManager.TicksGame).ToStringTicksToPeriodVerbose(true, true);
		}

		// Token: 0x0601143A RID: 70714 RVA: 0x004F2673 File Offset: 0x004F0873
		public override TaggedString GetExplanation()
		{
			if (this.Critical)
			{
				return "AlertMechanoidThreatCriticalDesc".Translate();
			}
			return "AlertMechanoidThreatDesc".Translate();
		}

		// Token: 0x17002CC8 RID: 11464
		// (get) Token: 0x0601143B RID: 70715 RVA: 0x004F2692 File Offset: 0x004F0892
		protected override Color BGColor
		{
			get
			{
				if (!this.Red)
				{
					return Color.clear;
				}
				return Alert_Critical.BgColor();
			}
		}

		// Token: 0x0400B316 RID: 45846
		public int raidTick;
	}
}
