using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003488 RID: 13448
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x06012EB3 RID: 77491 RVA: 0x0059A624 File Offset: 0x00598824
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return (from p in input
			orderby p.Name == null || p.Name.Numerical, p.RaceProps.petness, p.RaceProps.baseBodySize, p.def.label
			select p).ThenBy(delegate(Pawn p)
			{
				if (!(p.Name is NameSingle))
				{
					return 0;
				}
				return ((NameSingle)p.Name).Number;
			});
		}

		// Token: 0x06012EB4 RID: 77492 RVA: 0x0059A6E6 File Offset: 0x005988E6
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}
	}
}
