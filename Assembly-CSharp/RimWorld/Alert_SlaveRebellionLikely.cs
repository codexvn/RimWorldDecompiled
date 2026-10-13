using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003036 RID: 12342
	public class Alert_SlaveRebellionLikely : Alert
	{
		// Token: 0x06011598 RID: 71064 RVA: 0x004FA11D File Offset: 0x004F831D
		public Alert_SlaveRebellionLikely()
		{
			this.defaultLabel = "AlertSlaveRebellionLikely".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x06011599 RID: 71065 RVA: 0x004FA14C File Offset: 0x004F834C
		public override TaggedString GetExplanation()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return string.Empty;
			}
			int num = currentMap.mapPawns.SlavesOfColonySpawned.Count((Pawn pawn) => this.MTBMeetsRebelliousThreshold(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn)));
			Pawn mostRebelliousPawn = this.GetMostRebelliousPawn();
			int numTicks = (int)SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(mostRebelliousPawn) * 60000;
			int numTicks2 = (int)SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(currentMap) * 60000;
			this.sb.Length = 0;
			if (num >= 2)
			{
				this.sb.Append("AlertSlaveRebellionLikelyRebelliousCount".Translate(num.Named("REBELLIOUSCOUNT")) + " ");
			}
			this.sb.Append("AlertSlaveRebellionLikelyMostRebellious".Translate(numTicks2.ToStringTicksToPeriodVerbose(true, true).Named("COMBINEDTIME"), mostRebelliousPawn.Named("REBEL"), numTicks.ToStringTicksToPeriodVerbose(true, true).Named("INDIVIDUALTIME")));
			this.sb.Append("\n\n" + SlaveRebellionUtility.GetSlaveRebellionMtbCalculationExplanation(mostRebelliousPawn));
			return this.sb.ToString();
		}

		// Token: 0x0601159A RID: 71066 RVA: 0x004FA26C File Offset: 0x004F846C
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return false;
			}
			float mtb = SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(Find.CurrentMap);
			if (!this.MTBMeetsRebelliousThreshold(mtb))
			{
				return false;
			}
			return AlertReport.CulpritIs(this.GetMostRebelliousPawn());
		}

		// Token: 0x0601159B RID: 71067 RVA: 0x004FA2B4 File Offset: 0x004F84B4
		private Pawn GetMostRebelliousPawn()
		{
			IEnumerable<Pawn> source = from pawn in Find.CurrentMap.mapPawns.SlavesOfColonySpawned
			where SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn) > 0f
			select pawn;
			if (source.Any<Pawn>())
			{
				return source.MinBy(new Func<Pawn, float>(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays));
			}
			return null;
		}

		// Token: 0x0601159C RID: 71068 RVA: 0x004FA311 File Offset: 0x004F8511
		private bool MTBMeetsRebelliousThreshold(float mtb)
		{
			return 15f > mtb && mtb > 0f;
		}

		// Token: 0x0400B3AD RID: 45997
		private const float ReportAnySlaveMtbThreshold = 15f;

		// Token: 0x0400B3AE RID: 45998
		private StringBuilder sb = new StringBuilder();
	}
}
