using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C12 RID: 7186
	public class QuestPart_AllowDecreesForLodger : QuestPart
	{
		// Token: 0x0600A010 RID: 40976 RVA: 0x002E259B File Offset: 0x002E079B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.lodger, "lodger", false);
		}

		// Token: 0x04006EB2 RID: 28338
		public Pawn lodger;
	}
}
