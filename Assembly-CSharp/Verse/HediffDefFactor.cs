using System;

namespace Verse
{
	// Token: 0x0200071A RID: 1818
	public class HediffDefFactor
	{
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002C5A RID: 11354 RVA: 0x000E6221 File Offset: 0x000E4421
		public HediffDef HediffDef
		{
			get
			{
				return this.hediffDef;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x000E6229 File Offset: 0x000E4429
		public float Factor
		{
			get
			{
				return this.factor;
			}
		}

		// Token: 0x0400238F RID: 9103
		private HediffDef hediffDef;

		// Token: 0x04002390 RID: 9104
		private float factor = 1f;
	}
}
