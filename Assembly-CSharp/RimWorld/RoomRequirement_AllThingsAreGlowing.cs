using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037B4 RID: 14260
	public class RoomRequirement_AllThingsAreGlowing : RoomRequirement
	{
		// Token: 0x06013C38 RID: 80952 RVA: 0x005DDC30 File Offset: 0x005DBE30
		public override bool Met(Room r, Pawn p = null)
		{
			using (IEnumerator<Thing> enumerator = r.ContainedThings(this.thingDef).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.TryGetComp<CompGlower>().Glows)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06013C39 RID: 80953 RVA: 0x005DDC90 File Offset: 0x005DBE90
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
				yield break;
			}
			if (this.thingDef.GetCompProperties<CompProperties_Glower>() == null)
			{
				yield return "No comp glower on thingDef";
			}
			yield break;
		}

		// Token: 0x06013C3A RID: 80954 RVA: 0x005DDCA0 File Offset: 0x005DBEA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x0400DAD7 RID: 56023
		public ThingDef thingDef;
	}
}
