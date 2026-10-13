using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200348D RID: 13453
	public class PawnTable_PlayerPawnsTimetable : PawnTable_PlayerPawns
	{
		// Token: 0x06012EC6 RID: 77510 RVA: 0x0059A827 File Offset: 0x00598A27
		public PawnTable_PlayerPawnsTimetable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
