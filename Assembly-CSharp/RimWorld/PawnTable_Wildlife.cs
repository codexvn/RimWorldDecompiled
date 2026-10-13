using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200348E RID: 13454
	public class PawnTable_Wildlife : PawnTable
	{
		// Token: 0x06012EC7 RID: 77511 RVA: 0x0059A834 File Offset: 0x00598A34
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}

		// Token: 0x06012EC8 RID: 77512 RVA: 0x0059A6E6 File Offset: 0x005988E6
		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
