using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200300E RID: 12302
	public class Alert_CaravanIdle : Alert
	{
		// Token: 0x17002CDF RID: 11487
		// (get) Token: 0x060114FF RID: 70911 RVA: 0x004F69CC File Offset: 0x004F4BCC
		private List<Caravan> IdleCaravans
		{
			get
			{
				this.idleCaravansResult.Clear();
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					if (caravan.Spawned && caravan.IsPlayerControlled && !caravan.pather.MovingNow && !caravan.CantMove)
					{
						this.idleCaravansResult.Add(caravan);
					}
				}
				return this.idleCaravansResult;
			}
		}

		// Token: 0x06011500 RID: 70912 RVA: 0x004F6A60 File Offset: 0x004F4C60
		public Alert_CaravanIdle()
		{
			this.defaultLabel = "CaravanIdle".Translate();
		}

		// Token: 0x06011501 RID: 70913 RVA: 0x004F6A94 File Offset: 0x004F4C94
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Caravan caravan in this.idleCaravansResult)
			{
				this.sb.AppendLine("  - " + caravan.Label);
			}
			return "CaravanIdleDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x06011502 RID: 70914 RVA: 0x004F6B28 File Offset: 0x004F4D28
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.IdleCaravans);
		}

		// Token: 0x0400B36B RID: 45931
		private List<Caravan> idleCaravansResult = new List<Caravan>();

		// Token: 0x0400B36C RID: 45932
		private StringBuilder sb = new StringBuilder();
	}
}
