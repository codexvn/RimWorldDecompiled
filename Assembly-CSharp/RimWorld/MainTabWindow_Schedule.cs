using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003422 RID: 13346
	public class MainTabWindow_Schedule : MainTabWindow_PawnTable
	{
		// Token: 0x1700312B RID: 12587
		// (get) Token: 0x06012C7A RID: 76922 RVA: 0x005939D6 File Offset: 0x00591BD6
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x1700312C RID: 12588
		// (get) Token: 0x06012C7B RID: 76923 RVA: 0x005939DD File Offset: 0x00591BDD
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from pawn in base.Pawns.Concat(Find.CurrentMap.mapPawns.ColonySubhumansControllable)
				where !pawn.DevelopmentalStage.Baby()
				select pawn;
			}
		}

		// Token: 0x06012C7C RID: 76924 RVA: 0x00593A1D File Offset: 0x00591C1D
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, 65f));
		}

		// Token: 0x0400C332 RID: 49970
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x0400C333 RID: 49971
		private const int TimeAssignmentSelectorHeight = 65;
	}
}
