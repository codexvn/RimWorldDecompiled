using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C35 RID: 11317
	public class CompProperties_CanBeDormant : CompProperties
	{
		// Token: 0x0600FDBC RID: 64956 RVA: 0x00499818 File Offset: 0x00497A18
		public CompProperties_CanBeDormant()
		{
			this.compClass = typeof(CompCanBeDormant);
		}

		// Token: 0x0600FDBD RID: 64957 RVA: 0x00499899 File Offset: 0x00497A99
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (!parentDef.receivesSignals && !this.jobDormancy)
			{
				yield return "ThingDefs with CanBeDormant component must have receivesSignals set to true, otherwise wakeup won't work properly!";
			}
			yield break;
		}

		// Token: 0x0400A645 RID: 42565
		public bool startsDormant;

		// Token: 0x0400A646 RID: 42566
		[NoTranslate]
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x0400A647 RID: 42567
		public float maxDistAwakenByOther = 40f;

		// Token: 0x0400A648 RID: 42568
		public bool canWakeUpFogged = true;

		// Token: 0x0400A649 RID: 42569
		public bool jobDormancy;

		// Token: 0x0400A64A RID: 42570
		public IntRange wakeUpDelayRange = new IntRange(60, 300);

		// Token: 0x0400A64B RID: 42571
		[NoTranslate]
		public string awakeStateLabelKey = "AwokeDaysAgo";

		// Token: 0x0400A64C RID: 42572
		[NoTranslate]
		public string dormantStateLabelKey = "DormantCompInactive";

		// Token: 0x0400A64D RID: 42573
		[NoTranslate]
		public string wakeUpDelayStateLabelKey;

		// Token: 0x0400A64E RID: 42574
		public EffecterDef wakeUpEffect;

		// Token: 0x0400A64F RID: 42575
		public IntRange? wakeUpRepeatSignalDelayRange;

		// Token: 0x0400A650 RID: 42576
		public bool showSleepingZs = true;

		// Token: 0x0400A651 RID: 42577
		public bool delayedWakeUpDoesZs = true;

		// Token: 0x0400A652 RID: 42578
		public bool dontShowDevGizmos;

		// Token: 0x0400A653 RID: 42579
		public List<NeedDef> freezeNeeds = new List<NeedDef>();
	}
}
