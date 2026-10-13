using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002789 RID: 10121
	public class RitualPosition_BesideThing : RitualPosition
	{
		// Token: 0x0600DDDB RID: 56795 RVA: 0x00414BD0 File Offset: 0x00412DD0
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Thing thing = spot.GetThingList(p.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			CellRect cellRect = (thing != null) ? thing.OccupiedRect() : CellRect.CenteredOn(spot, 0);
			Map mapHeld = p.MapHeld;
			IntVec3 intVec = (thing != null) ? IntVec3.West.RotatedBy(thing.Rotation) : IntVec3.West;
			CellRect cellRect2 = new CellRect(cellRect.minX + intVec.x, cellRect.minZ + intVec.z, 1, cellRect.Height);
			CellRect cellRect3 = new CellRect(cellRect.maxX - intVec.z, cellRect.minZ - intVec.z, 1, cellRect.Height);
			Func<IntVec3, bool> func = CommonRitualCellPredicates.DefaultValidator(spot, mapHeld, p, cellRect);
			IntVec3 cell = IntVec3.Invalid;
			for (int i = 0; i < 16; i++)
			{
				IntVec3 randomCell;
				if (Rand.Chance(0.5f))
				{
					randomCell = cellRect2.RandomCell;
				}
				else
				{
					randomCell = cellRect3.RandomCell;
				}
				if (func(randomCell))
				{
					cell = randomCell;
					break;
				}
			}
			if (!cell.IsValid)
			{
				cell = this.GetFallbackSpot(cellRect, spot, p, ritual, func);
			}
			return new PawnStagePosition(cell, thing, this.faceThing ? Rot4.FromIntVec3(intVec.Inverse()) : Rot4.Invalid, this.highlight);
		}

		// Token: 0x0600DDDC RID: 56796 RVA: 0x00414D2D File Offset: 0x00412F2D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.faceThing, "faceThing", false, false);
		}

		// Token: 0x0400959D RID: 38301
		public bool faceThing;
	}
}
