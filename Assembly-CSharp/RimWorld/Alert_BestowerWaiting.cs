using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002FE5 RID: 12261
	public class Alert_BestowerWaiting : Alert
	{
		// Token: 0x17002CCA RID: 11466
		// (get) Token: 0x0601145B RID: 70747 RVA: 0x004F2AD8 File Offset: 0x004F0CD8
		private List<Pawn> BestowersWaiting
		{
			get
			{
				this.bestowersWaitingResult.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Lord lord in map.lordManager.lords)
					{
						if (lord.CurLordToil is LordToil_BestowingCeremony_Wait)
						{
							this.bestowersWaitingResult.Add(lord.ownedPawns[0]);
						}
					}
				}
				return this.bestowersWaitingResult;
			}
		}

		// Token: 0x0601145C RID: 70748 RVA: 0x004F2B98 File Offset: 0x004F0D98
		public Alert_BestowerWaiting()
		{
			this.defaultPriority = AlertPriority.High;
			this.defaultLabel = "BestowerWaitingAlert".Translate();
			this.defaultExplanation = "BestowerWaitingAlertDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0601145D RID: 70749 RVA: 0x004F2BEE File Offset: 0x004F0DEE
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BestowersWaiting);
		}

		// Token: 0x0400B31D RID: 45853
		private List<Pawn> bestowersWaitingResult = new List<Pawn>();
	}
}
