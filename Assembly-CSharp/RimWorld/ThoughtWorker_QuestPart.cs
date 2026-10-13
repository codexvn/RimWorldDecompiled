using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AC RID: 6316
	public class ThoughtWorker_QuestPart : ThoughtWorker
	{
		// Token: 0x0600905F RID: 36959 RVA: 0x002A9140 File Offset: 0x002A7340
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			QuestPart_SituationalThought questPart_SituationalThought = this.FindQuestPart(p);
			if (questPart_SituationalThought == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(questPart_SituationalThought.stage);
		}

		// Token: 0x06009060 RID: 36960 RVA: 0x002A916C File Offset: 0x002A736C
		public QuestPart_SituationalThought FindQuestPart(Pawn p)
		{
			List<QuestPart_SituationalThought> situationalThoughtQuestParts = Find.QuestManager.SituationalThoughtQuestParts;
			for (int i = 0; i < situationalThoughtQuestParts.Count; i++)
			{
				if (situationalThoughtQuestParts[i].quest.State == QuestState.Ongoing && situationalThoughtQuestParts[i].State == QuestPartState.Enabled && situationalThoughtQuestParts[i].def == this.def && situationalThoughtQuestParts[i].pawn == p)
				{
					return situationalThoughtQuestParts[i];
				}
			}
			return null;
		}
	}
}
