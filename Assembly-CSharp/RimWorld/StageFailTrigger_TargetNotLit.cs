using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026B0 RID: 9904
	public class StageFailTrigger_TargetNotLit : StageFailTrigger
	{
		// Token: 0x0600D93B RID: 55611 RVA: 0x00405378 File Offset: 0x00403578
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			if (this.onlyIfTargetIsOfDef != null)
			{
				Thing thing = ritual.selectedTarget.Thing;
				if (((thing != null) ? thing.def : null) != this.onlyIfTargetIsOfDef)
				{
					return false;
				}
			}
			Thing thing2 = ritual.selectedTarget.Thing;
			CompRefuelable compRefuelable = (thing2 != null) ? thing2.TryGetComp<CompRefuelable>() : null;
			return compRefuelable == null || !compRefuelable.HasFuel;
		}

		// Token: 0x0600D93C RID: 55612 RVA: 0x004053D4 File Offset: 0x004035D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x04009330 RID: 37680
		public ThingDef onlyIfTargetIsOfDef;
	}
}
