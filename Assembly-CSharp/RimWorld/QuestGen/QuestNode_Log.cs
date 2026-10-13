using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A0E RID: 14862
	public class QuestNode_Log : QuestNode
	{
		// Token: 0x060145D7 RID: 83415 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060145D8 RID: 83416 RVA: 0x0061679B File Offset: 0x0061499B
		protected override void RunInt()
		{
			Log.Message("QuestNode_Log: " + this.message.ToString(QuestGen.slate));
		}

		// Token: 0x0400E0EC RID: 57580
		[NoTranslate]
		public SlateRef<object> message;
	}
}
