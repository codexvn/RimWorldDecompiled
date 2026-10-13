using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156A RID: 5482
	public class WorkGiver_Deconstruct : WorkGiver_RemoveBuilding
	{
		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06008207 RID: 33287 RVA: 0x00223A33 File Offset: 0x00221C33
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x06008208 RID: 33288 RVA: 0x00272003 File Offset: 0x00270203
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Deconstruct;
			}
		}

		// Token: 0x06008209 RID: 33289 RVA: 0x0027200C File Offset: 0x0027020C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t.GetInnerIfMinified() as Building;
			return building != null && building.DeconstructibleBy(pawn.Faction) && base.HasJobOnThing(pawn, t, forced);
		}
	}
}
