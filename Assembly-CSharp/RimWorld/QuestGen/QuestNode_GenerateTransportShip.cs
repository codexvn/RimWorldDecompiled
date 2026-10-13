using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A11 RID: 14865
	public class QuestNode_GenerateTransportShip : QuestNode
	{
		// Token: 0x060145E0 RID: 83424 RVA: 0x00616830 File Offset: 0x00614A30
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			TransportShip transportShip = TransportShipMaker.MakeTransportShip(this.def.GetValue(slate), null, this.shipThing.GetValue(slate));
			if (!this.storeAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<TransportShip>(this.storeAs.GetValue(slate), transportShip, false);
			}
			QuestPart_SetupTransportShip questPart_SetupTransportShip = new QuestPart_SetupTransportShip();
			questPart_SetupTransportShip.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetupTransportShip.transportShip = transportShip;
			List<Thing> items;
			if (!(this.contents != null))
			{
				items = null;
			}
			else
			{
				items = (from c in this.contents.GetValue(slate)
				where !(c is Pawn)
				select c).ToList<Thing>();
			}
			questPart_SetupTransportShip.items = items;
			questPart_SetupTransportShip.pawns = ((this.contents != null) ? this.contents.GetValue(slate).OfType<Pawn>().ToList<Pawn>() : null);
			QuestPart_SetupTransportShip part = questPart_SetupTransportShip;
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x060145E1 RID: 83425 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E0EF RID: 57583
		public SlateRef<TransportShipDef> def;

		// Token: 0x0400E0F0 RID: 57584
		public SlateRef<Thing> shipThing;

		// Token: 0x0400E0F1 RID: 57585
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x0400E0F2 RID: 57586
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E0F3 RID: 57587
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
