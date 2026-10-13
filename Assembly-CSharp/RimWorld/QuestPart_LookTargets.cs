using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C62 RID: 7266
	public class QuestPart_LookTargets : QuestPart
	{
		// Token: 0x170018E5 RID: 6373
		// (get) Token: 0x0600A1EA RID: 41450 RVA: 0x002E87BF File Offset: 0x002E69BF
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return this.targets;
			}
		}

		// Token: 0x0600A1EB RID: 41451 RVA: 0x002E87C7 File Offset: 0x002E69C7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x04006FDF RID: 28639
		public List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
