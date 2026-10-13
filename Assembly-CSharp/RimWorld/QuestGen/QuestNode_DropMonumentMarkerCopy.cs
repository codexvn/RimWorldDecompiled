using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ABE RID: 15038
	public class QuestNode_DropMonumentMarkerCopy : QuestNode
	{
		// Token: 0x06014828 RID: 84008 RVA: 0x0061EFF5 File Offset: 0x0061D1F5
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06014829 RID: 84009 RVA: 0x0061F004 File Offset: 0x0061D204
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_DropMonumentMarkerCopy questPart_DropMonumentMarkerCopy = new QuestPart_DropMonumentMarkerCopy();
			questPart_DropMonumentMarkerCopy.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_DropMonumentMarkerCopy.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DropMonumentMarkerCopy.outSignalResult = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalResult.GetValue(slate));
			questPart_DropMonumentMarkerCopy.destroyOrPassToWorldOnCleanup = this.destroyOrPassToWorldOnCleanup.GetValue(slate);
			QuestGen.quest.AddPart(questPart_DropMonumentMarkerCopy);
		}

		// Token: 0x0400E334 RID: 58164
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E335 RID: 58165
		[NoTranslate]
		public SlateRef<string> outSignalResult;

		// Token: 0x0400E336 RID: 58166
		public SlateRef<bool> destroyOrPassToWorldOnCleanup;
	}
}
