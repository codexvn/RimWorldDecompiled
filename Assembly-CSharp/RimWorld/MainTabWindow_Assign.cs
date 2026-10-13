using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020033F6 RID: 13302
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x17003102 RID: 12546
		// (get) Token: 0x06012B51 RID: 76625 RVA: 0x00589D98 File Offset: 0x00587F98
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x17003103 RID: 12547
		// (get) Token: 0x06012B52 RID: 76626 RVA: 0x00589D9F File Offset: 0x00587F9F
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists
				where !pawn.DevelopmentalStage.Baby()
				select pawn;
			}
		}
	}
}
