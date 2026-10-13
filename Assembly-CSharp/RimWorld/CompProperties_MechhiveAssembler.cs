using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D38 RID: 11576
	public class CompProperties_MechhiveAssembler : CompProperties
	{
		// Token: 0x06010441 RID: 66625 RVA: 0x004B3AF7 File Offset: 0x004B1CF7
		public CompProperties_MechhiveAssembler()
		{
			this.compClass = typeof(CompMechhiveAssembler);
		}

		// Token: 0x0400A9E5 RID: 43493
		public List<PawnGenOption> options = new List<PawnGenOption>();
	}
}
