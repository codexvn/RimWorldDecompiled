using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000F96 RID: 3990
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06006260 RID: 25184 RVA: 0x001F0BBD File Offset: 0x001EEDBD
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
