using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA7 RID: 7079
	public class QuestPart_Filter_AnyOnTransporterCapableOfHacking : QuestPart_Filter
	{
		// Token: 0x06009DCE RID: 40398 RVA: 0x002DB438 File Offset: 0x002D9638
		protected override bool Pass(SignalArgs args)
		{
			foreach (Thing thing in ((IEnumerable<Thing>)this.transporter.TryGetComp<CompTransporter>().GetDirectlyHeldThings()))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && HackUtility.IsCapableOfHacking(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009DCF RID: 40399 RVA: 0x002DB4A0 File Offset: 0x002D96A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.transporter, "transporter", false);
		}

		// Token: 0x04006D4F RID: 27983
		public Thing transporter;
	}
}
