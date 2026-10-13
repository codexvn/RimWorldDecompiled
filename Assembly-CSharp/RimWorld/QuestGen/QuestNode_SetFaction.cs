using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AF3 RID: 15091
	public class QuestNode_SetFaction : QuestNode
	{
		// Token: 0x060148D2 RID: 84178 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148D3 RID: 84179 RVA: 0x0062161C File Offset: 0x0061F81C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_SetFaction questPart_SetFaction = new QuestPart_SetFaction();
			questPart_SetFaction.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFaction.faction = this.faction.GetValue(slate);
			questPart_SetFaction.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetFaction);
		}

		// Token: 0x0400E411 RID: 58385
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E412 RID: 58386
		public SlateRef<Faction> faction;

		// Token: 0x0400E413 RID: 58387
		public SlateRef<IEnumerable<Thing>> things;
	}
}
