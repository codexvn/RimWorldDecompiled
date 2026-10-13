using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B02 RID: 15106
	public class QuestNode_TrackWhenExitMentalState : QuestNode
	{
		// Token: 0x0601490A RID: 84234 RVA: 0x00622421 File Offset: 0x00620621
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x0601490B RID: 84235 RVA: 0x00622438 File Offset: 0x00620638
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_TrackWhenExitMentalState questPart_TrackWhenExitMentalState = new QuestPart_TrackWhenExitMentalState();
			questPart_TrackWhenExitMentalState.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_TrackWhenExitMentalState.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_TrackWhenExitMentalState.mentalStateDef = this.mentalStateDef.GetValue(slate);
			questPart_TrackWhenExitMentalState.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignal.GetValue(slate));
			questPart_TrackWhenExitMentalState.inSignals = new List<string>();
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_TrackWhenExitMentalState.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_TrackWhenExitMentalState);
		}

		// Token: 0x0400E455 RID: 58453
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400E456 RID: 58454
		public SlateRef<MentalStateDef> mentalStateDef;

		// Token: 0x0400E457 RID: 58455
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x0400E458 RID: 58456
		[NoTranslate]
		public SlateRef<string> outSignal;
	}
}
