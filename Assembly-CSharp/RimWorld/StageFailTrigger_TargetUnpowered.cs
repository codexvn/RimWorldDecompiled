using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026AF RID: 9903
	public class StageFailTrigger_TargetUnpowered : StageFailTrigger
	{
		// Token: 0x0600D938 RID: 55608 RVA: 0x00405304 File Offset: 0x00403504
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
			CompPowerTrader compPowerTrader = (thing2 != null) ? thing2.TryGetComp<CompPowerTrader>() : null;
			return compPowerTrader == null || !compPowerTrader.PowerOn;
		}

		// Token: 0x0600D939 RID: 55609 RVA: 0x00405360 File Offset: 0x00403560
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x0400932F RID: 37679
		public ThingDef onlyIfTargetIsOfDef;
	}
}
