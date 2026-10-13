using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037A8 RID: 14248
	public class RoomRequirement_Thing : RoomRequirement
	{
		// Token: 0x06013BD8 RID: 80856 RVA: 0x005DCCAF File Offset: 0x005DAEAF
		public override bool Met(Room r, Pawn p = null)
		{
			return r.ContainsThing(this.thingDef);
		}

		// Token: 0x06013BD9 RID: 80857 RVA: 0x005DCCC0 File Offset: 0x005DAEC0
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_Thing roomRequirement_Thing = (RoomRequirement_Thing)other;
			return this.thingDef == roomRequirement_Thing.thingDef;
		}

		// Token: 0x06013BDA RID: 80858 RVA: 0x005DCCED File Offset: 0x005DAEED
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.thingDef.label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x06013BDB RID: 80859 RVA: 0x005DCD2D File Offset: 0x005DAF2D
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
			}
			yield break;
		}

		// Token: 0x06013BDC RID: 80860 RVA: 0x005DCD3D File Offset: 0x005DAF3D
		public override bool PlayerCanBuildNow()
		{
			return this.thingDef.IsResearchFinished;
		}

		// Token: 0x06013BDD RID: 80861 RVA: 0x005DCD4A File Offset: 0x005DAF4A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x0400DAB4 RID: 55988
		public ThingDef thingDef;
	}
}
