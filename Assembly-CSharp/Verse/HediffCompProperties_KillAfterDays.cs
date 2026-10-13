using System;

namespace Verse
{
	// Token: 0x020006D6 RID: 1750
	public class HediffCompProperties_KillAfterDays : HediffCompProperties
	{
		// Token: 0x06002B70 RID: 11120 RVA: 0x000E3225 File Offset: 0x000E1425
		public HediffCompProperties_KillAfterDays()
		{
			this.compClass = typeof(HediffComp_KillAfterDays);
		}

		// Token: 0x040022FE RID: 8958
		public int days;
	}
}
