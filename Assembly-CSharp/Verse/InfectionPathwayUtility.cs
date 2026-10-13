using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D23 RID: 3363
	public static class InfectionPathwayUtility
	{
		// Token: 0x060055D2 RID: 21970 RVA: 0x001BB5AB File Offset: 0x001B97AB
		public static void AddInfectionPathway(InfectionPathwayDef def, Pawn to, Pawn source = null)
		{
			if (to.infectionVectors == null)
			{
				return;
			}
			to.infectionVectors.AddInfectionVector(def, source);
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x001BB5C4 File Offset: 0x001B97C4
		public static void AddInfectionPathways(List<InfectionPathwayDef> defs, Pawn to, Pawn source = null)
		{
			if (to.infectionVectors == null)
			{
				return;
			}
			foreach (InfectionPathwayDef def in defs)
			{
				to.infectionVectors.AddInfectionVector(def, source);
			}
		}
	}
}
