using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B9D RID: 7069
	public class QuestPart_Filter_AllRequiredThingsLoaded : QuestPart_Filter
	{
		// Token: 0x06009DB2 RID: 40370 RVA: 0x002DB044 File Offset: 0x002D9244
		protected override bool Pass(SignalArgs args)
		{
			if (this.shuttle == null)
			{
				return false;
			}
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			return compShuttle != null && compShuttle.AllRequiredThingsLoaded;
		}

		// Token: 0x06009DB3 RID: 40371 RVA: 0x002DB072 File Offset: 0x002D9272
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x04006D43 RID: 27971
		public Thing shuttle;
	}
}
