using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003424 RID: 13348
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x1700312D RID: 12589
		// (get) Token: 0x06012C81 RID: 76929 RVA: 0x00593A50 File Offset: 0x00591C50
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x1700312E RID: 12590
		// (get) Token: 0x06012C82 RID: 76930 RVA: 0x00593A57 File Offset: 0x00591C57
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && (p.Faction == null || p.Faction == Faction.OfInsects) && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map) && !p.IsPrisonerInPrisonCell()
				select p;
			}
		}
	}
}
