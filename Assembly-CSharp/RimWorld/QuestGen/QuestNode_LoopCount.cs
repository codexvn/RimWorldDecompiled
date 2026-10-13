using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AD8 RID: 15064
	public class QuestNode_LoopCount : QuestNode
	{
		// Token: 0x0601487F RID: 84095 RVA: 0x00620528 File Offset: 0x0061E728
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					if (!this.node.TestRun(slate))
					{
						return false;
					}
				}
				finally
				{
					slate.PopPrefix();
				}
			}
			return true;
		}

		// Token: 0x06014880 RID: 84096 RVA: 0x006205A0 File Offset: 0x0061E7A0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					QuestGen.slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					this.node.Run();
				}
				finally
				{
					QuestGen.slate.PopPrefix();
				}
			}
		}

		// Token: 0x0400E3BB RID: 58299
		public QuestNode node;

		// Token: 0x0400E3BC RID: 58300
		public SlateRef<int> loopCount;

		// Token: 0x0400E3BD RID: 58301
		[NoTranslate]
		public SlateRef<string> storeLoopCounterAs;
	}
}
