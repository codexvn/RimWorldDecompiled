using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039FE RID: 14846
	public class QuestNode_Delay : QuestNode
	{
		// Token: 0x0601459F RID: 83359 RVA: 0x006157BC File Offset: 0x006139BC
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x060145A0 RID: 83360 RVA: 0x006157D4 File Offset: 0x006139D4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Delay questPart_Delay;
			if (this.useAcceptanceExpiry.GetValue(slate))
			{
				questPart_Delay = this.MakeDelayQuestPart();
				questPart_Delay.delayTicks = QuestGen.quest.TicksUntilExpiry;
			}
			else if (this.delayTicksRange.GetValue(slate) != null)
			{
				questPart_Delay = new QuestPart_DelayRandom();
				((QuestPart_DelayRandom)questPart_Delay).delayTicksRange = this.delayTicksRange.GetValue(slate).Value;
			}
			else
			{
				questPart_Delay = this.MakeDelayQuestPart();
				questPart_Delay.delayTicks = this.delayTicks.GetValue(slate);
			}
			questPart_Delay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Delay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_Delay.reactivatable = this.reactivatable.GetValue(slate);
			questPart_Delay.waitUntilPlayerHasHomeMap = this.waitUntilPlayerHasHomeMap.GetValue(slate);
			if (!this.inspectStringTargets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				questPart_Delay.inspectString = this.inspectString.GetValue(slate);
				questPart_Delay.inspectStringTargets = new List<ISelectable>();
				questPart_Delay.inspectStringTargets.AddRange(this.inspectStringTargets.GetValue(slate));
			}
			if (this.isQuestTimeout.GetValue(slate))
			{
				questPart_Delay.isBad = true;
				questPart_Delay.expiryInfoPart = "QuestExpiresIn".Translate();
				questPart_Delay.expiryInfoPartTip = "QuestExpiresOn".Translate();
			}
			else
			{
				questPart_Delay.expiryInfoPart = this.expiryInfoPart.GetValue(slate);
				questPart_Delay.expiryInfoPartTip = this.expiryInfoPartTip.GetValue(slate);
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_Delay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_Delay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_Delay);
		}

		// Token: 0x060145A1 RID: 83361 RVA: 0x006159C5 File Offset: 0x00613BC5
		protected virtual QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_Delay();
		}

		// Token: 0x0400E0A7 RID: 57511
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400E0A8 RID: 57512
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400E0A9 RID: 57513
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x0400E0AA RID: 57514
		public SlateRef<string> expiryInfoPart;

		// Token: 0x0400E0AB RID: 57515
		public SlateRef<string> expiryInfoPartTip;

		// Token: 0x0400E0AC RID: 57516
		public SlateRef<string> inspectString;

		// Token: 0x0400E0AD RID: 57517
		public SlateRef<IEnumerable<ISelectable>> inspectStringTargets;

		// Token: 0x0400E0AE RID: 57518
		public SlateRef<int> delayTicks;

		// Token: 0x0400E0AF RID: 57519
		public SlateRef<IntRange?> delayTicksRange;

		// Token: 0x0400E0B0 RID: 57520
		public SlateRef<bool> isQuestTimeout;

		// Token: 0x0400E0B1 RID: 57521
		public SlateRef<bool> reactivatable;

		// Token: 0x0400E0B2 RID: 57522
		public SlateRef<bool> waitUntilPlayerHasHomeMap;

		// Token: 0x0400E0B3 RID: 57523
		public SlateRef<bool> useAcceptanceExpiry;

		// Token: 0x0400E0B4 RID: 57524
		public QuestNode node;
	}
}
