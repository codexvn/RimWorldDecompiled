using System;

namespace Verse
{
	// Token: 0x020006DE RID: 1758
	public class HediffCompProperties_MessageAboveSeverity : HediffCompProperties
	{
		// Token: 0x06002B8A RID: 11146 RVA: 0x000E37E0 File Offset: 0x000E19E0
		public HediffCompProperties_MessageAboveSeverity()
		{
			this.compClass = typeof(HediffComp_MessageAboveSeverity);
		}

		// Token: 0x0400230B RID: 8971
		public float severity;

		// Token: 0x0400230C RID: 8972
		public MessageTypeDef messageType;

		// Token: 0x0400230D RID: 8973
		[MustTranslate]
		public string message;
	}
}
