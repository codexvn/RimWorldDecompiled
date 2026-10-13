using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B0B RID: 11019
	public class Mechlink : ThingWithComps
	{
		// Token: 0x0600F60C RID: 62988 RVA: 0x00477122 File Offset: 0x00475322
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sentMechsToPlayer, "sentMechsToPlayer", false, false);
		}

		// Token: 0x0400A1F6 RID: 41462
		public bool sentMechsToPlayer;
	}
}
