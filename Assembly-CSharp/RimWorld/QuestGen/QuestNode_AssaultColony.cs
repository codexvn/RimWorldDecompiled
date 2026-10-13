using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A90 RID: 14992
	public class QuestNode_AssaultColony : QuestNode
	{
		// Token: 0x06014793 RID: 83859 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014794 RID: 83860 RVA: 0x0061D75C File Offset: 0x0061B95C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_AssaultColony questPart_AssaultColony = new QuestPart_AssaultColony();
			questPart_AssaultColony.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_AssaultColony.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_AssaultColony.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_AssaultColony.faction = this.faction.GetValue(slate);
			questPart_AssaultColony.canTimeoutOrFlee = (this.canTimeoutOrFlee.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_AssaultColony);
		}

		// Token: 0x0400E2B7 RID: 58039
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E2B8 RID: 58040
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E2B9 RID: 58041
		public SlateRef<Faction> faction;

		// Token: 0x0400E2BA RID: 58042
		public SlateRef<bool?> canTimeoutOrFlee;
	}
}
