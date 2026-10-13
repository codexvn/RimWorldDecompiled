using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AAF RID: 15023
	public class QuestNode_AllowDecreesForLodger : QuestNode
	{
		// Token: 0x060147FA RID: 83962 RVA: 0x0061E7DF File Offset: 0x0061C9DF
		protected override void RunInt()
		{
			QuestGen.quest.AddPart(new QuestPart_AllowDecreesForLodger
			{
				lodger = this.lodger.GetValue(QuestGen.slate)
			});
		}

		// Token: 0x060147FB RID: 83963 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E305 RID: 58117
		public SlateRef<Pawn> lodger;
	}
}
