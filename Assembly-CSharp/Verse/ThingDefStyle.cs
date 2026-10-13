using System;

namespace Verse
{
	// Token: 0x020002E6 RID: 742
	public class ThingDefStyle
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x000551A1 File Offset: 0x000533A1
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x000551A9 File Offset: 0x000533A9
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x04001083 RID: 4227
		private ThingDef thingDef;

		// Token: 0x04001084 RID: 4228
		private ThingStyleDef styleDef;
	}
}
