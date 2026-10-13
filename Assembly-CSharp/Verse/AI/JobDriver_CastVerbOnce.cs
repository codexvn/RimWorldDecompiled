using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000EAA RID: 3754
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x06005E8B RID: 24203 RVA: 0x001E0AD8 File Offset: 0x001DECD8
		public override string GetReport()
		{
			string value;
			if (base.TargetA.HasThing)
			{
				value = base.TargetThingA.LabelCap;
			}
			else
			{
				value = "AreaLower".Translate();
			}
			if (this.job.verbToUse == null)
			{
				return null;
			}
			return "UsingVerb".Translate(this.job.verbToUse.ReportLabel, value);
		}

		// Token: 0x06005E8C RID: 24204 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x001E0B4C File Offset: 0x001DED4C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, TargetIndex.B, false, 1f);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
