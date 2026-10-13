using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200348C RID: 13452
	public class PawnTable_PlayerPawns : PawnTable
	{
		// Token: 0x06012EC4 RID: 77508 RVA: 0x0059A81F File Offset: 0x00598A1F
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return PlayerPawnsDisplayOrderUtility.InOrder(input);
		}

		// Token: 0x06012EC5 RID: 77509 RVA: 0x0059A6E6 File Offset: 0x005988E6
		public PawnTable_PlayerPawns(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
