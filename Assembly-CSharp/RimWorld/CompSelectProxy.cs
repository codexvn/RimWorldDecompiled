using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DF8 RID: 11768
	public class CompSelectProxy : ThingComp
	{
		// Token: 0x060108FC RID: 67836 RVA: 0x004C653D File Offset: 0x004C473D
		public override void PostExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thingToSelect, "thingToSelect", false);
		}

		// Token: 0x0400ACA3 RID: 44195
		public Thing thingToSelect;
	}
}
