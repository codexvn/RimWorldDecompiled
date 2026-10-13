using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002742 RID: 10050
	public class RitualOutcomeComp_BuildingOfAnyDefPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x170021C8 RID: 8648
		// (get) Token: 0x0600DC87 RID: 56455 RVA: 0x0040E950 File Offset: 0x0040CB50
		protected override string LabelForDesc
		{
			get
			{
				return "RitualOutcomeLabelAnyOfThese".Translate() + ": " + (from d in this.defs
				select d.LabelCap.Resolve()).ToCommaList(false, false);
			}
		}

		// Token: 0x0600DC88 RID: 56456 RVA: 0x0040E9AC File Offset: 0x0040CBAC
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			foreach (ThingDef def in this.defs)
			{
				Thing firstThing = cell.GetFirstThing(map, def);
				if (firstThing != null)
				{
					return firstThing;
				}
			}
			return null;
		}

		// Token: 0x040094D0 RID: 38096
		public List<ThingDef> defs;
	}
}
