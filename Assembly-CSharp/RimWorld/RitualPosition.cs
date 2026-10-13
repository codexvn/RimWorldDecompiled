using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002788 RID: 10120
	public abstract class RitualPosition : IExposable
	{
		// Token: 0x0600DDD6 RID: 56790
		public abstract PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual);

		// Token: 0x0600DDD7 RID: 56791 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanUse(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x0600DDD8 RID: 56792 RVA: 0x00414B44 File Offset: 0x00412D44
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.highlight, "highlight", false, false);
		}

		// Token: 0x0600DDD9 RID: 56793 RVA: 0x00414B58 File Offset: 0x00412D58
		protected virtual IntVec3 GetFallbackSpot(CellRect rect, IntVec3 spot, Pawn p, LordJob_Ritual ritual, Func<IntVec3, bool> Validator)
		{
			foreach (IntVec3 intVec in rect.AdjacentCells)
			{
				if (Validator(intVec))
				{
					return intVec;
				}
			}
			IntVec3 result;
			CellFinder.TryFindRandomCellNear(spot, p.Map, 3, new Predicate<IntVec3>(Validator.Invoke), out result, -1);
			return result;
		}

		// Token: 0x0400959C RID: 38300
		public bool highlight;
	}
}
