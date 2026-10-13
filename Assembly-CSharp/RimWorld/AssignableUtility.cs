using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BF7 RID: 11255
	public static class AssignableUtility
	{
		// Token: 0x0600FC38 RID: 64568 RVA: 0x00492774 File Offset: 0x00490974
		public static Pawn GetAssignedPawn(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawnsForReading[0];
		}

		// Token: 0x0600FC39 RID: 64569 RVA: 0x004927A8 File Offset: 0x004909A8
		public static IEnumerable<Pawn> GetAssignedPawns(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawns;
		}
	}
}
