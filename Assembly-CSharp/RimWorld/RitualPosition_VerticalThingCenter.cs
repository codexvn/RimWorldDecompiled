using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200278B RID: 10123
	public abstract class RitualPosition_VerticalThingCenter : RitualPosition
	{
		// Token: 0x0600DDE0 RID: 56800 RVA: 0x00414D64 File Offset: 0x00412F64
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			Thing thing = spot.GetThingList(p.Map).FirstOrDefault((Thing t) => t == ritual.selectedTarget.Thing);
			CellRect cellRect = (thing != null) ? thing.OccupiedRect() : CellRect.SingleCell(spot);
			Map mapHeld = p.MapHeld;
			CellRect rect = this.GetRect(cellRect);
			Func<IntVec3, bool> func = CommonRitualCellPredicates.DefaultValidator(spot, mapHeld, p, cellRect);
			if (func(rect.CenterCell))
			{
				return new PawnStagePosition(rect.CenterCell, thing, Rot4.Invalid, this.highlight);
			}
			IntVec3 cell = IntVec3.Invalid;
			int num = 0;
			while (num < 16 && num < rect.Width)
			{
				IntVec3 randomCell = rect.RandomCell;
				if (func(randomCell))
				{
					cell = randomCell;
					break;
				}
				num++;
			}
			if (!cell.IsValid)
			{
				cell = this.GetFallbackSpot(cellRect, spot, p, ritual, func);
			}
			return new PawnStagePosition(cell, thing, Rot4.Invalid, this.highlight);
		}

		// Token: 0x0600DDE1 RID: 56801
		protected abstract CellRect GetRect(CellRect thingRect);

		// Token: 0x0400959F RID: 38303
		public IntVec3 offset;
	}
}
