using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A7D RID: 14973
	public class QuestNode_GetRelationsInfo : QuestNode
	{
		// Token: 0x06014751 RID: 83793 RVA: 0x0061C347 File Offset: 0x0061A547
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014752 RID: 83794 RVA: 0x0061C351 File Offset: 0x0061A551
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014753 RID: 83795 RVA: 0x0061C360 File Offset: 0x0061A560
		private void SetVars(Slate slate)
		{
			if (this.pawn.GetValue(slate) == null)
			{
				return;
			}
			if (this.otherPawns.GetValue(slate) == null)
			{
				return;
			}
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
			int num = 0;
			foreach (Pawn other in this.otherPawns.GetValue(slate))
			{
				PawnRelationDef mostImportantRelation = this.pawn.GetValue(slate).GetMostImportantRelation(other);
				if (mostImportantRelation != null)
				{
					QuestNode_GetRelationsInfo.tmpRelations.Add(mostImportantRelation.GetGenderSpecificLabel(other));
				}
				else
				{
					num++;
				}
			}
			if (num == 1)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(this.nonRelatedLabel.GetValue(slate));
			}
			else if (num >= 2)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(num.ToString() + " " + this.nonRelatedLabelPlural.GetValue(slate));
			}
			if (!QuestNode_GetRelationsInfo.tmpRelations.Any<string>())
			{
				return;
			}
			slate.Set<string>(this.storeAs.GetValue(slate), QuestNode_GetRelationsInfo.tmpRelations.ToCommaList(true, false), false);
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
		}

		// Token: 0x0400E265 RID: 57957
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E266 RID: 57958
		public SlateRef<IEnumerable<Pawn>> otherPawns;

		// Token: 0x0400E267 RID: 57959
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E268 RID: 57960
		public SlateRef<string> nonRelatedLabel;

		// Token: 0x0400E269 RID: 57961
		public SlateRef<string> nonRelatedLabelPlural;

		// Token: 0x0400E26A RID: 57962
		private static List<string> tmpRelations = new List<string>();
	}
}
