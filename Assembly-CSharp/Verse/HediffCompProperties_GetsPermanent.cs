using System;

namespace Verse
{
	// Token: 0x020006B6 RID: 1718
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x06002AFA RID: 11002 RVA: 0x000E13C0 File Offset: 0x000DF5C0
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x040022AD RID: 8877
		public float becomePermanentChanceFactor = 1f;

		// Token: 0x040022AE RID: 8878
		[MustTranslate]
		public string permanentLabel;

		// Token: 0x040022AF RID: 8879
		[MustTranslate]
		public string instantlyPermanentLabel;
	}
}
