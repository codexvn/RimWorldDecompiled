using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000774 RID: 1908
	public class HediffGiver_AddSeverity : HediffGiver
	{
		// Token: 0x06002ECC RID: 11980 RVA: 0x000F09E0 File Offset: 0x000EEBE0
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (pawn.IsNestedHashIntervalTick(60, HediffGiver_AddSeverity.mtbCheckInterval) && Rand.MTBEventOccurs(this.mtbHours, 2500f, (float)HediffGiver_AddSeverity.mtbCheckInterval))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false).Severity += this.severityAmount;
			}
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000F0A4F File Offset: 0x000EEC4F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (float.IsNaN(this.severityAmount))
			{
				yield return "severityAmount is not defined";
			}
			if (this.mtbHours < 0f)
			{
				yield return "mtbHours is not defined";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400249C RID: 9372
		public float severityAmount = float.NaN;

		// Token: 0x0400249D RID: 9373
		public float mtbHours = -1f;

		// Token: 0x0400249E RID: 9374
		private static int mtbCheckInterval = 1200;
	}
}
