using System;

namespace Verse
{
	// Token: 0x0200030C RID: 780
	public class ThingStyleChance
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x0005CE15 File Offset: 0x0005B015
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x0005CE1D File Offset: 0x0005B01D
		public float Chance
		{
			get
			{
				return this.chance;
			}
		}

		// Token: 0x04001274 RID: 4724
		private ThingStyleDef styleDef;

		// Token: 0x04001275 RID: 4725
		private float chance = 1f;
	}
}
