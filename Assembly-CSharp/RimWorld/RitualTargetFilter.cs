using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027D3 RID: 10195
	public abstract class RitualTargetFilter : IExposable
	{
		// Token: 0x0600DF24 RID: 57124 RVA: 0x000024DF File Offset: 0x000006DF
		public RitualTargetFilter()
		{
		}

		// Token: 0x0600DF25 RID: 57125 RVA: 0x00419437 File Offset: 0x00417637
		public RitualTargetFilter(RitualTargetFilterDef def)
		{
			this.def = def;
		}

		// Token: 0x0600DF26 RID: 57126
		public abstract bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason);

		// Token: 0x0600DF27 RID: 57127
		public abstract TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget);

		// Token: 0x0600DF28 RID: 57128
		public abstract IEnumerable<string> GetTargetInfos(TargetInfo initiator);

		// Token: 0x0600DF29 RID: 57129 RVA: 0x00419446 File Offset: 0x00417646
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualTargetFilterDef>(ref this.def, "def");
		}

		// Token: 0x04009642 RID: 38466
		public RitualTargetFilterDef def;
	}
}
