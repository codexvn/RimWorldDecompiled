using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020027E4 RID: 10212
	public class RitualTargetFilter_SelectedThing : RitualTargetFilter
	{
		// Token: 0x0600DF8D RID: 57229 RVA: 0x00419458 File Offset: 0x00417658
		public RitualTargetFilter_SelectedThing()
		{
		}

		// Token: 0x0600DF8E RID: 57230 RVA: 0x00419460 File Offset: 0x00417660
		public RitualTargetFilter_SelectedThing(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DF8F RID: 57231 RVA: 0x00406E9E File Offset: 0x0040509E
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			rejectionReason = null;
			return true;
		}

		// Token: 0x0600DF90 RID: 57232 RVA: 0x00161C9F File Offset: 0x0015FE9F
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			return selectedTarget;
		}

		// Token: 0x0600DF91 RID: 57233 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			return Enumerable.Empty<string>();
		}
	}
}
