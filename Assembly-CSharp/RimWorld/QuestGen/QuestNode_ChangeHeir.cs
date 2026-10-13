using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AB7 RID: 15031
	public class QuestNode_ChangeHeir : QuestNode
	{
		// Token: 0x06014813 RID: 83987 RVA: 0x0061EC80 File Offset: 0x0061CE80
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeHeir questPart_ChangeHeir = new QuestPart_ChangeHeir();
			questPart_ChangeHeir.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeHeir.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_ChangeHeir.holder = this.holder.GetValue(slate);
			questPart_ChangeHeir.heir = this.heir.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeHeir);
		}

		// Token: 0x06014814 RID: 83988 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E320 RID: 58144
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E321 RID: 58145
		public SlateRef<Faction> faction;

		// Token: 0x0400E322 RID: 58146
		public SlateRef<Thing> factionOf;

		// Token: 0x0400E323 RID: 58147
		public SlateRef<Pawn> holder;

		// Token: 0x0400E324 RID: 58148
		public SlateRef<Pawn> heir;
	}
}
