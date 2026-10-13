using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026E6 RID: 9958
	public class RitualObligationTargetWorker_Thing : RitualObligationTargetFilter
	{
		// Token: 0x0600DA50 RID: 55888 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_Thing()
		{
		}

		// Token: 0x0600DA51 RID: 55889 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_Thing(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DA52 RID: 55890 RVA: 0x00408E83 File Offset: 0x00407083
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!obligation.targetA.HasThing)
			{
				yield break;
			}
			yield return obligation.targetA.Thing;
			yield break;
		}

		// Token: 0x0600DA53 RID: 55891 RVA: 0x00408E93 File Offset: 0x00407093
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return target.HasThing && target.Thing == obligation.targetA.Thing;
		}

		// Token: 0x0600DA54 RID: 55892 RVA: 0x00408EBA File Offset: 0x004070BA
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return obligation.targetA.Thing.LabelShort;
			yield break;
		}
	}
}
