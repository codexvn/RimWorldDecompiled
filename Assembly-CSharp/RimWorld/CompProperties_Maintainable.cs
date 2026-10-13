using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D26 RID: 11558
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x060103CA RID: 66506 RVA: 0x004B1C03 File Offset: 0x004AFE03
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		// Token: 0x0400A994 RID: 43412
		public int ticksHealthy = 1000;

		// Token: 0x0400A995 RID: 43413
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x0400A996 RID: 43414
		public int damagePerTickRare = 10;
	}
}
