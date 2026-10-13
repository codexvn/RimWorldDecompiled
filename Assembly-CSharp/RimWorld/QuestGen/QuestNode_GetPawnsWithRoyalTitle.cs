using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A69 RID: 14953
	public class QuestNode_GetPawnsWithRoyalTitle : QuestNode
	{
		// Token: 0x06014709 RID: 83721 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601470A RID: 83722 RVA: 0x0061B548 File Offset: 0x00619748
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				IEnumerable<Pawn> filteredPawns = this.GetFilteredPawns(this.pawns.GetValue(slate));
				slate.Set<IEnumerable<Pawn>>(this.storeAs.GetValue(slate), filteredPawns, false);
				if (this.storeCountAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeCountAs.GetValue(slate), filteredPawns.Count<Pawn>(), false);
				}
				if (this.storePawnsLabelAs.GetValue(slate) != null)
				{
					slate.Set<string>(this.storePawnsLabelAs.GetValue(slate), (from p in filteredPawns
					select p.LabelNoCountColored.Resolve()).ToCommaList(true, false), false);
				}
			}
		}

		// Token: 0x0601470B RID: 83723 RVA: 0x0061B607 File Offset: 0x00619807
		private IEnumerable<Pawn> GetFilteredPawns(List<Pawn> pawns)
		{
			Slate slate = QuestGen.slate;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].royalty != null && pawns[i].royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400E225 RID: 57893
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x0400E226 RID: 57894
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E227 RID: 57895
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x0400E228 RID: 57896
		[NoTranslate]
		public SlateRef<string> storePawnsLabelAs;
	}
}
