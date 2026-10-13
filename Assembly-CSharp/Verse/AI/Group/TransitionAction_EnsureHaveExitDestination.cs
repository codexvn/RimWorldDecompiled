using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010B5 RID: 4277
	public class TransitionAction_EnsureHaveExitDestination : TransitionAction
	{
		// Token: 0x060068E3 RID: 26851 RVA: 0x00211E88 File Offset: 0x00210088
		public override void DoAction(Transition trans)
		{
			LordToil_Travel lordToil_Travel = (LordToil_Travel)trans.target;
			if (lordToil_Travel.HasDestination())
			{
				return;
			}
			Pawn pawn;
			if ((from x in lordToil_Travel.lord.ownedPawns
			where x.Spawned
			select x).TryRandomElement(out pawn))
			{
				IntVec3 destination;
				if (!CellFinder.TryFindRandomPawnExitCell(pawn, out destination))
				{
					RCellFinder.TryFindRandomPawnEntryCell(out destination, pawn.Map, 0f, false, null);
				}
				lordToil_Travel.SetDestination(destination);
			}
		}
	}
}
