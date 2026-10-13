using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A10 RID: 14864
	public class QuestNode_SlateDump : QuestNode
	{
		// Token: 0x060145DD RID: 83421 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060145DE RID: 83422 RVA: 0x0061681E File Offset: 0x00614A1E
		protected override void RunInt()
		{
			Log.Message(QuestGen.slate.ToString());
		}
	}
}
