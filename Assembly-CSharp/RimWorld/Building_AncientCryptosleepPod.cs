using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020029CB RID: 10699
	public class Building_AncientCryptosleepPod : Building_AncientCryptosleepCasket
	{
		// Token: 0x0600EBD9 RID: 60377 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}
	}
}
