using System;

namespace Verse
{
	// Token: 0x020006E3 RID: 1763
	public class HediffCompProperties_MessageBase : HediffCompProperties
	{
		// Token: 0x06002B9F RID: 11167 RVA: 0x000E3B79 File Offset: 0x000E1D79
		public HediffCompProperties_MessageBase()
		{
			this.compClass = typeof(HediffComp_MessageBase);
		}

		// Token: 0x0400231A RID: 8986
		public MessageTypeDef messageType;

		// Token: 0x0400231B RID: 8987
		[MustTranslate]
		public string message;

		// Token: 0x0400231C RID: 8988
		public bool onlyMessageForColonistsOrPrisoners = true;
	}
}
