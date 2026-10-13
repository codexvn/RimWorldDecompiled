using System;

namespace Verse
{
	// Token: 0x020006A9 RID: 1705
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x06002ACB RID: 10955 RVA: 0x000E073E File Offset: 0x000DE93E
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x04002293 RID: 8851
		public bool sendLetterWhenDiscovered;

		// Token: 0x04002294 RID: 8852
		public string discoverLetterLabel;

		// Token: 0x04002295 RID: 8853
		public string discoverLetterText;

		// Token: 0x04002296 RID: 8854
		public MessageTypeDef messageType;

		// Token: 0x04002297 RID: 8855
		public LetterDef letterType;
	}
}
