using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A41 RID: 14913
	public class QuestNode_GetEventDelays : QuestNode
	{
		// Token: 0x0601467D RID: 83581 RVA: 0x006187B7 File Offset: 0x006169B7
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0601467E RID: 83582 RVA: 0x006187C1 File Offset: 0x006169C1
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0601467F RID: 83583 RVA: 0x006187D0 File Offset: 0x006169D0
		private void SetVars(Slate slate)
		{
			if (this.intervalTicksRange.GetValue(slate).max <= 0)
			{
				Log.Error("intervalTicksRange with max <= 0");
				return;
			}
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num += this.intervalTicksRange.GetValue(slate).RandomInRange;
				if (num > this.durationTicks.GetValue(slate))
				{
					break;
				}
				slate.Set<int>(this.storeDelaysAs.GetValue(slate).Formatted(num2.Named("INDEX")), num, false);
				num2++;
			}
			slate.Set<int>(this.storeCountAs.GetValue(slate), num2, false);
		}

		// Token: 0x0400E18D RID: 57741
		public SlateRef<int> durationTicks;

		// Token: 0x0400E18E RID: 57742
		public SlateRef<IntRange> intervalTicksRange;

		// Token: 0x0400E18F RID: 57743
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x0400E190 RID: 57744
		[NoTranslate]
		public SlateRef<string> storeDelaysAs;
	}
}
