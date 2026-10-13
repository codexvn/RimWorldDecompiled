using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020025A2 RID: 9634
	public static class KidnapUtility
	{
		// Token: 0x0600D320 RID: 54048 RVA: 0x003E2ECC File Offset: 0x003E10CC
		public static bool IsKidnapped(this Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
