using System;

namespace Verse
{
	// Token: 0x020006E7 RID: 1767
	public class HediffCompProperties_MessageStageIncreased : HediffCompProperties
	{
		// Token: 0x06002BA7 RID: 11175 RVA: 0x000E3CBD File Offset: 0x000E1EBD
		public HediffCompProperties_MessageStageIncreased()
		{
			this.compClass = typeof(HediffComp_MessageStageIncreased);
		}

		// Token: 0x0400231F RID: 8991
		[MustTranslate]
		public string message;

		// Token: 0x04002320 RID: 8992
		public MessageTypeDef messageType;
	}
}
