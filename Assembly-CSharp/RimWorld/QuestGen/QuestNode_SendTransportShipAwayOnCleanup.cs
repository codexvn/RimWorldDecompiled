using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A18 RID: 14872
	public class QuestNode_SendTransportShipAwayOnCleanup : QuestNode
	{
		// Token: 0x060145F6 RID: 83446 RVA: 0x00616BB8 File Offset: 0x00614DB8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SendTransportShipAwayOnCleanup part = new QuestPart_SendTransportShipAwayOnCleanup
			{
				transportShip = this.transportShip.GetValue(slate),
				unsatisfiedDropMode = (this.unsatisfiedDropMode.GetValue(slate) ?? TransportShipDropMode.NonRequired),
				unloadContents = this.unloadContents.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x060145F7 RID: 83447 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E102 RID: 57602
		public SlateRef<TransportShip> transportShip;

		// Token: 0x0400E103 RID: 57603
		public SlateRef<bool> unloadContents;

		// Token: 0x0400E104 RID: 57604
		public SlateRef<TransportShipDropMode?> unsatisfiedDropMode;
	}
}
