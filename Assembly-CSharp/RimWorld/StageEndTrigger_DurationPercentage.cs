using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002696 RID: 9878
	public class StageEndTrigger_DurationPercentage : StageEndTrigger
	{
		// Token: 0x1700214B RID: 8523
		// (get) Token: 0x0600D8F4 RID: 55540 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CountsTowardsProgress
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600D8F5 RID: 55541 RVA: 0x004047F2 File Offset: 0x004029F2
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TicksPassedRitual(Mathf.RoundToInt((float)ritual.DurationTicks * this.percentage), stage);
		}

		// Token: 0x0600D8F6 RID: 55542 RVA: 0x0040480E File Offset: 0x00402A0E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.percentage, "percentage", 0f, false);
		}

		// Token: 0x0400930D RID: 37645
		public float percentage;
	}
}
