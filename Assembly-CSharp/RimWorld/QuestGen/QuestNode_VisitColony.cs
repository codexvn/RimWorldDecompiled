using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A91 RID: 14993
	public class QuestNode_VisitColony : QuestNode
	{
		// Token: 0x06014796 RID: 83862 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014797 RID: 83863 RVA: 0x0061D824 File Offset: 0x0061BA24
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_VisitColony questPart_VisitColony = new QuestPart_VisitColony();
			questPart_VisitColony.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_VisitColony.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_VisitColony.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_VisitColony.faction = this.faction.GetValue(slate);
			questPart_VisitColony.durationTicks = this.durationTicks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_VisitColony);
		}

		// Token: 0x0400E2BB RID: 58043
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E2BC RID: 58044
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E2BD RID: 58045
		public SlateRef<Faction> faction;

		// Token: 0x0400E2BE RID: 58046
		public SlateRef<int?> durationTicks;
	}
}
