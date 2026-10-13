using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002741 RID: 10049
	public class RitualOutcomeComp_BuildingOfDefPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x170021C7 RID: 8647
		// (get) Token: 0x0600DC84 RID: 56452 RVA: 0x0040E924 File Offset: 0x0040CB24
		protected override string LabelForDesc
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x0600DC85 RID: 56453 RVA: 0x0040E936 File Offset: 0x0040CB36
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			return cell.GetFirstThing(map, this.def);
		}

		// Token: 0x040094CF RID: 38095
		public ThingDef def;
	}
}
