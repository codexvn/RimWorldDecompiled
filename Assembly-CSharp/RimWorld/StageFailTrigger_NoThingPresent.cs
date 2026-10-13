using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026B2 RID: 9906
	public class StageFailTrigger_NoThingPresent : StageFailTrigger
	{
		// Token: 0x0600D941 RID: 55617 RVA: 0x004054E0 File Offset: 0x004036E0
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
			using (List<Thing>.Enumerator enumerator = thing2.Map.listerBuldingOfDefInProximity.GetForCell(thing2.Position, (float)this.maxDistance, this.thingDef, null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoom(RegionType.Set_All) == thing2.GetRoom(RegionType.Set_All))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600D942 RID: 55618 RVA: 0x004055A4 File Offset: 0x004037A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.maxDistance, "maxDistance", 0, false);
		}

		// Token: 0x04009333 RID: 37683
		public ThingDef onlyIfTargetIsOfDef;

		// Token: 0x04009334 RID: 37684
		public int maxDistance = 12;

		// Token: 0x04009335 RID: 37685
		public ThingDef thingDef;
	}
}
