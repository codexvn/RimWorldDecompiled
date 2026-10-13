using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001572 RID: 5490
	public class WorkGiver_Uninstall : WorkGiver_RemoveBuilding
	{
		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06008240 RID: 33344 RVA: 0x0022523F File Offset: 0x0022343F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06008241 RID: 33345 RVA: 0x00272B93 File Offset: 0x00270D93
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Uninstall;
			}
		}

		// Token: 0x06008242 RID: 33346 RVA: 0x00272B9A File Offset: 0x00270D9A
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.Claimable)
			{
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}
	}
}
