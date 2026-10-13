using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A52 RID: 14930
	public class QuestNode_GetMarketValue : QuestNode
	{
		// Token: 0x060146B8 RID: 83640 RVA: 0x0061937C File Offset: 0x0061757C
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x060146B9 RID: 83641 RVA: 0x00619385 File Offset: 0x00617585
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060146BA RID: 83642 RVA: 0x00619394 File Offset: 0x00617594
		private bool DoWork(Slate slate)
		{
			float num = 0f;
			if (this.things.GetValue(slate) != null)
			{
				foreach (Thing thing in this.things.GetValue(slate))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
			return true;
		}

		// Token: 0x0400E1C7 RID: 57799
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E1C8 RID: 57800
		public SlateRef<IEnumerable<Thing>> things;
	}
}
