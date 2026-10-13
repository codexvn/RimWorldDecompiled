using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026B1 RID: 9905
	public class StageFailTrigger_NoPoweredLoudspeakers : StageFailTrigger
	{
		// Token: 0x0600D93E RID: 55614 RVA: 0x004053EC File Offset: 0x004035EC
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
			if (thing2 == null || thing2.Destroyed)
			{
				return true;
			}
			foreach (Thing thing3 in thing2.Map.listerBuldingOfDefInProximity.GetForCell(thing2.Position, 12f, ThingDefOf.Loudspeaker, null))
			{
				CompPowerTrader compPowerTrader = thing3.TryGetComp<CompPowerTrader>();
				if (thing3.GetRoom(RegionType.Set_All) == thing2.GetRoom(RegionType.Set_All) && compPowerTrader.PowerNet != null && compPowerTrader.PowerNet.HasActivePowerSource)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D93F RID: 55615 RVA: 0x004054C8 File Offset: 0x004036C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x04009331 RID: 37681
		public ThingDef onlyIfTargetIsOfDef;

		// Token: 0x04009332 RID: 37682
		public const int maxDistance = 12;
	}
}
