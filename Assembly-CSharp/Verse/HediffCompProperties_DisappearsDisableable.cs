using System;

namespace Verse
{
	// Token: 0x020006A2 RID: 1698
	public class HediffCompProperties_DisappearsDisableable : HediffCompProperties_Disappears
	{
		// Token: 0x06002AB9 RID: 10937 RVA: 0x000E0578 File Offset: 0x000DE778
		public HediffCompProperties_DisappearsDisableable()
		{
			this.compClass = typeof(HediffComp_DisappearsDisableable);
		}

		// Token: 0x04002291 RID: 8849
		public bool initiallyDisabled;
	}
}
