using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002794 RID: 10132
	public abstract class RitualPosition_Cells : RitualPosition
	{
		// Token: 0x0600DDF7 RID: 56823
		public abstract void FindCells(List<IntVec3> cells, Thing thing, CellRect rect, IntVec3 spot, Rot4 rotation);

		// Token: 0x0600DDF8 RID: 56824 RVA: 0x00415350 File Offset: 0x00413550
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			RitualPosition_Cells.tmpPotentialCells.Clear();
			Thing thing = spot.GetThingList(ritual.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			Map mapHeld = p.MapHeld;
			CellRect rect = (thing != null) ? thing.OccupiedRect() : CellRect.CenteredOn(spot, 0);
			this.FindCells(RitualPosition_Cells.tmpPotentialCells, thing, rect, spot, (thing != null) ? thing.Rotation : Rot4.South);
			CommonRitualCellPredicates.RemoveLeastDesirableRitualCells(RitualPosition_Cells.tmpPotentialCells, spot, mapHeld, p, rect);
			Func<IntVec3, bool> validator = CommonRitualCellPredicates.DefaultValidator(spot, mapHeld, p, rect);
			IntVec3 intVec;
			if (RitualPosition_Cells.tmpPotentialCells.Count != 0)
			{
				intVec = RitualPosition_Cells.tmpPotentialCells[0];
			}
			else
			{
				intVec = this.GetFallbackSpot(rect, spot, p, ritual, validator);
			}
			if (!intVec.IsValid)
			{
				return null;
			}
			Rot4 orientation;
			if (this.faceThing)
			{
				if (this.facing != Rot4.Invalid)
				{
					Log.Error("Only one of faceThing and facing should be specified.");
				}
				orientation = Rot4.FromAngleFlat((thing.Position - intVec).AngleFlat);
			}
			else
			{
				orientation = this.facing;
			}
			return new PawnStagePosition(intVec, thing, orientation, this.highlight);
		}

		// Token: 0x0600DDF9 RID: 56825 RVA: 0x00415480 File Offset: 0x00413680
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.facing, "facing", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.faceThing, "faceThing", false, false);
		}

		// Token: 0x040095AA RID: 38314
		public Rot4 facing = Rot4.Invalid;

		// Token: 0x040095AB RID: 38315
		public bool faceThing;

		// Token: 0x040095AC RID: 38316
		private static List<IntVec3> tmpPotentialCells = new List<IntVec3>(8);
	}
}
