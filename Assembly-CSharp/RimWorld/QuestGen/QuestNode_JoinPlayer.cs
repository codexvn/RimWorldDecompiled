using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD2 RID: 15058
	public class QuestNode_JoinPlayer : QuestNode
	{
		// Token: 0x0601486D RID: 84077 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601486E RID: 84078 RVA: 0x0061FFB8 File Offset: 0x0061E1B8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_JoinPlayer questPart_JoinPlayer = new QuestPart_JoinPlayer();
			questPart_JoinPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_JoinPlayer.joinPlayer = this.joinPlayer.GetValue(slate);
			questPart_JoinPlayer.makePrisoners = this.makePrisoners.GetValue(slate);
			questPart_JoinPlayer.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			questPart_JoinPlayer.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_JoinPlayer);
		}

		// Token: 0x0400E398 RID: 58264
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E399 RID: 58265
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E39A RID: 58266
		public SlateRef<bool> joinPlayer;

		// Token: 0x0400E39B RID: 58267
		public SlateRef<bool> makePrisoners;
	}
}
