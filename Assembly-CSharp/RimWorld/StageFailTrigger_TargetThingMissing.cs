using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026AE RID: 9902
	public class StageFailTrigger_TargetThingMissing : StageFailTrigger
	{
		// Token: 0x0600D935 RID: 55605 RVA: 0x00405294 File Offset: 0x00403494
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
			return ritual.selectedTarget.Thing == null || ritual.selectedTarget.ThingDestroyed;
		}

		// Token: 0x0600D936 RID: 55606 RVA: 0x004052E4 File Offset: 0x004034E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x0400932E RID: 37678
		public ThingDef onlyIfTargetIsOfDef;
	}
}
